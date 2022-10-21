using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generators {

	public static class Export {

		public static void Generate(GeneratorExecutionContext context, INamedTypeSymbol c) {
			var members = c.GetMembers().
				Where(x => x is IFieldSymbol).
				Select(x => (IFieldSymbol)x).
				Where(x => x.GetAttributes().Where(x => x.AttributeClass.ToString() == "GDExtension.ExportAttribute").Count() > 0)
				.ToArray();

			var code = $$"""
			using System.Runtime.CompilerServices;
			using System.Runtime.InteropServices;

			namespace {{c.ContainingNamespace}};

			public unsafe partial class {{c.Name}} : {{c.BaseType.Name}} {

			""";

			code += $$"""
				public static unsafe Native.PropertyInfo* GetPropertyList(Native.GDExtensionClassInstancePtr instance, uint* count) {
					var ptr = (Native.PropertyInfo*)Native.gdInterface.mem_alloc.Call((nuint)sizeof(Native.PropertyInfo) * {{members.Length}});
					*count = {{members.Length}};

			""";

			for (var i = 0; i < members.Length; i++) {
				var member = members[i];
				code += $$"""
						ptr[{{i}}] = new Native.PropertyInfo() {
							type = (uint)Native.VariantType.{{TypeToVariantType(member.Type.ToString())}},
							name = (byte*)Marshal.StringToHGlobalAnsi("{{member.Name}}"),
							class_name = null,
							hint = (uint)PropertyHint.PROPERTY_HINT_NONE,
							hint_string = null,
							usage = (uint)PropertyUsageFlags.PROPERTY_USAGE_DEFAULT,
						};

				""";
			}

			code += $$"""
					return ptr;
				}

				public static unsafe void FreePropertyList(Native.GDExtensionClassInstancePtr instance, Native.PropertyInfo* infos) {
					for (var i = 0; i < {{members.Length}}; i++) {
						var info = infos[i];
						Marshal.FreeHGlobal(new IntPtr(info.name));
					}
					Native.gdInterface.mem_free.Call(new IntPtr(infos));
				}

				public static unsafe Native.Bool SetFunc(Native.GDExtensionClassInstancePtr instance, StringName* name, Native.VariantPtr varPtr) {
					var inst = ({{c.Name}})instance;
					switch ((string)*name) {

			""";

			foreach (var member in members) {
				var t = member.Type.ToString();
				code += $$"""
						case "{{member.Name}}":
							inst.{{member.Name}} = Variant.InteropGetFromPointer<{{t}}>(varPtr.data, Native.VariantType.{{TypeToVariantType(t)}});
							return true;

				""";
			}


			code += $$"""
					default:
						return false;
					}
				}

				public static unsafe Native.Bool GetFunc(Native.GDExtensionClassInstancePtr instance, StringName* name, Native.VariantPtr variant) {
					var inst = ({{c.Name}})instance;
					switch ((string)*name) {

			""";

			foreach (var member in members) {
				var t = member.Type.ToString();
				code += $$"""
						case "{{member.Name}}":
							Variant.InteropSaveIntoPointer(inst.{{member.Name}}, variant.data, Native.VariantType.{{TypeToVariantType(t)}});
							return true;

				""";
			}

			code += $$"""
					default:
						return false;
					}
				}
			}

			""";

			context.AddSource($"{c.Name}.export.gen.cs", code);
		}

		static string TypeToVariantType(string t) {
			return t switch {
				"double" => "Float",
				"long" => "Int",
				_ => "Todo throw error",
			};
		}
	}
}
