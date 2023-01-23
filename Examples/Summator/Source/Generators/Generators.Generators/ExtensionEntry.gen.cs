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
			Summator.Summator.Register();
			break;
		case GDExtensionInitializationLevel.GDEXTENSION_INITIALIZATION_EDITOR:
			GDExtension.Register.RegisterEditor();
			break;
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
			GDExtensionInterface.gdInterface.classdb_unregister_extension_class(GDExtensionInterface.gdLibrary, Summator.Summator.__godot_name._internal_pointer);
			break;
		case GDExtensionInitializationLevel.GDEXTENSION_INITIALIZATION_EDITOR:
			break;
		}
	}
}