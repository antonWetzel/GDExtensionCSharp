namespace ExampleGame;

[Register]
public unsafe partial class Walker : Node2D {

	[Export] public double speed;
	[Export] public long whatever;

	[Notify(NOTIFICATION_READY)]
	public void Ready() {
		General.Prints("hello", whatever);
		SetProcess(true);
	}

	[Notify(NOTIFICATION_PROCESS, arguments = "GetProcessDeltaTime()")]
	public void Process(double delta) {
		position += Vector2.RIGHT * delta * speed;
	}

}
