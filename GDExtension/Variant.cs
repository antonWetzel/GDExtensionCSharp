namespace GDExtension;

public unsafe class Variant {

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

	public static void InteropSaveIntoPointer<T>(T value, IntPtr _internal_pointer, Variant.Type t) where T : unmanaged {
		gdInterface.get_variant_from_type_constructor.Call(t)(_internal_pointer, new IntPtr(&value));
	}

	public static void InteropSaveIntoPointer(string value, IntPtr _internal_pointer) {
		gdInterface.get_variant_from_type_constructor.Call(Variant.Type.String)(_internal_pointer, StringMarshall.ToNative(value));
	}

	public static T InteropGetFromPointer<T>(IntPtr _internal_pointer, Variant.Type t) where T : unmanaged {
		var rT = gdInterface.variant_get_type.Call(_internal_pointer);
		if (rT == Type.Nil) {
			return default; //probatly bad idea
		}
		if (rT != t) {
			throw new Exception($"variant contains {rT}, tried to get {t}");
		}
		T res;
		gdInterface.get_variant_to_type_constructor.Call(t)(new IntPtr(&res), _internal_pointer);
		return res;
	}

	internal IntPtr _internal_pointer;

	public Variant.Type type => gdInterface.variant_get_type.Call(_internal_pointer);

	private Variant() => _internal_pointer = gdInterface.mem_alloc.Call(24);
	internal Variant(IntPtr data) => _internal_pointer = data;

	public static Variant Nil {
		get { var v = new Variant(); gdInterface.variant_new_nil.Call(v._internal_pointer); return v; }
	}

	public Variant(bool value) : this() => InteropSaveIntoPointer(value, _internal_pointer, Variant.Type.Bool);
	public Variant(long value) : this() => InteropSaveIntoPointer(value, _internal_pointer, Variant.Type.Int);
	public Variant(double value) : this() => InteropSaveIntoPointer(value, _internal_pointer, Variant.Type.Float);
	public Variant(string value) : this() => InteropSaveIntoPointer(value, _internal_pointer);

	public Variant(Vector2 value) : this() => InteropSaveIntoPointer(value, _internal_pointer, Variant.Type.Vector2);
	public Variant(Vector2i value) : this() => InteropSaveIntoPointer(value, _internal_pointer, Variant.Type.Vector2i);
	public Variant(Rect2 value) : this() => InteropSaveIntoPointer(value, _internal_pointer, Variant.Type.Rect2);
	public Variant(Rect2i value) : this() => InteropSaveIntoPointer(value, _internal_pointer, Variant.Type.Rect2i);
	public Variant(Vector3 value) : this() => InteropSaveIntoPointer(value, _internal_pointer, Variant.Type.Vector3);
	public Variant(Vector3i value) : this() => InteropSaveIntoPointer(value, _internal_pointer, Variant.Type.Vector3i);
	public Variant(Transform2D value) : this() => InteropSaveIntoPointer(value, _internal_pointer, Variant.Type.Transform2D);
	public Variant(Vector4 value) : this() => InteropSaveIntoPointer(value, _internal_pointer, Variant.Type.Vector4);
	public Variant(Vector4i value) : this() => InteropSaveIntoPointer(value, _internal_pointer, Variant.Type.Vector4);
	public Variant(Plane value) : this() => InteropSaveIntoPointer(value, _internal_pointer, Variant.Type.Plane);
	public Variant(Quaternion value) : this() => InteropSaveIntoPointer(value, _internal_pointer, Variant.Type.Quaternion);
	public Variant(AABB value) : this() => InteropSaveIntoPointer(value, _internal_pointer, Variant.Type.AABB);
	public Variant(Basis value) : this() => InteropSaveIntoPointer(value, _internal_pointer, Variant.Type.Basis);
	public Variant(Transform3D value) : this() => InteropSaveIntoPointer(value, _internal_pointer, Variant.Type.Transform3D);
	public Variant(Projection value) : this() => InteropSaveIntoPointer(value, _internal_pointer, Variant.Type.Projection);

	public Variant(Color value) : this() => InteropSaveIntoPointer(value, _internal_pointer, Variant.Type.Color);
	public Variant(StringName value) : this() => InteropSaveIntoPointer(value, _internal_pointer, Variant.Type.StringName);
	public Variant(NodePath value) : this() => InteropSaveIntoPointer(value, _internal_pointer, Variant.Type.NodePath);
	public Variant(RID value) : this() => InteropSaveIntoPointer(value, _internal_pointer, Variant.Type.RID);
	public Variant(Object value) : this() => InteropSaveIntoPointer(value != null ? value._internal_pointer : IntPtr.Zero, _internal_pointer, Variant.Type.Object);
	public Variant(Callable value) : this() => InteropSaveIntoPointer(value, _internal_pointer, Variant.Type.Callable);
	public Variant(Signal value) : this() => InteropSaveIntoPointer(value, _internal_pointer, Variant.Type.Signal);
	public Variant(Dictionary value) : this() => InteropSaveIntoPointer(value, _internal_pointer, Variant.Type.Dictionary);
	public Variant(Array value) : this() => InteropSaveIntoPointer(value, _internal_pointer, Variant.Type.Array);

