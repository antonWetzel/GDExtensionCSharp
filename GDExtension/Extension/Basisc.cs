namespace GDExtension;

public unsafe partial struct Basis {

	public Basis(
		double xAxisX, double xAxisY, double xAxisZ,
		double yAxisX, double yAxisY, double yAxisZ,
		double zAxisX, double zAxisY, double zAxisZ
	) {
		var constructor = gdInterface.variant_get_ptr_constructor.Call(VariantType.Basis, 4);
		var x = new Vector3(xAxisX, xAxisY, xAxisZ);
		var y = new Vector3(yAxisX, yAxisY, yAxisZ);
		var z = new Vector3(zAxisX, zAxisY, zAxisZ);
		var args = stackalloc TypePtr[3];
		args[0] = new IntPtr(&x);
		args[1] = new IntPtr(&y);
		args[2] = new IntPtr(&z);
		fixed (Basis* ptr = &this) {
			constructor(new IntPtr(ptr), args);
		}
	}
}
