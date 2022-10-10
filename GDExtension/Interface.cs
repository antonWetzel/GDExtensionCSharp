using System.Runtime.InteropServices;

namespace GDExtension;

//all named functions types are inline because a declared delegate type is always managed

/* VARIANT TYPES */

public enum VariantType : uint {
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
	Aabb,
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

	MAX,
}

public enum VariantOperator : uint {
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
	MAX,

}

/* VARIANT DATA I/O */

public enum CallErrorType : uint {
	Ok,
	InvalidMehtod,
	InvalidArgument,
	TooManyArguments,
	TooFewArguments,
	InstanceIsNull,
	MethoodNotConst,
}
[StructLayout(LayoutKind.Sequential)]
public struct CallError {
	CallErrorType error;
	int argument;
	int expected;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct InstanceBindingCallbacks {
	public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr> create_callback;
	public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, void> free_callback;
	public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, bool, bool> reference_callback;
}

/* EXTENSION CLASSES */

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
	char* name;
	PropertyInfo return_value;
	uint flags; // From ExtensionClassMethodFlags
	int id;
	PropertyInfo* arguments;
	uint argument_count;
	VariantPtr default_arguments;
	uint default_argument_count;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct ExtensionClassCreationInfo {
	public delegate* unmanaged[Cdecl]<ExtensionClassInstancePtr, StringNamePtr, VariantPtr, Bool> set_func;
	public delegate* unmanaged[Cdecl]<ExtensionClassInstancePtr, StringNamePtr, VariantPtr, Bool> get_func;
	public delegate* unmanaged[Cdecl]<ExtensionClassInstancePtr, uint*, PropertyInfo*> get_property_list_func;
	public delegate* unmanaged[Cdecl]<ExtensionClassInstancePtr, PropertyInfo*, void> free_property_list_func;
	public delegate* unmanaged[Cdecl]<ExtensionClassInstancePtr, StringNamePtr, Bool> property_can_revert_func;
	public delegate* unmanaged[Cdecl]<ExtensionClassInstancePtr, StringNamePtr, VariantPtr, Bool> property_get_revert_func;
	public delegate* unmanaged[Cdecl]<ExtensionClassInstancePtr, int, void> notification_func;
	public delegate* unmanaged[Cdecl]<ExtensionClassInstancePtr, InternalString*, void> to_string_func;
	public delegate* unmanaged[Cdecl]<ExtensionClassInstancePtr, void> reference_func;
	public delegate* unmanaged[Cdecl]<ExtensionClassInstancePtr, void> unreference_func;
	public delegate* unmanaged[Cdecl]<IntPtr, ObjectPtr> create_instance_func; /* this one is mandatory */
	public delegate* unmanaged[Cdecl]<IntPtr, ExtensionClassInstancePtr, void> free_instance_func; /* this one is mandatory */
	public delegate* unmanaged[Cdecl]<IntPtr, byte*, delegate* unmanaged[Cdecl]<ExtensionClassInstancePtr, TypePtr*, TypePtr, void>> get_virtual_func;
	public delegate* unmanaged[Cdecl]<ExtensionClassInstancePtr, long> get_rid_func;
	public IntPtr class_userdata;
}

public unsafe struct ExtensionClassLibraryPtr {
	IntPtr ptr;
	public static implicit operator IntPtr(ExtensionClassLibraryPtr ptr) => ptr.ptr;
	public static implicit operator ExtensionClassLibraryPtr(IntPtr ptr) => new ExtensionClassLibraryPtr() { ptr = ptr };
}

/* Method */

public enum ExtensionClassMethodFlags : uint {
	Normal = 1,
	Editor = 2,
	Const = 4,
	Virtual = 8,
	Vararg = 16,
	Static = 32,
	Default = Normal,
}

public enum ExtensionClassMethodArgumentMetadata : uint {
	None,
	IntIsInt8,
	IntIsInt16,
	IntIsInt32,
	IntIsInt64,
	IntIsUint8,
	IntIsUint16,
	IntIsUint32,
	IntIsUint64,
	RealIsFloat,
	RealIsDouble,
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct ExtensionClassMethodInfo {
	public char* name;
	public IntPtr method_userdata;
	public delegate* unmanaged[Cdecl]<IntPtr, ExtensionClassInstancePtr, VariantPtr*, Int, void> call_func;
	public delegate* unmanaged[Cdecl]<IntPtr, ExtensionClassInstancePtr, TypePtr*, TypePtr, void> ptrcall_func;
	public uint method_flags; /* ExtensionClassMethodFlags */
	public uint argument_count;
	public Bool has_return_value;
	public delegate* unmanaged[Cdecl]<IntPtr, int, VariantType> get_argument_type_func;
	public delegate* unmanaged[Cdecl]<IntPtr, int, PropertyInfo, void> get_argument_info_func; /* name and hint information for the argument can be omitted in release builds. Class name should always be present if it applies. */
	public delegate* unmanaged[Cdecl]<IntPtr, int, ExtensionClassMethodArgumentMetadata> get_argument_metadata_func;
	public uint default_argument_count;
	public VariantPtr* default_arguments;
}

/* SCRIPT INSTANCE EXTENSION */

public unsafe struct ExtensionScriptInstanceDataPtr {
	IntPtr ptr;
	public static implicit operator IntPtr(ExtensionScriptInstanceDataPtr ptr) => ptr.ptr;
	public static implicit operator ExtensionScriptInstanceDataPtr(IntPtr ptr) => new ExtensionScriptInstanceDataPtr() { ptr = ptr };
}
public unsafe struct ExtensionScriptLanguagePtr {
	IntPtr ptr;
	public static implicit operator IntPtr(ExtensionScriptLanguagePtr ptr) => ptr.ptr;
	public static implicit operator ExtensionScriptLanguagePtr(IntPtr ptr) => new ExtensionScriptLanguagePtr() { ptr = ptr };
}

public unsafe struct ScriptInstancePtr {
	IntPtr ptr;
	public static implicit operator IntPtr(ScriptInstancePtr ptr) => ptr.ptr;
	public static implicit operator ScriptInstancePtr(IntPtr ptr) => new ScriptInstancePtr() { ptr = ptr };
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct ExtensionScriptInstanceInfo {
	public delegate* unmanaged[Cdecl]<ExtensionScriptInstanceDataPtr, StringNamePtr, VariantPtr, Bool> set_func;
	public delegate* unmanaged[Cdecl]<ExtensionScriptInstanceDataPtr, StringNamePtr, VariantPtr, Bool> get_func;
	public delegate* unmanaged[Cdecl]<ExtensionScriptInstanceDataPtr, uint*, PropertyInfo*> get_property_list_func;
	public delegate* unmanaged[Cdecl]<ExtensionScriptInstanceDataPtr, PropertyInfo*, void> free_property_list_func;
	public delegate* unmanaged[Cdecl]<ExtensionScriptInstanceDataPtr, StringNamePtr, Bool*, VariantType> get_property_type_func;

	public delegate* unmanaged[Cdecl]<ExtensionScriptInstanceDataPtr, StringNamePtr, Bool> property_can_revert_func;
	public delegate* unmanaged[Cdecl]<ExtensionScriptInstanceDataPtr, StringNamePtr, VariantPtr, Bool> property_get_revert_func;

	public delegate* unmanaged[Cdecl]<ExtensionScriptInstanceDataPtr, ObjectPtr> get_owner_func;
	public delegate* unmanaged[Cdecl]<ExtensionScriptInstanceDataPtr, delegate* unmanaged[Cdecl]<StringNamePtr, VariantPtr, IntPtr, void>, IntPtr, void> get_property_state_func;

	public delegate* unmanaged[Cdecl]<ExtensionScriptInstanceDataPtr, uint*, MethodInfo*> get_method_list_func;
	public delegate* unmanaged[Cdecl]<ExtensionScriptInstanceDataPtr, MethodInfo*, void> free_method_list_func;

	public delegate* unmanaged[Cdecl]<ExtensionScriptInstanceDataPtr, StringNamePtr, Bool> has_method_func;

	public delegate* unmanaged[Cdecl]<ExtensionScriptInstanceDataPtr, StringNamePtr, VariantPtr*, Int, VariantPtr, CallError*, void> call_func;
	public delegate* unmanaged[Cdecl]<ExtensionScriptInstanceDataPtr, int> notification_func;

	public delegate* unmanaged[Cdecl]<ExtensionScriptInstanceDataPtr, Bool*, byte*> to_string_func;

	public delegate* unmanaged[Cdecl]<ExtensionScriptInstanceDataPtr, void> refcount_incremented_func;
	public delegate* unmanaged[Cdecl]<ExtensionScriptInstanceDataPtr, Bool> refcount_decremented_func;

	public delegate* unmanaged[Cdecl]<ExtensionScriptInstanceDataPtr, ObjectPtr> get_script_func;

	public delegate* unmanaged[Cdecl]<ExtensionScriptInstanceDataPtr, Bool> is_placeholder_func;

	public delegate*<ExtensionScriptInstanceDataPtr, StringNamePtr, VariantPtr, Bool> set_fallback_func;
	public delegate* unmanaged[Cdecl]<ExtensionScriptInstanceDataPtr, StringNamePtr, VariantPtr, Bool> get_fallback_func;

	public delegate* unmanaged[Cdecl]<ExtensionScriptInstanceDataPtr, ExtensionScriptLanguagePtr> get_language_func;

	public delegate* unmanaged[Cdecl]<ExtensionScriptInstanceDataPtr, void> free_func;
}
/* INTERFACE */
[StructLayout(LayoutKind.Sequential)]
public unsafe struct Interface {
	public uint version_major;
	public uint version_minor;
	public uint version_patch;

