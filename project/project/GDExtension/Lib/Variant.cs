using System.Runtime.InteropServices;

namespace GDExtension.Lib;

public unsafe class Variant {

	void* data;

	public Variant() {
		data = Native.Entry.@interface.mem_alloc(24);
	}

	public Variant(string value) : this() {
		var constructor = Native.Entry.@interface.get_variant_from_type_constructor(Native.VariantType.String);
		Native.StringPtr text;
		fixed (char* ptr = value) {
			Native.Entry.@interface.string_new_with_utf16_bytes(&text, ptr);
		}
		constructor((void*)this, &text);
		Native.Entry.@interface.mem_free(text);
	}

	public Variant(long value) : this() {
		var constructor = Native.Entry.@interface.get_variant_from_type_constructor(Native.VariantType.Int);
		constructor((void*)this, &value);
	}

	public Variant(bool value) : this() {
		var constructor = Native.Entry.@interface.get_variant_from_type_constructor(Native.VariantType.Bool);
		constructor((void*)this, &value);
	}

	public static implicit operator void*(Variant variant) => variant.data;
}
