namespace DodgeTheCreeps;

[Register]
public unsafe partial class Player : Area2D {

	[Signal] public delegate void Hit();
	[Export] long speed { get; set; } = 400;
	[Export] AnimatedSprite2D animatedSprite2D { get; set; }
	[Export] CollisionShape2D collisionShape { get; set; }

	public Vector2 screenSize;

	[Notify(Notifications.Ready)]
	void Ready() {
		SetProcess(true);
		screenSize = GetViewportRect().size;
		Hide();
	}

	[Notify(Notifications.Process, arguments = "GetProcessDeltaTime()")]
	void Process(double delta) {
		var velocity = Vector2.Zero;
		if (Input.IsActionPressed("move_up")) {
			velocity.y -= 1f;
		}
		if (Input.IsActionPressed("move_left")) {
			velocity.x -= 1f;
		}
		if (Input.IsActionPressed("move_down")) {
			velocity.y += 1f;
		}
		if (Input.IsActionPressed("move_right")) {
			velocity.x += 1f;
		}

		if (velocity.Length() > 0.0) {
			velocity = velocity.Normalized() * speed;
			animatedSprite2D.Play();
		} else {
			animatedSprite2D.Stop();
		}

		position += velocity * delta;
		position = new Vector2(
			//GDExtension.Math.Clamp(position.x, 0.0, screenSize.x).AsFloat(), //does not work
			//GDExtension.Math.Clamp(position.y, 0.0, screenSize.y).AsFloat()
			System.Math.Clamp(position.x, 0.0, screenSize.x),
			System.Math.Clamp(position.y, 0.0, screenSize.y)
		);

		if (velocity.x != 0f) {
			animatedSprite2D.animation = "walk";
			animatedSprite2D.flip_v = false;
			animatedSprite2D.flip_h = velocity.x < 0f;
		} else if (velocity.y != 0) {
			animatedSprite2D.animation = "up";
			animatedSprite2D.flip_v = velocity.y > 0f;
		}
	}

	[Method]
	public void Start(Vector2 pos) {
		position = pos;
		Show();
		collisionShape.disabled = false;
	}

	[Method]
	void OnPlayerBodyEntered(PhysicsBody2D body) {
		Hide();
		EmitSignalHit();
		collisionShape.SetDeferred("disabled", true);
	}
}
