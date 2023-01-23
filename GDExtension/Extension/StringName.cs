namespace GDExtension;

public unsafe partial class StringName {

	public static implicit operator StringName(string text) => new StringName(text);

	public static implicit operator string(StringName from) {
		var constructor = gdInterface.variant_get_ptr_constructor((GDExtensionVariantType)Variant.Type.String, 2);
		var args = stackalloc void*[1];
		args[0] = from._internal_pointer;
		void* res;
		constructor(&res, args);
		return StringMarshall.ToManaged(&res);
	}
}
