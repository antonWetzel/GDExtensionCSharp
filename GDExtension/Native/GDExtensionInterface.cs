namespace GDExtension.Native;

public unsafe partial struct GDExtensionInterface
{
    [NativeTypeName("uint32_t")]
    public uint version_major;

    [NativeTypeName("uint32_t")]
    public uint version_minor;

    [NativeTypeName("uint32_t")]
    public uint version_patch;

    [NativeTypeName("const char *")]
    public sbyte* version_string;

    [NativeTypeName("void *(*)(size_t)")]
    public delegate* unmanaged[Cdecl]<nuint, void*> mem_alloc;

    [NativeTypeName("void *(*)(void *, size_t)")]
    public delegate* unmanaged[Cdecl]<void*, nuint, void*> mem_realloc;

    [NativeTypeName("void (*)(void *)")]
    public delegate* unmanaged[Cdecl]<void*, void> mem_free;

    [NativeTypeName("void (*)(const char *, const char *, const char *, int32_t)")]
    public delegate* unmanaged[Cdecl]<sbyte*, sbyte*, sbyte*, int, void> print_error;

    [NativeTypeName("void (*)(const char *, const char *, const char *, int32_t)")]
    public delegate* unmanaged[Cdecl]<sbyte*, sbyte*, sbyte*, int, void> print_warning;

    [NativeTypeName("void (*)(const char *, const char *, const char *, int32_t)")]
    public delegate* unmanaged[Cdecl]<sbyte*, sbyte*, sbyte*, int, void> print_script_error;

    [NativeTypeName("uint64_t (*)(GDExtensionConstStringNamePtr)")]
    public delegate* unmanaged[Cdecl]<void*, ulong> get_native_struct_size;

    [NativeTypeName("void (*)(GDExtensionVariantPtr, GDExtensionConstVariantPtr)")]
    public delegate* unmanaged[Cdecl]<void*, void*, void> variant_new_copy;

    [NativeTypeName("void (*)(GDExtensionVariantPtr)")]
    public delegate* unmanaged[Cdecl]<void*, void> variant_new_nil;

    [NativeTypeName("void (*)(GDExtensionVariantPtr)")]
    public delegate* unmanaged[Cdecl]<void*, void> variant_destroy;

    [NativeTypeName("void (*)(GDExtensionVariantPtr, GDExtensionConstStringNamePtr, const GDExtensionConstVariantPtr *, GDExtensionInt, GDExtensionVariantPtr, GDExtensionCallError *)")]
    public delegate* unmanaged[Cdecl]<void*, void*, void**, long, void*, GDExtensionCallError*, void> variant_call;

    [NativeTypeName("void (*)(GDExtensionVariantType, GDExtensionConstStringNamePtr, const GDExtensionConstVariantPtr *, GDExtensionInt, GDExtensionVariantPtr, GDExtensionCallError *)")]
    public delegate* unmanaged[Cdecl]<GDExtensionVariantType, void*, void**, long, void*, GDExtensionCallError*, void> variant_call_static;

    [NativeTypeName("void (*)(GDExtensionVariantOperator, GDExtensionConstVariantPtr, GDExtensionConstVariantPtr, GDExtensionVariantPtr, GDExtensionBool *)")]
    public delegate* unmanaged[Cdecl]<GDExtensionVariantOperator, void*, void*, void*, byte*, void> variant_evaluate;

    [NativeTypeName("void (*)(GDExtensionVariantPtr, GDExtensionConstVariantPtr, GDExtensionConstVariantPtr, GDExtensionBool *)")]
    public delegate* unmanaged[Cdecl]<void*, void*, void*, byte*, void> variant_set;

    [NativeTypeName("void (*)(GDExtensionVariantPtr, GDExtensionConstStringNamePtr, GDExtensionConstVariantPtr, GDExtensionBool *)")]
    public delegate* unmanaged[Cdecl]<void*, void*, void*, byte*, void> variant_set_named;

