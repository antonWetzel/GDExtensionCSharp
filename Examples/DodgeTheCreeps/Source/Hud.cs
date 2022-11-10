namespace DodgeTheCreeps;

[Register]
public partial class Hud : CanvasLayer {

	[Signal] public delegate void StartGame();
	[Export] Label messageLabel;
	[Export] Label scoreLabel;
	[Export] Button startButton;
	[Export] Timer startButtonTimer;
	[Export] Timer startReadyMessageTimer;
	[Export] Timer startMessageTimer;

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
