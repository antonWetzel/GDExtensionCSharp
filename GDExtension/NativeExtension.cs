namespace GDExtension;

public static partial class Native {

	public unsafe partial struct Interface {

		public string versionString => Marshal.PtrToStringAnsi(new IntPtr(version_string))!;

		public IntPtr MoveToUnmanaged<T>(T value) where T : unmanaged {
			var ptr = (T*)mem_alloc((nuint)sizeof(T));
			*ptr = value;
			return new IntPtr(ptr);
		}
	}
}
