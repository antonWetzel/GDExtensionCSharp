using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ExampleGame;

[Register(name = "Test")]
public unsafe partial class TestClass : Node2D {

	public double speed;

	//attribute to create notification function
	//maybe [Notify(NOTIFICATION_PROCESS, new [] { "GetProcessDeltaTime()" })]
	//or short "alias" [Process]
	public void Process(double delta) {
		position += Vector2.RIGHT * delta * speed;
	}

	//todo: automate
	public void Notification(int what) {
		switch (what) {
		case NOTIFICATION_PROCESS:
			Process(GetProcessDeltaTime());
			break;
		}
	}


}
