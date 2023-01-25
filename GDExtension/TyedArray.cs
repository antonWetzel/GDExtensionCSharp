namespace GDExtension;

//try to set 'SetTyped' based on C# generics
public unsafe class TypedArray<T> : Array {
	public TypedArray(Variant.Type type) : base() {
		SetTyped((long)type, "", new Script(null));
	}
	public TypedArray(Variant.Type type, void* ptr) : base(ptr) {
		SetTyped((long)type, "", new Script(null));
	}
	public TypedArray(StringName name) : base() {
		SetTyped((long)Variant.Type.Nil, name, new Script(null));
	}
	public TypedArray(StringName name, void* ptr) : base(ptr) {
		SetTyped((long)Variant.Type.Nil, name, new Script(null));
	}
}
