
namespace GDExtension;

public unsafe class Variant {

	IntPtr data;

	public VariantType type => Entry.@interface.variant_get_type(this);

	private Variant() => data = Entry.@interface.mem_alloc(24);
	private Variant(IntPtr data) => this.data = data;

	public Variant(string value) : this() {
		var constructor = Entry.@interface.get_variant_from_type_constructor(VariantType.String);
		constructor(this, new IntPtr(InternalString.Create(value)));
	}

	public Variant(long value) : this() {
		var constructor = Entry.@interface.get_variant_from_type_constructor(VariantType.Int);
		constructor(this, new IntPtr(&value));
	}

	public Variant(bool value) : this() {
		var constructor = Entry.@interface.get_variant_from_type_constructor(VariantType.Bool);
		constructor(this, new IntPtr(&value));
	}

	~Variant() {
		Entry.@interface.variant_destroy(this);
	}

	public long AsInt() {
		if (type != VariantType.Int) {
			Console.WriteLine(type);
			throw new Exception();
		}
		var constructor = Entry.@interface.get_variant_to_type_constructor(VariantType.Int);
		long v;
		constructor(this, new IntPtr(&v));
		return v;
	}

	public unsafe static implicit operator VariantPtr(Variant variant) => variant.data;
	public unsafe static implicit operator Variant(VariantPtr data) => new Variant(data);
}
