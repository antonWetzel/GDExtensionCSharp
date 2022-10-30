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

			""";
			for (var i = 0; i < members.Length; i++) {
				var member = members[i];
				code += $"static Native.PropertyInfo __{member.Name}Info = " + Methods.CreatePropertyInfo(member.Type, member.Name);
			}

			code += $$"""
				static unsafe void RegisterExports() {
					Native.ExtensionClassMethodInfo info;

			""";

			for (var i = 0; i < members.Length; i++) {
				var member = members[i];
				methods.AddMethod(new Methods.Info() {
					name = member.SetMethod.Name,
					arguments = new (ITypeSymbol, string)[] { (member.Type, "value") },
					ret = null,
					property = member.Name,
				});
				methods.AddMethod(new Methods.Info() {
					name = member.GetMethod.Name,
					arguments = new (ITypeSymbol, string)[] { },
					ret = member.Type.Name,
					property = member.Name,
				});
				code += $$"""
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
			code += """
				}
			}

			""";
			context.AddSource($"{c.Name}.export.gen.cs", code);
		}
	}
}
