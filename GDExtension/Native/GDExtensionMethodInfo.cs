namespace GDExtension.Native;

public unsafe partial struct GDExtensionMethodInfo
{
    [NativeTypeName("GDExtensionStringNamePtr")]
    public void* name;

    public GDExtensionPropertyInfo return_value;

    [NativeTypeName("uint32_t")]
    public uint flags;

    [NativeTypeName("int32_t")]
    public int id;

    [NativeTypeName("uint32_t")]
    public uint argument_count;

    public GDExtensionPropertyInfo* arguments;

    [NativeTypeName("uint32_t")]
    public uint default_argument_count;

    [NativeTypeName("GDExtensionVariantPtr *")]
    public void** default_arguments;
}
