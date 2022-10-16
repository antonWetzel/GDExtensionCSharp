namespace GDExtension;

public static class Native {

	public static Interface gdInterface;
	public static ExtensionClassLibraryPtr gdLibrary;

	public enum VariantType {
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


	public enum VariantOperator {
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

	[StructLayout(LayoutKind.Sequential)]
	public record struct VariantPtr(IntPtr data) {
		public static implicit operator VariantPtr(IntPtr ptr) => new(ptr);
	}
	[StructLayout(LayoutKind.Sequential)]
	public record struct StringNamePtr(IntPtr data) {
		public static implicit operator StringNamePtr(IntPtr ptr) => new(ptr);
	}
	[StructLayout(LayoutKind.Sequential)]
	public record struct StringPtr(IntPtr data) {
		public static implicit operator StringPtr(IntPtr ptr) => new(ptr);
	}
	[StructLayout(LayoutKind.Sequential)]
	public record struct ObjectPtr(IntPtr data) {
		public static implicit operator ObjectPtr(IntPtr ptr) => new(ptr);
		public static implicit operator TypePtr(ObjectPtr ptr) => new TypePtr(ptr.data);
	}
	[StructLayout(LayoutKind.Sequential)]
	public record struct TypePtr(IntPtr data) {
		public static implicit operator TypePtr(IntPtr ptr) => new(ptr);
	}
	[StructLayout(LayoutKind.Sequential)]
	public record struct ExtensionPtr(IntPtr data) {
		public static implicit operator ExtensionPtr(IntPtr ptr) => new(ptr);
	}
	[StructLayout(LayoutKind.Sequential)]
	public record struct MethodBindPtr(IntPtr data) {
		public static implicit operator MethodBindPtr(IntPtr ptr) => new(ptr);
	}
	[StructLayout(LayoutKind.Sequential)]
	public record struct Int(long value) {
		public static implicit operator Int(long val) => new(val);
	}
	[StructLayout(LayoutKind.Sequential)]
	public record struct Bool(bool value) {
		public static implicit operator Bool(bool val) => new(val);
	}
	[StructLayout(LayoutKind.Sequential)]
	public record struct GDObjectInstanceID(ulong value) {
		public static implicit operator GDObjectInstanceID(ulong val) => new(val);
	}

	/* VARIANT DATA I/O */

	public enum CallErrorType {
		Ok,
		InvalidMethod,
		InvalidArgument, /* expected is variant type */
		TooManyArguments, /* expected is number of arguments */
		TooFewArguments, /*  expected is number of arguments */
		InstanceIsNull,
		MethodNotConst, /* used for call */
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct CallError {
		public CallErrorType error;
		public int argument;
		public int expected;
	}

	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void VariantFromTypeConstructorFunc(VariantPtr variant, TypePtr type);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void TypeFromVariantConstructorFunc(TypePtr type, VariantPtr variant);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void PtrOperatorEvaluator(TypePtr p_left, TypePtr p_right, TypePtr r_result);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void PtrBuiltInMethod(TypePtr p_base, TypePtr* p_args, TypePtr r_return, int p_argument_count);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void PtrConstructor(TypePtr p_base, TypePtr* p_args);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void PtrDestructor(TypePtr p_base);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void PtrSetter(TypePtr p_base, TypePtr p_value);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void PtrGetter(TypePtr p_base, TypePtr r_value);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void PtrIndexedSetter(TypePtr p_base, Int p_index, TypePtr p_value);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void PtrIndexedGetter(TypePtr p_base, Int p_index, TypePtr r_value);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void PtrKeyedSetter(TypePtr p_base, TypePtr p_key, TypePtr p_value);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void PtrKeyedGetter(TypePtr p_base, TypePtr p_key, TypePtr r_value);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate uint PtrKeyedChecker(VariantPtr p_base, VariantPtr p_key);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void PtrUtilityFunction(TypePtr r_return, TypePtr* p_arguments, int p_argument_count);

	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate ObjectPtr ClassConstructor();

	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate IntPtr InstanceBindingCreateCallback(IntPtr p_token, IntPtr p_instance);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void InstanceBindingFreeCallback(IntPtr p_token, IntPtr p_instance, IntPtr p_binding);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate Bool InstanceBindingReferenceCallback(IntPtr p_token, IntPtr p_binding, Bool p_reference);

	[StructLayout(LayoutKind.Sequential)]
	public struct InstanceBindingCallbacks {
		public FuncPtr<InstanceBindingCreateCallback> create_callback;
		public FuncPtr<InstanceBindingFreeCallback> free_callback;
		public FuncPtr<InstanceBindingReferenceCallback> reference_callback;
	}

	// /* EXTENSION CLASSES */

