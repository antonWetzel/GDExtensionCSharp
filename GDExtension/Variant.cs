namespace GDExtension;

public unsafe class Variant {

	public static void InteropSaveIntoPointer(bool value, IntPtr _internal_pointer) => gdInterface.get_variant_from_type_constructor.Call(VariantType.Bool)(_internal_pointer, new IntPtr(&value));
	public static void InteropSaveIntoPointer(double value, IntPtr _internal_pointer) => gdInterface.get_variant_from_type_constructor.Call(VariantType.Float)(_internal_pointer, new IntPtr(&value));
	public static void InteropSaveIntoPointer(long value, IntPtr _internal_pointer) => gdInterface.get_variant_from_type_constructor.Call(VariantType.Int)(_internal_pointer, new IntPtr(&value));
	public static void InteropSaveIntoPointer(Vector2i value, IntPtr _internal_pointer) => gdInterface.get_variant_from_type_constructor.Call(VariantType.Vector2i)(_internal_pointer, new IntPtr(&value));
	public static void InteropSaveIntoPointer(string value, IntPtr _internal_pointer) => gdInterface.get_variant_from_type_constructor.Call(VariantType.String)(_internal_pointer, StringMarshall.ToNative(value));
	public static void InteropSaveIntoPointer(StringName value, IntPtr _internal_pointer) => gdInterface.get_variant_from_type_constructor.Call(VariantType.StringName)(_internal_pointer, new IntPtr(&value));
	public static void InteropSaveIntoPointer(Vector3 value, IntPtr _internal_pointer) => gdInterface.get_variant_from_type_constructor.Call(VariantType.Vector3)(_internal_pointer, new IntPtr(&value));
	public static void InteropSaveIntoPointer(Object value, IntPtr _internal_pointer) {
		var ptr = value._internal_pointer;
		gdInterface.get_variant_from_type_constructor.Call(VariantType.Object)(_internal_pointer, new IntPtr(&ptr));
	}

	internal IntPtr _internal_pointer;

	private Variant() => _internal_pointer = gdInterface.mem_alloc.Call(24);
	public Variant(IntPtr data) => _internal_pointer = data;

	public Variant(bool value) : this() => InteropSaveIntoPointer(value, _internal_pointer);
	public Variant(double value) : this() => InteropSaveIntoPointer(value, _internal_pointer);
	public Variant(long value) : this() => InteropSaveIntoPointer(value, _internal_pointer);
	public Variant(Vector2i value) : this() => InteropSaveIntoPointer(value, _internal_pointer);
	public Variant(string value) : this() => InteropSaveIntoPointer(value, _internal_pointer);
	public Variant(StringName value) : this() => InteropSaveIntoPointer(value, _internal_pointer);
	public Variant(Object value) : this() => InteropSaveIntoPointer(value, _internal_pointer);
	public Variant(Vector3 value) : this() => InteropSaveIntoPointer(value, _internal_pointer);

	public Variant(int value) : this((long)value) { }
	public Variant(float value) : this((double)value) { }

	public long AsInt() {
		if (type != VariantType.Int) {
			throw new Exception($"{type}");
		}
		long v;
		gdInterface.get_variant_to_type_constructor.Call(VariantType.Int)(new IntPtr(&v), _internal_pointer);
		return v;
	}

	public VariantType type => gdInterface.variant_get_type.Call(_internal_pointer);

	public static implicit operator Variant(int value) => new Variant(value);
	public static implicit operator Variant(long value) => new Variant(value);
	public static implicit operator Variant(float value) => new Variant(value);
	public static implicit operator Variant(double value) => new Variant(value);
	public static implicit operator Variant(string value) => new Variant(value);
}
