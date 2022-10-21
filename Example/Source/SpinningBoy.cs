namespace ExampleGame;

[Register]
public unsafe partial class SpinningBoy : Node2D {

	[Export] public double rotationSpeed;

	[Notify(NOTIFICATION_READY)]
	public void Ready() {
		SetProcess(true);
	}

	[Notify(NOTIFICATION_PROCESS, arguments = "GetProcessDeltaTime()")]
	public void Process(double delta) {
		rotation += delta * rotationSpeed;
	}
}
