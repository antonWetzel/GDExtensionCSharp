namespace GDExtension;

public unsafe partial struct NodePath {


	public static implicit operator string(NodePath from) {
		var constructor = gdInterface.variant_get_ptr_constructor.Call(Variant.Type.String, 3);
		var args = stackalloc TypePtr[1];
		args[0] = new IntPtr(&from);
		IntPtr res;
		constructor(new IntPtr(&res), args);
		return StringMarshall.ToManaged(res);
	}
}
