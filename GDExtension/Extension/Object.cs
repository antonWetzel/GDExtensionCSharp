namespace GDExtension;

public partial class Object {
	public static Object ConstructUnknown(ObjectPtr ptr) {
		if (ptr.data == IntPtr.Zero) { return null!; }
		var o = new Object(ptr);
		var c = o.GetClass();
		return constructors[c](ptr);
	}

	public delegate Object Constructor(ObjectPtr data);
	public static void RegisterConstructor(string name, Constructor constructor) => constructors.Add(name, constructor);

	static System.Collections.Generic.Dictionary<string, Constructor> constructors = new();
}
