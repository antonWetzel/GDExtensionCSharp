namespace GDExtension;

public unsafe partial struct Transform2D {

	public Transform2D(
		double xAxisX, double xAxisY,
		double yAxisX, double yAxisY,
		double originX, double originY
	) {
		var x = new Vector2(xAxisX, xAxisY);
		var y = new Vector2(yAxisX, yAxisY);
		var o = new Vector2(originX, originY);
		var constructor = Initialization.inter.variant_get_ptr_constructor(VariantType.Transform2D, 4);
		var args = stackalloc IntPtr[3];
		args[0] = new IntPtr(&x);
		args[1] = new IntPtr(&y);
		args[2] = new IntPtr(&o);
		fixed (Transform2D* ptr = &this) {
			constructor(new IntPtr(ptr), args);
		}
	}
}