    [NativeTypeName("void (*)(GDExtensionVariantPtr, GDExtensionConstVariantPtr, GDExtensionConstVariantPtr, GDExtensionBool *)")]
    public delegate* unmanaged[Cdecl]<void*, void*, void*, byte*, void> variant_set_keyed;

    [NativeTypeName("void (*)(GDExtensionVariantPtr, GDExtensionInt, GDExtensionConstVariantPtr, GDExtensionBool *, GDExtensionBool *)")]
    public delegate* unmanaged[Cdecl]<void*, long, void*, byte*, byte*, void> variant_set_indexed;

    [NativeTypeName("void (*)(GDExtensionConstVariantPtr, GDExtensionConstVariantPtr, GDExtensionVariantPtr, GDExtensionBool *)")]
    public delegate* unmanaged[Cdecl]<void*, void*, void*, byte*, void> variant_get;

    [NativeTypeName("void (*)(GDExtensionConstVariantPtr, GDExtensionConstStringNamePtr, GDExtensionVariantPtr, GDExtensionBool *)")]
    public delegate* unmanaged[Cdecl]<void*, void*, void*, byte*, void> variant_get_named;

    [NativeTypeName("void (*)(GDExtensionConstVariantPtr, GDExtensionConstVariantPtr, GDExtensionVariantPtr, GDExtensionBool *)")]
    public delegate* unmanaged[Cdecl]<void*, void*, void*, byte*, void> variant_get_keyed;

    [NativeTypeName("void (*)(GDExtensionConstVariantPtr, GDExtensionInt, GDExtensionVariantPtr, GDExtensionBool *, GDExtensionBool *)")]
    public delegate* unmanaged[Cdecl]<void*, long, void*, byte*, byte*, void> variant_get_indexed;

    [NativeTypeName("GDExtensionBool (*)(GDExtensionConstVariantPtr, GDExtensionVariantPtr, GDExtensionBool *)")]
    public delegate* unmanaged[Cdecl]<void*, void*, byte*, byte> variant_iter_init;

    [NativeTypeName("GDExtensionBool (*)(GDExtensionConstVariantPtr, GDExtensionVariantPtr, GDExtensionBool *)")]
    public delegate* unmanaged[Cdecl]<void*, void*, byte*, byte> variant_iter_next;

    [NativeTypeName("void (*)(GDExtensionConstVariantPtr, GDExtensionVariantPtr, GDExtensionVariantPtr, GDExtensionBool *)")]
    public delegate* unmanaged[Cdecl]<void*, void*, void*, byte*, void> variant_iter_get;

    [NativeTypeName("GDExtensionInt (*)(GDExtensionConstVariantPtr)")]
    public delegate* unmanaged[Cdecl]<void*, long> variant_hash;

    [NativeTypeName("GDExtensionInt (*)(GDExtensionConstVariantPtr, GDExtensionInt)")]
    public delegate* unmanaged[Cdecl]<void*, long, long> variant_recursive_hash;

    [NativeTypeName("GDExtensionBool (*)(GDExtensionConstVariantPtr, GDExtensionConstVariantPtr)")]
    public delegate* unmanaged[Cdecl]<void*, void*, byte> variant_hash_compare;

    [NativeTypeName("GDExtensionBool (*)(GDExtensionConstVariantPtr)")]
    public delegate* unmanaged[Cdecl]<void*, byte> variant_booleanize;

    [NativeTypeName("void (*)(GDExtensionConstVariantPtr, GDExtensionVariantPtr, GDExtensionBool)")]
    public delegate* unmanaged[Cdecl]<void*, void*, byte, void> variant_duplicate;

    [NativeTypeName("void (*)(GDExtensionConstVariantPtr, GDExtensionStringPtr)")]
    public delegate* unmanaged[Cdecl]<void*, void*, void> variant_stringify;

    [NativeTypeName("GDExtensionVariantType (*)(GDExtensionConstVariantPtr)")]
    public delegate* unmanaged[Cdecl]<void*, GDExtensionVariantType> variant_get_type;

