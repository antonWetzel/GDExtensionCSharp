namespace GDExtension;

public unsafe partial class StringName {

	public static implicit operator StringName(string text) => new StringName(text);

	public static implicit operator string(StringName from) {
		var constructor = gdInterface.variant_get_ptr_constructor(Variant.Type.String, 2);
		var args = stackalloc IntPtr[1];
		args[0] = from._internal_pointer;
		IntPtr res;
		constructor(new IntPtr(&res), args);
		return StringMarshall.ToManaged(res);
	}
}
