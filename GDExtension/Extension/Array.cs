namespace GDExtension;

public unsafe partial class Array {

	public Variant this[int index] {
		get => this[(long)index];
	}

	public Variant this[long index] {
		get {
			VariantPtr res;
			res = gdInterface.array_operator_index.Call(_internal_pointer, index);
			return new Variant(res.data);
		}
	}
}