    [NativeTypeName("GDExtensionBool (*)(GDExtensionConstVariantPtr, GDExtensionConstStringNamePtr)")]
    public delegate* unmanaged[Cdecl]<void*, void*, byte> variant_has_method;

    [NativeTypeName("GDExtensionBool (*)(GDExtensionVariantType, GDExtensionConstStringNamePtr)")]
    public delegate* unmanaged[Cdecl]<GDExtensionVariantType, void*, byte> variant_has_member;

    [NativeTypeName("GDExtensionBool (*)(GDExtensionConstVariantPtr, GDExtensionConstVariantPtr, GDExtensionBool *)")]
    public delegate* unmanaged[Cdecl]<void*, void*, byte*, byte> variant_has_key;

    [NativeTypeName("void (*)(GDExtensionVariantType, GDExtensionStringPtr)")]
    public delegate* unmanaged[Cdecl]<GDExtensionVariantType, void*, void> variant_get_type_name;

    [NativeTypeName("GDExtensionBool (*)(GDExtensionVariantType, GDExtensionVariantType)")]
    public delegate* unmanaged[Cdecl]<GDExtensionVariantType, GDExtensionVariantType, byte> variant_can_convert;

    [NativeTypeName("GDExtensionBool (*)(GDExtensionVariantType, GDExtensionVariantType)")]
    public delegate* unmanaged[Cdecl]<GDExtensionVariantType, GDExtensionVariantType, byte> variant_can_convert_strict;

    [NativeTypeName("GDExtensionVariantFromTypeConstructorFunc (*)(GDExtensionVariantType)")]
    public delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void*, void*, void>> get_variant_from_type_constructor;

    [NativeTypeName("GDExtensionTypeFromVariantConstructorFunc (*)(GDExtensionVariantType)")]
    public delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void*, void*, void>> get_variant_to_type_constructor;

    [NativeTypeName("GDExtensionPtrOperatorEvaluator (*)(GDExtensionVariantOperator, GDExtensionVariantType, GDExtensionVariantType)")]
    public delegate* unmanaged[Cdecl]<GDExtensionVariantOperator, GDExtensionVariantType, GDExtensionVariantType, delegate* unmanaged[Cdecl]<void*, void*, void*, void>> variant_get_ptr_operator_evaluator;

    [NativeTypeName("GDExtensionPtrBuiltInMethod (*)(GDExtensionVariantType, GDExtensionConstStringNamePtr, GDExtensionInt)")]
    public delegate* unmanaged[Cdecl]<GDExtensionVariantType, void*, long, delegate* unmanaged[Cdecl]<void*, void**, void*, int, void>> variant_get_ptr_builtin_method;

    [NativeTypeName("GDExtensionPtrConstructor (*)(GDExtensionVariantType, int32_t)")]
    public delegate* unmanaged[Cdecl]<GDExtensionVariantType, int, delegate* unmanaged[Cdecl]<void*, void**, void>> variant_get_ptr_constructor;

    [NativeTypeName("GDExtensionPtrDestructor (*)(GDExtensionVariantType)")]
    public delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void*, void>> variant_get_ptr_destructor;

    [NativeTypeName("void (*)(GDExtensionVariantType, GDExtensionVariantPtr, const GDExtensionConstVariantPtr *, int32_t, GDExtensionCallError *)")]
    public delegate* unmanaged[Cdecl]<GDExtensionVariantType, void*, void**, int, GDExtensionCallError*, void> variant_construct;

    [NativeTypeName("GDExtensionPtrSetter (*)(GDExtensionVariantType, GDExtensionConstStringNamePtr)")]
    public delegate* unmanaged[Cdecl]<GDExtensionVariantType, void*, delegate* unmanaged[Cdecl]<void*, void*, void>> variant_get_ptr_setter;

