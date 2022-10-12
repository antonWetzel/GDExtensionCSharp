namespace GDExtension;

public unsafe static class GD {


	public static unsafe void Print(params Variant[] variants) {
		fixed (byte* ptr = System.Text.Encoding.UTF8.GetBytes("print")) {
			var print = Initialization.inter.variant_get_ptr_utility_function(ptr, 2648703342);
			var args = stackalloc IntPtr[variants.Length];
			for (var i = 0; i < variants.Length; i++) {
				args[i] = variants[i]._internal_pointer;
			}
			print(IntPtr.Zero, args, variants.Length);
		}
	}
}
