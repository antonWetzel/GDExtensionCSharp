namespace GDExtension;

public unsafe class Wrapped {

	public static ObjectPtr InteropToPointer(Wrapped wrapped) => wrapped._internal_pointer;

	public ObjectPtr _internal_pointer;

	protected Wrapped(ObjectPtr data) => this._internal_pointer = data;

	//to lazy to check if the base class is custom or builtin so create this as base to always exist
	public static unsafe void __Notification(Native.GDExtensionClassInstancePtr instance, int what) { /* pass */ }
}