    [NativeTypeName("GDExtensionPtrGetter (*)(GDExtensionVariantType, GDExtensionConstStringNamePtr)")]
    public delegate* unmanaged[Cdecl]<GDExtensionVariantType, void*, delegate* unmanaged[Cdecl]<void*, void*, void>> variant_get_ptr_getter;

    [NativeTypeName("GDExtensionPtrIndexedSetter (*)(GDExtensionVariantType)")]
    public delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void*, long, void*, void>> variant_get_ptr_indexed_setter;

    [NativeTypeName("GDExtensionPtrIndexedGetter (*)(GDExtensionVariantType)")]
    public delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void*, long, void*, void>> variant_get_ptr_indexed_getter;

    [NativeTypeName("GDExtensionPtrKeyedSetter (*)(GDExtensionVariantType)")]
    public delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void*, void*, void*, void>> variant_get_ptr_keyed_setter;

    [NativeTypeName("GDExtensionPtrKeyedGetter (*)(GDExtensionVariantType)")]
    public delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void*, void*, void*, void>> variant_get_ptr_keyed_getter;

    [NativeTypeName("GDExtensionPtrKeyedChecker (*)(GDExtensionVariantType)")]
    public delegate* unmanaged[Cdecl]<GDExtensionVariantType, delegate* unmanaged[Cdecl]<void*, void*, uint>> variant_get_ptr_keyed_checker;

    [NativeTypeName("void (*)(GDExtensionVariantType, GDExtensionConstStringNamePtr, GDExtensionVariantPtr)")]
    public delegate* unmanaged[Cdecl]<GDExtensionVariantType, void*, void*, void> variant_get_constant_value;

    [NativeTypeName("GDExtensionPtrUtilityFunction (*)(GDExtensionConstStringNamePtr, GDExtensionInt)")]
    public delegate* unmanaged[Cdecl]<void*, long, delegate* unmanaged[Cdecl]<void*, void**, int, void>> variant_get_ptr_utility_function;

    [NativeTypeName("void (*)(GDExtensionStringPtr, const char *)")]
    public delegate* unmanaged[Cdecl]<void*, sbyte*, void> string_new_with_latin1_chars;

    [NativeTypeName("void (*)(GDExtensionStringPtr, const char *)")]
    public delegate* unmanaged[Cdecl]<void*, sbyte*, void> string_new_with_utf8_chars;

    [NativeTypeName("void (*)(GDExtensionStringPtr, const char16_t *)")]
    public delegate* unmanaged[Cdecl]<void*, ushort*, void> string_new_with_utf16_chars;

    [NativeTypeName("void (*)(GDExtensionStringPtr, const uint32_t *)")]
    public delegate* unmanaged[Cdecl]<void*, uint*, void> string_new_with_utf32_chars;

    [NativeTypeName("void (*)(GDExtensionStringPtr, const wchar_t *)")]
    public delegate* unmanaged[Cdecl]<void*, ushort*, void> string_new_with_wide_chars;

    [NativeTypeName("void (*)(GDExtensionStringPtr, const char *, GDExtensionInt)")]
    public delegate* unmanaged[Cdecl]<void*, sbyte*, long, void> string_new_with_latin1_chars_and_len;

    [NativeTypeName("void (*)(GDExtensionStringPtr, const char *, GDExtensionInt)")]
    public delegate* unmanaged[Cdecl]<void*, sbyte*, long, void> string_new_with_utf8_chars_and_len;

    [NativeTypeName("void (*)(GDExtensionStringPtr, const char16_t *, GDExtensionInt)")]
    public delegate* unmanaged[Cdecl]<void*, ushort*, long, void> string_new_with_utf16_chars_and_len;

    [NativeTypeName("void (*)(GDExtensionStringPtr, const uint32_t *, GDExtensionInt)")]
    public delegate* unmanaged[Cdecl]<void*, uint*, long, void> string_new_with_utf32_chars_and_len;

    [NativeTypeName("void (*)(GDExtensionStringPtr, const wchar_t *, GDExtensionInt)")]
    public delegate* unmanaged[Cdecl]<void*, ushort*, long, void> string_new_with_wide_chars_and_len;

