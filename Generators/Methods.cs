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
			public string ret;
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
					ret = method.ReturnsVoid ? null : method.ReturnType.Name,
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
					code += $$"""
					static Native.PropertyInfo __{{method.name}}_returnInfo = new Native.PropertyInfo() {
						type = (uint)Variant.Type.{{TypeToVariantType(method.ret)}},
						name = (byte*)Marshal.StringToHGlobalAnsi("return"),
						class_name = null,
						hint = (uint)PropertyHint.None,
						hint_string = null,
						usage = (uint)PropertyUsageFlags.Default,
					};


				""";
				}
				for (var j = 0; j < method.arguments.Length; j++) {
					var arg = method.arguments[j];
					var t = TypeToVariantType(arg.Item1.Name);
					code += $$"""
					static Native.PropertyInfo __{{method.name}}_{{arg.Item2}}Info = new Native.PropertyInfo() {
						type = (uint)Variant.Type.{{t}},
						name = (byte*)Marshal.StringToHGlobalAnsi("{{arg.Item2}}"),
						class_name = null,
						hint = (uint)PropertyHint.None,
						hint_string = null,
						usage = (uint)PropertyUsageFlags.Default,
					};


				""";
				}
			}

			code += $$"""
				static unsafe void RegisterMethods() {
					Native.ExtensionClassMethodInfo info;

			""";



			for (var i = 0; i < methods.Count; i++) {
				var method = methods[i];
				code += $$"""
						info = new Native.ExtensionClassMethodInfo() {
							name = (byte*)Marshal.StringToHGlobalAnsi("{{method.name}}"),
							method_userdata = new IntPtr({{i}}),
							call_func = new(CallFunc),
							ptrcall_func = new(CallFuncPtr),
							method_flags = (uint)Native.ExtensionClassMethodFlags.Default,
							argument_count = {{method.arguments.Length}},
							has_return_value = {{(method.ret != null ? "true" : "false")}},
							get_argument_type_func = new(ArgumentType),
							get_argument_info_func = new(ArgumentInfo),
							get_argument_metadata_func = new(ArgumentMetadata),
							default_argument_count = 0,
							default_arguments = null,
						};
						Native.gdInterface.classdb_register_extension_class_method.Call(Native.gdLibrary, "{{c.Name}}", &info);


				""";
			}

			code += $$"""
				}

				static void CallFuncPtr(IntPtr method_userdata, Native.GDExtensionClassInstancePtr p_instance, Native.TypePtr* p_args, Native.TypePtr r_ret) {
					var instance = ({{c.Name}})p_instance;
					switch ((int)method_userdata) {

			""";
			for (var i = 0; i < methods.Count; i++) {
				var method = methods[i];
				var args = "";
				for (var j = 0; j < method.arguments.Length; j++) {
					var arg = method.arguments[j];
					if (arg.Item1.Name == "String") {
						args += $"StringMarshall.ToManaged(p_args[{j}].data)";
					} else if (TypeToVariantType(arg.Item1.Name) == "Object") {
						args += $"({arg.Item1.Name})GDExtension.Object.ConstructUnknown(*(IntPtr*)(void*)p_args[{j}].data)";
					} else {
						args += $"*({arg.Item1.Name}*)(void*)p_args[{j}].data";
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
						code += $"*({method.ret}*)r_ret.data = ";
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

				static void CallFunc(
					IntPtr method_userdata,
					Native.GDExtensionClassInstancePtr p_instance,
					Native.VariantPtr* p_args,
					Native.Int p_argument_count,
					Native.VariantPtr r_return,
					Native.CallError* r_error
				) {
					var instance = ({{c.Name}})p_instance;
					switch ((int)method_userdata) {

			""";
			for (var i = 0; i < methods.Count; i++) {
				var method = methods[i];
				var args = "";
				for (var j = 0; j < method.arguments.Length; j++) {
					var arg = method.arguments[j];
					var t = TypeToVariantType(arg.Item1.Name);
					if (arg.Item1.Name == "String") {
						args += $"StringMarshall.ToManaged(Variant.InteropGetFromPointer<IntPtr>(p_args[{j}].data, Variant.Type.String))";
					} else if (t == "Object") {
						args += $"({arg.Item1.Name})GDExtension.Object.ConstructUnknown(Variant.InteropGetFromPointer<IntPtr>(p_args[{j}].data, Variant.Type.{t}))";
					} else {
						args += $"Variant.InteropGetFromPointer<{arg.Item1}>(p_args[{j}].data, Variant.Type.{t})";
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
					var getter = t == "Object" ? "res == null? IntPtr.Zero : res._internal_pointer" : "res";
					code += $"\t\t\tVariant.InteropSaveIntoPointer({getter}, r_return.data, Variant.Type.{t});\n";
				}
				code += $"\t\t\tbreak;\n";
				code += "\t\t\t}\n";
			}

			code += $$"""
					}
				}

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
					code += $"\t\t\t{j} => Variant.Type.{TypeToVariantType(method.arguments[j].Item1.Name)},\n";
				}
				code += "\t\t\t_ => Variant.Type.Nil,\n\t\t},\n";
			}
			code += $$"""
						_ => Variant.Type.Nil,
					};
				}

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

				static Native.ExtensionClassMethodArgumentMetadata ArgumentMetadata(IntPtr p_method_userdata, int p_argument) {
					return Native.ExtensionClassMethodArgumentMetadata.None;
				}
			}
			""";

			context.AddSource($"{c.Name}.methods.gen.cs", code);
		}

		public enum SpecialBase {
			None,
			Node,
			Resource,
		}

		public static SpecialBase GetSpecialBase(ITypeSymbol type) {
			return type.Name switch {
				"Node" => SpecialBase.Node,
				"Resource" => SpecialBase.Resource,
				_ => type.BaseType switch {
					null => SpecialBase.Node,
					_ => GetSpecialBase(type.BaseType),
				},
			};
		}

		public static string TypeToVariantType(ITypeSymbol type, SpecialBase sBase) {
			return TypeToVariantType(type.Name);
		}

		public static string TypeToHintString(ITypeSymbol type, SpecialBase sBase) {
			return sBase switch {
				SpecialBase.Node or SpecialBase.Resource => $"(byte*)Marshal.StringToHGlobalAnsi(\"{type.Name}\")",
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
			var sBase = GetSpecialBase(type);

			return $$"""
			new Native.PropertyInfo() {
					type = (uint)Variant.Type.{{TypeToVariantType(type, sBase)}},
					name = (byte*)Marshal.StringToHGlobalAnsi("{{name}}"),
					class_name = null,
					hint = (uint){{TypeToHint(type, sBase)}},
					hint_string = {{TypeToHintString(type, sBase)}},
					usage = (uint)PropertyUsageFlags.Default,
				};

			""";
		}

		public static string TypeToVariantType(string t) {
			return t switch {
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
				_ => "Object",
			};
		}
	}
}
