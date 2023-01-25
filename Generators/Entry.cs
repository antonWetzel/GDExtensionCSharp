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
						unregistrations = $"GDExtensionInterface.gdInterface.classdb_unregister_extension_class(GDExtensionInterface.gdLibrary, {n.@namespace}.{n.name}.__godot_name._internal_pointer);\n\t\t\t" + unregistrations;
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
			using GDExtension.Native;

			public static class ExtensionEntry {

				[UnmanagedCallersOnly(EntryPoint = "gd_extension_entry", CallConvs = new[] { typeof(CallConvCdecl) })]
				public static unsafe bool EntryPoint(GDExtensionInterface @interface, void* library, GDExtensionInitialization* init) {
					GDExtensionInterface.gdInterface = @interface;
					GDExtensionInterface.gdLibrary = library;

					*init = new GDExtensionInitialization() {
						minimum_initialization_level = GDExtensionInitializationLevel.GDEXTENSION_INITIALIZATION_SCENE,
						initialize = &Initialize,
						deinitialize = &Deinitialize,
					};

					return true;
				}

				[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
				public static unsafe void Initialize(void* userdata, GDExtensionInitializationLevel level) {
					switch (level) {
					case GDExtensionInitializationLevel.GDEXTENSION_INITIALIZATION_CORE:
						break;
					case GDExtensionInitializationLevel.GDEXTENSION_INITIALIZATION_SERVERS:
						break;
					case GDExtensionInitializationLevel.GDEXTENSION_INITIALIZATION_SCENE:
						GDExtension.Register.RegisterBuiltin();
						GDExtension.Register.RegisterUtility();
						GDExtension.Register.RegisterCore();
						{{registrations}}
						DodgeTheCreeps.Test.Register();
						break;
					case GDExtensionInitializationLevel.GDEXTENSION_INITIALIZATION_EDITOR:
						GDExtension.Register.RegisterEditor();
						{{editorRegistrations}}break;
					}
				}

				[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
				public static unsafe void Deinitialize(void* userdata, GDExtensionInitializationLevel level) {
					switch (level) {
					case GDExtensionInitializationLevel.GDEXTENSION_INITIALIZATION_CORE:
						break;
					case GDExtensionInitializationLevel.GDEXTENSION_INITIALIZATION_SERVERS:
						break;
					case GDExtensionInitializationLevel.GDEXTENSION_INITIALIZATION_SCENE:
						{{unregistrations}}break;
					case GDExtensionInitializationLevel.GDEXTENSION_INITIALIZATION_EDITOR:
						break;
					}
				}
			}
			""";
			context.AddSource("ExtensionEntry.gen.cs", source);
		}
	}
}
