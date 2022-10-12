namespace GDExtension;

public unsafe partial struct StringName {

	public static implicit operator StringName(string text) => new StringName(new String(text));
	public static implicit operator StringName(String text) => new StringName(text);
}
