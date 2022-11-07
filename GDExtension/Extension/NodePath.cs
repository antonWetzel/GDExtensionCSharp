namespace GDExtension;

public unsafe partial class NodePath {

	public static implicit operator NodePath(string text) => new NodePath(text);

	public static implicit operator string(NodePath from) {
		var constructor = gdInterface.variant_get_ptr_constructor.Call(Variant.Type.String, 3);
		var args = stackalloc TypePtr[1];
		args[0] = from._internal_pointer;
		IntPtr res;
		constructor(new IntPtr(&res), args);
		return StringMarshall.ToManaged(res);
	}
}
