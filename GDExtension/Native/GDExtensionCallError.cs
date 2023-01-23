namespace GDExtension.Native;

public partial struct GDExtensionCallError
{
    public GDExtensionCallErrorType error;

    [NativeTypeName("int32_t")]
    public int argument;

    [NativeTypeName("int32_t")]
    public int expected;
}
