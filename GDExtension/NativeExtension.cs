namespace GDExtension;

public static partial class Native {

	public unsafe partial struct Interface {

		public string versionString => Marshal.PtrToStringAnsi(new IntPtr(version_string))!;
	}
}