	public byte* version_string;

	/* GODOT CORE */
	public unsafe delegate* unmanaged[Cdecl]<nint, IntPtr> mem_alloc;
	public unsafe delegate* unmanaged[Cdecl]<IntPtr, nint, IntPtr> mem_realloc;
	public unsafe delegate* unmanaged[Cdecl]<IntPtr, void> mem_free;

	public unsafe delegate* unmanaged[Cdecl]<string, byte*, byte*, int, void> print_error;
	public unsafe delegate* unmanaged[Cdecl]<byte*, byte*, byte*, int, void> print_warning;
	public unsafe delegate* unmanaged[Cdecl]<byte*, byte*, byte*, int, void> print_script_error;

	public unsafe delegate* unmanaged[Cdecl]<byte*, long> get_native_struct_size;

	/* GODOT VARIANT */

	/* variant general */
	public unsafe delegate* unmanaged[Cdecl]<VariantPtr, VariantPtr, void> variant_new_copy;
	public unsafe delegate* unmanaged[Cdecl]<VariantPtr, void> variant_new_nil;
	public unsafe delegate* unmanaged[Cdecl]<VariantPtr, void> variant_destroy;

	/* variant type */
	public unsafe delegate* unmanaged[Cdecl]<VariantPtr, StringNamePtr, VariantPtr*, long, VariantPtr, CallError*, void> variant_call;
	public unsafe delegate* unmanaged[Cdecl]<VariantType, StringNamePtr, VariantPtr*, long, VariantPtr, CallError*, void> variant_call_static;
	public unsafe delegate* unmanaged[Cdecl]<VariantOperator, VariantPtr, VariantPtr, VariantPtr, bool*, void> variant_evaluate;
	public unsafe delegate* unmanaged[Cdecl]<VariantPtr, VariantPtr, VariantPtr, bool*, void> variant_set;
	public unsafe delegate* unmanaged[Cdecl]<VariantPtr, StringNamePtr, VariantPtr, bool*, void> variant_set_named;
	public unsafe delegate* unmanaged[Cdecl]<VariantPtr, VariantPtr, VariantPtr, bool*, void> variant_set_keyed;
	public unsafe delegate* unmanaged[Cdecl]<VariantPtr, long, VariantPtr, bool*, bool*, void> variant_set_indexed;
	public unsafe delegate* unmanaged[Cdecl]<VariantPtr, VariantPtr, VariantPtr, bool*, void> variant_get;
	public unsafe delegate* unmanaged[Cdecl]<VariantPtr, StringNamePtr, VariantPtr, bool*, void> variant_get_named;
	public unsafe delegate* unmanaged[Cdecl]<VariantPtr, VariantPtr, VariantPtr, bool*, void> variant_get_keyed;
	public unsafe delegate* unmanaged[Cdecl]<VariantPtr, long, VariantPtr, bool*, bool*, void> variant_get_indexed;
	public unsafe delegate* unmanaged[Cdecl]<VariantPtr, VariantPtr, bool*, bool> variant_iter_init;
	public unsafe delegate* unmanaged[Cdecl]<VariantPtr, VariantPtr, bool*, bool> variant_iter_next;
	public unsafe delegate* unmanaged[Cdecl]<VariantPtr, VariantPtr, VariantPtr, bool*, void> variant_iter_get;
	public unsafe delegate* unmanaged[Cdecl]<VariantPtr, long> variant_hash;
	public unsafe delegate* unmanaged[Cdecl]<VariantPtr, long, long> variant_recursive_hash;
	public unsafe delegate* unmanaged[Cdecl]<VariantPtr, VariantPtr, bool> variant_hash_compare;
	public unsafe delegate* unmanaged[Cdecl]<VariantPtr, bool> variant_booleanize;
	public unsafe delegate* unmanaged[Cdecl]<VariantPtr, VariantPtr, bool, void> variant_duplicate;
	public unsafe delegate* unmanaged[Cdecl]<VariantPtr, InternalString*, void> variant_stringify;

