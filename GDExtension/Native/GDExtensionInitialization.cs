namespace GDExtension.Native;

public unsafe partial struct GDExtensionInitialization
{
    public GDExtensionInitializationLevel minimum_initialization_level;

    public void* userdata;

    [NativeTypeName("void (*)(void *, GDExtensionInitializationLevel)")]
    public delegate* unmanaged[Cdecl]<void*, GDExtensionInitializationLevel, void> initialize;

    [NativeTypeName("void (*)(void *, GDExtensionInitializationLevel)")]
    public delegate* unmanaged[Cdecl]<void*, GDExtensionInitializationLevel, void> deinitialize;
}
