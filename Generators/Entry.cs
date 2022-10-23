using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Generators {

	public static class Entry {

		public static void Execute(GeneratorExecutionContext context, List<(string, string)> classes) {
			var registrations = "";
			var unregistrations = "";
			while (classes.Count > 0) {
				for (var i = 0; i < classes.Count; i++) {
					var b = classes[i].Item2;
					var valid = true;
					foreach (var o in classes) {
						if (o.Item1 == b) {
							valid = false;
							break;
						}
					}
					if (valid) {
						var n = classes[i].Item1;
						registrations += $"{n}.Register();\n\t\t\t";
						unregistrations = $"Native.gdInterface.classdb_unregister_extension_class.Call(Native.gdLibrary, \"{classes[i].Item1}\");\n\t\t\t" + unregistrations;
						classes[i] = classes.Last();
						classes.RemoveAt(classes.Count - 1);
						break;
					}
				}
			}

			var source = $$"""
			using System;
			using System.Runtime.CompilerServices;
			using System.Runtime.InteropServices;
			using GDExtension;

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
						break;
					case Native.InitializationLevel.Editor:
						{{registrations}}break;
					}
				}

				public static unsafe void Deinitialize(IntPtr userdata, Native.InitializationLevel level) {
					switch (level) {
					case Native.InitializationLevel.Core:
						break;
					case Native.InitializationLevel.Servers:
						break;
					case Native.InitializationLevel.Scene:
						break;
					case Native.InitializationLevel.Editor:
						{{unregistrations}}break;
					}
				}
			}
			""";
			context.AddSource("ExtensionEntry.gen.cs", source);
		}
	}
}
