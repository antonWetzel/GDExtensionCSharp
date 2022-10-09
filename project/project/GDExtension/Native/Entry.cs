using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace GDExtension.Native;

public static class Entry {

	public static Interface @interface;
	public static ExtensionClassLibraryPtr library;

	[UnmanagedCallersOnly(EntryPoint = "gd_extension_entry", CallConvs = new[] { typeof(CallConvCdecl) })]
	public static unsafe bool _(Interface* @interface, ExtensionClassLibraryPtr library, Initialization* init) {
		Entry.@interface = *@interface;
		Entry.library = library;

		*init = new Initialization() {
			minimum_initialization_level = InitializationLevel.Editor,
			initialize = &Initialize,
			deinitialize = &Deinitialize,
		};

		return true;
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	public static unsafe void Initialize(void* userdata, InitializationLevel level) {

		switch (level) {
		case Native.InitializationLevel.Core:
			break;
		case Native.InitializationLevel.Servers:
			break;
		case Native.InitializationLevel.Scene:
			break;
		case Native.InitializationLevel.Editor:
			Register.Test();
			break;
		}
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	public static unsafe void Deinitialize(void* userdata, InitializationLevel level) {

	}


}
