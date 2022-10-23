namespace ExampleGame;

[Register]
public unsafe partial class AntiRotating : Rotating {

	[Export] public double antiRotationSpeed;

	[Notify(NOTIFICATION_READY)]
	void Ready() {
		SetProcess(true);
	}

	[Notify(NOTIFICATION_PROCESS, arguments = "GetProcessDeltaTime()")]
	void Process(double delta) {
		rotation -= delta * antiRotationSpeed;
	}
}
