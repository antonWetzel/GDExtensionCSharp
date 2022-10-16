namespace GDExtension;

public unsafe partial struct Projection {

	public Projection(
		double xAxisX, double xAxisY, double xAxisZ, double xAxisW,
		double yAxisX, double yAxisY, double yAxisZ, double yAxisW,
		double zAxisX, double zAxisY, double zAxisZ, double zAxisW,
		double wAxisX, double wAxisY, double wAxisZ, double wAxisW
	) {
		var constructor = gdInterface.variant_get_ptr_constructor.Call(VariantType.Projection, 3);
		var x = new Vector4(xAxisX, xAxisY, xAxisZ, xAxisW);
		var y = new Vector4(yAxisX, yAxisY, yAxisZ, yAxisW);
		var z = new Vector4(zAxisX, zAxisY, zAxisZ, zAxisW);
		var w = new Vector4(wAxisX, wAxisY, wAxisZ, wAxisW);
		var args = stackalloc TypePtr[4];
		args[0] = new IntPtr(&x);
		args[1] = new IntPtr(&y);
		args[2] = new IntPtr(&z);
		args[3] = new IntPtr(&w);
		fixed (Projection* ptr = &this) {
			constructor(new IntPtr(ptr), args);
		}
	}
}
