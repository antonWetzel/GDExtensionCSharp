namespace GDExtension;

public sealed unsafe class Variant {

	public enum Type {
		Nil,

		/*  atomic types */
		Bool,
		Int,
		Float,
		String,

		/* math types */
		Vector2,
		Vector2i,
		Rect2,
		Rect2i,
		Vector3,
		Vector3i,
		Transform2D,
		Vector4,
		Vector4i,
		Plane,
		Quaternion,
		AABB,
		Basis,
		Transform3D,
		Projection,

		/* misc types */
		Color,
		StringName,
		NodePath,
		RID,
		Object,
		Callable,
		Signal,
		Dictionary,
		Array,

		/* typed arrays */
		PackedByteArray,
		PackedInt32Array,
		PackedInt64Array,
		PackedFloat32Array,
		PackedFloat64Array,
		PackedStringArray,
		PackedVector2Array,
		PackedVector3Array,
		PackedColorArray,

		MAX
	}

	public enum Operator {
		/* comparison */
		Equal,
		NotEqual,
		Less,
		LessEqual,
		Greater,
		GreaterEqual,
		/* mathematic */
		Add,
		Subtract,
		Multiply,
		Divide,
		Negate,
		Positive,
		Module,
		Power,
		/* bitwise */
		ShiftLeft,
		ShiftRight,
		BitAnd,
		BitOr,
		BitXor,
		BitNegate,
		/* logic */
		And,
		Or,
		Xor,
		Not,
		/* containment */
		In,
		MAX
	}
	struct Constructor {
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void> fromType;
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void> toType;
	}

	static Constructor[] constructors = new Constructor[(int)Type.MAX];

	public static void Register() {
		for (var i = 1; i < (int)Type.MAX; i++) {
			constructors[i] = new Constructor() {
				fromType = gdInterface.get_variant_from_type_constructor((Type)i),
				toType = gdInterface.get_variant_to_type_constructor((Type)i),
			};
		}
	}

