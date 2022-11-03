namespace DodgeTheCreeps;

[Register]
public partial class Hud : CanvasLayer {

	[Signal] public delegate void StartGame();
	[Export] Label messageLabel { get; set; }
	[Export] Label scoreLabel { get; set; }
	[Export] Button startButton { get; set; }
	[Export] Timer startButtonTimer { get; set; }
	[Export] Timer startReadyMessageTimer { get; set; }
	[Export] Timer startMessageTimer { get; set; }

	[Method]
	public void ShowGetReady() {
		messageLabel.text = "Get Ready";
		messageLabel.Show();
		startReadyMessageTimer.Start();
	}

	[Method]
	public void ShowGameOver() {
		messageLabel.text = "Game Over";
		messageLabel.Show();
		startMessageTimer.Start();
	}

	[Method]
	public void UpdateScore(long score) {
		scoreLabel.text = $"{score}";
	}

	[Method]
	void OnStartButtonPressed() {
		startButton.Hide();
		EmitSignalStartGame();
	}

	[Method]
	void OnStartMessageTimerTimeout() {
		messageLabel.text = "Dodge the Creeps!";
		messageLabel.Show();
		startButtonTimer.Start();
	}

	[Method]
	unsafe void OnGetReadyMessageTimerTimeout() {
		messageLabel.Hide();
	}

	[Method]
	void OnStartButtonTimer() {
		startButton.Show();
	}
}
