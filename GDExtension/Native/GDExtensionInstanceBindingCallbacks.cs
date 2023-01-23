namespace GDExtension.Native;

public unsafe partial struct GDExtensionInstanceBindingCallbacks
{
    [NativeTypeName("GDExtensionInstanceBindingCreateCallback")]
    public delegate* unmanaged[Cdecl]<void*, void*, void*> create_callback;

    [NativeTypeName("GDExtensionInstanceBindingFreeCallback")]
    public delegate* unmanaged[Cdecl]<void*, void*, void*, void> free_callback;

    [NativeTypeName("GDExtensionInstanceBindingReferenceCallback")]
    public delegate* unmanaged[Cdecl]<void*, void*, byte, byte> reference_callback;
}
