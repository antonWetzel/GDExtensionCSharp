public static class Convert {

	public static void Api(Api api, string dir, string configName) {
		BuiltinClasses(api, dir, configName);
	}

	static void BuiltinClasses(Api api, string dir, string configName) {

		foreach (var c in api.builtinClasses) {
			switch (c.name) {
			case "int":
			case "float":
			case "bool":
				break;
			default:
				BuildinClass(api, c, dir, configName);
				break;
			}
		}
	}

	static void BuildinClass(Api api, Api.BuiltinClass c, string dir, string configName) {

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
				BuiltinMethod(meth, c.name, file);
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
		file.WriteLine($"\t\tvar constructor = Initialization.inter.variant_get_ptr_constructor(VariantType.{Fixer.Type(c.name)}, {constructor.index});");

		if (constructor.arguments != null) {
			file.WriteLine($"\t\tvar args = stackalloc IntPtr[{constructor.arguments.Length}];");
			for (var i = 0; i < constructor.arguments.Length; i++) {
				var arg = constructor.arguments[i];
				file.WriteLine($"\t\targs[{i}] = new IntPtr(&{Fixer.Name(arg.name)});");
			}
		}
		file.WriteLine($"\t\tfixed ({Fixer.Type(c.name)}* ptr = &this) {{");
		if (constructor.arguments != null) {
			file.WriteLine("\t\t\tconstructor(new IntPtr(ptr), args);");
		} else {
			file.WriteLine("\t\t\tconstructor(new IntPtr(ptr), (IntPtr*)(void*)IntPtr.Zero);");
		}
		file.WriteLine("\t\t}");
		file.WriteLine("\t}");
		file.WriteLine();
	}

	static void Operator(Api.Operator op, string className, StreamWriter file) {
		if (op.rightType != null) {
			var name = op.name switch {
				"or" => "operator |",
				"and" => "operator &",
				"xor" => "operator ^",
				"**" => "OperatorPower",
				"in" => "OperatorIn",
				_ => $"operator {op.name}",
			};
			file.WriteLine($"\tpublic static {Fixer.Type(op.returnType)} {name}({Fixer.Type(className)} left, {Fixer.Type(op.rightType)} right) {{");
			file.WriteLine($"\t\tvar op = Initialization.inter.variant_get_ptr_operator_evaluator(VariantOperator.{Fixer.VariantOperator(op.name)}, VariantType.{className}, VariantType.{Fixer.VariantName(op.rightType)});");
			file.WriteLine($"\t\t{Fixer.Type(op.returnType)} res;");
			file.WriteLine($"\t\top(new IntPtr(&left), new IntPtr(&right), new IntPtr(&res));");
			file.WriteLine("\t\treturn res;");
		} else {
			var name = op.name switch {
				"unary-" => "operator -",
				"not" => "operator !",
				"unary+" => "operator +",
				_ => $"operator {op.name}",
			};
			file.WriteLine($"\tpublic static {Fixer.Type(op.returnType)} {name}({Fixer.Type(className)} value) {{");
			file.WriteLine($"\t\tvar op = Initialization.inter.variant_get_ptr_operator_evaluator(VariantOperator.{Fixer.VariantOperator(op.name)}, VariantType.{className}, VariantType.Nil);");
			file.WriteLine($"\t\t{Fixer.Type(op.returnType)} res;");
			file.WriteLine($"\t\top(new IntPtr(&value), IntPtr.Zero, new IntPtr(&res));");
			file.WriteLine("\t\treturn res;");
		}
		file.WriteLine("\t}");
		file.WriteLine();
	}

	static void Enum(Api.Enum e, StreamWriter file) {
		if (e.isBitfield != null) {
			throw new NotImplementedException();
		}
		file.WriteLine($"\tpublic enum {e.name} {{");
		foreach (var v in e.values) {
			file.WriteLine($"\t\t{v.name} = {v.value},");
		}
		file.WriteLine("\t}");
		file.WriteLine();
	}

	static string ValueToPointer(string name, string type) {
		switch (type) {
		case "Variant":
			return $"Variant.InteropToPointer({Fixer.Name(name)})";
		default:
			return $"new IntPtr(&{Fixer.Name(name)})";
		}
	}

	static string ReturnLocationType(string type) {
		return type switch {
			"Variant" => "IntPtr",
			_ => $"{Fixer.Type(type)}",
		};
	}

	static string ReturnStatementValue(string type) {
		return type switch {
			"Variant" => "Variant.InteropFromPointer(res)",
			_ => "res",
		};
	}

	static void BuiltinMethod(Api.BuiltinMethod meth, string className, StreamWriter file) {
		file.Write("\tpublic ");
		if (meth.isStatic) {
			file.Write("static ");
		}
		if (meth.returnType != null) {
			file.Write(Fixer.Type(meth.returnType));
		} else {
			file.Write("void");
		}
		file.Write($" {meth.name}(");
		if (meth.arguments != null) {
			for (var i = 0; i < meth.arguments.Length; i++) {
				var arg = meth.arguments[i];
				if (meth.isVararg) {
					file.Write($"params {Fixer.Type(arg.type)}[] {Fixer.Name(arg.name)}");
				} else {
					file.Write($"{Fixer.Type(arg.type)} {Fixer.Name(arg.name)}");
				}
				if (arg.defaultValue != null) {
					var def = arg.defaultValue;
					if (def.Contains('(')) { goto skip; }
					if (def.Contains('"')) { goto skip; }
					if (def == "null") { goto skip; }
					file.Write($" = {Fixer.Value(def)}");
				skip:
					;
				}
				if (i < meth.arguments.Length - 1) {
					file.Write(", ");
				}
			}
		}
		file.WriteLine(") {");
		file.WriteLine($"\t\tvar m = Initialization.inter.variant_get_ptr_builtin_method(VariantType.{className}, (byte*)Marshal.StringToHGlobalAnsi(\"{meth.name}\"), {meth.hash});");

		if (meth.arguments != null) {
			if (meth.isVararg) {
				if (meth.arguments.Length != 1) {
					throw new NotImplementedException();
				}
				var v = meth.arguments[0];
				file.WriteLine($"\t\tfixed ({Fixer.Type(v.type)}* {v.name}_ptr = {v.name}) {{");
				file.WriteLine($"\t\tvar args = (IntPtr*)(void*){v.name}_ptr;");
			} else {
				file.WriteLine($"\t\tvar args = stackalloc IntPtr[{meth.arguments.Length}];");
				for (var i = 0; i < meth.arguments.Length; i++) {
					var arg = meth.arguments[i];
					file.WriteLine($"\t\targs[{i}] = {ValueToPointer(arg.name, arg.type)};");
				}
			}

		}
		if (meth.returnType != null) {
			file.WriteLine($"\t\t{ReturnLocationType(meth.returnType)} res;");
		}
		if (meth.isStatic) {
			file.Write("\t\tm(IntPtr.Zero");
		} else {
			file.WriteLine($"\t\tfixed ({Fixer.Type(className)}* ptr = &this) {{");
			file.Write("\t\t\tm(new IntPtr(ptr)");
		}
		if (meth.arguments != null) {
			file.Write(", args");
		} else {
			file.Write(", (IntPtr*)(void*)IntPtr.Zero");
		}
		if (meth.returnType != null) {
			file.Write($", new IntPtr(&res)");
		} else {
			file.Write(", IntPtr.Zero");
		}
		if (meth.arguments != null) {
			file.WriteLine($", {meth.arguments.Length});");
		} else {
			file.WriteLine(", 0);");
		}
		if (meth.isStatic == false) {
			file.WriteLine("\t\t}");
		}
		if (meth.arguments != null && meth.isVararg) {
			file.WriteLine("\t\t}");
		}
		if (meth.returnType != null) {
			file.WriteLine($"\t\treturn {ReturnStatementValue(meth.returnType)};");
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
}
