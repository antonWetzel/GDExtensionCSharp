namespace GDExtension;

public unsafe class Variant {

	public static Variant InteropFromPointer(IntPtr data) => new Variant(data);
	public static IntPtr InteropToPointer(Variant variant) => variant.data;
	public static void InteropSaveIntoPointer(bool value, IntPtr data) => Initialization.inter.get_variant_from_type_constructor(VariantType.Bool)(data, new IntPtr(&value));
	public static void InteropSaveIntoPointer(long value, IntPtr data) => Initialization.inter.get_variant_from_type_constructor(VariantType.Int)(data, new IntPtr(&value));
	public static void InteropSaveIntoPointer(Vector2i value, IntPtr data) => Initialization.inter.get_variant_from_type_constructor(VariantType.Vector2i)(data, new IntPtr(&value));
	public static void InteropSaveIntoPointer(String value, IntPtr data) => Initialization.inter.get_variant_from_type_constructor(VariantType.String)(data, new IntPtr(&value));
	public static void InteropSaveIntoPointer(StringName value, IntPtr data) => Initialization.inter.get_variant_from_type_constructor(VariantType.StringName)(data, new IntPtr(&value));

	IntPtr data;

	public VariantType type => Initialization.inter.variant_get_type(data);

	private Variant(IntPtr data) => this.data = data;
	private Variant() => data = Initialization.inter.mem_alloc(24);

	public Variant(bool value) : this() => InteropSaveIntoPointer(value, data);
	public Variant(long value) : this() => InteropSaveIntoPointer(value, data);
	public Variant(Vector2i value) : this() => InteropSaveIntoPointer(value, data);
	public Variant(String value) : this() => InteropSaveIntoPointer(value, data);
	public Variant(StringName value) : this() => InteropSaveIntoPointer(value, data);

	public long AsInt() {
		if (type != VariantType.Int) {
			throw new Exception($"{type}");
		}
		long v;
		Initialization.inter.get_variant_to_type_constructor(VariantType.Int)(new IntPtr(&v), data);
		return v;
	}
}
