namespace GDExtension;

static class GD {

	public static unsafe void Print(params Variant[] a) {
		delegate* unmanaged[Cdecl]<TypePtr, TypePtr*, int, void> print;
		fixed (byte* ptr = System.Text.Encoding.UTF8.GetBytes("print")) {
			print = Entry.@interface.variant_get_ptr_utility_function(ptr, 2648703342);
		}
		var m = new TypePtr[a.Length];
		for (var i = 0; i < a.Length; i++) {
			m[i] = a[i];
		}
		fixed (TypePtr* ptr = m) {
			print(IntPtr.Zero, ptr, a.Length);
		}
	}
}
