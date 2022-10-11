using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ExampleGame;

public static class ExtensionEntry {

	[UnmanagedCallersOnly(EntryPoint = "gd_extension_entry", CallConvs = new[] { typeof(CallConvCdecl) })]
	public static unsafe bool EntryPoint(Interface* @interface, ExtensionClassLibraryPtr library, Initialization* init) {
		Initialization.inter = *@interface;
		Initialization.lib = library;

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
			TestClass.Register();
			break;
		}
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	public static unsafe void Deinitialize(IntPtr userdata, InitializationLevel level) {

	}
}