	public unsafe delegate* unmanaged[Cdecl]<VariantPtr, VariantType> variant_get_type;
	public unsafe delegate* unmanaged[Cdecl]<VariantPtr, StringNamePtr, bool> variant_has_method;
	public unsafe delegate* unmanaged[Cdecl]<VariantType, StringNamePtr, bool> variant_has_member;
	public unsafe delegate* unmanaged[Cdecl]<VariantPtr, VariantPtr, bool*, bool> variant_has_key;
	public unsafe delegate* unmanaged[Cdecl]<VariantType, InternalString*, void> variant_get_type_name;
	public unsafe delegate* unmanaged[Cdecl]<VariantType, VariantType, bool> variant_can_convert;
	public unsafe delegate* unmanaged[Cdecl]<VariantType, VariantType, bool> variant_can_convert_strict;

	/* ptrcalls */
	public unsafe delegate* unmanaged[Cdecl]<VariantType, delegate* unmanaged[Cdecl]<VariantPtr, TypePtr, void>> get_variant_from_type_constructor;
	public unsafe delegate* unmanaged[Cdecl]<VariantType, delegate* unmanaged[Cdecl]<TypePtr, VariantPtr, void>> get_variant_to_type_constructor;
	public unsafe delegate* unmanaged[Cdecl]<VariantOperator, VariantType, VariantType, delegate* unmanaged[Cdecl]<TypePtr, TypePtr, TypePtr, void>> variant_get_ptr_operator_evaluator;
	public unsafe delegate* unmanaged[Cdecl]<VariantType, byte*, long, delegate* unmanaged[Cdecl]<TypePtr, TypePtr*, TypePtr, int, void>> variant_get_ptr_builtin_method;
	public unsafe delegate* unmanaged[Cdecl]<VariantType, int, delegate* unmanaged[Cdecl]<TypePtr, TypePtr*, void>> variant_get_ptr_constructor;
	public unsafe delegate* unmanaged[Cdecl]<VariantType, delegate* unmanaged[Cdecl]<TypePtr, void>> variant_get_ptr_destructor;
	public unsafe delegate* unmanaged[Cdecl]<VariantType, VariantPtr, VariantPtr*, int, CallError*, void> variant_construct;
	public unsafe delegate* unmanaged[Cdecl]<VariantType, byte*, delegate* unmanaged[Cdecl]<TypePtr, TypePtr, void>> variant_get_ptr_setter;
	public unsafe delegate* unmanaged[Cdecl]<VariantType, byte*, delegate* unmanaged[Cdecl]<TypePtr, TypePtr, void>> variant_get_ptr_getter;
	public unsafe delegate* unmanaged[Cdecl]<VariantType, delegate* unmanaged[Cdecl]<TypePtr, Int, TypePtr, void>> variant_get_ptr_indexed_setter;
	public unsafe delegate* unmanaged[Cdecl]<VariantType, delegate* unmanaged[Cdecl]<TypePtr, Int, TypePtr, void>> variant_get_ptr_indexed_getter;
	public unsafe delegate* unmanaged[Cdecl]<VariantType, delegate* unmanaged[Cdecl]<TypePtr, TypePtr, TypePtr, void>> variant_get_ptr_keyed_setter;
	public unsafe delegate* unmanaged[Cdecl]<VariantType, delegate* unmanaged[Cdecl]<TypePtr, TypePtr, TypePtr, void>> variant_get_ptr_keyed_getter;
	public unsafe delegate* unmanaged[Cdecl]<VariantType, delegate* unmanaged[Cdecl]<VariantPtr, VariantPtr, uint>> variant_get_ptr_keyed_checker;
	public unsafe delegate* unmanaged[Cdecl]<VariantType, byte*, VariantPtr, void> variant_get_constant_value;
	public unsafe delegate* unmanaged[Cdecl]<byte*, long, delegate* unmanaged[Cdecl]<TypePtr, TypePtr*, int, void>> variant_get_ptr_utility_function;

