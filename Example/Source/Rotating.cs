namespace ExampleGame;

[Register]
public unsafe partial class Rotating : Node2D {

	[Export] public double rotationSpeed;

	[Notify(NOTIFICATION_READY)]
	void Ready() {
		SetProcess(true);
	}

	[Notify(NOTIFICATION_PROCESS, arguments = "GetProcessDeltaTime()")]
	void Process(double delta) {
		rotation += delta * rotationSpeed;
	}
}
