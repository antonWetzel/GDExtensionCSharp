namespace DodgeTheCreeps;

[Register]
public unsafe partial class Main : Node {

	[Export] PackedScene mobScene;
	[Export] Timer mobTimer;
	[Export] Timer scoreTimer;
	[Export] Hud hud;
	[Export] Player player;
	[Export] AudioStreamPlayer music;
	[Export] AudioStreamPlayer deathSound;
	[Export] Marker2D startPosition;
	[Export] Timer startTimer;
	[Export] PathFollow2D mobSpawnLocation;

	long score;

	[Notify(NotificationReady)]
	void Ready() {
		GDExtension.Random.Randomize();
	}

	[Method]
	public void GameOver() {
		mobTimer.Stop();
		scoreTimer.Stop();
		hud.ShowGameOver();
		//GetTree().CallGroup("mobs", "queue_free"); //crashes
		var mobs = GetTree().GetNodesInGroup("mobs");
		for (var i = 0; i < mobs.Size(); i++) {
			var test = mobs[i];
			var mob = (Mob)(test.AsObject());
			mob.QueueFree();
		}
		music.Stop();
		deathSound.Play();
	}

	[Method]
	public void NewGame() {
		score = 0;

		player.Start(startPosition.position);
		startTimer.Start();

		hud.UpdateScore(score);
		hud.ShowGetReady();

		music.Play();
	}

	[Method]
	public void OnScoreTimerTimeout() {
		score += 1;
		hud.UpdateScore(score);
	}

	[Method]
	public void OnStartTimerTimeout() {
		mobTimer.Start();
		scoreTimer.Start();
	}

	[Method]
	public void OnMobTimerTimeout() {
		// Create a new instance of the Mob scene.
		var mob = (Mob)mobScene.Instantiate(PackedScene.GenEditState.Disabled);

		// Choose a random location on Path2D.
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

		AddChild(mob);
	}
}
