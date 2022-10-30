public static class Convert {

	static HashSet<string> objectTypes = new() { "Variant" };

	public static void Api(Api api, string dir, string configName) {

		var register = File.CreateText(dir + "/Register.cs");
		register.WriteLine("namespace GDExtension;");
		register.WriteLine("public static class Register {");
		register.WriteLine("\tpublic static void RegisterAll() {");

		foreach (var c in api.classes) {
			objectTypes.Add(c.name);
			register.WriteLine($"\t\t{c.name}.Register();");
		}
		register.WriteLine("\t}");
		register.WriteLine("}");
		register.Close();

		BuiltinClasses(api, dir, configName);
		Classes(api, dir);

		Directory.CreateDirectory(dir + "/Enums");
		foreach (var e in api.globalEnums) {
			GlobalEnum(e, dir + "/Enums");
		}

		Directory.CreateDirectory(dir + "/NativeStructures");
		foreach (var native in api.nativeStructures) {
			var file = File.CreateText(dir + "/NativeStructures/" + Fixer.Type(native.name) + ".cs");
			file.WriteLine("namespace GDExtension;");
			file.WriteLine(value: "[StructLayout(LayoutKind.Sequential)]");
			file.WriteLine($"public unsafe struct {native.name} {{");
			foreach (var member in native.format.Split(";")) {
				var pair = member.Split(" ");
				var name = Fixer.Name(pair[1]);
				var type = Fixer.Type(pair[0]);
				if (name.Contains("*")) {
					type = "IntPtr"; //pointer to `'Object', which is managed in bindings
					name = name.Replace("*", "");
				}
				if (name.Contains("[")) {
					var size = int.Parse(name.Split("[")[1].Split("]")[0]);
					name = name.Split("[")[0];
					for (var i = 0; i < size; i++) {
						file.WriteLine($"\t{type} {name}{i};");
					}
					continue;
				}
				file.WriteLine($"\t{type} {name};");
			}
			file.WriteLine("}");
			file.Close();
		}
		Directory.CreateDirectory(dir + "/UtilityFunctions");
		var files = new Dictionary<string, StreamWriter>();
		foreach (var f in api.untilityFunction) {
			var cat = f.category![0].ToString().ToUpper() + f.category.Substring(1);
			if (files.TryGetValue(cat, out var file) == false) {
				file = File.CreateText(dir + "/UtilityFunctions/" + cat + ".cs");
				files.Add(cat, file);
				file.WriteLine("namespace GDExtension;");
				file.WriteLine($"public static unsafe class {cat} {{");
			}
			Method(f, "", file, MethodType.Utility);
		}
		foreach (var (_, file) in files) {
			file.WriteLine("}");
			file.Close();
		}
	}

	static void GlobalEnum(Api.Enum e, string dir) {
		if (e.name.Contains(".")) { return; }
		var name = Fixer.Type(e.name).Replace(".", "");
		var file = File.CreateText(dir + "/" + Fixer.Type(name) + ".cs");
		file.WriteLine("namespace GDExtension {");
		Enum(e, file);
		file.WriteLine("}");
		file.Close();
	}

	static void BuiltinClasses(Api api, string dir, string configName) {
		dir += "/BuiltinClasses";
		Directory.CreateDirectory(dir);
		foreach (var c in api.builtinClasses) {
			switch (c.name) {
			case "int":
			case "float":
			case "bool":
			case "String":
			case "Nil":
				break;
			default:
				BuiltinClass(api, c, dir, configName);
				break;
			}
		}
	}

	static void BuiltinClass(Api api, Api.BuiltinClass c, string dir, string configName) {

		var file = File.CreateText(dir + "/" + Fixer.Type(c.name) + ".cs");

		int size = -1;
		foreach (var config in api.builtinClassSizes) {
			if (config.buildConfiguration == configName) {
				foreach (var sizePair in config.sizes) {
					if (sizePair.name == c.name) {
						size = sizePair.size;
						break;
					}
				}
				break;
			}
		}

		file.WriteLine("namespace GDExtension;");
		file.WriteLine("");
		file.WriteLine($"[StructLayout(LayoutKind.Explicit, Size = {size})]");
		file.WriteLine($"public unsafe partial struct {Fixer.Type(c.name)} {{");
		file.WriteLine();

		if (c.isKeyed) {
			//Dictionary
		}

		if (c.indexingReturnType != null) {
			//array?
		}

		if (c.members != null) {
			foreach (var member in c.members) {
				Member(api, c, member, configName, file);
			}
		}

		if (c.constants != null) {
			foreach (var con in c.constants) {
				Constant(con, file);
			}
			file.WriteLine();
		}

		if (c.constructors != null) {
			foreach (var constructor in c.constructors) {
				Constructor(c, constructor, file);
			}
		}

		if (c.operators != null) {
			foreach (var op in c.operators) {
				Operator(op, c.name, file);
			}
		}

		if (c.enums != null) {
			foreach (var e in c.enums) {
				Enum(e, file);
			}
		}

		if (c.methods != null) {
			foreach (var meth in c.methods) {
				Method(meth, c.name, file, MethodType.Native);
			}
		}

		EqualAndHash(c.name, file);

		file.WriteLine("}");
		file.Close();
	}


	static void Member(Api api, Api.BuiltinClass c, Api.Member member, string configName, StreamWriter file) {
		var offset = -1;
		foreach (var config in api.builtinClassMemberOffsets) {
			if (config.buildConfiguration == configName) {
				foreach (var cl in config.classes) {
					if (cl.name == c.name) {
						foreach (var memberOffset in cl.members) {
							if (memberOffset.member == member.name) {
								offset = memberOffset.offset;
							}
						}
					}
				}
			}
		}
		if (offset >= 0) {
			file.WriteLine($"\t[FieldOffset({offset})]");
			file.WriteLine($"\tpublic {member.type} {member.name};");
		} else {
			file.WriteLine($"\tpublic {member.type} {member.name} {{ get => default; set {{ }} }}");
		}
		file.WriteLine();
	}

	static void Constant(Api.Constant con, StreamWriter file) {
		file.WriteLine($"\tpublic static {Fixer.Type(con.type)} {con.name} = {Fixer.Value(con.value)};");
	}

	static void Constructor(Api.BuiltinClass c, Api.Constructor constructor, StreamWriter file) {
		file.Write($"\tpublic {Fixer.Type(c.name)}(");
		if (constructor.arguments != null) {
			for (var i = 0; i < constructor.arguments.Length - 1; i++) {
				var arg = constructor.arguments[i];
				file.Write(value: $"{Fixer.Type(arg.type)} {Fixer.Name(arg.name)}, ");
			}
			var a = constructor.arguments.Last();
			file.Write(value: $"{Fixer.Type(a.type)} {Fixer.Name(a.name)}");
		}
		file.WriteLine(") {");
		file.WriteLine($"\t\tvar constructor = gdInterface.variant_get_ptr_constructor.Call(Variant.Type.{Fixer.Type(c.name)}, {constructor.index});");

		if (constructor.arguments != null) {
			file.WriteLine($"\t\tvar args = stackalloc TypePtr[{constructor.arguments.Length}];");
			for (var i = 0; i < constructor.arguments.Length; i++) {
				var arg = constructor.arguments[i];
				file.WriteLine($"\t\targs[{i}] = {ValueToPointer(Fixer.Name(arg.name), arg.type)};");
			}
		}
		file.WriteLine($"\t\tfixed ({Fixer.Type(c.name)}* ptr = &this) {{");
		if (constructor.arguments != null) {
			file.WriteLine("\t\t\tconstructor(new IntPtr(ptr), args);");
		} else {
			file.WriteLine("\t\t\tconstructor(new IntPtr(ptr), (TypePtr*)(void*)IntPtr.Zero);");
		}
		file.WriteLine("\t\t}");
		file.WriteLine("\t}");
		file.WriteLine();
	}

	static void Operator(Api.Operator op, string className, StreamWriter file) {

		if (op.rightType != null) {
			if (op.rightType == "Variant") { return; }
			var name = op.name switch {
				"or" => "operator |",
				"and" => "operator &",
				"xor" => "operator ^",
				"**" => "OperatorPower",
				"in" => "OperatorIn",
				_ => $"operator {op.name}",
			};

			file.WriteLine($"\tpublic static {Fixer.Type(op.returnType)} {name}({Fixer.Type(className)} left, {Fixer.Type(op.rightType)} right) {{");
			file.WriteLine($"\t\tvar __op = gdInterface.variant_get_ptr_operator_evaluator.Call(Variant.Operator.{Fixer.VariantOperator(op.name)}, Variant.Type.{className}, Variant.Type.{Fixer.VariantName(op.rightType)});");
			file.WriteLine($"\t\t{Fixer.Type(op.returnType)} __res;");
			file.WriteLine($"\t\t__op(new IntPtr(&left), {ValueToPointer("right", op.rightType)}, new IntPtr(&__res));");
			file.WriteLine("\t\treturn __res;");
		} else {
			var name = op.name switch {
				"unary-" => "operator -",
				"not" => "operator !",
				"unary+" => "operator +",
				_ => $"operator {op.name}",
			};
			file.WriteLine($"\tpublic static {Fixer.Type(op.returnType)} {name}({Fixer.Type(className)} value) {{");
			file.WriteLine($"\t\tvar __op = gdInterface.variant_get_ptr_operator_evaluator.Call(Variant.Operator.{Fixer.VariantOperator(op.name)}, Variant.Type.{className}, Variant.Type.Nil);");
			file.WriteLine($"\t\t{Fixer.Type(op.returnType)} __res;");
			file.WriteLine($"\t\t__op(new IntPtr(&value), IntPtr.Zero, new IntPtr(&__res));");
			file.WriteLine("\t\treturn __res;");
		}
		file.WriteLine("\t}");
		file.WriteLine();
	}

	static void Enum(Api.Enum e, StreamWriter file) {
		if (e.isBitfield != null) {
			//throw new NotImplementedException();
		}
		file.WriteLine($"\tpublic enum {Fixer.Type(e.name)} {{");
		foreach (var v in e.values) {
			file.WriteLine($"\t\t{v.name} = {v.value},");
		}
		file.WriteLine("\t}");
		file.WriteLine();
	}

	static string ValueToPointer(string name, string type) {
		if (type == "String") {
			return $"StringMarshall.ToNative({name})";
		} else if (objectTypes.Contains(type)) {
			return $"{name}._internal_pointer";
		} else {
			return $"new IntPtr(&{Fixer.Name(name)})";
		}
	}

	static string ReturnLocationType(string type) {
		if (objectTypes.Contains(type) || type == "String") {
			return "IntPtr";
		} else {
			return $"{Fixer.Type(type)}";
		}
	}

	static string ReturnStatementValue(string type) {
		if (type == "String") {
			return "StringMarshall.ToManaged(__res)";
		} else if (objectTypes.Contains(type)) {
			return $"({type})Object.ConstructUnknown(__res)";
		} else {
			return $"__res";
		}
	}

	enum MethodType {
		Class,
		Native,
		Utility,
	}

	static void Method(Api.Method meth, string className, StreamWriter file, MethodType type, bool isSingleton = false) {
		var ret = meth.returnType ?? meth.returnValue?.type ?? "";
		file.Write("\tpublic ");
		if (meth.isStatic || type == MethodType.Utility || isSingleton) {
			file.Write("static ");
		}
		if (ret != "") {
			file.Write(Fixer.Type(ret));
		} else {
			file.Write("void");
		}
		file.Write($" {Fixer.SnakeToPascal(meth.name)}(");
		if (meth.arguments != null) {
			for (var i = 0; i < meth.arguments.Length; i++) {
				var arg = meth.arguments[i];
				file.Write($"{Fixer.Type(arg.type)} {Fixer.Name(arg.name)}");
				/*if (arg.defaultValue != null) {
					var def = arg.defaultValue;
					if (def.Contains('(')) { goto skip; }
					if (def.Contains('"')) { goto skip; }
					if (def == "{}") { goto skip; }
					if (def == "[]") { goto skip; }
					if (def == "null") { goto skip; }
					if (def == "") { goto skip; }
					file.Write($" = {Fixer.Value(def)}");
				skip:
					;
				}*/
				if (i < meth.arguments.Length - 1) {
					file.Write(", ");
				}
			}
		}
		if (meth.isVararg) {
			if (meth.arguments != null) {
				file.Write(", ");
			}
			file.Write("params Variant[] arguments");
		}
		file.WriteLine(") {");
		switch (type) {
		case MethodType.Class:
			file.WriteLine($"\t\tvar __m = gdInterface.classdb_get_method_bind.Call(\"{className}\", \"{meth.name}\", {meth.hash});");
			break;
		case MethodType.Native:
			file.WriteLine(value: $"\t\tvar __m = gdInterface.variant_get_ptr_builtin_method.Call(Variant.Type.{className}, \"{meth.name}\", {meth.hash});");
			break;
		case MethodType.Utility:
			file.WriteLine($"\t\tvar __m = gdInterface.variant_get_ptr_utility_function.Call(\"{meth.name}\", {meth.hash});");
			break;
		}
		if (meth.isVararg) {
			var t = type == MethodType.Class ? "VariantPtr" : "TypePtr";
			if (meth.arguments != null) {
				file.WriteLine($"\t\tvar __args = stackalloc {t}[{meth.arguments.Length} + arguments.Length];");
			} else {
				file.WriteLine($"\t\tvar __args = stackalloc {t}[arguments.Length];");
			}
		} else if (meth.arguments != null) {
			file.WriteLine($"\t\tvar __args = stackalloc TypePtr[{meth.arguments.Length}];");
		}
		if (meth.arguments != null) {
			for (var i = 0; i < meth.arguments.Length; i++) {
				var arg = meth.arguments[i];
				file.Write($"\t\t__args[{i}] = ");
				if (meth.isVararg) {
					var val = arg.type != "Variant" ? $"new Variant({Fixer.Name(arg.name)})" : Fixer.Name(arg.name);
					file.WriteLine($"{val}._internal_pointer;");
				} else {
					file.WriteLine($"{ValueToPointer(Fixer.Name(arg.name), arg.type)};");
				}
			}
		}
		if (meth.isVararg) {
			var offset = meth.arguments != null ? $"{meth.arguments.Length} + " : "";
			file.WriteLine($"\t\tfor (var i = 0; i < arguments.Length; i++) {{");
			file.WriteLine($"\t\t\t__args[{offset}i] = arguments[i]._internal_pointer;");
			file.WriteLine("\t\t};");
		}
		if (ret != "") {
			file.WriteLine($"\t\t{ReturnLocationType(ret)} __res;");
		}
		if (type == MethodType.Class && meth.isVararg) {
			file.WriteLine("\t\tCallError __err;");
		}
		if (meth.isStatic == false && type == MethodType.Native) {
			file.WriteLine($"\t\tvar __temp = this;");
		}
		if (type == MethodType.Class) {
			if (meth.isVararg) {
				file.Write("\t\tgdInterface.object_method_bind_call.Call(__m, ");
			} else {
				file.Write("\t\tgdInterface.object_method_bind_ptrcall.Call(__m, ");
			}
		} else {
			file.Write("\t\t__m(");
		}
		if (type == MethodType.Utility) {
			if (ret != "") {
				file.Write("new IntPtr(&__res)");
			} else {
				file.Write("IntPtr.Zero");
			}
		} else if (meth.isStatic) {
			file.Write("IntPtr.Zero");
		} else if (type == MethodType.Class) {
			file.Write($"{(isSingleton ? "Singleton" : "this")}._internal_pointer");
		} else {
			file.Write("new IntPtr(&__temp)");
		}
		if (meth.arguments != null || meth.isVararg) {
			file.Write(", __args");
		} else {
			file.Write(", null");
		}
		if (type == MethodType.Class && meth.isVararg) {
			file.Write($", {(meth.arguments != null ? $"{meth.arguments.Length} + " : "")}arguments.Length");
		}
		if (type == MethodType.Utility) {
			//pass
		} else if (ret != "") {
			file.Write($", new IntPtr(&__res)");
		} else {
			file.Write(", IntPtr.Zero");
		}
		if (type != MethodType.Class) {
			file.Write(", ");
			if (meth.isVararg) {
				file.Write($"{(meth.arguments != null ? $"{meth.arguments.Length} + " : "")}arguments.Length");
			} else if (meth.arguments != null) {
				file.Write($"{meth.arguments.Length}");
			} else {
				file.Write("0");
			}
		}
		if (type == MethodType.Class && meth.isVararg) {
			file.Write(", &__err");
		}
		file.WriteLine(");");
		if (ret != "") {
			file.WriteLine($"\t\treturn {ReturnStatementValue(ret)};");
		}
		file.WriteLine("\t}");
		file.WriteLine();
	}

	public static void EqualAndHash(string className, StreamWriter file) {
		file.WriteLine("\tpublic override bool Equals(object? obj) {");
		file.WriteLine("\t\tif (obj == null) { return false; }");
		file.WriteLine($"\t\tif (obj is {Fixer.Type(className)} other == false) {{ return false; }}");
		file.WriteLine("\t\treturn this == other;");
		file.WriteLine("\t}");
		file.WriteLine();

		//todo: based on members
		file.WriteLine("\tpublic override int GetHashCode() {");
		file.WriteLine("\t\treturn base.GetHashCode();");
		file.WriteLine("\t}");
		file.WriteLine();
	}

	static Api.Method? GetMethod(Api api, string cName, string name) {
		foreach (var c in api.classes)
			if (cName == c.name) {
				if (c.methods != null) {
					foreach (var m in c.methods!) {
						if (m.name == name) {
							return m;
						}
					}
				}
				if (c.inherits != null) {
					return GetMethod(api, c.inherits, name);
				}
			}
		return null;
	}

	static void Classes(Api api, string dir) {
		dir += "/Classes";
		Directory.CreateDirectory(dir);
		foreach (var c in api.classes) {
			switch (c.name) {
			case "int":
			case "float":
			case "bool":
				break;
			default:
				Class(api, c, dir);
				break;
			}
		}
	}

	static void Class(Api api, Api.Class c, string dir) {

		var file = File.CreateText(dir + "/" + c.name + ".cs");

		file.WriteLine("namespace GDExtension;");
		file.WriteLine("");
		file.Write("public unsafe ");
		if (c.isInstantiable == false) {
			//file.Write("abstract ");
		}

		file.WriteLine($"partial class {c.name} : {(c.inherits == null ? "Wrapped" : c.inherits)} {{");
		file.WriteLine();

		var isSingleton = api.singletons.Any(x => x.type == c.name);
		if (isSingleton) {
			file.WriteLine($"\tpublic static {c.name} Singleton => new {c.name}(gdInterface.global_get_singleton.Call(\"{c.name}\"));");
			file.WriteLine();
		}

		file.WriteLine($"\tpublic static new void Register() => Object.RegisterConstructor(\"{c.name}\", Construct);");

		if (c.constants != null) {
			foreach (var con in c.constants) {
				file.WriteLine($"\tpublic const int {con.name} = {con.value};");
			}
			file.WriteLine();
		}

		if (c.enums != null) {
			foreach (var e in c.enums) {
				Enum(e, file);
			}
		}

		if (c.properties != null) {
			foreach (var prop in c.properties) {
				var type = prop.type;
				var cast = "";

				var getter = GetMethod(api, c.name, prop.getter);
				var setter = GetMethod(api, c.name, prop.setter);

				if (getter == null && setter == null) {
					Console.WriteLine($"{c.name} setter {prop.setter} and getter {prop.getter} not found");
					continue;
				}
				if (getter != null) { type = getter.Value.returnValue!.Value.type; } else { type = setter!.Value.arguments![0].type; }

				file.Write($"\tpublic {Fixer.Type(type)} {Fixer.Name(prop.name)} {{ ");

				if (prop.index >= 0) {
					cast = $"({Fixer.Type((getter ?? setter!).Value.arguments![0].type)})";
				}

				if (getter != null) {
					file.Write($"get => {Fixer.SnakeToPascal(prop.getter)}(");
					if (prop.index >= 0) {
						file.Write($"{cast}{prop.index}");
					}
					file.Write("); ");
				}

				if (setter != null) {
					file.Write($"set => {Fixer.SnakeToPascal(prop.setter)}(");
					if (prop.index >= 0) {
						file.Write($"{cast}{prop.index}, ");
					}
					file.Write("value); ");
				}
				file.WriteLine("}");
			}
			file.WriteLine();
		}

		if (c.methods != null) {
			foreach (var meth in c.methods) {
				Method(meth, c.name, file, MethodType.Class, isSingleton);
			}
		}

		if (c.signals != null) {
			foreach (var sig in c.signals) {

			}
		}

		EqualAndHash(c.name, file);

		var content = c.name == "RefCounted" ? " Reference(); " : " ";
		file.WriteLine($"\tpublic {c.name}() : base(gdInterface.classdb_construct_object.Call(\"{c.name}\")) {{{content}}}");
		file.WriteLine($"\tprotected {c.name}(ObjectPtr ptr) : base(ptr) {{{content}}}");
		file.WriteLine($"\tprivate static {c.name} Construct(ObjectPtr ptr) => new {c.name}(ptr);");

		file.WriteLine("}");
		file.Close();
	}
}
