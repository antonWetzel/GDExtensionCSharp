namespace GDExtension;

[StructLayout(LayoutKind.Sequential)]
public struct FuncPtr<T> where T : Delegate {
	IntPtr ptr;

	public FuncPtr(T f) {
		ptr = Marshal.GetFunctionPointerForDelegate(f);
	}

	public T Call => Marshal.GetDelegateForFunctionPointer<T>(ptr);
}
