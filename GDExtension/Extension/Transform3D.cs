namespace GDExtension;

public unsafe partial struct Transform3D {

	public Transform3D(
		double xAxisX, double xAxisY, double xAxisZ,
		double yAxisX, double yAxisY, double yAxisZ,
		double zAxisX, double zAxisY, double zAxisZ,
		double originX, double originY, double originZ
	) {
		var x = new Vector3(xAxisX, xAxisY, xAxisZ);
		var y = new Vector3(yAxisX, yAxisY, yAxisZ);
		var z = new Vector3(zAxisX, zAxisY, zAxisZ);
		var o = new Vector3(originX, originY, originZ);
		var constructor = Initialization.inter.variant_get_ptr_constructor(VariantType.Transform3D, 3);
		var args = stackalloc IntPtr[4];
		args[0] = new IntPtr(&x);
		args[1] = new IntPtr(&y);
		args[2] = new IntPtr(&z);
		args[3] = new IntPtr(&o);
		fixed (Transform3D* ptr = &this) {
			constructor(new IntPtr(ptr), args);
		}
	}
}
