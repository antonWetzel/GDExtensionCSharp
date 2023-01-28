namespace DodgeTheCreeps;

[Register]
public unsafe partial class Mob : RigidBody2D {

	[Export] AnimatedSprite2D animSprite2D;

	[Notify(NotificationReady)]
	void Ready() {
		animSprite2D.Play();
		var mobTypes = animSprite2D.sprite_frames.GetAnimationNames();
		animSprite2D.animation = mobTypes[Randi() % mobTypes.Size()];
	}

	[Method]
	void OnVisibleOnScreenNotifier2DScreenExited() {
		QueueFree();
	}
}
