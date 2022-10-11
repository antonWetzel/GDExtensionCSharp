namespace GDExtension;

public unsafe partial struct String {

	public String(string text) {
		fixed (char* ptr = text) {
			fixed (String* loc = &this) {
				Initialization.inter.string_new_with_utf16_bytes(loc, ptr);
			}
		}
	}
}
