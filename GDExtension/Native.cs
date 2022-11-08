//genereated with ClangSharpPInvokeGenerator and edited

namespace GDExtension;

public static unsafe partial class Native {

	public static Interface gdInterface;
	public static IntPtr gdLibrary;

	public enum CallErrorType {
		Ok,
		InvalidMethod,
		InvalidArgument,
		TooManyArguments,
		TooFewArguments,
		InstanceIsNull,
		MethodNotConst,
	}

	public partial struct CallError {
		public CallErrorType error;

		//[NativeTypeName("int32_t")]
		public int argument;

		//[NativeTypeName("int32_t")]
		public int expected;
	}

	public partial struct InstanceBindingCallbacks {
		//[NativeTypeName("InstanceBindingCreateCallback")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr> create_callback;

		//[NativeTypeName("InstanceBindingFreeCallback")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, void> free_callback;

		//[NativeTypeName("InstanceBindingReferenceCallback")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, byte, byte> reference_callback;
	}

	public partial struct PropertyInfo {
		public Variant.Type type;

		//[NativeTypeName("const char *")]
		public sbyte* name;

		//[NativeTypeName("const char *")]
		public sbyte* class_name;

		//[NativeTypeName("uint32_t")]
		public PropertyHint hint;

		//[NativeTypeName("const char *")]
		public sbyte* hint_string;

		//[NativeTypeName("uint32_t")]
		public PropertyUsageFlags usage;
	}

	public partial struct MethodInfo {
		//[NativeTypeName("const char *")]
		public sbyte* name;

		public PropertyInfo return_value;

		//[NativeTypeName("uint32_t")]
		public uint flags;

		//[NativeTypeName("int32_t")]
		public int id;

		public PropertyInfo* arguments;

		//[NativeTypeName("uint32_t")]
		public uint argument_count;

		//[NativeTypeName("VariantPtr")]
		public IntPtr default_arguments;

		//[NativeTypeName("uint32_t")]
		public uint default_argument_count;
	}

	public partial struct ExtensionClassCreationInfo {
		//[NativeTypeName("Bool")]
		public bool is_virtual;

		//[NativeTypeName("Bool")]
		public bool is_abstract;

		//[NativeTypeName("ExtensionClassSet")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, byte> set_func;

		//[NativeTypeName("ExtensionClassGet")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, byte> get_func;

		//[NativeTypeName ("ExtensionClassGetPropertyList")]
		public delegate* unmanaged[Cdecl]<IntPtr, uint*, PropertyInfo*> get_property_list_func;

		//[NativeTypeName ("ExtensionClassFreePropertyList")]
		public delegate* unmanaged[Cdecl]<IntPtr, PropertyInfo*, void> free_property_list_func;

		//[NativeTypeName ("ExtensionClassPropertyCanRevert")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, byte> property_can_revert_func;

		//[NativeTypeName ("ExtensionClassPropertyGetRevert")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, byte> property_get_revert_func;

		//[NativeTypeName ("ExtensionClassNotification")]
		public delegate* unmanaged[Cdecl]<IntPtr, int, void> notification_func;

		//[NativeTypeName ("ExtensionClassToString")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void> to_string_func;

		//[NativeTypeName ("ExtensionClassReference")]
		public delegate* unmanaged[Cdecl]<IntPtr, void> reference_func;

		//[NativeTypeName ("ExtensionClassUnreference")]
		public delegate* unmanaged[Cdecl]<IntPtr, void> unreference_func;

		//[NativeTypeName ("ExtensionClassCreateInstance")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr> create_instance_func;

		//[NativeTypeName ("ExtensionClassFreeInstance")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void> free_instance_func;

		//[NativeTypeName ("ExtensionClassGetVirtual")]
		public delegate* unmanaged[Cdecl]<IntPtr, sbyte*, delegate* unmanaged[Cdecl]<IntPtr, IntPtr*, IntPtr, void>> get_virtual_func;

		//[NativeTypeName ("ExtensionClassGetRID")]
		public delegate* unmanaged[Cdecl]<IntPtr, ulong> get_rid_func;

