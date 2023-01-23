//genereated with ClangSharpPInvokeGenerator and edited

namespace GDExtension.Native;

public unsafe partial struct GDExtensionInterface {

	public static GDExtensionInterface gdInterface;
	public static void* gdLibrary;

	public string versionString => Marshal.PtrToStringAnsi(new IntPtr(version_string))!;

	public void* MoveToUnmanaged<T>(T value) where T : unmanaged {
		var ptr = (T*)mem_alloc((nuint)sizeof(T));
		*ptr = value;
		return ptr;
	}
}
