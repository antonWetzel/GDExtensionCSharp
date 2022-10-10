using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace GDExtension;

public static class Entry {

	public static Interface @interface;
	public static ExtensionClassLibraryPtr library;

	[UnmanagedCallersOnly(EntryPoint = "gd_extension_entry", CallConvs = new[] { typeof(CallConvCdecl) })]
	public static unsafe bool _(Interface* @interface, ExtensionClassLibraryPtr library, Initialization* init) {
		Entry.@interface = *@interface;
		Entry.library = library;

		*init = new Initialization() {
			minimum_initialization_level = InitializationLevel.Scene,
			initialize = &Initialize,
			deinitialize = &Deinitialize,
		};
		return true;
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	public static unsafe void Initialize(IntPtr userdata, InitializationLevel level) {

		switch (level) {
		case InitializationLevel.Core:
			break;
		case InitializationLevel.Servers:
			break;
		case InitializationLevel.Scene:
			break;
		case InitializationLevel.Editor:
			Register.Test();
			break;
		}
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	public static unsafe void Deinitialize(IntPtr userdata, InitializationLevel level) {

	}


}
