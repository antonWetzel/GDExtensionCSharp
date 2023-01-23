namespace GDExtension.Native;

public unsafe partial struct GDExtensionClassMethodInfo
{
    [NativeTypeName("GDExtensionStringNamePtr")]
    public void* name;

    public void* method_userdata;

    [NativeTypeName("GDExtensionClassMethodCall")]
    public delegate* unmanaged[Cdecl]<void*, void*, void**, long, void*, GDExtensionCallError*, void> call_func;

    [NativeTypeName("GDExtensionClassMethodPtrCall")]
    public delegate* unmanaged[Cdecl]<void*, void*, void**, void*, void> ptrcall_func;

    [NativeTypeName("uint32_t")]
    public uint method_flags;

    [NativeTypeName("GDExtensionBool")]
    public byte has_return_value;

    public GDExtensionPropertyInfo* return_value_info;

    public GDExtensionClassMethodArgumentMetadata return_value_metadata;

    [NativeTypeName("uint32_t")]
    public uint argument_count;

    public GDExtensionPropertyInfo* arguments_info;

    public GDExtensionClassMethodArgumentMetadata* arguments_metadata;

    [NativeTypeName("uint32_t")]
    public uint default_argument_count;

    [NativeTypeName("GDExtensionVariantPtr *")]
    public void** default_arguments;
}
