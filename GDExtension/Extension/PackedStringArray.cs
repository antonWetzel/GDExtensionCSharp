namespace GDExtension;

public unsafe partial struct PackedStringArray {

	public string this[int index] {
		get => this[(long)index];
	}

	public string this[long index] {
		get {
			StringPtr res;
			fixed (PackedStringArray* ptr = &this) {
				res = gdInterface.packed_string_array_operator_index.Call(new IntPtr(ptr), index);
			}
			return StringMarshall.ToManaged(res);
		}
	}
}