	/*  extra utilities */

	public unsafe delegate* unmanaged[Cdecl]<InternalString*, byte*, void> string_new_with_latin1_bytes;
	public unsafe delegate* unmanaged[Cdecl]<InternalString*, byte*, void> string_new_with_utf8_bytes;
	public unsafe delegate* unmanaged[Cdecl]<InternalString*, char*, void> string_new_with_utf16_bytes;
	public unsafe delegate* unmanaged[Cdecl]<InternalString*, int*, void> string_new_with_utf32_bytes;
	public unsafe delegate* unmanaged[Cdecl]<InternalString*, char*, void> string_new_with_wide_bytes; //not sure if char is the right size for w_char
	public unsafe delegate* unmanaged[Cdecl]<InternalString*, byte*, long, void> string_new_with_latin1_bytes_and_len;
	public unsafe delegate* unmanaged[Cdecl]<InternalString*, byte*, long, void> string_new_with_utf8_bytes_and_len;
	public unsafe delegate* unmanaged[Cdecl]<InternalString*, char*, long, void> string_new_with_utf16_bytes_and_len;
	public unsafe delegate* unmanaged[Cdecl]<InternalString*, int*, long, void> string_new_with_utf32_bytes_and_len;
	public unsafe delegate* unmanaged[Cdecl]<InternalString*, char*, long, void> string_new_with_wide_bytes_and_len; //not sure if char is the right size for w_char
	/* Information about the following functions:
	 * - The return value is the resulting encoded string length.
	 * - The length returned is in characters, not in bytes. It also does not include a trailing zero.
	 * - These functions also do not write trailing zero, If you need it, write it yourself at the position indicated by the length (and make sure to allocate it).
	 * - Passing NULL in r_text means only the length is computed (again, without including trailing zero).
	 * - p_max_write_length argument is in characters, not bytes. It will be ignored if r_text is NULL.
	 * - p_max_write_length argument does not affect the return value, it's only to cap write length.
	 */
	public unsafe delegate* unmanaged[Cdecl]<InternalString*, byte*, long, long> string_to_latin1_bytes;
	public unsafe delegate* unmanaged[Cdecl]<InternalString*, byte*, long, long> string_to_utf8_bytes;
	public unsafe delegate* unmanaged[Cdecl]<InternalString*, char*, long, long> string_to_utf16_bytes;
	public unsafe delegate* unmanaged[Cdecl]<InternalString*, int*, long, long> string_to_utf32_bytes;
	public unsafe delegate* unmanaged[Cdecl]<InternalString*, char*, long, long> string_to_wide_bytes;
	public unsafe delegate* unmanaged[Cdecl]<InternalString*, long, int*> string_operator_index;
	public unsafe delegate* unmanaged[Cdecl]<InternalString*, long, int*> string_operator_index_;