    [NativeTypeName("GDExtensionInt (*)(GDExtensionConstStringPtr, char *, GDExtensionInt)")]
    public delegate* unmanaged[Cdecl]<void*, sbyte*, long, long> string_to_latin1_chars;

    [NativeTypeName("GDExtensionInt (*)(GDExtensionConstStringPtr, char *, GDExtensionInt)")]
    public delegate* unmanaged[Cdecl]<void*, sbyte*, long, long> string_to_utf8_chars;

    [NativeTypeName("GDExtensionInt (*)(GDExtensionConstStringPtr, char16_t *, GDExtensionInt)")]
    public delegate* unmanaged[Cdecl]<void*, ushort*, long, long> string_to_utf16_chars;

    [NativeTypeName("GDExtensionInt (*)(GDExtensionConstStringPtr, uint32_t *, GDExtensionInt)")]
    public delegate* unmanaged[Cdecl]<void*, uint*, long, long> string_to_utf32_chars;

    [NativeTypeName("GDExtensionInt (*)(GDExtensionConstStringPtr, wchar_t *, GDExtensionInt)")]
    public delegate* unmanaged[Cdecl]<void*, ushort*, long, long> string_to_wide_chars;

    [NativeTypeName("uint32_t *(*)(GDExtensionStringPtr, GDExtensionInt)")]
    public delegate* unmanaged[Cdecl]<void*, long, uint*> string_operator_index;

    [NativeTypeName("const uint32_t *(*)(GDExtensionConstStringPtr, GDExtensionInt)")]
    public delegate* unmanaged[Cdecl]<void*, long, uint*> string_operator_index_const;

    [NativeTypeName("void (*)(GDExtensionStringPtr, GDExtensionConstStringPtr)")]
    public delegate* unmanaged[Cdecl]<void*, void*, void> string_operator_plus_eq_string;

    [NativeTypeName("void (*)(GDExtensionStringPtr, uint32_t)")]
    public delegate* unmanaged[Cdecl]<void*, uint, void> string_operator_plus_eq_char;

    [NativeTypeName("void (*)(GDExtensionStringPtr, const char *)")]
    public delegate* unmanaged[Cdecl]<void*, sbyte*, void> string_operator_plus_eq_cstr;

    [NativeTypeName("void (*)(GDExtensionStringPtr, const wchar_t *)")]
    public delegate* unmanaged[Cdecl]<void*, ushort*, void> string_operator_plus_eq_wcstr;

    [NativeTypeName("void (*)(GDExtensionStringPtr, const uint32_t *)")]
    public delegate* unmanaged[Cdecl]<void*, uint*, void> string_operator_plus_eq_c32str;

    [NativeTypeName("GDExtensionInt (*)(GDExtensionObjectPtr, const uint8_t *, size_t)")]
    public delegate* unmanaged[Cdecl]<void*, byte*, nuint, long> xml_parser_open_buffer;

    [NativeTypeName("void (*)(GDExtensionObjectPtr, const uint8_t *, uint64_t)")]
    public delegate* unmanaged[Cdecl]<void*, byte*, ulong, void> file_access_store_buffer;

    [NativeTypeName("uint64_t (*)(GDExtensionConstObjectPtr, uint8_t *, uint64_t)")]
    public delegate* unmanaged[Cdecl]<void*, byte*, ulong, ulong> file_access_get_buffer;

    [NativeTypeName("int64_t (*)(GDExtensionObjectPtr, void (*)(void *, uint32_t), void *, int, int, GDExtensionBool, GDExtensionConstStringPtr)")]
    public delegate* unmanaged[Cdecl]<void*, delegate* unmanaged[Cdecl]<void*, uint, void>, void*, int, int, byte, void*, long> worker_thread_pool_add_native_group_task;

