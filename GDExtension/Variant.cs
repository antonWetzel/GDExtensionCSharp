namespace GDExtension;

public sealed unsafe partial class Variant {

	struct Constructor {
		public delegate* unmanaged[Cdecl]<void*, void*, void> fromType;
		public delegate* unmanaged[Cdecl]<void*, void*, void> toType;
	}

	static Constructor[] constructors = new Constructor[(int)Type.Max];

	public static void SaveIntoPointer(Variant variant, void* ptr) {
		*(void**)ptr = variant._internal_pointer;
		GC.SuppressFinalize(variant);
	}

	public static void Register() {
		for (var i = 1; i < (int)Type.Max; i++) {
			constructors[i] = new Constructor() {
				fromType = gdInterface.get_variant_from_type_constructor((GDExtensionVariantType)i),
				toType = gdInterface.get_variant_to_type_constructor((GDExtensionVariantType)i),
			};
		}
	}

	public static void SaveIntoPointer(Object value, void* ptr) {
		var objectPtr = value != null ? value._internal_pointer : null;
		constructors[(int)Variant.Type.Object].fromType(ptr, &objectPtr);
	}

	public static Object GetObjectFromPointer(void* ptr) {
		void* res;
		constructors[(int)Type.Object].toType(&res, ptr);
		return Object.ConstructUnknown(res);
	}

	internal void* _internal_pointer;

	public Type type => (Type)gdInterface.variant_get_type(_internal_pointer);

	private Variant() => _internal_pointer = gdInterface.mem_alloc(24);

	internal Variant(void* data) {
		_internal_pointer = data;
		GC.SuppressFinalize(this);
	}

	public static Variant Nil {
		get { var v = new Variant(); gdInterface.variant_new_nil(v._internal_pointer); return v; }
	}

	public Variant(int value) : this((long)value) { }
	public Variant(float value) : this((double)value) { }

	~Variant() {
		gdInterface.variant_destroy(_internal_pointer);
		gdInterface.mem_free(_internal_pointer);
	}
}
