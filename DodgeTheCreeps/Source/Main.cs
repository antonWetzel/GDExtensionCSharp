namespace DodgeTheCreeps;

[Register]
public unsafe partial class Main : Node {

	[Export] PackedScene mobScene { get; set; }

	public long score;

	[Notify(NOTIFICATION_READY)]
	void Ready() {
		GDExtension.Random.Randomize();
	}

	[Method]
	public void GameOver() {
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();
		var hud = GetNode<Hud>("Hud");
		hud.ShowGameOver();
		var mobs = GetTree().GetNodesInGroup("mobs");
		for (var i = 0; i < mobs.Size(); i++) {
			var test = mobs[i];
			var node = new Node(test.AsObject()._internal_pointer);
			node.QueueFree();
		}
		//GetTree().CallGroup("mobs", "queue_free"); //crashes (probatly problems with vararg)

		var music = GetNode<AudioStreamPlayer>("Music");
		music.Stop();

		var deathSound = GetNode<AudioStreamPlayer>("DeathSound");
		deathSound.Play(0.0);
	}

	[Method]
	public void NewGame() {
		score = 0;
		var player = GetNode<Player>("Player");
		var startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.position);
		GetNode<Timer>("StartTimer").Start(-1.0);

		var hud = GetNode<Hud>("Hud");
		hud.UpdateScore(score);
		hud.ShowGetReady();

		var music = GetNode<AudioStreamPlayer>("Music");
		music.Play(0.0);
	}

	[Method]
	public void OnScoreTimerTimeout() {
		score += 1;
		var hud = GetNode<Hud>("Hud");
		hud.UpdateScore(score);
	}

	[Method]
	public void OnStartTimerTimeout() {
		GetNode<Timer>("MobTimer").Start(-1.0);
		GetNode<Timer>("ScoreTimer").Start(-1.0);
	}

	[Method]
	public void OnMobTimerTimeout() {
		// Create a new instance of the Mob scene.
		var mob = mobScene.Instantiate<Mob>(PackedScene.GenEditState.GEN_EDIT_STATE_DISABLED);

		// Choose a random location on Path2D.
		var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
		mobSpawnLocation.progress_ratio = GDExtension.Random.Randf();

		// Set the mob's direction perpendicular to the path direction.
		double direction = mobSpawnLocation.rotation + System.Math.PI / 2.0;

		// Set the mob's position to a random location.
		mob.position = mobSpawnLocation.position;

		// Add some randomness to the direction.
		direction += GDExtension.Random.RandfRange(-System.Math.PI / 4.0, System.Math.PI / 4.0);
		mob.rotation = direction;

		// Choose the velocity.
		var velocity = new Vector2(GDExtension.Random.RandfRange(150.0, 250.0), 0.0);
		mob.linear_velocity = velocity.Rotated(direction);

		AddChild(mob, false, InternalMode.INTERNAL_MODE_DISABLED);
	}
}
