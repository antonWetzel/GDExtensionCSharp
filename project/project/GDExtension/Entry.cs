using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace GDExtension;

public static class Entry {
	[UnmanagedCallersOnly(EntryPoint = "gd_extension_entry", CallConvs = new[] { typeof(CallConvCdecl) })]
	public static unsafe bool _(IntPtr inter, IntPtr library, Initialization* init) {
		*init = new Initialization() {
			minimumInitializationLevel = InitializationLevel.Editor,
			//deinitialize = &Deinitialize,
			//initialize = &Initialize,
		};
		return true;
	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	public static unsafe void Initialize(IntPtr userdata, InitializationLevel level) {

	}

	[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
	public static unsafe void Deinitialize(IntPtr userdata, InitializationLevel level) {

	}
}
