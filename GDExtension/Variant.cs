namespace GDExtension;

[StructLayout(LayoutKind.Explicit, Size = 24)]
public unsafe struct Variant {

	public VariantType type {
		get {
			fixed (Variant* ptr = &this) {
				return Initialization.inter.variant_get_type(new IntPtr(ptr));
			}
		}
	}

	public Variant() {
		var constructor = Initialization.inter.get_variant_from_type_constructor(VariantType.Nil);
		fixed (Variant* ptr = &this) {
			constructor(new IntPtr(ptr), IntPtr.Zero);
		}
	}

	public Variant(long value) {
		var constructor = Initialization.inter.get_variant_from_type_constructor(VariantType.Int);
		fixed (Variant* ptr = &this) {
			constructor(new IntPtr(ptr), new IntPtr(&value));
		}
	}

	public Variant(bool value) {
		var constructor = Initialization.inter.get_variant_from_type_constructor(VariantType.Bool);
		fixed (Variant* ptr = &this) {
			constructor(new IntPtr(ptr), new IntPtr(&value));
		}
	}

	public Variant(String value) {
		var constructor = Initialization.inter.get_variant_from_type_constructor(VariantType.String);
		fixed (Variant* ptr = &this) {
			constructor(new IntPtr(ptr), new IntPtr(&value));
		}
	}

	public Variant(StringName value) {
		var constructor = Initialization.inter.get_variant_from_type_constructor(VariantType.StringName);
		fixed (Variant* ptr = &this) {
			constructor(new IntPtr(ptr), new IntPtr(&value));
		}
	}

	public long AsInt() {
		if (type != VariantType.Int) {
			Console.WriteLine(type);
			throw new Exception();
		}
		var constructor = Initialization.inter.get_variant_to_type_constructor(VariantType.Int);
		long v;
		fixed (Variant* ptr = &this) {
			constructor(new IntPtr(&v), new IntPtr(ptr));
		}
		return v;
	}
}