	[StructLayout(LayoutKind.Sequential)]
	public record struct GDExtensionClassInstancePtr(IntPtr data);

	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate Bool ExtensionClassSet(GDExtensionClassInstancePtr p_instance, StringNamePtr p_name, VariantPtr p_value);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate Bool ExtensionClassGet(GDExtensionClassInstancePtr p_instance, StringNamePtr p_name, VariantPtr r_ret);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate ulong ExtensionClassGetRID(GDExtensionClassInstancePtr p_instance);

	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct PropertyInfo {
		public uint type;
		public byte* name;
		public byte* class_name;
		public uint hint;
		public byte* hint_string;
		public uint usage;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct MethodInfo {
		public byte* name;
		public PropertyInfo return_value;
		public uint flags; // From ExtensionClassMethodFlags
		public int id;
		public PropertyInfo* arguments;
		public uint argument_count;
		public VariantPtr default_arguments;
		public uint default_argument_count;
	};

	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate PropertyInfo* ExtensionClassGetPropertyList(GDExtensionClassInstancePtr p_instance, uint* r_count);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void ExtensionClassFreePropertyList(GDExtensionClassInstancePtr p_instance, PropertyInfo* p_list);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate Bool ExtensionClassPropertyCanRevert(GDExtensionClassInstancePtr p_instance, StringNamePtr p_name);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate Bool ExtensionClassPropertyGetRevert(GDExtensionClassInstancePtr p_instance, StringNamePtr p_name, VariantPtr r_ret);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void ExtensionClassNotification(GDExtensionClassInstancePtr p_instance, int p_what);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void ExtensionClassToString(GDExtensionClassInstancePtr p_instance, StringPtr p_out);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void ExtensionClassReference(GDExtensionClassInstancePtr p_instance);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void ExtensionClassUnreference(GDExtensionClassInstancePtr p_instance);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void ExtensionClassCallVirtual(GDExtensionClassInstancePtr p_instance, TypePtr* p_args, TypePtr r_ret);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate ObjectPtr ExtensionClassCreateInstance(IntPtr p_userdata);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void ExtensionClassFreeInstance(IntPtr p_userdata, GDExtensionClassInstancePtr p_instance);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void ExtensionClassObjectInstance(GDExtensionClassInstancePtr p_instance, ObjectPtr p_object_instance);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate ExtensionClassCallVirtual ExtensionClassGetVirtual(IntPtr p_userdata, byte* p_name);

	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct ExtensionClassCreationInfo {
		public FuncPtr<ExtensionClassSet> set_func;
		public FuncPtr<ExtensionClassGet> get_func;
		public FuncPtr<ExtensionClassGetPropertyList> get_property_list_func;
		public FuncPtr<ExtensionClassFreePropertyList> free_property_list_func;
		public FuncPtr<ExtensionClassPropertyCanRevert> property_can_revert_func;
		public FuncPtr<ExtensionClassPropertyGetRevert> property_get_revert_func;
		public FuncPtr<ExtensionClassNotification> notification_func;
		public FuncPtr<ExtensionClassToString> to_string_func;
		public FuncPtr<ExtensionClassReference> reference_func;
		public FuncPtr<ExtensionClassUnreference> unreference_func;
		public FuncPtr<ExtensionClassCreateInstance> create_instance_func; /* this one is mandatory */
		public FuncPtr<ExtensionClassFreeInstance> free_instance_func; /* this one is mandatory */
		public FuncPtr<ExtensionClassGetVirtual> get_virtual_func;
		public FuncPtr<ExtensionClassGetRID> get_rid_func;
		public IntPtr class_userdata;
	};

	[StructLayout(LayoutKind.Sequential)]
	public record struct ExtensionClassLibraryPtr(IntPtr data);

	// /* Method */

	public enum ExtensionClassMethodFlags {
		Normal = 1,
		Editor = 2,
		Const = 4,
		Virtual = 8,
		Vararg = 16,
		Static = 32,
		Default = Normal,
	}

	public enum ExtensionClassMethodArgumentMetadata {
		None,
		IsInt8,
		IsInt16,
		IsInt32,
		IsInt64,
		IsUint8,
		IsUint16,
		IsUint32,
		IsUint64,
		IsFloat,
		IsDouble
	}

	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void ExtensionClassMethodCall(IntPtr method_userdata, GDExtensionClassInstancePtr p_instance, VariantPtr* p_args, Int p_argument_count, VariantPtr r_return, CallError* r_error);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void ExtensionClassMethodPtrCall(IntPtr method_userdata, GDExtensionClassInstancePtr p_instance, TypePtr* p_args, TypePtr r_ret);

	/* passing -1 as argument in the following functions refers to the return type */
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate VariantType ExtensionClassMethodGetArgumentType(IntPtr p_method_userdata, int p_argument);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void ExtensionClassMethodGetArgumentInfo(IntPtr p_method_userdata, int p_argument, PropertyInfo* r_info);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate ExtensionClassMethodArgumentMetadata ExtensionClassMethodGetArgumentMetadata(IntPtr p_method_userdata, int p_argument);

	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct ExtensionClassMethodInfo {
		public byte* name;
		public IntPtr method_userdata;
		public FuncPtr<ExtensionClassMethodCall> call_func;
		public FuncPtr<ExtensionClassMethodPtrCall> ptrcall_func;
		public uint method_flags; /* ExtensionClassMethodFlags */
		public uint argument_count;
		public Bool has_return_value;
		public FuncPtr<ExtensionClassMethodGetArgumentType> get_argument_type_func;
		public FuncPtr<ExtensionClassMethodGetArgumentInfo> get_argument_info_func; /* name and hint information for the argument can be omitted in release builds. Class name should always be present if it applies. */
		public FuncPtr<ExtensionClassMethodGetArgumentMetadata> get_argument_metadata_func;
		public uint default_argument_count;
		public VariantPtr* default_arguments;
	}

	// /* SCRIPT INSTANCE EXTENSION */

	[StructLayout(LayoutKind.Sequential)] public record struct ExtensionScriptInstanceDataPtr(IntPtr data); // Pointer to custom ScriptInstance native implementation

	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate Bool ExtensionScriptInstanceSet(ExtensionScriptInstanceDataPtr p_instance, StringNamePtr p_name, VariantPtr p_value);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate Bool ExtensionScriptInstanceGet(ExtensionScriptInstanceDataPtr p_instance, StringNamePtr p_name, VariantPtr r_ret);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate PropertyInfo* ExtensionScriptInstanceGetPropertyList(ExtensionScriptInstanceDataPtr p_instance, uint* r_count);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void ExtensionScriptInstanceFreePropertyList(ExtensionScriptInstanceDataPtr p_instance, PropertyInfo* p_list);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate VariantType ExtensionScriptInstanceGetPropertyType(ExtensionScriptInstanceDataPtr p_instance, StringNamePtr p_name, Bool* r_is_valid);

	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate Bool ExtensionScriptInstancePropertyCanRevert(ExtensionScriptInstanceDataPtr p_instance, StringNamePtr p_name);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate Bool ExtensionScriptInstancePropertyGetRevert(ExtensionScriptInstanceDataPtr p_instance, StringNamePtr p_name, VariantPtr r_ret);

	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate ObjectPtr ExtensionScriptInstanceGetOwner(ExtensionScriptInstanceDataPtr p_instance);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void ExtensionScriptInstancePropertyStateAdd(StringNamePtr p_name, VariantPtr p_value, IntPtr p_userdata);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void ExtensionScriptInstanceGetPropertyState(ExtensionScriptInstanceDataPtr p_instance, ExtensionScriptInstancePropertyStateAdd p_add_func, IntPtr p_userdata);

	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate MethodInfo* ExtensionScriptInstanceGetMethodList(ExtensionScriptInstanceDataPtr p_instance, uint* r_count);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void ExtensionScriptInstanceFreeMethodList(ExtensionScriptInstanceDataPtr p_instance, MethodInfo* p_list);

	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate Bool ExtensionScriptInstanceHasMethod(ExtensionScriptInstanceDataPtr p_instance, StringNamePtr p_name);

	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void ExtensionScriptInstanceCall(ExtensionScriptInstanceDataPtr p_self, StringNamePtr p_method, VariantPtr* p_args, Int p_argument_count, VariantPtr r_return, CallError* r_error);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void ExtensionScriptInstanceNotification(ExtensionScriptInstanceDataPtr p_instance, int p_what);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate byte* ExtensionScriptInstanceToString(ExtensionScriptInstanceDataPtr p_instance, Bool* r_is_valid);

	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void ExtensionScriptInstanceRefCountIncremented(ExtensionScriptInstanceDataPtr p_instance);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate Bool ExtensionScriptInstanceRefCountDecremented(ExtensionScriptInstanceDataPtr p_instance);

	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate ObjectPtr ExtensionScriptInstanceGetScript(ExtensionScriptInstanceDataPtr p_instance);
	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate Bool ExtensionScriptInstanceIsPlaceholder(ExtensionScriptInstanceDataPtr p_instance);

	[StructLayout(LayoutKind.Sequential)] public record struct ExtensionScriptLanguagePtr(IntPtr data);

	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate ExtensionScriptLanguagePtr ExtensionScriptInstanceGetLanguage(ExtensionScriptInstanceDataPtr p_instance);

	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void ExtensionScriptInstanceFree(ExtensionScriptInstanceDataPtr p_instance);

	[StructLayout(LayoutKind.Sequential)] public record struct ScriptInstancePtr(IntPtr data); // Pointer to ScriptInstance.

	[StructLayout(LayoutKind.Sequential)]
	public struct ExtensionScriptInstanceInfo {
		public FuncPtr<ExtensionScriptInstanceSet> set_func;
		public FuncPtr<ExtensionScriptInstanceGet> get_func;
		public FuncPtr<ExtensionScriptInstanceGetPropertyList> get_property_list_func;
		public FuncPtr<ExtensionScriptInstanceFreePropertyList> free_property_list_func;
		public FuncPtr<ExtensionScriptInstanceGetPropertyType> get_property_type_func;

		public FuncPtr<ExtensionScriptInstancePropertyCanRevert> property_can_revert_func;
		public FuncPtr<ExtensionScriptInstancePropertyGetRevert> property_get_revert_func;

		public FuncPtr<ExtensionScriptInstanceGetOwner> get_owner_func;
		public FuncPtr<ExtensionScriptInstanceGetPropertyState> get_property_state_func;

		public FuncPtr<ExtensionScriptInstanceGetMethodList> get_method_list_func;
		public FuncPtr<ExtensionScriptInstanceFreeMethodList> free_method_list_func;

		public FuncPtr<ExtensionScriptInstanceHasMethod> has_method_func;

		public FuncPtr<ExtensionScriptInstanceCall> call_func;
		public FuncPtr<ExtensionScriptInstanceNotification> notification_func;

		public FuncPtr<ExtensionScriptInstanceToString> to_string_func;

		public FuncPtr<ExtensionScriptInstanceRefCountIncremented> refcount_incremented_func;
		public FuncPtr<ExtensionScriptInstanceRefCountDecremented> refcount_decremented_func;

		public FuncPtr<ExtensionScriptInstanceGetScript> get_script_func;

		public FuncPtr<ExtensionScriptInstanceIsPlaceholder> is_placeholder_func;

		public FuncPtr<ExtensionScriptInstanceSet> set_fallback_func;
		public FuncPtr<ExtensionScriptInstanceGet> get_fallback_func;

		public FuncPtr<ExtensionScriptInstanceGetLanguage> get_language_func;

		public FuncPtr<ExtensionScriptInstanceFree> free_func;
	}

	// /* INTERFACE */

	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct Interface {
		public uint version_major;
		public uint version_minor;
		public uint version_patch;
		public byte* version_string;

		/* GODOT CORE */
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate IntPtr MemAlloc(nuint p_bytes);
		public FuncPtr<MemAlloc> mem_alloc;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate IntPtr MemRealloc(IntPtr p_ptr, nuint p_bytes);
		public FuncPtr<MemRealloc> mem_realloc;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void MemFree(IntPtr p_ptr);
		public FuncPtr<MemFree> mem_free;

		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void PrintError(byte* p_description, byte* p_function, byte* p_file, int p_line);
		public FuncPtr<PrintError> print_error;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void PrintWarning(byte* p_description, byte* p_function, byte* p_file, int p_line);
		public FuncPtr<PrintWarning> print_warning;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void PrintScriptError(byte* p_description, byte* p_function, byte* p_file, int p_line);
		public FuncPtr<PrintScriptError> print_script_error;

		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate ulong GetNativeStructSize(byte* p_name);
		public FuncPtr<GetNativeStructSize> get_native_struct_size;

		/* GODOT VARIANT */

		/* variant general */
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void VariantNewCopy(VariantPtr r_dest, VariantPtr p_src);
		public FuncPtr<VariantNewCopy> variant_new_copy;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void VariantNewNil(VariantPtr r_dest);
		public FuncPtr<VariantNewNil> variant_new_nil;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void VariantDestroy(VariantPtr p_self);
		public FuncPtr<VariantDestroy> variant_destroy;

		/* variant type */
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void VariantCall(VariantPtr p_self, StringNamePtr p_method, VariantPtr* p_args, Int p_argument_count, VariantPtr r_return, CallError* r_error);
		public FuncPtr<VariantCall> variant_call;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void VariantCallStatic(VariantType p_type, StringNamePtr p_method, VariantPtr* p_args, Int p_argument_count, VariantPtr r_return, CallError* r_error);
		public FuncPtr<VariantCallStatic> variant_call_static;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void VariantEvaluate(VariantOperator p_op, VariantPtr p_a, VariantPtr p_b, VariantPtr r_return, Bool* r_valid);
		public FuncPtr<VariantEvaluate> variant_evaluate;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void VariantSet(VariantPtr p_self, VariantPtr p_key, VariantPtr p_value, Bool* r_valid);
		public FuncPtr<VariantSet> variant_set;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void VariantSetNamed(VariantPtr p_self, StringNamePtr p_key, VariantPtr p_value, Bool* r_valid);
		public FuncPtr<VariantSetNamed> variant_set_named;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void VariantSetKeyed(VariantPtr p_self, VariantPtr p_key, VariantPtr p_value, Bool* r_valid);
		public FuncPtr<VariantSetKeyed> variant_set_keyed;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void VariantSetIndexed(VariantPtr p_self, Int p_index, VariantPtr p_value, Bool* r_valid, Bool* r_oob);
		public FuncPtr<VariantSetIndexed> variant_set_indexed;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void VariantGet(VariantPtr p_self, VariantPtr p_key, VariantPtr r_ret, Bool* r_valid);
		public FuncPtr<VariantGet> variant_get;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void VariantGetNamed(VariantPtr p_self, StringNamePtr p_key, VariantPtr r_ret, Bool* r_valid);
		public FuncPtr<VariantGetNamed> variant_get_named;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void VariantGetKeyed(VariantPtr p_self, VariantPtr p_key, VariantPtr r_ret, Bool* r_valid);
		public FuncPtr<VariantGetKeyed> variant_get_keyed;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void VariantGetIndexed(VariantPtr p_self, Int p_index, VariantPtr r_ret, Bool* r_valid, Bool* r_oob);
		public FuncPtr<VariantGetIndexed> variant_get_indexed;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate Bool VariantIterInit(VariantPtr p_self, VariantPtr r_iter, Bool* r_valid);
		public FuncPtr<VariantIterInit> variant_iter_init;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate Bool VariantIterNext(VariantPtr p_self, VariantPtr r_iter, Bool* r_valid);
		public FuncPtr<VariantIterNext> variant_iter_next;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void VariantIterGet(VariantPtr p_self, VariantPtr r_iter, VariantPtr r_ret, Bool* r_valid);
		public FuncPtr<VariantIterGet> variant_iter_get;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate Int VariantHash(VariantPtr p_self);
		public FuncPtr<VariantHash> variant_hash;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate Int VariantRecursiveHash(VariantPtr p_self, Int p_recursion_count);
		public FuncPtr<VariantRecursiveHash> variant_recursive_hash;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate Bool VariantHashCompare(VariantPtr p_self, VariantPtr p_other);
		public FuncPtr<VariantHashCompare> variant_hash_compare;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate Bool VariantBooleanize(VariantPtr p_self);
		public FuncPtr<VariantBooleanize> variant_booleanize;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void VariantDuplicate(VariantPtr p_self, VariantPtr r_ret, Bool p_deep);
		public FuncPtr<VariantDuplicate> variant_duplicate;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void VariantStringify(VariantPtr p_self, StringPtr r_ret);
		public FuncPtr<VariantStringify> variant_stringify;

		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate VariantType VariantGetType(VariantPtr p_self);
		public FuncPtr<VariantGetType> variant_get_type;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate Bool VariantHasMethod(VariantPtr p_self, StringNamePtr p_method);
		public FuncPtr<VariantHasMethod> variant_has_method;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate Bool VariantHasMember(VariantType p_type, StringNamePtr p_member);
		public FuncPtr<VariantHasMember> variant_has_member;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate Bool VariantHasKey(VariantPtr p_self, VariantPtr p_key, Bool* r_valid);
		public FuncPtr<VariantHasKey> variant_has_key;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void VariantGetTypeName(VariantType p_type, StringPtr r_name);
		public FuncPtr<VariantGetTypeName> variant_get_type_name;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate Bool VariantCanConvert(VariantType p_from, VariantType p_to);
		public FuncPtr<VariantCanConvert> variant_can_convert;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate Bool VariantCanConvertStrict(VariantType p_from, VariantType p_to);
		public FuncPtr<VariantCanConvertStrict> variant_can_convert_strict;

		/* ptrcalls */
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate VariantFromTypeConstructorFunc GetVariantFromTypeConstructor(VariantType p_type);
		public FuncPtr<GetVariantFromTypeConstructor> get_variant_from_type_constructor;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate TypeFromVariantConstructorFunc GetVariantToTypeConstructor(VariantType p_type);
		public FuncPtr<GetVariantToTypeConstructor> get_variant_to_type_constructor;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate PtrOperatorEvaluator VariantGetPtrOperatorEvaluator(VariantOperator p_operator, VariantType p_type_a, VariantType p_type_b);
		public FuncPtr<VariantGetPtrOperatorEvaluator> variant_get_ptr_operator_evaluator;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate PtrBuiltInMethod VariantGetPtrBuiltinMethod(VariantType p_type, byte* p_method, Int p_hash);
		public FuncPtr<VariantGetPtrBuiltinMethod> variant_get_ptr_builtin_method;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate PtrConstructor VariantGetPtrConstructor(VariantType p_type, int p_constructor);
		public FuncPtr<VariantGetPtrConstructor> variant_get_ptr_constructor;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate PtrDestructor VariantGetPtrDestructor(VariantType p_type);
		public FuncPtr<VariantGetPtrDestructor> variant_get_ptr_destructor;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void VariantConstruct(VariantType p_type, VariantPtr p_base, VariantPtr* p_args, int p_argument_count, CallError* r_error);
		public FuncPtr<VariantConstruct> variant_construct;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate PtrSetter VariantGetPtrSetter(VariantType p_type, byte* p_member);
		public FuncPtr<VariantGetPtrSetter> variant_get_ptr_setter;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate PtrGetter VariantGetPtrGetter(VariantType p_type, byte* p_member);
		public FuncPtr<VariantGetPtrGetter> variant_get_ptr_getter;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate PtrIndexedSetter VariantGetPtrIndexedSetter(VariantType p_type);
		public FuncPtr<VariantGetPtrIndexedSetter> variant_get_ptr_indexed_setter;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate PtrIndexedGetter VariantGetPtrIndexedGetter(VariantType p_type);
		public FuncPtr<VariantGetPtrIndexedGetter> variant_get_ptr_indexed_getter;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate PtrKeyedSetter VariantGetPtrKeyedSetter(VariantType p_type);
		public FuncPtr<VariantGetPtrKeyedSetter> variant_get_ptr_keyed_setter;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate PtrKeyedGetter VariantGetPtrKeyedGetter(VariantType p_type);
		public FuncPtr<VariantGetPtrKeyedGetter> variant_get_ptr_keyed_getter;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate PtrKeyedChecker VariantGetPtrKeyedChecker(VariantType p_type);
		public FuncPtr<VariantGetPtrKeyedChecker> variant_get_ptr_keyed_checker;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void VariantGetConstantValue(VariantType p_type, byte* p_constant, VariantPtr r_ret);
		public FuncPtr<VariantGetConstantValue> variant_get_constant_value;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate PtrUtilityFunction VariantGetPtrUtilityFunction(byte* p_function, Int p_hash);
		public FuncPtr<VariantGetPtrUtilityFunction> variant_get_ptr_utility_function;

		/*  extra utilities */

		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void StringNewWithLatin1Chars(StringPtr r_dest, byte* p_contents);
		public FuncPtr<StringNewWithLatin1Chars> string_new_with_latin1_chars;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void StringNewWithUtf8Chars(StringPtr r_dest, byte* p_contents);
		public FuncPtr<StringNewWithUtf8Chars> string_new_with_utf8_chars;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void StringNewWithUtf16Chars(StringPtr r_dest, char* p_contents);
		public FuncPtr<StringNewWithUtf16Chars> string_new_with_utf16_chars;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void StringNewWithUtf32Chars(StringPtr r_dest, uint* p_contents);
		public FuncPtr<StringNewWithUtf32Chars> string_new_with_utf32_chars;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void StringNewWithWideChars(StringPtr r_dest, byte* p_contents);
		public FuncPtr<StringNewWithWideChars> string_new_with_wide_chars;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void StringNewWithLatin1CharsAndLen(StringPtr r_dest, byte* p_contents, Int p_size);
		public FuncPtr<StringNewWithLatin1CharsAndLen> string_new_with_latin1_chars_and_len;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void StringNewWithUtf8CharsAndLen(StringPtr r_dest, byte* p_contents, Int p_size);
		public FuncPtr<StringNewWithUtf8CharsAndLen> string_new_with_utf8_chars_and_len;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void StringNewWithUtf16CharsAndLen(StringPtr r_dest, char* p_contents, Int p_size);
		public FuncPtr<StringNewWithUtf16CharsAndLen> string_new_with_utf16_chars_and_len;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void StringNewWithUtf32CharsAndLen(StringPtr r_dest, uint* p_contents, Int p_size);
		public FuncPtr<StringNewWithUtf32CharsAndLen> string_new_with_utf32_chars_and_len;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void StringNewWithWideCharsAndLen(StringPtr r_dest, byte* p_contents, Int p_size);
		public FuncPtr<StringNewWithWideCharsAndLen> string_new_with_wide_chars_and_len;
		/* Information about the following functions:
		 * - The return value is the resulting encoded string length.
		 * - The length returned is in characters, not in bytes. It also does not include a trailing zero.
		 * - These functions also do not write trailing zero, If you need it, write it yourself at the position indicated by the length (and make sure to allocate it).
		 * - Passing NULL in r_text means only the length is computed (again, without including trailing zero).
		 * - p_max_write_length argument is in characters, not bytes. It will be ignored if r_text is NULL.
		 * - p_max_write_length argument does not affect the return value, it's only to cap write length.
		 */
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate Int StringToLatin1Chars(StringPtr p_self, byte* r_text, Int p_max_write_length);
		public FuncPtr<StringToLatin1Chars> string_to_latin1_chars;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate Int StringToUtf8Chars(StringPtr p_self, byte* r_text, Int p_max_write_length);
		public FuncPtr<StringToUtf8Chars> string_to_utf8_chars;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate Int StringToUtf16Chars(StringPtr p_self, char* r_text, Int p_max_write_length);
		public FuncPtr<StringToUtf16Chars> string_to_utf16_chars;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate Int StringToUtf32Chars(StringPtr p_self, uint* r_text, Int p_max_write_length);
		public FuncPtr<StringToUtf32Chars> string_to_utf32_chars;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate Int StringToWideChars(StringPtr p_self, byte* r_text, Int p_max_write_length);
		public FuncPtr<StringToWideChars> string_to_wide_chars;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate uint* StringOperatorIndex(StringPtr p_self, Int p_index);
		public FuncPtr<StringOperatorIndex> string_operator_index;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate uint* StringOperatorIndexConst(StringPtr p_self, Int p_index);
		public FuncPtr<StringOperatorIndexConst> string_operator_index_const;

		/* Packed array functions */

		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate byte* PackedByteArrayOperatorIndex(TypePtr p_self, Int p_index);
		public FuncPtr<PackedByteArrayOperatorIndex> packed_byte_array_operator_index; // p_self should be a PackedByteArray
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate byte* PackedByteArrayOperatorIndexConst(TypePtr p_self, Int p_index);
		public FuncPtr<PackedByteArrayOperatorIndexConst> packed_byte_array_operator_index_const; // p_self should be a PackedByteArray

		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate TypePtr PackedColorArrayOperatorIndex(TypePtr p_self, Int p_index);
		public FuncPtr<PackedColorArrayOperatorIndex> packed_color_array_operator_index; // p_self should be a PackedColorArray, returns Color ptr
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate TypePtr PackedColorArrayOperatorIndexConst(TypePtr p_self, Int p_index);
		public FuncPtr<PackedColorArrayOperatorIndexConst> packed_color_array_operator_index_const; // p_self should be a PackedColorArray, returns Color ptr

		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate float* PackedFloat32ArrayOperatorIndex(TypePtr p_self, Int p_index);
		public FuncPtr<PackedFloat32ArrayOperatorIndex> packed_float32_array_operator_index; // p_self should be a PackedFloat32Array
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate float* PackedFloat32ArrayOperatorIndexConst(TypePtr p_self, Int p_index);
		public FuncPtr<PackedFloat32ArrayOperatorIndexConst> packed_float32_array_operator_index_const; // p_self should be a PackedFloat32Array
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate double* PackedFloat64ArrayOperatorIndex(TypePtr p_self, Int p_index);
		public FuncPtr<PackedFloat64ArrayOperatorIndex> packed_float64_array_operator_index; // p_self should be a PackedFloat64Array
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate double* PackedFloat64ArrayOperatorIndexConst(TypePtr p_self, Int p_index);
		public FuncPtr<PackedFloat64ArrayOperatorIndexConst> packed_float64_array_operator_index_const; // p_self should be a PackedFloat64Array

		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate int* PackedInt32ArrayOperatorIndex(TypePtr p_self, Int p_index);
		public FuncPtr<PackedInt32ArrayOperatorIndex> packed_int32_array_operator_index; // p_self should be a PackedInt32Array
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate int* PackedInt32ArrayOperatorIndexConst(TypePtr p_self, Int p_index);
		public FuncPtr<PackedInt32ArrayOperatorIndexConst> packed_int32_array_operator_index_const; // p_self should be a PackedInt32Array
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate long* PackedInt64ArrayOperatorIndex(TypePtr p_self, Int p_index);
		public FuncPtr<PackedInt64ArrayOperatorIndex> packed_int64_array_operator_index; // p_self should be a PackedInt32Array
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate long* PackedInt64ArrayOperatorIndexConst(TypePtr p_self, Int p_index);
		public FuncPtr<PackedInt64ArrayOperatorIndexConst> packed_int64_array_operator_index_const; // p_self should be a PackedInt32Array

		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate StringPtr PackedStringArrayOperatorIndex(TypePtr p_self, Int p_index);
		public FuncPtr<PackedStringArrayOperatorIndex> packed_string_array_operator_index; // p_self should be a PackedStringArray
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate StringPtr PackedStringArrayOperatorIndexConst(TypePtr p_self, Int p_index);
		public FuncPtr<PackedStringArrayOperatorIndexConst> packed_string_array_operator_index_const; // p_self should be a PackedStringArray

		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate TypePtr PackedVector2ArrayOperatorIndex(TypePtr p_self, Int p_index);
		public FuncPtr<PackedVector2ArrayOperatorIndex> packed_vector2_array_operator_index; // p_self should be a PackedVector2Array, returns Vector2 ptr
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate TypePtr PackedVector2ArrayOperatorIndexConst(TypePtr p_self, Int p_index);
		public FuncPtr<PackedVector2ArrayOperatorIndexConst> packed_vector2_array_operator_index_const; // p_self should be a PackedVector2Array, returns Vector2 ptr
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate TypePtr PackedVector3ArrayOperatorIndex(TypePtr p_self, Int p_index);
		public FuncPtr<PackedVector3ArrayOperatorIndex> packed_vector3_array_operator_index; // p_self should be a PackedVector3Array, returns Vector3 ptr
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate TypePtr PackedVector3ArrayOperatorIndexConst(TypePtr p_self, Int p_index);
		public FuncPtr<PackedVector3ArrayOperatorIndexConst> packed_vector3_array_operator_index_const; // p_self should be a PackedVector3Array, returns Vector3 ptr

		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate VariantPtr ArrayOperatorIndex(TypePtr p_self, Int p_index);
		public FuncPtr<ArrayOperatorIndex> array_operator_index; // p_self should be an Array ptr
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate VariantPtr ArrayOperatorIndexConst(TypePtr p_self, Int p_index);
		public FuncPtr<ArrayOperatorIndexConst> array_operator_index_const; // p_self should be an Array ptr

		/* Dictionary functions */

		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate VariantPtr DictionaryOperatorIndex(TypePtr p_self, VariantPtr p_key);
		public FuncPtr<DictionaryOperatorIndex> dictionary_operator_index; // p_self should be an Dictionary ptr
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate VariantPtr DictionaryOperatorIndexConst(TypePtr p_self, VariantPtr p_key);
		public FuncPtr<DictionaryOperatorIndexConst> dictionary_operator_index_const; // p_self should be an Dictionary ptr

		/* OBJECT */

		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void ObjectMethodBindCall(MethodBindPtr p_method_bind, ObjectPtr p_instance, VariantPtr* p_args, Int p_arg_count, VariantPtr r_ret, CallError* r_error);
		public FuncPtr<ObjectMethodBindCall> object_method_bind_call;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void ObjectMethodBindPtrcall(MethodBindPtr p_method_bind, ObjectPtr p_instance, TypePtr* p_args, TypePtr r_ret);
		public FuncPtr<ObjectMethodBindPtrcall> object_method_bind_ptrcall;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void ObjectDestroy(ObjectPtr p_o);
		public FuncPtr<ObjectDestroy> object_destroy;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate ObjectPtr GlobalGetSingleton(byte* p_name);
		public FuncPtr<GlobalGetSingleton> global_get_singleton;

		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate IntPtr ObjectGetInstanceBinding(ObjectPtr p_o, IntPtr p_token, InstanceBindingCallbacks* p_callbacks);
		public FuncPtr<ObjectGetInstanceBinding> object_get_instance_binding;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void ObjectSetInstanceBinding(ObjectPtr p_o, IntPtr p_token, IntPtr p_binding, InstanceBindingCallbacks* p_callbacks);
		public FuncPtr<ObjectSetInstanceBinding> object_set_instance_binding;

		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void ObjectSetInstance(ObjectPtr p_o, byte* p_classname, GDExtensionClassInstancePtr p_instance);
		public FuncPtr<ObjectSetInstance> object_set_instance; /* p_classname should be a registered extension class and should extend the p_o object's class. */

		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate ObjectPtr ObjectCastTo(ObjectPtr p_object, IntPtr p_class_tag);
		public FuncPtr<ObjectCastTo> object_cast_to;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate ObjectPtr ObjectGetInstanceFromId(GDObjectInstanceID p_instance_id);
		public FuncPtr<ObjectGetInstanceFromId> object_get_instance_from_id;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate GDObjectInstanceID ObjectGetInstanceId(ObjectPtr p_object);
		public FuncPtr<ObjectGetInstanceId> object_get_instance_id;

		/* SCRIPT INSTANCE */

		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate ScriptInstancePtr ScriptInstanceCreate(ExtensionScriptInstanceInfo* p_info, ExtensionScriptInstanceDataPtr p_instance_data);
		public FuncPtr<ScriptInstanceCreate> script_instance_create;

		/* CLASSDB */
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate ObjectPtr ClassDBConstructObject(byte* p_classname);
		public FuncPtr<ClassDBConstructObject> classdb_construct_object; /* The passed class must be a built-in godot class, or an already-registered extension class. In both case, object_set_instance should be called to fully initialize the object. */
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate MethodBindPtr ClassDBGetMethodBind(byte* p_classname, byte* p_methodname, Int p_hash);
		public FuncPtr<ClassDBGetMethodBind> classdb_get_method_bind;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate IntPtr ClassDBGetClassTag(byte* p_classname);
		public FuncPtr<ClassDBGetClassTag> classdb_get_class_tag;

		/* CLASSDB EXTENSION */

		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void ClassDBRegisterExtensionClass(ExtensionClassLibraryPtr p_library, byte* p_class_name, byte* p_parent_class_name, ExtensionClassCreationInfo* p_extension_funcs);
		public FuncPtr<ClassDBRegisterExtensionClass> classdb_register_extension_class;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void ClassDBRegisterExtensionClassMethod(ExtensionClassLibraryPtr p_library, byte* p_class_name, ExtensionClassMethodInfo* p_method_info);
		public FuncPtr<ClassDBRegisterExtensionClassMethod> classdb_register_extension_class_method;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void ClassDBRegisterExtensionClassIntegerConstant(ExtensionClassLibraryPtr p_library, byte* p_class_name, byte* p_enum_name, byte* p_constant_name, Int p_constant_value, Bool p_is_bitfield);
		public FuncPtr<ClassDBRegisterExtensionClassIntegerConstant> classdb_register_extension_class_integer_constant;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void ClassDBRegisterExtensionClassProperty(ExtensionClassLibraryPtr p_library, byte* p_class_name, PropertyInfo* p_info, byte* p_setter, byte* p_getter);
		public FuncPtr<ClassDBRegisterExtensionClassProperty> classdb_register_extension_class_property;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void ClassDBRegisterExtensionClassPropertyGroup(ExtensionClassLibraryPtr p_library, byte* p_class_name, byte* p_group_name, byte* p_prefix);
		public FuncPtr<ClassDBRegisterExtensionClassPropertyGroup> classdb_register_extension_class_property_group;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void ClassDBRegisterExtensionClassPropertySubgroup(ExtensionClassLibraryPtr p_library, byte* p_class_name, byte* p_subgroup_name, byte* p_prefix);
		public FuncPtr<ClassDBRegisterExtensionClassPropertySubgroup> classdb_register_extension_class_property_subgroup;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void ClassDBRegisterExtensionClassSignal(ExtensionClassLibraryPtr p_library, byte* p_class_name, byte* p_signal_name, PropertyInfo* p_argument_info, Int p_argument_count);
		public FuncPtr<ClassDBRegisterExtensionClassSignal> classdb_register_extension_class_signal;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void ClassDBUnregisterExtensionClass(ExtensionClassLibraryPtr p_library, byte* p_class_name);
		public FuncPtr<ClassDBUnregisterExtensionClass> classdb_unregister_extension_class; /* Unregistering a parent class before a class that inherits it will result in failure. Inheritors must be unregistered first. */

		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void GetLibraryPath(ExtensionClassLibraryPtr p_library, StringPtr r_path);
		public FuncPtr<GetLibraryPath> get_library_path;
	};

	// /* INITIALIZATION */

	public enum InitializationLevel {
		Core,
		Servers,
		Scene,
		Editor,
		MAX,
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct Initialization {
		/* Minimum initialization level required.
		 * If Core or Servers, the extension needs editor or game restart to take effect */
		public InitializationLevel minimum_initialization_level;
		/* Up to the user to supply when initializing */
		public IntPtr userdata;
		/* This function will be called multiple times for each initialization level. */
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void Initialize(IntPtr userdata, InitializationLevel p_level);
		public FuncPtr<Initialize> initialize;
		[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate void Deinitialize(IntPtr userdata, InitializationLevel p_level);
		public FuncPtr<Deinitialize> deinitialize;
	};

	/* Define a C function prototype that implements the function below and expose it to dlopen() (or similar).
	 * It will be called on initialization. The name must be an unique one specified in the .gdextension config file.
	 */

	[UnmanagedFunctionPointer(callingConvention: CallingConvention.Cdecl)] public unsafe delegate Bool InitializationFunction(Interface* p_interface, ExtensionClassLibraryPtr p_library, Initialization* r_initialization);
}