    [NativeTypeName("int64_t (*)(GDExtensionObjectPtr, void (*)(void *), void *, GDExtensionBool, GDExtensionConstStringPtr)")]
    public delegate* unmanaged[Cdecl]<void*, delegate* unmanaged[Cdecl]<void*, void>, void*, byte, void*, long> worker_thread_pool_add_native_task;

    [NativeTypeName("uint8_t *(*)(GDExtensionTypePtr, GDExtensionInt)")]
    public delegate* unmanaged[Cdecl]<void*, long, byte*> packed_byte_array_operator_index;

    [NativeTypeName("const uint8_t *(*)(GDExtensionConstTypePtr, GDExtensionInt)")]
    public delegate* unmanaged[Cdecl]<void*, long, byte*> packed_byte_array_operator_index_const;

    [NativeTypeName("GDExtensionTypePtr (*)(GDExtensionTypePtr, GDExtensionInt)")]
    public delegate* unmanaged[Cdecl]<void*, long, void*> packed_color_array_operator_index;

    [NativeTypeName("GDExtensionTypePtr (*)(GDExtensionConstTypePtr, GDExtensionInt)")]
    public delegate* unmanaged[Cdecl]<void*, long, void*> packed_color_array_operator_index_const;

    [NativeTypeName("float *(*)(GDExtensionTypePtr, GDExtensionInt)")]
    public delegate* unmanaged[Cdecl]<void*, long, float*> packed_float32_array_operator_index;

    [NativeTypeName("const float *(*)(GDExtensionConstTypePtr, GDExtensionInt)")]
    public delegate* unmanaged[Cdecl]<void*, long, float*> packed_float32_array_operator_index_const;

    [NativeTypeName("double *(*)(GDExtensionTypePtr, GDExtensionInt)")]
    public delegate* unmanaged[Cdecl]<void*, long, double*> packed_float64_array_operator_index;

    [NativeTypeName("const double *(*)(GDExtensionConstTypePtr, GDExtensionInt)")]
    public delegate* unmanaged[Cdecl]<void*, long, double*> packed_float64_array_operator_index_const;

    [NativeTypeName("int32_t *(*)(GDExtensionTypePtr, GDExtensionInt)")]
    public delegate* unmanaged[Cdecl]<void*, long, int*> packed_int32_array_operator_index;

    [NativeTypeName("const int32_t *(*)(GDExtensionConstTypePtr, GDExtensionInt)")]
    public delegate* unmanaged[Cdecl]<void*, long, int*> packed_int32_array_operator_index_const;

    [NativeTypeName("int64_t *(*)(GDExtensionTypePtr, GDExtensionInt)")]
    public delegate* unmanaged[Cdecl]<void*, long, long*> packed_int64_array_operator_index;

    [NativeTypeName("const int64_t *(*)(GDExtensionConstTypePtr, GDExtensionInt)")]
    public delegate* unmanaged[Cdecl]<void*, long, long*> packed_int64_array_operator_index_const;

    [NativeTypeName("GDExtensionStringPtr (*)(GDExtensionTypePtr, GDExtensionInt)")]
    public delegate* unmanaged[Cdecl]<void*, long, void*> packed_string_array_operator_index;

    [NativeTypeName("GDExtensionStringPtr (*)(GDExtensionConstTypePtr, GDExtensionInt)")]
    public delegate* unmanaged[Cdecl]<void*, long, void*> packed_string_array_operator_index_const;

    [NativeTypeName("GDExtensionTypePtr (*)(GDExtensionTypePtr, GDExtensionInt)")]
    public delegate* unmanaged[Cdecl]<void*, long, void*> packed_vector2_array_operator_index;

    [NativeTypeName("GDExtensionTypePtr (*)(GDExtensionConstTypePtr, GDExtensionInt)")]
    public delegate* unmanaged[Cdecl]<void*, long, void*> packed_vector2_array_operator_index_const;

    [NativeTypeName("GDExtensionTypePtr (*)(GDExtensionTypePtr, GDExtensionInt)")]
    public delegate* unmanaged[Cdecl]<void*, long, void*> packed_vector3_array_operator_index;