	public Variant(PackedByteArray value) : this() => InteropSaveIntoPointer(value, _internal_pointer, Variant.Type.PackedByteArray);
	public Variant(PackedInt32Array value) : this() => InteropSaveIntoPointer(value, _internal_pointer, Variant.Type.PackedInt32Array);
	public Variant(PackedInt64Array value) : this() => InteropSaveIntoPointer(value, _internal_pointer, Variant.Type.PackedInt64Array);
	public Variant(PackedFloat32Array value) : this() => InteropSaveIntoPointer(value, _internal_pointer, Variant.Type.PackedFloat32Array);
	public Variant(PackedFloat64Array value) : this() => InteropSaveIntoPointer(value, _internal_pointer, Variant.Type.PackedFloat64Array);
	public Variant(PackedStringArray value) : this() => InteropSaveIntoPointer(value, _internal_pointer, Variant.Type.PackedStringArray);
	public Variant(PackedVector2Array value) : this() => InteropSaveIntoPointer(value, _internal_pointer, Variant.Type.PackedVector2Array);
	public Variant(PackedVector3Array value) : this() => InteropSaveIntoPointer(value, _internal_pointer, Variant.Type.PackedVector3Array);
	public Variant(PackedColorArray value) : this() => InteropSaveIntoPointer(value, _internal_pointer, Variant.Type.PackedColorArray);

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
	public StringName AsStringName() => InteropGetFromPointer<StringName>(_internal_pointer, Variant.Type.StringName);
	public NodePath AsNodePath() => InteropGetFromPointer<NodePath>(_internal_pointer, Variant.Type.NodePath);
	public RID AsRID() => InteropGetFromPointer<RID>(_internal_pointer, Variant.Type.RID);
	public Object AsObject() => Object.ConstructUnknown(InteropGetFromPointer<IntPtr>(_internal_pointer, Variant.Type.Object));
	public Callable AsCallable() => InteropGetFromPointer<Callable>(_internal_pointer, Variant.Type.Callable);
	public Signal AsSignal() => InteropGetFromPointer<Signal>(_internal_pointer, Variant.Type.Signal);
	public Dictionary AsDictionary() => InteropGetFromPointer<Dictionary>(_internal_pointer, Variant.Type.Dictionary);
	public Array AsArray() => InteropGetFromPointer<Array>(_internal_pointer, Variant.Type.Array);
	public PackedByteArray AsPackedByteArray() => InteropGetFromPointer<PackedByteArray>(_internal_pointer, Variant.Type.PackedByteArray);
	public PackedInt32Array AsPackedInt32Array() => InteropGetFromPointer<PackedInt32Array>(_internal_pointer, Variant.Type.PackedInt32Array);
	public PackedInt64Array AsPackedInt64Array() => InteropGetFromPointer<PackedInt64Array>(_internal_pointer, Variant.Type.PackedInt64Array);
	public PackedFloat32Array AsPackedFloat32Array() => InteropGetFromPointer<PackedFloat32Array>(_internal_pointer, Variant.Type.PackedFloat32Array);
	public PackedFloat64Array AsPackedFloat64Array() => InteropGetFromPointer<PackedFloat64Array>(_internal_pointer, Variant.Type.PackedFloat64Array);
	public PackedStringArray AsPackedStringArray() => InteropGetFromPointer<PackedStringArray>(_internal_pointer, Variant.Type.PackedStringArray);
	public PackedVector2Array AsPackedVector2Array() => InteropGetFromPointer<PackedVector2Array>(_internal_pointer, Variant.Type.PackedVector2Array);
	public PackedVector3Array AsPackedVector3Array() => InteropGetFromPointer<PackedVector3Array>(_internal_pointer, Variant.Type.PackedVector3Array);
	public PackedColorArray AsPackedColorArray() => InteropGetFromPointer<PackedColorArray>(_internal_pointer, Variant.Type.PackedColorArray);

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
		//not right
		//gdInterface.variant_destroy.Call(_internal_pointer);
		//gdInterface.mem_free.Call(_internal_pointer);
	}
}
