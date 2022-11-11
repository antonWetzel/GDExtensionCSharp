using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generators {

	public class Methods {

		public struct Info {
			public string name;
			public (ITypeSymbol, string)[] arguments;
			public ITypeSymbol ret;
			public string property;
		}

		List<Info> methods;

		public Methods() {
			methods = new List<Info>();
		}

		public void AddMethod(Info info) {
			methods.Add(info);
		}

		void AddAttributeMethods(GeneratorExecutionContext context, INamedTypeSymbol c) {
			var ms = c.GetMembers().
				Where(x => x is IMethodSymbol).
				Select(x => (IMethodSymbol)x).
				Where(x => x.GetAttributes().Where(x => x.AttributeClass.ToString() == "GDExtension.MethodAttribute").Count() > 0)
				.ToArray();

			foreach (var method in ms) {
				var info = new Info() {
					name = method.Name,
					arguments = method.Parameters.Select(x => (x.Type, x.Name)).ToArray(),
					ret = method.ReturnsVoid ? null : method.ReturnType,
					property = null,
				};
				AddMethod(info);
			}
		}

		public void Generate(GeneratorExecutionContext context, INamedTypeSymbol c) {
			AddAttributeMethods(context, c);

			var code = $$"""
			using System.Runtime.CompilerServices;
			using System.Runtime.InteropServices;

			namespace {{c.ContainingNamespace}};

			public unsafe partial class {{c.Name}} : {{c.BaseType.Name}} {

			""";

			for (var i = 0; i < methods.Count; i++) {
				var method = methods[i];
				if (method.ret != null) {
					code += $"static Native.PropertyInfo __{method.name}_returnInfo = {CreatePropertyInfo(method.ret, "return")}";
				}
				for (var j = 0; j < method.arguments.Length; j++) {
					var arg = method.arguments[j];
					code += $"static Native.PropertyInfo __{method.name}_{arg.Item2}Info = {CreatePropertyInfo(arg.Item1, arg.Item2)}";
				}
			}

			code += $$"""
				static unsafe void RegisterMethods() {
					Native.ExtensionClassMethodInfo info;
					var namePtr = Marshal.StringToHGlobalAnsi("{{c.Name}}");

			""";

			for (var i = 0; i < methods.Count; i++) {
				var method = methods[i];
				code += $$"""
						info = new Native.ExtensionClassMethodInfo() {
							name = (sbyte*)Marshal.StringToHGlobalAnsi("{{Renamer.ToSnake(method.name)}}"),
							method_userdata = new IntPtr({{i}}),
							call_func = &CallFunc,
							ptrcall_func = &CallFuncPtr,
							method_flags = Native.ExtensionClassMethodFlags.Default,
							argument_count = {{method.arguments.Length}},
							has_return_value = {{(method.ret != null ? "true" : "false")}},
							get_argument_type_func = &ArgumentType,
							get_argument_info_func = &ArgumentInfo,
							get_argument_metadata_func = &ArgumentMetadata,
							default_argument_count = 0,
							default_arguments = null,
						};

						Native.gdInterface.classdb_register_extension_class_method(Native.gdLibrary, (sbyte*)namePtr, &info);
						Marshal.FreeHGlobal((IntPtr)info.name);


				""";
			}

			code += $$"""
					Marshal.FreeHGlobal(namePtr);
				}

				[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
				static void CallFuncPtr(IntPtr method_userdata, IntPtr p_instance, IntPtr* p_args, IntPtr r_ret) {
					var instance = ({{c.Name}})p_instance;
					switch ((int)method_userdata) {

			""";
			for (var i = 0; i < methods.Count; i++) {
				var method = methods[i];
				var args = "";
				for (var j = 0; j < method.arguments.Length; j++) {
					var arg = method.arguments[j];
					if (arg.Item1.Name == "String") {
						args += $"StringMarshall.ToManaged(p_args[{j}])";
					} else if (TypeToVariantType(arg.Item1) == "Object") {
						args += $"({arg.Item1.Name})GDExtension.Object.ConstructUnknown(*(IntPtr*)(void*)p_args[{j}])";
					} else {
						args += $"*({arg.Item1.Name}*)(void*)p_args[{j}]";
					}
					if (j < method.arguments.Length - 1) {
						args += ", ";
					}
				}
				code += $"\t\tcase {i}:\n\t\t\t";
				if (method.ret != null) {
					if (TypeToVariantType(method.ret) == "Object") {
						code += "throw new NotImplementedException();\n";
						continue;
					} else {
						code += $"*({method.ret}*)r_ret = ";
					}
				}
				if (method.property != null) {
					if (method.ret != null) {
						code += $"instance.{method.property};\n";
					} else {
						code += $"instance.{method.property} = {args};\n";
					}
				} else {
					code += $"instance.{method.name}({args});\n";
				}
				code += $"\t\t\tbreak;\n";
			}

			code += $$"""
					}
				}

				[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
				static void CallFunc(
					IntPtr method_userdata,
					IntPtr p_instance,
					IntPtr* p_args,
					long p_argument_count,
					IntPtr r_return,
					Native.CallError* r_error
				) {
					Native.gdInterface.variant_new_nil(r_return); //no clue why this is needed
					var instance = ({{c.Name}})p_instance;
					switch ((int)method_userdata) {

			""";
			for (var i = 0; i < methods.Count; i++) {
				var method = methods[i];
				var args = "";
				for (var j = 0; j < method.arguments.Length; j++) {
					var arg = method.arguments[j];
					var t = TypeToVariantType(arg.Item1);
					if (arg.Item1.Name == "String") {
						args += $"StringMarshall.ToManaged(Variant.InteropGetFromPointer<IntPtr>(p_args[{j}], Variant.Type.String))";
					} else if (t == "Object") {
						args += $"({arg.Item1.Name})GDExtension.Object.ConstructUnknown(Variant.InteropGetFromPointer<IntPtr>(p_args[{j}], Variant.Type.{t}))";
					} else {
						args += $"Variant.InteropGetFromPointer<{arg.Item1}>(p_args[{j}], Variant.Type.{t})";
					}
					if (j < method.arguments.Length - 1) {
						args += ", ";
					}
				}
				code += $"\t\tcase {i}: {{\n\t\t\t";
				if (method.ret != null) {
					code += "var res = ";
				}
				if (method.property != null) {
					if (method.ret != null) {
						code += $"instance.{method.property};\n";
					} else {
						code += $"instance.{method.property} = {args};\n";
					}
				} else {
					code += $"instance.{method.name}({args});\n";
				}
				if (method.ret != null) {
					var t = TypeToVariantType(method.ret);
					code += $"\t\t\tVariant.SaveIntoPointer(res, r_return);\n";
				}
				code += $"\t\t\tbreak;\n";
				code += "\t\t\t}\n";
			}

			code += $$"""
					}
				}

				[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
				static Variant.Type ArgumentType(IntPtr p_method_userdata, int p_argument) {
					return (int)p_method_userdata switch {

			""";
			for (var i = 0; i < methods.Count; i++) {
				var method = methods[i];
				code += $"\t\t{i} => p_argument switch {{\n";
				if (method.ret != null) {
					code += $"\t\t\t-1 => Variant.Type.{TypeToVariantType(method.ret)},\n";
				}
				for (var j = 0; j < method.arguments.Length; j++) {
					code += $"\t\t\t{j} => Variant.Type.{TypeToVariantType(method.arguments[j].Item1)},\n";
				}
				code += "\t\t\t_ => Variant.Type.Nil,\n\t\t},\n";
			}
			code += $$"""
						_ => Variant.Type.Nil,
					};
				}

				[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
				static void ArgumentInfo(IntPtr p_method_userdata, int p_argument, Native.PropertyInfo* r_info) {
					*r_info = (int)p_method_userdata switch {

			""";

			for (var i = 0; i < methods.Count; i++) {
				var method = methods[i];
				code += $"\t\t{i} =>  p_argument switch {{\n";
				if (method.ret != null) {
					code += $"\t\t\t-1 => __{method.name}_returnInfo,\n";
				}
				for (var j = 0; j < method.arguments.Length; j++) {
					var arg = method.arguments[j];
					code += $"\t\t\t{j} => __{method.name}_{arg.Item2}Info,\n";
				}
				code += "\t\t\t_ => default,\n\t\t},\n";
			}
			code += $$"""
						_ => default,
					};
				}

				[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
				static Native.ExtensionClassMethodArgumentMetadata ArgumentMetadata(IntPtr p_method_userdata, int p_argument) {
					return Native.ExtensionClassMethodArgumentMetadata.None;
				}
			}
			""";

			context.AddSource($"{c.Name}.methods.gen.cs", code);
		}

		public static string TypeToVariantType(ITypeSymbol type) {
			return TypeToVariantType(type, Generators.GetSpecialBase(type));
		}

		public static string TypeToVariantType(ITypeSymbol type, SpecialBase sBase) {
			return sBase switch {
				SpecialBase.Node => "Object",
				SpecialBase.Resource => "Object",
				_ => type.Name switch {
					"Boolean" => "Bool",
					"Int64" => "Int",
					"Double" => "Float",
					"String" => "String",
					"Vector2" => "Vector2",
					"Vector2i" => "Vector2i",
					"Rect2" => "Rect2",
					"Rect2i" => "Rect2i",
					"Vector3" => "Vector3",
					"Vector3i" => "Vector3i",
					"Transform2D" => "Transform2D",
					"Vector4" => "Vector4",
					"Vector4i" => "Vector4i",
					"Plane" => "Plane",
					"Quaternion" => "Quaternion",
					"AABB" => "AABB",
					"Basis" => "Basis",
					"Transform3D" => "Transform3D",
					"Projection" => "Projection",
					"Color" => "Color",
					"StringName" => "StringName",
					"NodePath" => "NodePath",
					"RID" => "RID",
					"Callable" => "Callable",
					"Signal" => "Signal",
					"Dictionary" => "Dictionary",
					"Array" => "Array",
					"PackedByteArray" => "PackedByteArray",
					"PackedInt32Array" => "PackedInt32Array",
					"PackedInt64Array" => "PackedInt64Array",
					"PackedFloat32Array" => "PackedFloat32Array",
					"PackedFloat64Array" => "PackedFloat64Array",
					"PackedStringArray" => "PackedStringArray",
					"PackedVector2Array" => "PackedVector2Array",
					"PackedVector3Array" => "PackedVector3Array",
					"PackedColorArray" => "PackedColorArray",
					_ => throw new Exception($"Unknown type: {type.Name}"),
				},
			};
		}

		public static string TypeToHintString(ITypeSymbol type, SpecialBase sBase) {
			return sBase switch {
				SpecialBase.Node or SpecialBase.Resource => $"(sbyte*)Marshal.StringToHGlobalAnsi(\"{type.Name}\")",
				SpecialBase.None => "null",
				_ => throw new Exception(),
			};
		}

		public static string TypeToHint(ITypeSymbol type, SpecialBase sBase) {
			return sBase switch {
				SpecialBase.Node => "PropertyHint.NodeType",
				SpecialBase.Resource => "PropertyHint.ResourceType",
				SpecialBase.None => "PropertyHint.None",
				_ => throw new Exception(),
			};
		}

		public static string CreatePropertyInfo(ITypeSymbol type, string name) {
			var sBase = Generators.GetSpecialBase(type);

			return $$"""
			new Native.PropertyInfo() {
					type = Variant.Type.{{TypeToVariantType(type, sBase)}},
					name = (sbyte*)Marshal.StringToHGlobalAnsi("{{Renamer.ToSnake(name)}}"),
					class_name = null,
					hint = {{TypeToHint(type, sBase)}},
					hint_string = {{TypeToHintString(type, sBase)}},
					usage = PropertyUsageFlags.Default,
				};

			""";
		}
	}
}
