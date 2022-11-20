using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Generators {

	public static class Entry {

		public static void Execute(GeneratorExecutionContext context, List<Register.Data> classes) {
			var registrations = "";
			var editorRegistrations = "";
			var unregistrations = "";
			while (classes.Count > 0) {
				for (var i = 0; i < classes.Count; i++) {
					var b = classes[i].@base;
					var valid = true;
					foreach (var o in classes) {
						if (o.name == b) {
							valid = false;
							break;
						}
					}
					if (valid) {
						var n = classes[i];
						switch (n.level) {
						case Register.Level.Scene:
							registrations += $"{n.@namespace}.{n.name}.Register();\n\t\t\t";
							break;
						case Register.Level.Editor:
							editorRegistrations += $"{n.@namespace}.{n.name}.Register();\n\t\t\t";
							break;
						}
						unregistrations = $"Native.gdInterface.classdb_unregister_extension_class(Native.gdLibrary, {n.@namespace}.{n.name}.__godot_name._internal_pointer);\n\t\t\t" + unregistrations;
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

			public static class ExtensionEntry {

				[UnmanagedCallersOnly(EntryPoint = "gd_extension_entry", CallConvs = new[] { typeof(CallConvCdecl) })]
				public static unsafe bool EntryPoint(Native.Interface @interface, IntPtr library, Native.Initialization* init) {
					Native.gdInterface = @interface;
					Native.gdLibrary = library;

					*init = new Native.Initialization() {
						minimum_initialization_level = Native.InitializationLevel.Scene,
						initialize = &Initialize,
						deinitialize = &Deinitialize,
					};

					return true;
				}

				[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
				public static unsafe void Initialize(IntPtr userdata, Native.InitializationLevel level) {
					switch (level) {
					case Native.InitializationLevel.Core:
						break;
					case Native.InitializationLevel.Servers:
						break;
					case Native.InitializationLevel.Scene:
						GDExtension.Register.RegisterBuiltin();
						GDExtension.Register.RegisterUtility();
						GDExtension.Register.RegisterCore();
						{{registrations}}break;
					case Native.InitializationLevel.Editor:
						GDExtension.Register.RegisterEditor();
						{{editorRegistrations}}break;
					}
				}

				[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
				public static unsafe void Deinitialize(IntPtr userdata, Native.InitializationLevel level) {
					switch (level) {
					case Native.InitializationLevel.Core:
						break;
					case Native.InitializationLevel.Servers:
						break;
					case Native.InitializationLevel.Scene:
						{{unregistrations}}break;
					case Native.InitializationLevel.Editor:
						break;
					}
				}
			}
			""";
			context.AddSource("ExtensionEntry.gen.cs", source);
		}
	}
}
