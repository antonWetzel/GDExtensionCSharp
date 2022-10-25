using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generators {

	public static class Signal {

		public static void Generate(GeneratorExecutionContext context, INamedTypeSymbol c) {
			var events = c.GetMembers().
				Where(x => x is IEventSymbol).
				Select(x => (IEventSymbol)x).
				Where(x => x.GetAttributes().Where(x => x.AttributeClass.ToString() == "GDExtension.SignalAttribute").Count() > 0)
				.ToArray();

			var code = $$"""
			using System.Runtime.CompilerServices;
			using System.Runtime.InteropServices;

			namespace {{c.ContainingNamespace}};

			public unsafe partial class {{c.Name}} : {{c.BaseType.Name}} {

				static unsafe void RegisterSignals() {

			""";
			for (var i = 0; i < events.Length; i++) {
				var ev = events[i];

				code += $$"""
						var infos = stackalloc Native.PropertyInfo[0];
						Native.gdInterface.classdb_register_extension_class_signal.Call(Native.gdLibrary, "{{c.Name}}", "{{ev.Name}}", infos, 0);

				""";
			}
			code += $$"""
				}
			}

			""";
			context.AddSource($"{c.Name}.signal.gen.cs", code);
		}

		static string TypeToVariantType(string t) {
			return t switch {
				"Double" => "Float",
				"Int64" => "Int",
				_ => throw new System.Exception($"unknwon type '{t}' for variant"),
			};
		}
	}
}
