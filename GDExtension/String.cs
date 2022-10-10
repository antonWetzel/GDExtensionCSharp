namespace GDExtension;

[StructLayout(LayoutKind.Sequential, Size = 8)]
public unsafe struct InternalString {

	public InternalString() {
		throw new Exception();
	}

	public static InternalString* Create(string value) {
		InternalString text;
		fixed (char* ptr = value) {
			Entry.@interface.string_new_with_utf16_bytes(&text, ptr);
		}
		return &text;
	}

	public static string Create(InternalString* value) {
		var length = Entry.@interface.string_to_utf16_bytes(value, null, 0);
		var chars = new char[length];
		fixed (char* ptr = chars) {
			Entry.@interface.string_to_utf16_bytes(value, ptr, length);
		}
		return new String(chars);
	}
}
