namespace GDExtension;

public unsafe partial class Node : Object {

	public T GetNode<T>(NodePath path) where T : Wrapped, new() {
		var ptr = GetNode(path)._internal_pointer;
		var t = new T();
		t._internal_pointer = ptr;
		return t;
	}
}
