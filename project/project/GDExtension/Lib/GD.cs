namespace GDExtension.Lib;

static class GD {

	public static unsafe void Print(params Variant[] a) {
		delegate* unmanaged[Cdecl]<Native.TypePtr, Native.TypePtr*, int, void> print;
		fixed (byte* ptr = System.Text.Encoding.UTF8.GetBytes("print")) {
			print = Native.Entry.@interface.variant_get_ptr_utility_function(ptr, 2648703342);
		}
		var m = new Native.TypePtr[a.Length];
		for (var i = 0; i < a.Length; i++) {
			m[i] = (void*)a[i];
		}
		fixed (Native.TypePtr* ptr = m) {
			print(null, ptr, a.Length);
		}
	}
}
