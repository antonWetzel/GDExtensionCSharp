namespace GDExtension;

public unsafe static class StringMarshall {

	public static IntPtr ToNative(string managed) {
		var x = new StringPtr(gdInterface.mem_alloc.Call(8));
		gdInterface.string_new_with_utf16_chars.Call(x, managed);
		return x.data;
	}

	public static string ToManaged(IntPtr native) {
		var str = new StringPtr(new IntPtr(&native));
		var l = (int)gdInterface.string_to_utf16_chars.Call(str, null, 0).value;
		var b = new System.Text.StringBuilder(l);
		gdInterface.string_to_utf16_chars.Call(str, b, l);
		return b.ToString();
	}
}
