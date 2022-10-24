using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generators {

	public static class Export {

		public static void Generate(GeneratorExecutionContext context, INamedTypeSymbol c) {
			var members = c.GetMembers().
				Where(x => x is IPropertySymbol).
				Select(x => (IPropertySymbol)x).
				Where(x => x.GetAttributes().Where(x => x.AttributeClass.ToString() == "GDExtension.ExportAttribute").Count() > 0)
				.ToArray();

			var code = $$"""
			using System.Runtime.CompilerServices;
			using System.Runtime.InteropServices;

			namespace {{c.ContainingNamespace}};

			public unsafe partial class {{c.Name}} : {{c.BaseType.Name}} {

			""";
			for (var i = 0; i < members.Length; i++) {
				var member = members[i];
				code += $$"""
					static Native.PropertyInfo __{{member.Name}}Info = new Native.PropertyInfo() {
						type = (uint)Variant.Type.{{TypeToVariantType(member.Type.Name)}},
						name = (byte*)Marshal.StringToHGlobalAnsi("{{member.Name}}"),
						class_name = null,
						hint = (uint)PropertyHint.PROPERTY_HINT_NONE,
						hint_string = null,
						usage = (uint)PropertyUsageFlags.PROPERTY_USAGE_DEFAULT,
					};


				""";
			}

			code += $$"""
				static unsafe void RegisterExports() {
					Native.ExtensionClassMethodInfo info;

			""";

			for (var i = 0; i < members.Length; i++) {
				var member = members[i];
				code += $$"""
						info = new Native.ExtensionClassMethodInfo() {
							name = (byte*)Marshal.StringToHGlobalAnsi("{{member.SetMethod.Name}}"),
							method_userdata = new IntPtr({{i * 2}}),
							call_func = new(CallFunc),
							ptrcall_func = new(CallFuncPtr),
							method_flags = (uint)Native.ExtensionClassMethodFlags.Default,
							argument_count = 1,
							has_return_value = false,
							get_argument_type_func = new(ArgumentType),
							get_argument_info_func = new(ArgumentInfo),
							get_argument_metadata_func = new(ArgumentMetadata),
							default_argument_count = 0,
							default_arguments = null,
						};
						Native.gdInterface.classdb_register_extension_class_method.Call(Native.gdLibrary, "{{c.Name}}", &info);

						info = new Native.ExtensionClassMethodInfo() {
							name = (byte*)Marshal.StringToHGlobalAnsi("{{member.GetMethod.Name}}"),
							method_userdata = new IntPtr({{i * 2 + 1}}),
							call_func = new(CallFunc),
							ptrcall_func = new(CallFuncPtr),
							method_flags = (uint)Native.ExtensionClassMethodFlags.Default,
							argument_count = 0,
							has_return_value = true,
							get_argument_type_func = new(ArgumentType),
							get_argument_info_func = new(ArgumentInfo),
							get_argument_metadata_func = new(ArgumentMetadata),
							default_argument_count = 0,
							default_arguments = null,
						};
						Native.gdInterface.classdb_register_extension_class_method.Call(Native.gdLibrary, "{{c.Name}}", &info);

						fixed (Native.PropertyInfo* infoPtr = &__{{member.Name}}Info) {
							Native.gdInterface.classdb_register_extension_class_property.Call(
								Native.gdLibrary,
								"{{c.Name}}",
								infoPtr,
								"{{member.SetMethod.Name}}",
								"{{member.GetMethod.Name}}"
							);
						}

				""";
			}

			code += $$"""
				}

				static void CallFuncPtr(IntPtr method_userdata, Native.GDExtensionClassInstancePtr p_instance, Native.TypePtr* p_args, Native.TypePtr r_ret) {
					throw new NotImplementedException();
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
			for (var i = 0; i < members.Length; i++) {
				var member = members[i];
				var vt = TypeToVariantType(member.Type.Name);
				code += $$"""
						case {{i * 2 + 0}}:
							instance.{{member.Name}} = Variant.InteropGetFromPointer<{{member.Type.Name}}>(p_args[0].data, Variant.Type.{{vt}});
							break;
						case {{i * 2 + 1}}:
							Variant.InteropSaveIntoPointer(instance.{{member.Name}}, r_return.data, Variant.Type.{{vt}});
							break;

				""";
			}

			code += $$"""
					}
				}

				static Variant.Type ArgumentType(IntPtr p_method_userdata, int p_argument) {
					return (int)p_method_userdata switch {

			""";
			for (var i = 0; i < members.Length; i++) {
				var member = members[i];
				var t = member.Type.ToString();
				var vt = TypeToVariantType(member.Type.Name);
				code += $$"""
						{{i * 2 + 0}} => p_argument switch {
							0 => Variant.Type.{{vt}},
							_ => Variant.Type.Nil,
						},
						{{i * 2 + 1}} => p_argument switch {
							-1 => Variant.Type.{{vt}},
							_ => Variant.Type.Nil,
						},

				""";
			}
			code += $$"""
						_ => Variant.Type.Nil,
					};
				}

				static void ArgumentInfo(IntPtr p_method_userdata, int p_argument, Native.PropertyInfo* r_info) {
					*r_info = (int)p_method_userdata switch {

			""";
			for (var i = 0; i < members.Length; i++) {
				var member = members[i];
				var t = member.Type.ToString();
				var vt = TypeToVariantType(member.Type.Name);
				code += $$"""
						{{i * 2 + 0}} => p_argument switch {
							0 => __{{member.Name}}Info,
							_ => default,
						},
						{{i * 2 + 1}} => p_argument switch {
							-1 => __{{member.Name}}Info,
							_ => default,
						},

				""";
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

			context.AddSource($"{c.Name}.export.gen.cs", code);
		}

		static string TypeToVariantType(string t) {
			return t switch {
				"Double" => "Float",
				"Long" => "Int",
				_ => "Todo throw error",
			};
		}
	}
}
