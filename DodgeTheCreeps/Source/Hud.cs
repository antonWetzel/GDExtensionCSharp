namespace DodgeTheCreeps;

[Register]
public partial class Hud : CanvasLayer {

	[Signal] public event Action startGame;

	[Notify(NOTIFICATION_READY)]
	void Ready() {
	}

	[Method]
	public void ShowGetReady() {
		var message = GetNode<Label>("Message");
		message.text = "Get Ready";
		message.Show();
		GetNode<Timer>("GetReadyMessageTimer").Start(-1.0);
	}

	[Method]
	public void ShowGameOver() {
		var message = GetNode<Label>("Message");
		message.text = "Game Over";
		message.Show();
		GetNode<Timer>("StartMessageTimer").Start(-1.0);
	}

	[Method]
	public void UpdateScore(long score) {
		var l = GetNode<Label>("ScoreLabel");
		l.text = $"{score}";
	}

	[Method]
	void OnStartButtonPressed() {
		GetNode<Button>("StartButton").Hide();
		EmitSignal(nameof(startGame));
	}

	[Method]
	void OnStartMessageTimerTimeout() {
		var message = GetNode<Label>("Message");
		message.text = "Dodge the Creeps!";
		message.Show();
		GetNode<Timer>("StartButtonTimer").Start(-1.0);
	}

	[Method]
	void OnGetReadyMessageTimerTimeout() {
		var message = GetNode<Label>("Message");
		message.Hide();
	}

	[Method]
	void OnStartButtonTimer() {
		GetNode<Button>("StartButton").Show();
	}
}
