using System.Runtime.InteropServices;

namespace GDExtension;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct Initialization {
	public InitializationLevel minimumInitializationLevel;
	public IntPtr userdata;
	public delegate* unmanaged[Cdecl]<IntPtr, InitializationLevel, void> initialize;
	public delegate* unmanaged[Cdecl]<IntPtr, InitializationLevel, void> deinitialize;
}
