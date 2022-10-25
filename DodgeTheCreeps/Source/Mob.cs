namespace DodgeTheCreeps;

[Register]
public unsafe partial class Mob : RigidBody2D {

	[Notify(NOTIFICATION_READY)]
	void Ready() {
		var animSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2d");
		animSprite2D.playing = true;
		var mobTypes = animSprite2D.frames.GetAnimationNames();
		animSprite2D.animation = mobTypes[GDExtension.Random.Randi() % mobTypes.Size()];
	}

	[Method]
	void OnVisibleOnScreenNotifier2DScreenExited() {
		QueueFree();
	}
}
