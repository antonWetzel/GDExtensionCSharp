namespace GDExtension;

public unsafe partial struct StringName {

	public static implicit operator StringName(string text) => new StringName(text);

	public static implicit operator string(StringName from) {
		var constructor = gdInterface.variant_get_ptr_constructor.Call(VariantType.String, 2);
		var args = stackalloc TypePtr[1];
		args[0] = new IntPtr(&from);
		IntPtr res;
		constructor(new IntPtr(&res), args);
		return StringMarshall.ToManaged(res);
	}
}
