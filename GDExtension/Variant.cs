namespace GDExtension;

public sealed unsafe partial class Variant {

	struct Constructor {
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void> fromType;
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void> toType;
	}

	static Constructor[] constructors = new Constructor[(int)Type.Max];

	public static void Register() {
		for (var i = 1; i < (int)Type.Max; i++) {
			constructors[i] = new Constructor() {
				fromType = gdInterface.get_variant_from_type_constructor((Type)i),
				toType = gdInterface.get_variant_to_type_constructor((Type)i),
			};
		}
	}

	public static void SaveIntoPointer(Object value, IntPtr ptr) {
		var objectPtr = value != null ? value._internal_pointer : IntPtr.Zero;
		constructors[(int)Variant.Type.Object].fromType(ptr, new IntPtr(&objectPtr));
	}

	public static Object GetObjectFromPointer(IntPtr ptr) {
		IntPtr res;
		constructors[(int)Type.Object].toType(new IntPtr(&res), ptr);
		return Object.ConstructUnknown(res);
	}

	internal IntPtr _internal_pointer;

	public Variant.Type type => gdInterface.variant_get_type(_internal_pointer);

	private Variant() => _internal_pointer = gdInterface.mem_alloc(24);

	internal Variant(IntPtr data) {
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