	/* Packed array functions */

	public unsafe delegate* unmanaged[Cdecl]<TypePtr, long, byte*> packed_byte_array_operator_index; // should be a PackedByteArray
	public unsafe delegate* unmanaged[Cdecl]<TypePtr, long, byte*> packed_byte_array_operator_index_; // should be a PackedByteArray

	public unsafe delegate* unmanaged[Cdecl]<TypePtr, long, TypePtr> packed_color_array_operator_index; // should be a PackedColorArray, returns Color ptr
	public unsafe delegate* unmanaged[Cdecl]<TypePtr, long, TypePtr> packed_color_array_operator_index_; // should be a PackedColorArray, returns Color ptr

	public unsafe delegate* unmanaged[Cdecl]<TypePtr, long, float*> packed_float32_array_operator_index; // should be a PackedFloat32Array
	public unsafe delegate* unmanaged[Cdecl]<TypePtr, long, float*> packed_float32_array_operator_index_; // should be a PackedFloat32Array
	public unsafe delegate* unmanaged[Cdecl]<TypePtr, long, double*> packed_float64_array_operator_index; // should be a PackedFloat64Array
	public unsafe delegate* unmanaged[Cdecl]<TypePtr, long, double*> packed_float64_array_operator_index_; // should be a PackedFloat64Array

