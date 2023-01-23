namespace GDExtension.Native;

public unsafe partial struct GDExtensionPropertyInfo
{
    public GDExtensionVariantType type;

    [NativeTypeName("GDExtensionStringNamePtr")]
    public void* name;

    [NativeTypeName("GDExtensionStringNamePtr")]
    public void* class_name;

    [NativeTypeName("uint32_t")]
    public uint hint;

    [NativeTypeName("GDExtensionStringPtr")]
    public void* hint_string;

    [NativeTypeName("uint32_t")]
    public uint usage;
}