	public static void SaveIntoPointer(bool value, IntPtr ptr) => constructors[(int)Variant.Type.Bool].fromType(ptr, new IntPtr(&value));
	public static void SaveIntoPointer(long value, IntPtr ptr) => constructors[(int)Variant.Type.Int].fromType(ptr, new IntPtr(&value));
	public static void SaveIntoPointer(double value, IntPtr ptr) => constructors[(int)Variant.Type.Float].fromType(ptr, new IntPtr(&value));
	public static void SaveIntoPointer(string value, IntPtr ptr) => constructors[(int)Variant.Type.String].fromType(ptr, StringMarshall.ToNative(value));
	public static void SaveIntoPointer(Vector2 value, IntPtr ptr) => constructors[(int)Variant.Type.Vector2].fromType(ptr, new IntPtr(&value));
	public static void SaveIntoPointer(Vector2i value, IntPtr ptr) => constructors[(int)Variant.Type.Vector2i].fromType(ptr, new IntPtr(&value));
	public static void SaveIntoPointer(Rect2 value, IntPtr ptr) => constructors[(int)Variant.Type.Rect2].fromType(ptr, new IntPtr(&value));
	public static void SaveIntoPointer(Rect2i value, IntPtr ptr) => constructors[(int)Variant.Type.Rect2i].fromType(ptr, new IntPtr(&value));
	public static void SaveIntoPointer(Vector3 value, IntPtr ptr) => constructors[(int)Variant.Type.Vector3].fromType(ptr, new IntPtr(&value));
	public static void SaveIntoPointer(Vector3i value, IntPtr ptr) => constructors[(int)Variant.Type.Vector3i].fromType(ptr, new IntPtr(&value));
	public static void SaveIntoPointer(Transform2D value, IntPtr ptr) => constructors[(int)Variant.Type.Transform2D].fromType(ptr, new IntPtr(&value));
	public static void SaveIntoPointer(Vector4 value, IntPtr ptr) => constructors[(int)Variant.Type.Vector4].fromType(ptr, new IntPtr(&value));
	public static void SaveIntoPointer(Vector4i value, IntPtr ptr) => constructors[(int)Variant.Type.Vector4i].fromType(ptr, new IntPtr(&value));
	public static void SaveIntoPointer(Plane value, IntPtr ptr) => constructors[(int)Variant.Type.Plane].fromType(ptr, new IntPtr(&value));
	public static void SaveIntoPointer(Quaternion value, IntPtr ptr) => constructors[(int)Variant.Type.Quaternion].fromType(ptr, new IntPtr(&value));
	public static void SaveIntoPointer(AABB value, IntPtr ptr) => constructors[(int)Variant.Type.AABB].fromType(ptr, new IntPtr(&value));
	public static void SaveIntoPointer(Basis value, IntPtr ptr) => constructors[(int)Variant.Type.Basis].fromType(ptr, new IntPtr(&value));
	public static void SaveIntoPointer(Transform3D value, IntPtr ptr) => constructors[(int)Variant.Type.Transform3D].fromType(ptr, new IntPtr(&value));
	public static void SaveIntoPointer(Projection value, IntPtr ptr) => constructors[(int)Variant.Type.Projection].fromType(ptr, new IntPtr(&value));
	public static void SaveIntoPointer(Color value, IntPtr ptr) => constructors[(int)Variant.Type.Color].fromType(ptr, new IntPtr(&value));
	public static void SaveIntoPointer(StringName value, IntPtr ptr) => constructors[(int)Variant.Type.StringName].fromType(ptr, value._internal_pointer);
	public static void SaveIntoPointer(NodePath value, IntPtr ptr) => constructors[(int)Variant.Type.NodePath].fromType(ptr, value._internal_pointer);
	public static void SaveIntoPointer(RID value, IntPtr ptr) => constructors[(int)Variant.Type.RID].fromType(ptr, new IntPtr(&value));
	public static void SaveIntoPointer(Object value, IntPtr ptr) {
		var objectPtr = value != null ? value._internal_pointer : IntPtr.Zero;
		constructors[(int)Variant.Type.Object].fromType(ptr, new IntPtr(&objectPtr));
	}
	public static void SaveIntoPointer(Callable value, IntPtr ptr) => constructors[(int)Variant.Type.Callable].fromType(ptr, value._internal_pointer);
	public static void SaveIntoPointer(Signal value, IntPtr ptr) => constructors[(int)Variant.Type.Signal].fromType(ptr, value._internal_pointer);
	public static void SaveIntoPointer(Dictionary value, IntPtr ptr) => constructors[(int)Variant.Type.Dictionary].fromType(ptr, value._internal_pointer);
	public static void SaveIntoPointer(Array value, IntPtr ptr) => constructors[(int)Variant.Type.Array].fromType(ptr, value._internal_pointer);
	public static void SaveIntoPointer(PackedByteArray value, IntPtr ptr) => constructors[(int)Variant.Type.PackedByteArray].fromType(ptr, value._internal_pointer);
	public static void SaveIntoPointer(PackedInt32Array value, IntPtr ptr) => constructors[(int)Variant.Type.PackedInt32Array].fromType(ptr, value._internal_pointer);
	public static void SaveIntoPointer(PackedInt64Array value, IntPtr ptr) => constructors[(int)Variant.Type.PackedInt64Array].fromType(ptr, value._internal_pointer);
	public static void SaveIntoPointer(PackedFloat32Array value, IntPtr ptr) => constructors[(int)Variant.Type.PackedFloat32Array].fromType(ptr, value._internal_pointer);
	public static void SaveIntoPointer(PackedFloat64Array value, IntPtr ptr) => constructors[(int)Variant.Type.PackedFloat64Array].fromType(ptr, value._internal_pointer);
	public static void SaveIntoPointer(PackedStringArray value, IntPtr ptr) => constructors[(int)Variant.Type.PackedStringArray].fromType(ptr, value._internal_pointer);
	public static void SaveIntoPointer(PackedVector2Array value, IntPtr ptr) => constructors[(int)Variant.Type.PackedVector2Array].fromType(ptr, value._internal_pointer);
	public static void SaveIntoPointer(PackedVector3Array value, IntPtr ptr) => constructors[(int)Variant.Type.PackedVector3Array].fromType(ptr, value._internal_pointer);
	public static void SaveIntoPointer(PackedColorArray value, IntPtr ptr) => constructors[(int)Variant.Type.PackedColorArray].fromType(ptr, value._internal_pointer);