    [NativeTypeName("GDExtensionTypePtr (*)(GDExtensionConstTypePtr, GDExtensionInt)")]
    public delegate* unmanaged[Cdecl]<void*, long, void*> packed_vector3_array_operator_index_const;

    [NativeTypeName("GDExtensionVariantPtr (*)(GDExtensionTypePtr, GDExtensionInt)")]
    public delegate* unmanaged[Cdecl]<void*, long, void*> array_operator_index;

    [NativeTypeName("GDExtensionVariantPtr (*)(GDExtensionConstTypePtr, GDExtensionInt)")]
    public delegate* unmanaged[Cdecl]<void*, long, void*> array_operator_index_const;

    [NativeTypeName("GDExtensionVariantPtr (*)(GDExtensionTypePtr, GDExtensionConstVariantPtr)")]
    public delegate* unmanaged[Cdecl]<void*, void*, void*> dictionary_operator_index;

    [NativeTypeName("GDExtensionVariantPtr (*)(GDExtensionConstTypePtr, GDExtensionConstVariantPtr)")]
    public delegate* unmanaged[Cdecl]<void*, void*, void*> dictionary_operator_index_const;

    [NativeTypeName("void (*)(GDExtensionMethodBindPtr, GDExtensionObjectPtr, const GDExtensionConstVariantPtr *, GDExtensionInt, GDExtensionVariantPtr, GDExtensionCallError *)")]
    public delegate* unmanaged[Cdecl]<void*, void*, void**, long, void*, GDExtensionCallError*, void> object_method_bind_call;

    [NativeTypeName("void (*)(GDExtensionMethodBindPtr, GDExtensionObjectPtr, const GDExtensionConstTypePtr *, GDExtensionTypePtr)")]
    public delegate* unmanaged[Cdecl]<void*, void*, void**, void*, void> object_method_bind_ptrcall;

    [NativeTypeName("void (*)(GDExtensionObjectPtr)")]
    public delegate* unmanaged[Cdecl]<void*, void> object_destroy;

    [NativeTypeName("GDExtensionObjectPtr (*)(GDExtensionConstStringNamePtr)")]
    public delegate* unmanaged[Cdecl]<void*, void*> global_get_singleton;

    [NativeTypeName("void *(*)(GDExtensionObjectPtr, void *, const GDExtensionInstanceBindingCallbacks *)")]
    public delegate* unmanaged[Cdecl]<void*, void*, GDExtensionInstanceBindingCallbacks*, void*> object_get_instance_binding;

    [NativeTypeName("void (*)(GDExtensionObjectPtr, void *, void *, const GDExtensionInstanceBindingCallbacks *)")]
    public delegate* unmanaged[Cdecl]<void*, void*, void*, GDExtensionInstanceBindingCallbacks*, void> object_set_instance_binding;

    [NativeTypeName("void (*)(GDExtensionObjectPtr, GDExtensionConstStringNamePtr, GDExtensionClassInstancePtr)")]
    public delegate* unmanaged[Cdecl]<void*, void*, void*, void> object_set_instance;

    [NativeTypeName("GDExtensionObjectPtr (*)(GDExtensionConstObjectPtr, void *)")]
    public delegate* unmanaged[Cdecl]<void*, void*, void*> object_cast_to;

    [NativeTypeName("GDExtensionObjectPtr (*)(GDObjectInstanceID)")]
    public delegate* unmanaged[Cdecl]<ulong, void*> object_get_instance_from_id;

    [NativeTypeName("GDObjectInstanceID (*)(GDExtensionConstObjectPtr)")]
    public delegate* unmanaged[Cdecl]<void*, ulong> object_get_instance_id;

    [NativeTypeName("GDExtensionObjectPtr (*)(GDExtensionConstRefPtr)")]
    public delegate* unmanaged[Cdecl]<void*, void*> ref_get_object;

    [NativeTypeName("void (*)(GDExtensionRefPtr, GDExtensionObjectPtr)")]
    public delegate* unmanaged[Cdecl]<void*, void*, void> ref_set_object;

