var api = Api.Create("./godot/extension_api.json");

var dir = "./GDExtension/Generated";

var configName = "float_64";

if (Directory.Exists(dir)) {
	Directory.Delete(dir, true);
}
Directory.CreateDirectory(dir);

foreach (var c in api.builtinClasses) {
	switch (c.name) {
	case "int":
	case "float":
	case "bool":
		continue;
	default:
		break;
	}

	Console.WriteLine($"{Fixer.Type(c.name)} {c.indexingReturnType}");

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

	if (c.members != null) {

		foreach (var member in c.members) {
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
	}

	if (c.isKeyed) {
		//Dictionary
	}

	if (c.constructors != null) {
		foreach (var constructor in c.constructors) {
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
	}

	if (c.operators != null) {
		foreach (var op in c.operators) {
			if (op.rightType != null) {
				var name = op.name switch {
					"or" => "operator |",
					"and" => "operator &",
					"xor" => "operator ^",
					"**" => "OperatorPower",
					"in" => "OperatorIn",
					_ => $"operator {op.name}",
				};
				file.WriteLine($"\tpublic static {Fixer.Type(op.returnType)} {name}({Fixer.Type(c.name)} left, {Fixer.Type(op.rightType)} right) {{");
				file.WriteLine($"\t\tvar op = Initialization.inter.variant_get_ptr_operator_evaluator(VariantOperator.{Fixer.VariantOperator(op.name)}, VariantType.{c.name}, VariantType.{Fixer.VariantName(op.rightType)});");
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
				Console.WriteLine($"\t{name}");
				file.WriteLine($"\tpublic static {Fixer.Type(op.returnType)} {name}({Fixer.Type(c.name)} value) {{");
				file.WriteLine($"\t\tvar op = Initialization.inter.variant_get_ptr_operator_evaluator(VariantOperator.{Fixer.VariantOperator(op.name)}, VariantType.{c.name}, VariantType.Nil);");
				file.WriteLine($"\t\t{Fixer.Type(op.returnType)} res;");
				file.WriteLine($"\t\top(new IntPtr(&value), IntPtr.Zero, new IntPtr(&res));");
				file.WriteLine("\t\treturn res;");
			}
			file.WriteLine("\t}");
			file.WriteLine();
		}
	}
	{ //equals and hash
		file.WriteLine("\tpublic override bool Equals(object? obj) {");
		file.WriteLine("\t\tif (obj == null) { return false; }");
		file.WriteLine($"\t\tif (obj is {Fixer.Type(c.name)} other == false) {{ return false; }}");
		file.WriteLine("\t\treturn this == other;");
		file.WriteLine("\t}");
		file.WriteLine();

		//todo: based on members
		file.WriteLine("\tpublic override int GetHashCode() {");
		file.WriteLine("\t\treturn base.GetHashCode();");
		file.WriteLine("\t}");
		file.WriteLine();
	}

	file.WriteLine("}");
	file.Close();
}
