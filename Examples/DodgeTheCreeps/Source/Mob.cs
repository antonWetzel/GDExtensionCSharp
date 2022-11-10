namespace DodgeTheCreeps;

[Register]
public unsafe partial class Mob : RigidBody2D {

	[Export] AnimatedSprite2D animSprite2D;

	[Notify(NotificationReady)]
	void Ready() {
		animSprite2D.playing = true;
		var mobTypes = animSprite2D.frames.GetAnimationNames();
		animSprite2D.animation = mobTypes[GDExtension.Random.Randi() % mobTypes.Size()];
	}

	[Method]
	void OnVisibleOnScreenNotifier2DScreenExited() {
		QueueFree();
	}
}
