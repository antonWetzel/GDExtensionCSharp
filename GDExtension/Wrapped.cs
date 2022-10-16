namespace GDExtension;

public abstract unsafe class Wrapped {

	public static ObjectPtr InteropToPointer(Wrapped wrapped) => wrapped._internal_pointer;

	public ObjectPtr _internal_pointer;

	protected Wrapped(ObjectPtr data) => this._internal_pointer = data;
}
