namespace GDExtension;

public abstract unsafe class Wrapped {

	public static IntPtr InteropToPointer(Wrapped variant) => variant._internal_pointer;

	public IntPtr _internal_pointer;

	protected Wrapped(IntPtr data) => this._internal_pointer = data;
}