	public static T InteropGetFromPointer<T>(IntPtr _internal_pointer, Variant.Type t) where T : unmanaged {
		var rT = gdInterface.variant_get_type(_internal_pointer);
		if (rT == Type.Nil) {
			return default; //probably bad idea
		}
		if (rT != t) {
			throw new Exception($"variant contains {rT}, tried to get {t}");
		}
		T res;
		constructors[(int)t].toType(new IntPtr(&res), _internal_pointer);
		return res;
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

	public Variant(bool value) : this() => SaveIntoPointer(value, _internal_pointer);
	public Variant(long value) : this() => SaveIntoPointer(value, _internal_pointer);
	public Variant(double value) : this() => SaveIntoPointer(value, _internal_pointer);
	public Variant(string value) : this() => SaveIntoPointer(value, _internal_pointer);
	public Variant(Vector2 value) : this() => SaveIntoPointer(value, _internal_pointer);
	public Variant(Vector2i value) : this() => SaveIntoPointer(value, _internal_pointer);
	public Variant(Rect2 value) : this() => SaveIntoPointer(value, _internal_pointer);
	public Variant(Rect2i value) : this() => SaveIntoPointer(value, _internal_pointer);
	public Variant(Vector3 value) : this() => SaveIntoPointer(value, _internal_pointer);
	public Variant(Vector3i value) : this() => SaveIntoPointer(value, _internal_pointer);
	public Variant(Transform2D value) : this() => SaveIntoPointer(value, _internal_pointer);
	public Variant(Vector4 value) : this() => SaveIntoPointer(value, _internal_pointer);
	public Variant(Vector4i value) : this() => SaveIntoPointer(value, _internal_pointer);
	public Variant(Plane value) : this() => SaveIntoPointer(value, _internal_pointer);
	public Variant(Quaternion value) : this() => SaveIntoPointer(value, _internal_pointer);
	public Variant(AABB value) : this() => SaveIntoPointer(value, _internal_pointer);
	public Variant(Basis value) : this() => SaveIntoPointer(value, _internal_pointer);
	public Variant(Transform3D value) : this() => SaveIntoPointer(value, _internal_pointer);
	public Variant(Projection value) : this() => SaveIntoPointer(value, _internal_pointer);
	public Variant(Color value) : this() => SaveIntoPointer(value, _internal_pointer);
	public Variant(StringName value) : this() => SaveIntoPointer(value, _internal_pointer);
	public Variant(NodePath value) : this() => SaveIntoPointer(value, _internal_pointer);
	public Variant(RID value) : this() => SaveIntoPointer(value, _internal_pointer);
	public Variant(Object value) : this() => SaveIntoPointer(value, _internal_pointer);
	public Variant(Callable value) : this() => SaveIntoPointer(value, _internal_pointer);
	public Variant(Signal value) : this() => SaveIntoPointer(value, _internal_pointer);
	public Variant(Dictionary value) : this() => SaveIntoPointer(value, _internal_pointer);
	public Variant(Array value) : this() => SaveIntoPointer(value, _internal_pointer);
	public Variant(PackedByteArray value) : this() => SaveIntoPointer(value, _internal_pointer);
	public Variant(PackedInt32Array value) : this() => SaveIntoPointer(value, _internal_pointer);
	public Variant(PackedInt64Array value) : this() => SaveIntoPointer(value, _internal_pointer);
	public Variant(PackedFloat32Array value) : this() => SaveIntoPointer(value, _internal_pointer);
	public Variant(PackedFloat64Array value) : this() => SaveIntoPointer(value, _internal_pointer);
	public Variant(PackedStringArray value) : this() => SaveIntoPointer(value, _internal_pointer);
	public Variant(PackedVector2Array value) : this() => SaveIntoPointer(value, _internal_pointer);
	public Variant(PackedVector3Array value) : this() => SaveIntoPointer(value, _internal_pointer);
	public Variant(PackedColorArray value) : this() => SaveIntoPointer(value, _internal_pointer);

	public Variant(int value) : this((long)value) { }
	public Variant(float value) : this((double)value) { }

	public bool AsBool() => InteropGetFromPointer<bool>(_internal_pointer, Variant.Type.Bool);
	public long AsInt() => InteropGetFromPointer<long>(_internal_pointer, Variant.Type.Int);
	public double AsFloat() => InteropGetFromPointer<double>(_internal_pointer, Variant.Type.Float);
	public string AsString() => StringMarshall.ToManaged(InteropGetFromPointer<IntPtr>(_internal_pointer, Variant.Type.String));
	public Vector2 AsVector2() => InteropGetFromPointer<Vector2>(_internal_pointer, Variant.Type.Vector2);
	public Vector2i AsVector2i() => InteropGetFromPointer<Vector2i>(_internal_pointer, Variant.Type.Vector2i);
	public Rect2 AsRect2() => InteropGetFromPointer<Rect2>(_internal_pointer, Variant.Type.Rect2);
	public Rect2i AsRect2i() => InteropGetFromPointer<Rect2i>(_internal_pointer, Variant.Type.Rect2i);
	public Vector3 AsVector3() => InteropGetFromPointer<Vector3>(_internal_pointer, Variant.Type.Vector3);
	public Vector3i AsVector3i() => InteropGetFromPointer<Vector3i>(_internal_pointer, Variant.Type.Vector3i);
	public Transform2D AsTransform2D() => InteropGetFromPointer<Transform2D>(_internal_pointer, Variant.Type.Transform2D);
	public Vector4 AsVector4() => InteropGetFromPointer<Vector4>(_internal_pointer, Variant.Type.Vector4);
	public Vector4i AsVector4i() => InteropGetFromPointer<Vector4i>(_internal_pointer, Variant.Type.Vector4i);
	public Plane AsPlane() => InteropGetFromPointer<Plane>(_internal_pointer, Variant.Type.Plane);
	public Quaternion AsQuaternion() => InteropGetFromPointer<Quaternion>(_internal_pointer, Variant.Type.Quaternion);
	public AABB AsAABB() => InteropGetFromPointer<AABB>(_internal_pointer, Variant.Type.AABB);
	public Basis AsBasis() => InteropGetFromPointer<Basis>(_internal_pointer, Variant.Type.Basis);
	public Transform3D AsTransform3D() => InteropGetFromPointer<Transform3D>(_internal_pointer, Variant.Type.Transform3D);
	public Projection AsProjection() => InteropGetFromPointer<Projection>(_internal_pointer, Variant.Type.Projection);
	public Color AsColor() => InteropGetFromPointer<Color>(_internal_pointer, Variant.Type.Color);
	public StringName AsStringName() => new StringName(InteropGetFromPointer<IntPtr>(_internal_pointer, Variant.Type.StringName));
	public NodePath AsNodePath() => new NodePath(InteropGetFromPointer<IntPtr>(_internal_pointer, Variant.Type.NodePath));
	public RID AsRID() => InteropGetFromPointer<RID>(_internal_pointer, Variant.Type.RID);
	public Object AsObject() => Object.ConstructUnknown(InteropGetFromPointer<IntPtr>(_internal_pointer, Variant.Type.Object));
	public Callable AsCallable() => new Callable(InteropGetFromPointer<IntPtr>(_internal_pointer, Variant.Type.Callable));
	public Signal AsSignal() => new Signal(InteropGetFromPointer<IntPtr>(_internal_pointer, Variant.Type.Signal));
	public Dictionary AsDictionary() => new Dictionary(InteropGetFromPointer<IntPtr>(_internal_pointer, Variant.Type.Dictionary));
	public Array AsArray() => new Array(InteropGetFromPointer<IntPtr>(_internal_pointer, Variant.Type.Array));
	public PackedByteArray AsPackedByteArray() => new PackedByteArray(InteropGetFromPointer<IntPtr>(_internal_pointer, Variant.Type.PackedByteArray));
	public PackedInt32Array AsPackedInt32Array() => new PackedInt32Array(InteropGetFromPointer<IntPtr>(_internal_pointer, Variant.Type.PackedInt32Array));
	public PackedInt64Array AsPackedInt64Array() => new PackedInt64Array(InteropGetFromPointer<IntPtr>(_internal_pointer, Variant.Type.PackedInt64Array));
	public PackedFloat32Array AsPackedFloat32Array() => new PackedFloat32Array(InteropGetFromPointer<IntPtr>(_internal_pointer, Variant.Type.PackedFloat32Array));
	public PackedFloat64Array AsPackedFloat64Array() => new PackedFloat64Array(InteropGetFromPointer<IntPtr>(_internal_pointer, Variant.Type.PackedFloat64Array));
	public PackedStringArray AsPackedStringArray() => new PackedStringArray(InteropGetFromPointer<IntPtr>(_internal_pointer, Variant.Type.PackedStringArray));
	public PackedVector2Array AsPackedVector2Array() => new PackedVector2Array(InteropGetFromPointer<IntPtr>(_internal_pointer, Variant.Type.PackedVector2Array));
	public PackedVector3Array AsPackedVector3Array() => new PackedVector3Array(InteropGetFromPointer<IntPtr>(_internal_pointer, Variant.Type.PackedVector3Array));
	public PackedColorArray AsPackedColorArray() => new PackedColorArray(InteropGetFromPointer<IntPtr>(_internal_pointer, Variant.Type.PackedColorArray));

	public static implicit operator Variant(bool value) => new Variant(value);
	public static implicit operator Variant(long value) => new Variant(value);
	public static implicit operator Variant(double value) => new Variant(value);
	public static implicit operator Variant(string value) => new Variant(value);
	public static implicit operator Variant(Vector2 value) => new Variant(value);
	public static implicit operator Variant(Vector2i value) => new Variant(value);
	public static implicit operator Variant(Rect2 value) => new Variant(value);
	public static implicit operator Variant(Rect2i value) => new Variant(value);
	public static implicit operator Variant(Vector3 value) => new Variant(value);
	public static implicit operator Variant(Vector3i value) => new Variant(value);
	public static implicit operator Variant(Transform2D value) => new Variant(value);
	public static implicit operator Variant(Vector4 value) => new Variant(value);
	public static implicit operator Variant(Vector4i value) => new Variant(value);
	public static implicit operator Variant(Plane value) => new Variant(value);
	public static implicit operator Variant(Quaternion value) => new Variant(value);
	public static implicit operator Variant(AABB value) => new Variant(value);
	public static implicit operator Variant(Basis value) => new Variant(value);
	public static implicit operator Variant(Transform3D value) => new Variant(value);
	public static implicit operator Variant(Projection value) => new Variant(value);
	public static implicit operator Variant(Color value) => new Variant(value);
	public static implicit operator Variant(StringName value) => new Variant(value);
	public static implicit operator Variant(NodePath value) => new Variant(value);
	public static implicit operator Variant(RID value) => new Variant(value);
	public static implicit operator Variant(Object value) => new Variant(value);
	public static implicit operator Variant(Callable value) => new Variant(value);
	public static implicit operator Variant(Signal value) => new Variant(value);
	public static implicit operator Variant(Dictionary value) => new Variant(value);
	public static implicit operator Variant(Array value) => new Variant(value);
	public static implicit operator Variant(PackedByteArray value) => new Variant(value);
	public static implicit operator Variant(PackedInt32Array value) => new Variant(value);
	public static implicit operator Variant(PackedInt64Array value) => new Variant(value);
	public static implicit operator Variant(PackedFloat32Array value) => new Variant(value);
	public static implicit operator Variant(PackedFloat64Array value) => new Variant(value);
	public static implicit operator Variant(PackedStringArray value) => new Variant(value);
	public static implicit operator Variant(PackedVector2Array value) => new Variant(value);
	public static implicit operator Variant(PackedVector3Array value) => new Variant(value);
	public static implicit operator Variant(PackedColorArray value) => new Variant(value);

	public static explicit operator bool(Variant value) => value.AsBool();
	public static explicit operator long(Variant value) => value.AsInt();
	public static explicit operator double(Variant value) => value.AsFloat();
	public static explicit operator string(Variant value) => value.AsString();
	public static explicit operator Vector2(Variant value) => value.AsVector2();
	public static explicit operator Vector2i(Variant value) => value.AsVector2i();
	public static explicit operator Rect2(Variant value) => value.AsRect2();
	public static explicit operator Rect2i(Variant value) => value.AsRect2i();
	public static explicit operator Vector3(Variant value) => value.AsVector3();
	public static explicit operator Vector3i(Variant value) => value.AsVector3i();
	public static explicit operator Transform2D(Variant value) => value.AsTransform2D();
	public static explicit operator Vector4(Variant value) => value.AsVector4();
	public static explicit operator Vector4i(Variant value) => value.AsVector4i();
	public static explicit operator Plane(Variant value) => value.AsPlane();
	public static explicit operator Quaternion(Variant value) => value.AsQuaternion();
	public static explicit operator AABB(Variant value) => value.AsAABB();
	public static explicit operator Basis(Variant value) => value.AsBasis();
	public static explicit operator Transform3D(Variant value) => value.AsTransform3D();
	public static explicit operator Projection(Variant value) => value.AsProjection();
	public static explicit operator Color(Variant value) => value.AsColor();
	public static explicit operator StringName(Variant value) => value.AsStringName();
	public static explicit operator NodePath(Variant value) => value.AsNodePath();
	public static explicit operator RID(Variant value) => value.AsRID();
	public static explicit operator Object(Variant value) => value.AsObject();
	public static explicit operator Callable(Variant value) => value.AsCallable();
	public static explicit operator Signal(Variant value) => value.AsSignal();
	public static explicit operator Dictionary(Variant value) => value.AsDictionary();
	public static explicit operator Array(Variant value) => value.AsArray();
	public static explicit operator PackedByteArray(Variant value) => value.AsPackedByteArray();
	public static explicit operator PackedInt32Array(Variant value) => value.AsPackedInt32Array();
	public static explicit operator PackedInt64Array(Variant value) => value.AsPackedInt64Array();
	public static explicit operator PackedFloat32Array(Variant value) => value.AsPackedFloat32Array();
	public static explicit operator PackedFloat64Array(Variant value) => value.AsPackedFloat64Array();
	public static explicit operator PackedStringArray(Variant value) => value.AsPackedStringArray();
	public static explicit operator PackedVector2Array(Variant value) => value.AsPackedVector2Array();
	public static explicit operator PackedVector3Array(Variant value) => value.AsPackedVector3Array();
	public static explicit operator PackedColorArray(Variant value) => value.AsPackedColorArray();

	~Variant() {
		gdInterface.variant_destroy(_internal_pointer);
		gdInterface.mem_free(_internal_pointer);
	}
}
