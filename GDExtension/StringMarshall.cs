namespace GDExtension;

public unsafe static class StringMarshall {

	public static IntPtr ToNative(string managed) {
		var x = gdInterface.mem_alloc(8);
		fixed (char* ptr = managed) {
			gdInterface.string_new_with_utf16_chars(x, ptr);
		}
		return x;
	}

	public static string ToManaged(IntPtr str) {
		var l = (int)gdInterface.string_to_utf16_chars(str, null, 0);
		var span = (Span<char>)stackalloc char[l];
		fixed (char* ptr = span) {
			gdInterface.string_to_utf16_chars(str, ptr, l);
		}
		return new string(span);
	}
}
