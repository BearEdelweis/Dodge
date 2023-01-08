using Godot;

public class HUDScript : CanvasLayer
{
    [Signal]
    public delegate void StartGame();

    private Label messageLabel;
    private Label scoreLabel;
    private Timer messageTimer;
    private Button startButton;

    public override void _Ready()
    {
        messageLabel = GetNode<Label>("Message");
        scoreLabel = GetNode<Label>("ScoreLabel");
        messageTimer = GetNode<Timer>("MessageTimer");
        startButton = GetNode<Button>("StartButton");
    }

    public void ShowMessage(string text)
    {
        messageLabel.Text = text;
        messageLabel.Show();

        messageTimer.Start();
    }

    async public void ShowGameOver()
    {
        ShowMessage("Game Over");
        await ToSignal(messageTimer, "timeout");

        //показали 2 сек надпись "Game Over", теперь показываем надпись "Dodge the..." =>
        messageLabel.Text = "Dodge the \n Creeps!";
        messageLabel.Show();

        // => и еще через 1 сек появляется кнопка "Start"
        await ToSignal(GetTree().CreateTimer(1), "timeout");
        startButton.Show();
    }

    public void UpdateScore(int score)
    {
        scoreLabel.Text = score.ToString();
    }

    public void OnMessageTimerTimeout()
    {
        messageLabel.Hide();
    }

    public void OnStartButtonPressed()
    {
        startButton.Hide();
        EmitSignal("StartGame");
    }
}