namespace GDExtension;

public unsafe partial class PackedScene : Resource {
	public T Instantiate<T>(PackedScene.GenEditState edit_state) where T : Wrapped, new() {
		var ptr = Instantiate(edit_state)._internal_pointer;
		var t = new T();
		t._internal_pointer = ptr;
		return t;
	}
}
