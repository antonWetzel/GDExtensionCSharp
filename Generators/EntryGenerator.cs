using Microsoft.CodeAnalysis;

namespace Generators {
	[Generator]
	public class EntryGenerator : ISourceGenerator {

		public void Execute(GeneratorExecutionContext context) {
			var registrations = "TestClass.Register();\n";
			var source = $$"""
				using System.Runtime.CompilerServices;
				using System.Runtime.InteropServices;

				namespace ExampleGame;

				public static class ExtensionEntry {

					[UnmanagedCallersOnly(EntryPoint = "gd_extension_entry", CallConvs = new[] { typeof(CallConvCdecl) })]
					public static unsafe bool EntryPoint(Native.Interface @interface, Native.ExtensionClassLibraryPtr library, Native.Initialization* init) {
						Native.gdInterface = @interface;
						Native.gdLibrary = library;

						*init = new Native.Initialization() {
							minimum_initialization_level = Native.InitializationLevel.Scene,
							initialize = new(Initialize),
							deinitialize = new(Deinitialize),
						};

						return true;
					}

					public static unsafe void Initialize(IntPtr userdata, Native.InitializationLevel level) {
						switch (level) {
						case Native.InitializationLevel.Core:
							break;
						case Native.InitializationLevel.Servers:
							break;
						case Native.InitializationLevel.Scene:
							{{registrations}}
							break;
						case Native.InitializationLevel.Editor:
							break;
						}
					}

					public static unsafe void Deinitialize(IntPtr userdata, Native.InitializationLevel level) {

					}
				}
			""";
			context.AddSource("entry.gen.cs", source);
		}

		public void Initialize(GeneratorInitializationContext context) {
			//todo: see what classes must be registered
		}
	}
}