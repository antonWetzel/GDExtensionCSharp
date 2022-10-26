namespace GDExtension;

public unsafe partial struct Array {

	public Variant this[int index] {
		get => this[(long)index];
	}

	public Variant this[long index] {
		get {
			VariantPtr res;
			fixed (Array* ptr = &this) {
				res = gdInterface.array_operator_index.Call(new IntPtr(ptr), index);
			}
			return new Variant(res.data);
		}
	}
}
