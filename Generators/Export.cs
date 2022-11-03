using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generators {

	public static class Export {

		public static void Generate(GeneratorExecutionContext context, INamedTypeSymbol c, Methods methods) {
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

				static unsafe void RegisterExports() {

			""";

			for (var i = 0; i < members.Length; i++) {
				var member = members[i];

				code += $"\t\tvar __{member.Name}Info = " + Methods.CreatePropertyInfo(member.Type, member.Name);

				methods.AddMethod(new Methods.Info() {
					name = member.SetMethod.Name,
					arguments = new (ITypeSymbol, string)[] { (member.Type, "value") },
					ret = null,
					property = member.Name,
				});
				methods.AddMethod(new Methods.Info() {
					name = member.GetMethod.Name,
					arguments = new (ITypeSymbol, string)[] { },
					ret = member.Type,
					property = member.Name,
				});
				code += $$"""
						Native.gdInterface.classdb_register_extension_class_property.Call(
							Native.gdLibrary,
							"{{c.Name}}",
							&__{{member.Name}}Info,
							"{{member.SetMethod.Name}}",
							"{{member.GetMethod.Name}}"
						);

						if (__{{member.Name}}Info.hint_string != null) {
							Marshal.FreeHGlobal((IntPtr)__{{member.Name}}Info.hint_string);
						}
						Marshal.FreeHGlobal((IntPtr)__{{member.Name}}Info.name);

				""";
			}
			code += """
				}
			}

			""";
			context.AddSource($"{c.Name}.export.gen.cs", code);
		}
	}
}
