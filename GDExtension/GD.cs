namespace GDExtension;

public static class GD {

	public static unsafe void Print(Variant variant) {
		delegate* unmanaged[Cdecl]<TypePtr, TypePtr*, int, void> print;
		fixed (byte* ptr = System.Text.Encoding.UTF8.GetBytes("print")) {
			print = Initialization.inter.variant_get_ptr_utility_function(ptr, 2648703342);
		}

		var args = stackalloc IntPtr[1];
		args[0] = new IntPtr(&variant);
		print(IntPtr.Zero, args, 1);
	}
}
