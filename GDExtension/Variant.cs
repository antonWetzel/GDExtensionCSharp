namespace GDExtension;

public unsafe class Variant {

	public static void InteropSaveIntoPointer(bool value, IntPtr _internal_pointer) => Initialization.inter.get_variant_from_type_constructor(VariantType.Bool)(_internal_pointer, new IntPtr(&value));
	public static void InteropSaveIntoPointer(double value, IntPtr _internal_pointer) => Initialization.inter.get_variant_from_type_constructor(VariantType.Float)(_internal_pointer, new IntPtr(&value));
	public static void InteropSaveIntoPointer(long value, IntPtr _internal_pointer) => Initialization.inter.get_variant_from_type_constructor(VariantType.Int)(_internal_pointer, new IntPtr(&value));
	public static void InteropSaveIntoPointer(Vector2i value, IntPtr _internal_pointer) => Initialization.inter.get_variant_from_type_constructor(VariantType.Vector2i)(_internal_pointer, new IntPtr(&value));
	public static void InteropSaveIntoPointer(String value, IntPtr _internal_pointer) => Initialization.inter.get_variant_from_type_constructor(VariantType.String)(_internal_pointer, new IntPtr(&value));
	public static void InteropSaveIntoPointer(StringName value, IntPtr _internal_pointer) => Initialization.inter.get_variant_from_type_constructor(VariantType.StringName)(_internal_pointer, new IntPtr(&value));
	public static void InteropSaveIntoPointer(Vector3 value, IntPtr _internal_pointer) => Initialization.inter.get_variant_from_type_constructor(VariantType.Vector3)(_internal_pointer, new IntPtr(&value));

	public static void InteropSaveIntoPointer(Object value, IntPtr _internal_pointer) {
		fixed (IntPtr* ptr = &value._internal_pointer) {
			Initialization.inter.get_variant_from_type_constructor(VariantType.Object)(_internal_pointer, new IntPtr(ptr));
		}
	}


	internal IntPtr _internal_pointer;

	public VariantType type => Initialization.inter.variant_get_type(_internal_pointer);

	private Variant() => _internal_pointer = Initialization.inter.mem_alloc(24);
	public Variant(IntPtr data) => _internal_pointer = data;

	public Variant(bool value) : this() => InteropSaveIntoPointer(value, _internal_pointer);
	public Variant(double value) : this() => InteropSaveIntoPointer(value, _internal_pointer);
	public Variant(long value) : this() => InteropSaveIntoPointer(value, _internal_pointer);
	public Variant(Vector2i value) : this() => InteropSaveIntoPointer(value, _internal_pointer);
	public Variant(String value) : this() => InteropSaveIntoPointer(value, _internal_pointer);
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
		Initialization.inter.get_variant_to_type_constructor(VariantType.Int)(new IntPtr(&v), _internal_pointer);
		return v;
	}
}
