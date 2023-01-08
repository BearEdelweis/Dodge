using Godot;
using System;

public class HUDScript : CanvasLayer
{
    [Signal]
    public delegate void StartGame();

    public void ShowMessage(string text)
    {
        Label message = GetNode<Label>("Message");
        message.Text = text;
        message.Show();

        GetNode<Timer>("MessageTimer").Start();
    }

    async public void ShowGameOver()
    {
        ShowMessage("Game Over");

        Timer messageTimer = GetNode<Timer>("MessageTimer");
        await ToSignal(messageTimer, "timeout");

        //показали 2 сек надпись "Game Over", теперь показываем надпись "Dodge the..." =>

        Label message = GetNode<Label>("Message");
        message.Text = "Dodge the \n Creeps!";
        message.Show();

        // => и еще через 1 сек появляется кнопка "Start"

        await ToSignal(GetTree().CreateTimer(1), "timeout");
        GetNode<Button>("StartButton").Show();
    }

    public void UpdateScore(int score)
    {
        GetNode<Label>("ScoreLabel").Text = score.ToString();
    }

    public void OnMessageTimerTimeout()
    {
        GetNode<Label>("Message").Hide();
    }

    public void OnStartButtonPressed()
    {
        GetNode<Button>("StartButton").Hide();
        EmitSignal("StartGame");
    }
}