	public unsafe delegate* unmanaged[Cdecl]<TypePtr, long, int*> packed_int32_array_operator_index; // should be a Packedlong32Array
	public unsafe delegate* unmanaged[Cdecl]<TypePtr, long, int*> packed_int32_array_operator_index_; // should be a Packedlong32Array
	public unsafe delegate* unmanaged[Cdecl]<TypePtr, long, long*> packed_int64_array_operator_index; // should be a Packedlong32Array
	public unsafe delegate* unmanaged[Cdecl]<TypePtr, long, long*> packed_int64_array_operator_index_; // should be a Packedlong32Array

	public unsafe delegate* unmanaged[Cdecl]<TypePtr, long, InternalString*> packed_string_array_operator_index; // should be a PackedStringArray
	public unsafe delegate* unmanaged[Cdecl]<TypePtr, long, InternalString*> packed_string_array_operator_index_; // should be a PackedStringArray

	public unsafe delegate* unmanaged[Cdecl]<TypePtr, long, TypePtr> packed_vector2_array_operator_index; // should be a PackedVector2Array, returns Vector2 ptr
	public unsafe delegate* unmanaged[Cdecl]<TypePtr, long, TypePtr> packed_vector2_array_operator_index_; // should be a PackedVector2Array, returns Vector2 ptr
	public unsafe delegate* unmanaged[Cdecl]<TypePtr, long, TypePtr> packed_vector3_array_operator_index; // should be a PackedVector3Array, returns Vector3 ptr
	public unsafe delegate* unmanaged[Cdecl]<TypePtr, long, TypePtr> packed_vector3_array_operator_index_; // should be a PackedVector3Array, returns Vector3 ptr

	public unsafe delegate* unmanaged[Cdecl]<TypePtr, long, VariantPtr> array_operator_index; // should be an Array ptr
	public unsafe delegate* unmanaged[Cdecl]<TypePtr, long, VariantPtr> array_operator_index_; // should be an Array ptr

	/* Dictionary functions */

	public unsafe delegate* unmanaged[Cdecl]<TypePtr, VariantPtr, VariantPtr> dictionary_operator_index; // should be an Dictionary ptr
	public unsafe delegate* unmanaged[Cdecl]<TypePtr, VariantPtr, VariantPtr> dictionary_operator_index_; // should be an Dictionary ptr

	/* OBJECT */

	public unsafe delegate* unmanaged[Cdecl]<MethodBindPtr, ObjectPtr, VariantPtr*, long, VariantPtr, CallError*, void> object_method_bind_call;
	public unsafe delegate* unmanaged[Cdecl]<MethodBindPtr, ObjectPtr, TypePtr*, TypePtr, void> object_method_bind_ptrcall;
	public unsafe delegate* unmanaged[Cdecl]<ObjectPtr, void> object_destroy;
	public unsafe delegate* unmanaged[Cdecl]<byte*, ObjectPtr> global_get_singleton;