		public IntPtr class_userdata;
	}

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
		IntIsInt8,
		IntIsInt16,
		IntIsInt32,
		IntIsInt64,
		IntIsIntT8,
		IntIsIntT16,
		IntIsIntT32,
		IntIsIntT64,
		RealIsFloat,
		RealIsDouble,
	}

	public partial struct ExtensionClassMethodInfo {
		//[NativeTypeName ("const char *")]
		public sbyte* name;

		public IntPtr method_userdata;

		//[NativeTypeName ("ExtensionClassMethodCall")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr*, long, IntPtr, CallError*, void> call_func;

		//[NativeTypeName ("ExtensionClassMethodPtrCall")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr*, IntPtr, void> ptrcall_func;

		//[NativeTypeName ("uint32_t")]
		public ExtensionClassMethodFlags method_flags;

		//[NativeTypeName ("uint32_t")]
		public uint argument_count;

		//[NativeTypeName ("Bool")]
		public bool has_return_value;

		//[NativeTypeName ("ExtensionClassMethodGetArgumentType")]
		public delegate* unmanaged[Cdecl]<IntPtr, int, Variant.Type> get_argument_type_func;

		//[NativeTypeName ("ExtensionClassMethodGetArgumentInfo")]
		public delegate* unmanaged[Cdecl]<IntPtr, int, PropertyInfo*, void> get_argument_info_func;

		//[NativeTypeName ("ExtensionClassMethodGetArgumentMetadata")]
		public delegate* unmanaged[Cdecl]<IntPtr, int, ExtensionClassMethodArgumentMetadata> get_argument_metadata_func;

		//[NativeTypeName ("uint32_t")]
		public uint default_argument_count;

		//[NativeTypeName ("VariantPtr *")]
		public IntPtr* default_arguments;
	}

	public partial struct ExtensionScriptInstanceInfo {
		//[NativeTypeName ("ExtensionScriptInstanceSet")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, byte> set_func;

		//[NativeTypeName ("ExtensionScriptInstanceGet")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, byte> get_func;

		//[NativeTypeName ("ExtensionScriptInstanceGetPropertyList")]
		public delegate* unmanaged[Cdecl]<IntPtr, uint*, PropertyInfo*> get_property_list_func;

		//[NativeTypeName ("ExtensionScriptInstanceFreePropertyList")]
		public delegate* unmanaged[Cdecl]<IntPtr, PropertyInfo*, void> free_property_list_func;

		//[NativeTypeName ("ExtensionScriptInstanceGetPropertyType")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, byte*, Variant.Type> get_property_type_func;

		//[NativeTypeName ("ExtensionScriptInstancePropertyCanRevert")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, byte> property_can_revert_func;

		//[NativeTypeName ("ExtensionScriptInstancePropertyGetRevert")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, byte> property_get_revert_func;

		//[NativeTypeName ("ExtensionScriptInstanceGetOwner")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr> get_owner_func;

		//[NativeTypeName ("ExtensionScriptInstanceGetPropertyState")]
		public delegate* unmanaged[Cdecl]<IntPtr, delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, void>, IntPtr, void> get_property_state_func;

		//[NativeTypeName ("ExtensionScriptInstanceGetMethodList")]
		public delegate* unmanaged[Cdecl]<IntPtr, uint*, MethodInfo*> get_method_list_func;

		//[NativeTypeName ("ExtensionScriptInstanceFreeMethodList")]
		public delegate* unmanaged[Cdecl]<IntPtr, MethodInfo*, void> free_method_list_func;

		//[NativeTypeName ("ExtensionScriptInstanceHasMethod")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, byte> has_method_func;

		//[NativeTypeName ("ExtensionScriptInstanceCall")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr*, long, IntPtr, CallError*, void> call_func;

		//[NativeTypeName ("ExtensionScriptInstanceNotification")]
		public delegate* unmanaged[Cdecl]<IntPtr, int, void> notification_func;

		//[NativeTypeName ("ExtensionScriptInstanceToString")]
		public delegate* unmanaged[Cdecl]<IntPtr, byte*, sbyte*> to_string_func;

		//[NativeTypeName ("ExtensionScriptInstanceRefCountIncremented")]
		public delegate* unmanaged[Cdecl]<IntPtr, void> refcount_incremented_func;

		//[NativeTypeName ("ExtensionScriptInstanceRefCountDecremented")]
		public delegate* unmanaged[Cdecl]<IntPtr, byte> refcount_decremented_func;

		//[NativeTypeName ("ExtensionScriptInstanceGetScript")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr> get_script_func;

		//[NativeTypeName ("ExtensionScriptInstanceIsPlaceholder")]
		public delegate* unmanaged[Cdecl]<IntPtr, byte> is_placeholder_func;

		//[NativeTypeName ("ExtensionScriptInstanceSet")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, byte> set_fallback_func;

		//[NativeTypeName ("ExtensionScriptInstanceGet")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, byte> get_fallback_func;

		//[NativeTypeName ("ExtensionScriptInstanceGetLanguage")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr> get_language_func;

		//[NativeTypeName ("ExtensionScriptInstanceFree")]
		public delegate* unmanaged[Cdecl]<IntPtr, void> free_func;
	}

	public partial struct Interface {
		//[NativeTypeName ("uint32_t")]
		public uint version_major;

		//[NativeTypeName ("uint32_t")]
		public uint version_minor;

		//[NativeTypeName ("uint32_t")]
		public uint version_patch;

		//[NativeTypeName ("const char *")]
		public sbyte* version_string;

		//[NativeTypeName ("void *(*)(size_t)")]
		public delegate* unmanaged[Cdecl]<nuint, IntPtr> mem_alloc;

		//[NativeTypeName ("void *(*)(void *, size_t)")]
		public delegate* unmanaged[Cdecl]<IntPtr, nuint, IntPtr> mem_realloc;

		//[NativeTypeName ("void (*)(void *)")]
		public delegate* unmanaged[Cdecl]<IntPtr, void> mem_free;

		//[NativeTypeName ("void (*)(const char *, const char *, const char *, int32_t)")]
		public delegate* unmanaged[Cdecl]<sbyte*, sbyte*, sbyte*, int, void> print_error;

		//[NativeTypeName ("void (*)(const char *, const char *, const char *, int32_t)")]
		public delegate* unmanaged[Cdecl]<sbyte*, sbyte*, sbyte*, int, void> print_warning;

		//[NativeTypeName ("void (*)(const char *, const char *, const char *, int32_t)")]
		public delegate* unmanaged[Cdecl]<sbyte*, sbyte*, sbyte*, int, void> print_script_error;

		//[NativeTypeName ("uint64_t (*)(const char *)")]
		public delegate* unmanaged[Cdecl]<sbyte*, ulong> get_native_struct_size;

		//[NativeTypeName ("void (*)(VariantPtr, const VariantPtr)")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void> variant_new_copy;

		//[NativeTypeName ("void (*)(VariantPtr)")]
		public delegate* unmanaged[Cdecl]<IntPtr, void> variant_new_nil;

		//[NativeTypeName ("void (*)(VariantPtr)")]
		public delegate* unmanaged[Cdecl]<IntPtr, void> variant_destroy;

		//[NativeTypeName ("void (*)(VariantPtr, const StringNamePtr, const VariantPtr *, const Int, VariantPtr, CallError *)")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr*, long, IntPtr, CallError*, void> variant_call;

		//[NativeTypeName ("void (*)(Variant.Type, const StringNamePtr, const VariantPtr *, const Int, VariantPtr, CallError *)")]
		public delegate* unmanaged[Cdecl]<Variant.Type, IntPtr, IntPtr*, long, IntPtr, CallError*, void> variant_call_static;

		//[NativeTypeName ("void (*)(Variant.Operator, const VariantPtr, const VariantPtr, VariantPtr, Bool *)")]
		public delegate* unmanaged[Cdecl]<Variant.Operator, IntPtr, IntPtr, IntPtr, bool*, void> variant_evaluate;

		//[NativeTypeName ("void (*)(VariantPtr, const VariantPtr, const VariantPtr, Bool *)")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, bool*, void> variant_set;

		//[NativeTypeName ("void (*)(VariantPtr, const StringNamePtr, const VariantPtr, Bool *)")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, bool*, void> variant_set_named;

		//[NativeTypeName ("void (*)(VariantPtr, const VariantPtr, const VariantPtr, Bool *)")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, bool*, void> variant_set_keyed;

		//[NativeTypeName ("void (*)(VariantPtr, Int, const VariantPtr, Bool *, Bool *)")]
		public delegate* unmanaged[Cdecl]<IntPtr, long, IntPtr, bool*, bool*, void> variant_set_indexed;

		//[NativeTypeName ("void (*)(const VariantPtr, const VariantPtr, VariantPtr, Bool *)")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, bool*, void> variant_get;

		//[NativeTypeName ("void (*)(const VariantPtr, const StringNamePtr, VariantPtr, Bool *)")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, bool*, void> variant_get_named;

		//[NativeTypeName ("void (*)(const VariantPtr, const VariantPtr, VariantPtr, Bool *)")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, bool*, void> variant_get_keyed;

		//[NativeTypeName ("void (*)(const VariantPtr, Int, VariantPtr, Bool *, Bool *)")]
		public delegate* unmanaged[Cdecl]<IntPtr, long, IntPtr, bool*, bool*, void> variant_get_indexed;

		//[NativeTypeName ("Bool (*)(const VariantPtr, VariantPtr, Bool *)")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, bool*, bool> variant_iter_init;

		//[NativeTypeName ("Bool (*)(const VariantPtr, VariantPtr, Bool *)")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, bool*, bool> variant_iter_next;

		//[NativeTypeName ("void (*)(const VariantPtr, VariantPtr, VariantPtr, Bool *)")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, bool*, void> variant_iter_get;

		//[NativeTypeName ("Int (*)(const VariantPtr)")]
		public delegate* unmanaged[Cdecl]<IntPtr, long> variant_hash;

		//[NativeTypeName ("Int (*)(const VariantPtr, Int)")]
		public delegate* unmanaged[Cdecl]<IntPtr, long, long> variant_recursive_hash;

		//[NativeTypeName ("Bool (*)(const VariantPtr, const VariantPtr)")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, bool> variant_hash_compare;

		//[NativeTypeName ("Bool (*)(const VariantPtr)")]
		public delegate* unmanaged[Cdecl]<IntPtr, bool> variant_booleanize;

		//[NativeTypeName ("void (*)(const VariantPtr, VariantPtr, Bool)")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, bool, void> variant_duplicate;

		//[NativeTypeName ("void (*)(const VariantPtr, StringPtr)")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void> variant_stringify;

		//[NativeTypeName ("Variant.Type (*)(const VariantPtr)")]
		public delegate* unmanaged[Cdecl]<IntPtr, Variant.Type> variant_get_type;

		//[NativeTypeName ("Bool (*)(const VariantPtr, const StringNamePtr)")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, bool> variant_has_method;

		//[NativeTypeName ("Bool (*)(Variant.Type, const StringNamePtr)")]
		public delegate* unmanaged[Cdecl]<Variant.Type, IntPtr, bool> variant_has_member;

		//[NativeTypeName ("Bool (*)(const VariantPtr, const VariantPtr, Bool *)")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, bool*, bool> variant_has_key;

		//[NativeTypeName ("void (*)(Variant.Type, StringPtr)")]
		public delegate* unmanaged[Cdecl]<Variant.Type, IntPtr, void> variant_get_type_name;

		//[NativeTypeName ("Bool (*)(Variant.Type, Variant.Type)")]
		public delegate* unmanaged[Cdecl]<Variant.Type, Variant.Type, bool> variant_can_convert;

		//[NativeTypeName ("Bool (*)(Variant.Type, Variant.Type)")]
		public delegate* unmanaged[Cdecl]<Variant.Type, Variant.Type, bool> variant_can_convert_strict;

		//[NativeTypeName ("VariantFromTypeConstructorFunc (*)(Variant.Type)")]
		public delegate* unmanaged[Cdecl]<Variant.Type, delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void>> get_variant_from_type_constructor;

		//[NativeTypeName ("TypeFromVariantConstructorFunc (*)(Variant.Type)")]
		public delegate* unmanaged[Cdecl]<Variant.Type, delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void>> get_variant_to_type_constructor;

		//[NativeTypeName ("PtrOperatorEvaluator (*)(Variant.Operator, Variant.Type, Variant.Type)")]
		public delegate* unmanaged[Cdecl]<Variant.Operator, Variant.Type, Variant.Type, delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, void>> variant_get_ptr_operator_evaluator;

		//[NativeTypeName ("PtrBuiltInMethod (*)(Variant.Type, const char *, Int)")]
		public delegate* unmanaged[Cdecl]<Variant.Type, sbyte*, long, delegate* unmanaged[Cdecl]<IntPtr, IntPtr*, IntPtr, int, void>> variant_get_ptr_builtin_method;

		//[NativeTypeName ("PtrConstructor (*)(Variant.Type, int32_t)")]
		public delegate* unmanaged[Cdecl]<Variant.Type, int, delegate* unmanaged[Cdecl]<IntPtr, IntPtr*, void>> variant_get_ptr_constructor;

		//[NativeTypeName ("PtrDestructor (*)(Variant.Type)")]
		public delegate* unmanaged[Cdecl]<Variant.Type, delegate* unmanaged[Cdecl]<IntPtr, void>> variant_get_ptr_destructor;

		//[NativeTypeName ("void (*)(Variant.Type, VariantPtr, const VariantPtr *, int32_t, CallError *)")]
		public delegate* unmanaged[Cdecl]<Variant.Type, IntPtr, IntPtr*, int, CallError*, void> variant_construct;

		//[NativeTypeName ("PtrSetter (*)(Variant.Type, const char *)")]
		public delegate* unmanaged[Cdecl]<Variant.Type, sbyte*, delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void>> variant_get_ptr_setter;

		//[NativeTypeName ("PtrGetter (*)(Variant.Type, const char *)")]
		public delegate* unmanaged[Cdecl]<Variant.Type, sbyte*, delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void>> variant_get_ptr_getter;

		//[NativeTypeName ("PtrIndexedSetter (*)(Variant.Type)")]
		public delegate* unmanaged[Cdecl]<Variant.Type, delegate* unmanaged[Cdecl]<IntPtr, long, IntPtr, void>> variant_get_ptr_indexed_setter;

		//[NativeTypeName ("PtrIndexedGetter (*)(Variant.Type)")]
		public delegate* unmanaged[Cdecl]<Variant.Type, delegate* unmanaged[Cdecl]<IntPtr, long, IntPtr, void>> variant_get_ptr_indexed_getter;

		//[NativeTypeName ("PtrKeyedSetter (*)(Variant.Type)")]
		public delegate* unmanaged[Cdecl]<Variant.Type, delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, void>> variant_get_ptr_keyed_setter;

		//[NativeTypeName ("PtrKeyedGetter (*)(Variant.Type)")]
		public delegate* unmanaged[Cdecl]<Variant.Type, delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, void>> variant_get_ptr_keyed_getter;

		//[NativeTypeName ("PtrKeyedChecker (*)(Variant.Type)")]
		public delegate* unmanaged[Cdecl]<Variant.Type, delegate* unmanaged[Cdecl]<IntPtr, IntPtr, uint>> variant_get_ptr_keyed_checker;

		//[NativeTypeName ("void (*)(Variant.Type, const char *, VariantPtr)")]
		public delegate* unmanaged[Cdecl]<Variant.Type, sbyte*, IntPtr, void> variant_get_constant_value;

		//[NativeTypeName ("PtrUtilityFunction (*)(const char *, Int)")]
		public delegate* unmanaged[Cdecl]<sbyte*, long, delegate* unmanaged[Cdecl]<IntPtr, IntPtr*, int, void>> variant_get_ptr_utility_function;

		//[NativeTypeName ("void (*)(StringPtr, const char *)")]
		public delegate* unmanaged[Cdecl]<IntPtr, sbyte*, void> string_new_with_latin1_chars;

		//[NativeTypeName ("void (*)(StringPtr, const char *)")]
		public delegate* unmanaged[Cdecl]<IntPtr, sbyte*, void> string_new_with_utf8_chars;

		//[NativeTypeName ("void (*)(StringPtr, const char16_t *)")]
		public delegate* unmanaged[Cdecl]<IntPtr, char*, void> string_new_with_utf16_chars;

		//[NativeTypeName ("void (*)(StringPtr, const char32_t *)")]
		public delegate* unmanaged[Cdecl]<IntPtr, int*, void> string_new_with_utf32_chars;

		//[NativeTypeName("void (*)(StringPtr, const wchar_t *)")]
		public delegate* unmanaged[Cdecl]<IntPtr, char*, void> string_new_with_wide_chars;

		//[NativeTypeName ("void (*)(StringPtr, const char *, const Int)")]
		public delegate* unmanaged[Cdecl]<IntPtr, sbyte*, long, void> string_new_with_latin1_chars_and_len;

		//[NativeTypeName ("void (*)(StringPtr, const char *, const Int)")]
		public delegate* unmanaged[Cdecl]<IntPtr, sbyte*, long, void> string_new_with_utf8_chars_and_len;

		//[NativeTypeName ("void (*)(StringPtr, const char16_t *, const Int)")]
		public delegate* unmanaged[Cdecl]<IntPtr, char*, long, void> string_new_with_utf16_chars_and_len;

		//[NativeTypeName ("void (*)(StringPtr, const char32_t *, const Int)")]
		public delegate* unmanaged[Cdecl]<IntPtr, int*, long, void> string_new_with_utf32_chars_and_len;

		//[NativeTypeName("void (*)(StringPtr, const wchar_t *, const Int)")]
		public delegate* unmanaged[Cdecl]<IntPtr, char*, long, void> string_new_with_wide_chars_and_len;

		//[NativeTypeName ("Int (*)(const StringPtr, char *, Int)")]
		public delegate* unmanaged[Cdecl]<IntPtr, sbyte*, long, long> string_to_latin1_chars;

		//[NativeTypeName ("Int (*)(const StringPtr, char *, Int)")]
		public delegate* unmanaged[Cdecl]<IntPtr, sbyte*, long, long> string_to_utf8_chars;

		//[NativeTypeName ("Int (*)(const StringPtr, char16_t *, Int)")]
		public delegate* unmanaged[Cdecl]<IntPtr, char*, long, long> string_to_utf16_chars;

		//[NativeTypeName ("Int (*)(const StringPtr, char32_t *, Int)")]
		public delegate* unmanaged[Cdecl]<IntPtr, int*, long, long> string_to_utf32_chars;

		//[NativeTypeName ("Int (*)(const StringPtr, wchar_t *, Int)")]
		public delegate* unmanaged[Cdecl]<IntPtr, char*, long, long> string_to_wide_chars;

		//[NativeTypeName ("char32_t *(*)(StringPtr, Int)")]
		public delegate* unmanaged[Cdecl]<IntPtr, long, int*> string_operator_index;

		//[NativeTypeName ("const char32_t *(*)(const StringPtr, Int)")]
		public delegate* unmanaged[Cdecl]<IntPtr, long, int*> string_operator_index_const;

		//[NativeTypeName("uint8_t *(*)(TypePtr, Int)")]
		public delegate* unmanaged[Cdecl]<IntPtr, long, byte*> packed_byte_array_operator_index;

		//[NativeTypeName ("const uint8_t *(*)(const TypePtr, Int)")]
		public delegate* unmanaged[Cdecl]<IntPtr, long, byte*> packed_byte_array_operator_index_const;

		//[NativeTypeName ("TypePtr (*)(TypePtr, Int)")]
		public delegate* unmanaged[Cdecl]<IntPtr, long, IntPtr> packed_color_array_operator_index;

		//[NativeTypeName ("TypePtr (*)(const TypePtr, Int)")]
		public delegate* unmanaged[Cdecl]<IntPtr, long, IntPtr> packed_color_array_operator_index_const;

		//[NativeTypeName ("float *(*)(TypePtr, Int)")]
		public delegate* unmanaged[Cdecl]<IntPtr, long, float*> packed_float32_array_operator_index;

		//[NativeTypeName ("const float *(*)(const TypePtr, Int)")]
		public delegate* unmanaged[Cdecl]<IntPtr, long, float*> packed_float32_array_operator_index_const;

		//[NativeTypeName ("double *(*)(TypePtr, Int)")]
		public delegate* unmanaged[Cdecl]<IntPtr, long, double*> packed_float64_array_operator_index;

		//[NativeTypeName ("const double *(*)(const TypePtr, Int)")]
		public delegate* unmanaged[Cdecl]<IntPtr, long, double*> packed_float64_array_operator_index_const;

		//[NativeTypeName ("int32_t *(*)(TypePtr, Int)")]
		public delegate* unmanaged[Cdecl]<IntPtr, long, int*> packed_int32_array_operator_index;

		//[NativeTypeName ("const int32_t *(*)(const TypePtr, Int)")]
		public delegate* unmanaged[Cdecl]<IntPtr, long, int*> packed_int32_array_operator_index_const;

		//[NativeTypeName ("int64_t *(*)(TypePtr, Int)")]
		public delegate* unmanaged[Cdecl]<IntPtr, long, long*> packed_int64_array_operator_index;

		//[NativeTypeName ("const int64_t *(*)(const TypePtr, Int)")]
		public delegate* unmanaged[Cdecl]<IntPtr, long, long*> packed_int64_array_operator_index_const;

		//[NativeTypeName ("StringPtr (*)(TypePtr, Int)")]
		public delegate* unmanaged[Cdecl]<IntPtr, long, IntPtr> packed_string_array_operator_index;

		//[NativeTypeName ("StringPtr (*)(const TypePtr, Int)")]
		public delegate* unmanaged[Cdecl]<IntPtr, long, IntPtr> packed_string_array_operator_index_const;

		//[NativeTypeName ("TypePtr (*)(TypePtr, Int)")]
		public delegate* unmanaged[Cdecl]<IntPtr, long, IntPtr> packed_vector2_array_operator_index;

		//[NativeTypeName ("TypePtr (*)(const TypePtr, Int)")]
		public delegate* unmanaged[Cdecl]<IntPtr, long, IntPtr> packed_vector2_array_operator_index_const;

		//[NativeTypeName ("TypePtr (*)(TypePtr, Int)")]
		public delegate* unmanaged[Cdecl]<IntPtr, long, IntPtr> packed_vector3_array_operator_index;

		//[NativeTypeName ("TypePtr (*)(const TypePtr, Int)")]
		public delegate* unmanaged[Cdecl]<IntPtr, long, IntPtr> packed_vector3_array_operator_index_const;

		//[NativeTypeName ("VariantPtr (*)(TypePtr, Int)")]
		public delegate* unmanaged[Cdecl]<IntPtr, long, IntPtr> array_operator_index;

		//[NativeTypeName ("VariantPtr (*)(const TypePtr, Int)")]
		public delegate* unmanaged[Cdecl]<IntPtr, long, IntPtr> array_operator_index_const;

		//[NativeTypeName ("VariantPtr (*)(TypePtr, const VariantPtr)")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr> dictionary_operator_index;

		//[NativeTypeName ("VariantPtr (*)(const TypePtr, const VariantPtr)")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr> dictionary_operator_index_const;

		//[NativeTypeName ("void (*)(const MethodBindPtr, ObjectPtr, const VariantPtr *, Int, VariantPtr, CallError *)")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr*, long, IntPtr, CallError*, void> object_method_bind_call;

		//[NativeTypeName ("void (*)(const MethodBindPtr, ObjectPtr, const TypePtr *, TypePtr)")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr*, IntPtr, void> object_method_bind_ptrcall;

		//[NativeTypeName ("void (*)(ObjectPtr)")]
		public delegate* unmanaged[Cdecl]<IntPtr, void> object_destroy;

		//[NativeTypeName ("ObjectPtr (*)(const char *)")]
		public delegate* unmanaged[Cdecl]<sbyte*, IntPtr> global_get_singleton;

		//[NativeTypeName ("void *(*)(ObjectPtr, void *, const InstanceBindingCallbacks *)")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, InstanceBindingCallbacks*, IntPtr> object_get_instance_binding;

		//[NativeTypeName ("void (*)(ObjectPtr, void *, void *, const InstanceBindingCallbacks *)")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, InstanceBindingCallbacks*, void> object_set_instance_binding;

		//[NativeTypeName ("void (*)(ObjectPtr, const char *, GDExtensionClassInstancePtr)")]
		public delegate* unmanaged[Cdecl]<IntPtr, sbyte*, IntPtr, void> object_set_instance;

		//[NativeTypeName ("ObjectPtr (*)(const ObjectPtr, void *)")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr> object_cast_to;

		//[NativeTypeName ("ObjectPtr (*)(GDObjectInstanceID)")]
		public delegate* unmanaged[Cdecl]<ulong, IntPtr> object_get_instance_from_id;

		//[NativeTypeName ("GDObjectInstanceID (*)(const ObjectPtr)")]
		public delegate* unmanaged[Cdecl]<IntPtr, ulong> object_get_instance_id;

		//[NativeTypeName ("ScriptInstancePtr (*)(const ExtensionScriptInstanceInfo *, ExtensionScriptInstanceDataPtr)")]
		public delegate* unmanaged[Cdecl]<ExtensionScriptInstanceInfo*, IntPtr, IntPtr> script_instance_create;

		//[NativeTypeName ("ObjectPtr (*)(const char *)")]
		public delegate* unmanaged[Cdecl]<sbyte*, IntPtr> classdb_construct_object;

		//[NativeTypeName ("MethodBindPtr (*)(const char *, const char *, Int)")]
		public delegate* unmanaged[Cdecl]<sbyte*, sbyte*, long, IntPtr> classdb_get_method_bind;

		//[NativeTypeName ("void *(*)(const char *)")]
		public delegate* unmanaged[Cdecl]<sbyte*, IntPtr> classdb_get_class_tag;

		//[NativeTypeName ("void (*)(const ExtensionClassLibraryPtr, const char *, const char *, const ExtensionClassCreationInfo *)")]
		public delegate* unmanaged[Cdecl]<IntPtr, sbyte*, sbyte*, ExtensionClassCreationInfo*, void> classdb_register_extension_class;

		//[NativeTypeName ("void (*)(const ExtensionClassLibraryPtr, const char *, const ExtensionClassMethodInfo *)")]
		public delegate* unmanaged[Cdecl]<IntPtr, sbyte*, ExtensionClassMethodInfo*, void> classdb_register_extension_class_method;

		//[NativeTypeName ("void (*)(const ExtensionClassLibraryPtr, const char *, const char *, const char *, Int, Bool)")]
		public delegate* unmanaged[Cdecl]<IntPtr, sbyte*, sbyte*, sbyte*, long, bool, void> classdb_register_extension_class_integer_constant;

		//[NativeTypeName ("void (*)(const ExtensionClassLibraryPtr, const char *, const PropertyInfo *, const char *, const char *)")]
		public delegate* unmanaged[Cdecl]<IntPtr, sbyte*, PropertyInfo*, sbyte*, sbyte*, void> classdb_register_extension_class_property;

		//[NativeTypeName ("void (*)(const ExtensionClassLibraryPtr, const char *, const char *, const char *)")]
		public delegate* unmanaged[Cdecl]<IntPtr, sbyte*, sbyte*, sbyte*, void> classdb_register_extension_class_property_group;

		//[NativeTypeName ("void (*)(const ExtensionClassLibraryPtr, const char *, const char *, const char *)")]
		public delegate* unmanaged[Cdecl]<IntPtr, sbyte*, sbyte*, sbyte*, void> classdb_register_extension_class_property_subgroup;

		//[NativeTypeName ("void (*)(const ExtensionClassLibraryPtr, const char *, const char *, const PropertyInfo *, Int)")]
		public delegate* unmanaged[Cdecl]<IntPtr, sbyte*, sbyte*, PropertyInfo*, long, void> classdb_register_extension_class_signal;

		//[NativeTypeName ("void (*)(const ExtensionClassLibraryPtr, const char *)")]
		public delegate* unmanaged[Cdecl]<IntPtr, sbyte*, void> classdb_unregister_extension_class;

		//[NativeTypeName ("void (*)(const ExtensionClassLibraryPtr, StringPtr)")]
		public delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void> get_library_path;
	}

	public enum InitializationLevel {
		Core,
		Servers,
		Scene,
		Editor,
		MAX,
	}

	public partial struct Initialization {
		public InitializationLevel minimum_initialization_level;

		public IntPtr userdata;

		//[NativeTypeName ("void (*)(void *, InitializationLevel)")]
		public delegate* unmanaged[Cdecl]<IntPtr, InitializationLevel, void> initialize;

		//[NativeTypeName ("void (*)(void *, InitializationLevel)")]
		public delegate* unmanaged[Cdecl]<IntPtr, InitializationLevel, void> deinitialize;
	}
}
