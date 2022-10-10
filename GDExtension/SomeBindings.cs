namespace GDExtension;

static unsafe class SomeBindings {

	static delegate* unmanaged[Cdecl]<IntPtr, IntPtr*, void> stringNameConstructorWithString;

	public static void Register() {
		stringNameConstructorWithString = Entry.@interface.variant_get_ptr_constructor(VariantType.StringName, 2);
	}

	public static StringPtr Create()
}
