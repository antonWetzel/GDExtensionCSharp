namespace GDExtension;

public unsafe class Wrapped {

	public IntPtr _internal_pointer;

	protected Wrapped(StringName type) {
		_internal_pointer = gdInterface.classdb_construct_object(type._internal_pointer);
	}
	protected Wrapped(IntPtr data) => _internal_pointer = data;

	//to lazy to check if the base class is custom or builtin so create this as base to always exist
	public static unsafe void __Notification(IntPtr instance, int what) { /* pass */ }
	public static void Register() { }
}
