namespace GDExtension;

public unsafe class Wrapped {

	public IntPtr _internal_pointer;

	protected Wrapped(string type) {
		var ptr = Marshal.StringToHGlobalAnsi(type);
		_internal_pointer = gdInterface.classdb_construct_object((sbyte*)ptr);
		Marshal.FreeHGlobal(hglobal: ptr);
	}
	protected Wrapped(IntPtr data) => _internal_pointer = data;

	//to lazy to check if the base class is custom or builtin so create this as base to always exist
	public static unsafe void __Notification(IntPtr instance, int what) { /* pass */ }
	public static void Register() { }
}