	public unsafe delegate* unmanaged[Cdecl]<ObjectPtr, IntPtr, IntPtr, InstanceBindingCallbacks*, void> object_set_instance_binding;
	public unsafe delegate* unmanaged[Cdecl]<ObjectPtr, IntPtr, InstanceBindingCallbacks*, IntPtr> object_get_instance_binding;

	public unsafe delegate* unmanaged[Cdecl]<ObjectPtr, byte*, ExtensionClassInstancePtr, void> object_set_instance; /* should be a registered extension class and should extend the object's class. */

	public unsafe delegate* unmanaged[Cdecl]<ObjectPtr, IntPtr, ObjectPtr> object_cast_to;
	public unsafe delegate* unmanaged[Cdecl]<ObjectInstanceID, ObjectPtr> object_get_instance_from_id;
	public unsafe delegate* unmanaged[Cdecl]<ObjectPtr, ObjectInstanceID> object_get_instance_id;

	/* SCRIPT INSTANCE */

	public unsafe delegate* unmanaged[Cdecl]<ExtensionScriptInstanceInfo*, ExtensionScriptInstanceDataPtr, ScriptInstancePtr> script_instance_create;

	/* CLASSDB */
	public unsafe delegate* unmanaged[Cdecl]<byte*, ObjectPtr> classdb_construct_object; /* The passed class must be a built-in godot class, or an already-registered extension class. In both case, object_set_instance should be called to fully initialize the object. */
	public unsafe delegate* unmanaged[Cdecl]<byte*, byte*, long, MethodBindPtr> classdb_get_method_bind;
	public unsafe delegate* unmanaged[Cdecl]<byte*, IntPtr> classdb_get_class_tag;

	/* CLASSDB EXTENSION */

	public unsafe delegate* unmanaged[Cdecl]<ExtensionClassLibraryPtr, byte*, byte*, ExtensionClassCreationInfo*, void> classdb_register_extension_class;
	public unsafe delegate* unmanaged[Cdecl]<ExtensionClassLibraryPtr, byte*, ExtensionClassMethodInfo*, void> classdb_register_extension_class_method;
	public unsafe delegate* unmanaged[Cdecl]<ExtensionClassLibraryPtr, byte*, byte*, byte*, long, bool, void> classdb_register_extension_class_integer_ant;
	public unsafe delegate* unmanaged[Cdecl]<ExtensionClassLibraryPtr, byte*, PropertyInfo*, byte*, byte*, void> classdb_register_extension_class_property;
	public unsafe delegate* unmanaged[Cdecl]<ExtensionClassLibraryPtr, byte*, byte*, byte*, void> classdb_register_extension_class_property_group;
	public unsafe delegate* unmanaged[Cdecl]<ExtensionClassLibraryPtr, byte*, byte*, byte*, void> classdb_register_extension_class_property_subgroup;
	public unsafe delegate* unmanaged[Cdecl]<ExtensionClassLibraryPtr, byte*, byte*, PropertyInfo*, long, void> classdb_register_extension_class_signal;
	public unsafe delegate* unmanaged[Cdecl]<ExtensionClassLibraryPtr, byte*, void> classdb_unregister_extension_class; /* Unregistering a parent class before a class that inherits it will result in failure. Inheritors must be unregistered first. */

	public unsafe delegate* unmanaged[Cdecl]<ExtensionClassLibraryPtr, InternalString*, void> get_library_path;

}

/* INITIALIZATION */

public enum InitializationLevel : uint {
	Core,
	Servers,
	Scene,
	Editor,
	MAX,
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct Initialization {
	public InitializationLevel minimum_initialization_level;
	public IntPtr userdata;
	public delegate* unmanaged[Cdecl]<IntPtr, InitializationLevel, void> initialize;
	public delegate* unmanaged[Cdecl]<IntPtr, InitializationLevel, void> deinitialize;
}
