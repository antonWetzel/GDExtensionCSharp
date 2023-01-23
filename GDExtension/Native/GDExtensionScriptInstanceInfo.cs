namespace GDExtension.Native;

public unsafe partial struct GDExtensionScriptInstanceInfo
{
    [NativeTypeName("GDExtensionScriptInstanceSet")]
    public delegate* unmanaged[Cdecl]<void*, void*, void*, byte> set_func;

    [NativeTypeName("GDExtensionScriptInstanceGet")]
    public delegate* unmanaged[Cdecl]<void*, void*, void*, byte> get_func;

    [NativeTypeName("GDExtensionScriptInstanceGetPropertyList")]
    public delegate* unmanaged[Cdecl]<void*, uint*, GDExtensionPropertyInfo*> get_property_list_func;

    [NativeTypeName("GDExtensionScriptInstanceFreePropertyList")]
    public delegate* unmanaged[Cdecl]<void*, GDExtensionPropertyInfo*, void> free_property_list_func;

    [NativeTypeName("GDExtensionScriptInstancePropertyCanRevert")]
    public delegate* unmanaged[Cdecl]<void*, void*, byte> property_can_revert_func;

    [NativeTypeName("GDExtensionScriptInstancePropertyGetRevert")]
    public delegate* unmanaged[Cdecl]<void*, void*, void*, byte> property_get_revert_func;

    [NativeTypeName("GDExtensionScriptInstanceGetOwner")]
    public delegate* unmanaged[Cdecl]<void*, void*> get_owner_func;

    [NativeTypeName("GDExtensionScriptInstanceGetPropertyState")]
    public delegate* unmanaged[Cdecl]<void*, delegate* unmanaged[Cdecl]<void*, void*, void*, void>, void*, void> get_property_state_func;

    [NativeTypeName("GDExtensionScriptInstanceGetMethodList")]
    public delegate* unmanaged[Cdecl]<void*, uint*, GDExtensionMethodInfo*> get_method_list_func;

    [NativeTypeName("GDExtensionScriptInstanceFreeMethodList")]
    public delegate* unmanaged[Cdecl]<void*, GDExtensionMethodInfo*, void> free_method_list_func;

    [NativeTypeName("GDExtensionScriptInstanceGetPropertyType")]
    public delegate* unmanaged[Cdecl]<void*, void*, byte*, GDExtensionVariantType> get_property_type_func;

    [NativeTypeName("GDExtensionScriptInstanceHasMethod")]
    public delegate* unmanaged[Cdecl]<void*, void*, byte> has_method_func;

    [NativeTypeName("GDExtensionScriptInstanceCall")]
    public delegate* unmanaged[Cdecl]<void*, void*, void**, long, void*, GDExtensionCallError*, void> call_func;

    [NativeTypeName("GDExtensionScriptInstanceNotification")]
    public delegate* unmanaged[Cdecl]<void*, int, void> notification_func;

    [NativeTypeName("GDExtensionScriptInstanceToString")]
    public delegate* unmanaged[Cdecl]<void*, byte*, void*, void> to_string_func;

    [NativeTypeName("GDExtensionScriptInstanceRefCountIncremented")]
    public delegate* unmanaged[Cdecl]<void*, void> refcount_incremented_func;

    [NativeTypeName("GDExtensionScriptInstanceRefCountDecremented")]
    public delegate* unmanaged[Cdecl]<void*, byte> refcount_decremented_func;

    [NativeTypeName("GDExtensionScriptInstanceGetScript")]
    public delegate* unmanaged[Cdecl]<void*, void*> get_script_func;

    [NativeTypeName("GDExtensionScriptInstanceIsPlaceholder")]
    public delegate* unmanaged[Cdecl]<void*, byte> is_placeholder_func;

    [NativeTypeName("GDExtensionScriptInstanceSet")]
    public delegate* unmanaged[Cdecl]<void*, void*, void*, byte> set_fallback_func;

    [NativeTypeName("GDExtensionScriptInstanceGet")]
    public delegate* unmanaged[Cdecl]<void*, void*, void*, byte> get_fallback_func;

    [NativeTypeName("GDExtensionScriptInstanceGetLanguage")]
    public delegate* unmanaged[Cdecl]<void*, void*> get_language_func;

    [NativeTypeName("GDExtensionScriptInstanceFree")]
    public delegate* unmanaged[Cdecl]<void*, void> free_func;
}
