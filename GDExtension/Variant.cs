namespace GDExtension;

public unsafe class Variant {

	public static void InteropSaveIntoPointer<T>(T value, IntPtr _internal_pointer, VariantType t) where T : unmanaged {
		gdInterface.get_variant_from_type_constructor.Call(t)(_internal_pointer, new IntPtr(&value));
	}
	public static void InteropSaveIntoPointer(string value, IntPtr _internal_pointer) {
		gdInterface.get_variant_from_type_constructor.Call(VariantType.String)(_internal_pointer, StringMarshall.ToNative(value));
	}

	public static T InteropGetFromPointer<T>(IntPtr _internal_pointer, VariantType t) where T : unmanaged {
		var rT = gdInterface.variant_get_type.Call(_internal_pointer);
		if (rT != t) {
			throw new Exception($"variant contains {rT}, tried to get {t}");
		}
		T res;
		gdInterface.get_variant_to_type_constructor.Call(t)(new IntPtr(&res), _internal_pointer);
		return res;
	}

	internal IntPtr _internal_pointer;

	private Variant() => _internal_pointer = gdInterface.mem_alloc.Call(24);
	internal Variant(IntPtr data) => _internal_pointer = data;

	public static Variant Nil {
		get { var v = new Variant(); gdInterface.variant_new_nil.Call(v._internal_pointer); return v; }
	}

	public Variant(bool value) : this() => InteropSaveIntoPointer(value, _internal_pointer, VariantType.Bool);
	public Variant(long value) : this() => InteropSaveIntoPointer(value, _internal_pointer, VariantType.Int);
	public Variant(double value) : this() => InteropSaveIntoPointer(value, _internal_pointer, VariantType.Float);
	public Variant(string value) : this() => InteropSaveIntoPointer(value, _internal_pointer);

	public Variant(Vector2 value) : this() => InteropSaveIntoPointer(value, _internal_pointer, VariantType.Vector2);
	public Variant(Vector2i value) : this() => InteropSaveIntoPointer(value, _internal_pointer, VariantType.Vector2i);
	public Variant(Rect2 value) : this() => InteropSaveIntoPointer(value, _internal_pointer, VariantType.Rect2);
	public Variant(Rect2i value) : this() => InteropSaveIntoPointer(value, _internal_pointer, VariantType.Rect2i);
	public Variant(Vector3 value) : this() => InteropSaveIntoPointer(value, _internal_pointer, VariantType.Vector3);
	public Variant(Vector3i value) : this() => InteropSaveIntoPointer(value, _internal_pointer, VariantType.Vector3i);
	public Variant(Transform2D value) : this() => InteropSaveIntoPointer(value, _internal_pointer, VariantType.Transform2D);
	public Variant(Vector4 value) : this() => InteropSaveIntoPointer(value, _internal_pointer, VariantType.Vector4);
	public Variant(Vector4i value) : this() => InteropSaveIntoPointer(value, _internal_pointer, VariantType.Vector4);
	public Variant(Plane value) : this() => InteropSaveIntoPointer(value, _internal_pointer, VariantType.Plane);
	public Variant(Quaternion value) : this() => InteropSaveIntoPointer(value, _internal_pointer, VariantType.Quaternion);
	public Variant(AABB value) : this() => InteropSaveIntoPointer(value, _internal_pointer, VariantType.AABB);
	public Variant(Basis value) : this() => InteropSaveIntoPointer(value, _internal_pointer, VariantType.Basis);
	public Variant(Transform3D value) : this() => InteropSaveIntoPointer(value, _internal_pointer, VariantType.Transform3D);
	public Variant(Projection value) : this() => InteropSaveIntoPointer(value, _internal_pointer, VariantType.Projection);

	public Variant(Color value) : this() => InteropSaveIntoPointer(value, _internal_pointer, VariantType.Color);
	public Variant(StringName value) : this() => InteropSaveIntoPointer(value, _internal_pointer, VariantType.StringName);
	public Variant(NodePath value) : this() => InteropSaveIntoPointer(value, _internal_pointer, VariantType.NodePath);
	public Variant(RID value) : this() => InteropSaveIntoPointer(value, _internal_pointer, VariantType.RID);
	public Variant(Object value) : this() => InteropSaveIntoPointer(value._internal_pointer, _internal_pointer, VariantType.Object);
	public Variant(Callable value) : this() => InteropSaveIntoPointer(value, _internal_pointer, VariantType.Callable);
	public Variant(Signal value) : this() => InteropSaveIntoPointer(value, _internal_pointer, VariantType.Signal);
	public Variant(Dictionary value) : this() => InteropSaveIntoPointer(value, _internal_pointer, VariantType.Dictionary);
	public Variant(Array value) : this() => InteropSaveIntoPointer(value, _internal_pointer, VariantType.Array);

	public Variant(PackedByteArray value) : this() => InteropSaveIntoPointer(value, _internal_pointer, VariantType.PackedByteArray);
	public Variant(PackedInt32Array value) : this() => InteropSaveIntoPointer(value, _internal_pointer, VariantType.PackedInt32Array);
	public Variant(PackedInt64Array value) : this() => InteropSaveIntoPointer(value, _internal_pointer, VariantType.PackedInt64Array);
	public Variant(PackedFloat32Array value) : this() => InteropSaveIntoPointer(value, _internal_pointer, VariantType.PackedFloat32Array);
	public Variant(PackedFloat64Array value) : this() => InteropSaveIntoPointer(value, _internal_pointer, VariantType.PackedFloat64Array);
	public Variant(PackedStringArray value) : this() => InteropSaveIntoPointer(value, _internal_pointer, VariantType.PackedStringArray);
	public Variant(PackedVector2Array value) : this() => InteropSaveIntoPointer(value, _internal_pointer, VariantType.PackedVector2Array);
	public Variant(PackedVector3Array value) : this() => InteropSaveIntoPointer(value, _internal_pointer, VariantType.PackedVector3Array);
	public Variant(PackedColorArray value) : this() => InteropSaveIntoPointer(value, _internal_pointer, VariantType.PackedColorArray);

	public Variant(int value) : this((long)value) { }
	public Variant(float value) : this((double)value) { }

	public long AsInt() => InteropGetFromPointer<long>(_internal_pointer, VariantType.Int);
	public double AsFloat() => InteropGetFromPointer<double>(_internal_pointer, VariantType.Float);

	public VariantType type => gdInterface.variant_get_type.Call(_internal_pointer);

	public static implicit operator Variant(int value) => new Variant(value);
	public static implicit operator Variant(long value) => new Variant(value);
	public static implicit operator Variant(float value) => new Variant(value);
	public static implicit operator Variant(double value) => new Variant(value);
	public static implicit operator Variant(string value) => new Variant(value);

	~Variant() {
		gdInterface.variant_destroy.Call(_internal_pointer);
		gdInterface.mem_free.Call(_internal_pointer);
	}
}