    [NativeTypeName("GDExtensionScriptInstancePtr (*)(const GDExtensionScriptInstanceInfo *, GDExtensionScriptInstanceDataPtr)")]
    public delegate* unmanaged[Cdecl]<GDExtensionScriptInstanceInfo*, void*, void*> script_instance_create;

    [NativeTypeName("GDExtensionObjectPtr (*)(GDExtensionConstStringNamePtr)")]
    public delegate* unmanaged[Cdecl]<void*, void*> classdb_construct_object;

    [NativeTypeName("GDExtensionMethodBindPtr (*)(GDExtensionConstStringNamePtr, GDExtensionConstStringNamePtr, GDExtensionInt)")]
    public delegate* unmanaged[Cdecl]<void*, void*, long, void*> classdb_get_method_bind;

    [NativeTypeName("void *(*)(GDExtensionConstStringNamePtr)")]
    public delegate* unmanaged[Cdecl]<void*, void*> classdb_get_class_tag;

    [NativeTypeName("void (*)(GDExtensionClassLibraryPtr, GDExtensionConstStringNamePtr, GDExtensionConstStringNamePtr, const GDExtensionClassCreationInfo *)")]
    public delegate* unmanaged[Cdecl]<void*, void*, void*, GDExtensionClassCreationInfo*, void> classdb_register_extension_class;

    [NativeTypeName("void (*)(GDExtensionClassLibraryPtr, GDExtensionConstStringNamePtr, const GDExtensionClassMethodInfo *)")]
    public delegate* unmanaged[Cdecl]<void*, void*, GDExtensionClassMethodInfo*, void> classdb_register_extension_class_method;

    [NativeTypeName("void (*)(GDExtensionClassLibraryPtr, GDExtensionConstStringNamePtr, GDExtensionConstStringNamePtr, GDExtensionConstStringNamePtr, GDExtensionInt, GDExtensionBool)")]
    public delegate* unmanaged[Cdecl]<void*, void*, void*, void*, long, byte, void> classdb_register_extension_class_integer_constant;

    [NativeTypeName("void (*)(GDExtensionClassLibraryPtr, GDExtensionConstStringNamePtr, const GDExtensionPropertyInfo *, GDExtensionConstStringNamePtr, GDExtensionConstStringNamePtr)")]
    public delegate* unmanaged[Cdecl]<void*, void*, GDExtensionPropertyInfo*, void*, void*, void> classdb_register_extension_class_property;

    [NativeTypeName("void (*)(GDExtensionClassLibraryPtr, GDExtensionConstStringNamePtr, GDExtensionConstStringPtr, GDExtensionConstStringPtr)")]
    public delegate* unmanaged[Cdecl]<void*, void*, void*, void*, void> classdb_register_extension_class_property_group;

    [NativeTypeName("void (*)(GDExtensionClassLibraryPtr, GDExtensionConstStringNamePtr, GDExtensionConstStringPtr, GDExtensionConstStringPtr)")]
    public delegate* unmanaged[Cdecl]<void*, void*, void*, void*, void> classdb_register_extension_class_property_subgroup;

    [NativeTypeName("void (*)(GDExtensionClassLibraryPtr, GDExtensionConstStringNamePtr, GDExtensionConstStringNamePtr, const GDExtensionPropertyInfo *, GDExtensionInt)")]
    public delegate* unmanaged[Cdecl]<void*, void*, void*, GDExtensionPropertyInfo*, long, void> classdb_register_extension_class_signal;

    [NativeTypeName("void (*)(GDExtensionClassLibraryPtr, GDExtensionConstStringNamePtr)")]
    public delegate* unmanaged[Cdecl]<void*, void*, void> classdb_unregister_extension_class;

    [NativeTypeName("void (*)(GDExtensionClassLibraryPtr, GDExtensionStringPtr)")]
    public delegate* unmanaged[Cdecl]<void*, void*, void> get_library_path;
}
