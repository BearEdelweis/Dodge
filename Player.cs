using Godot;
using System;

public class Player : Area2D
{
    [Signal]
    public delegate void Hit();

    [Export]
    public int speed;

    public Vector2 screenSize;

    private Vector2 velocity;

    private AnimatedSprite animatedSprite;

    public override void _Ready()
    {
        screenSize = GetViewportRect().Size;
        velocity = Vector2.Zero;
        animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        Hide();
    }

    public override void _Process(float delta)
    {
        velocity = Vector2.Zero;
        if (Input.IsActionPressed("ui_up")) velocity.y -= 1;
        if (Input.IsActionPressed("ui_down")) velocity.y += 1;
        if (Input.IsActionPressed("ui_left")) velocity.x -= 1;
        if (Input.IsActionPressed("ui_right")) velocity.x += 1;

        if (velocity.Length() > 0)
        {
            velocity = velocity.Normalized() * speed;
            animatedSprite.Play();
        }
        else animatedSprite.Stop();

        if(velocity.x!=0)
        {
            animatedSprite.Animation = "walk";
            animatedSprite.FlipV = false;
            animatedSprite.FlipH = (velocity.x < 0);
        }
        else if(velocity.y!=0)
        {
            animatedSprite.Animation = "up";
            animatedSprite.FlipV = (velocity.y > 0);
        }
        Position += velocity*delta;
        Position = new Vector2(
            x: Mathf.Clamp(Position.x, 0, screenSize.x),
            y: Mathf.Clamp(Position.y, 0, screenSize.y)
            );
    }
    public void OnPlayerBodyEntered(PhysicsBody2D body)
    {
        GD.Print("public void OnPlayerBodyEntered() works.");
        Hide(); //disappears after hit
        EmitSignal(nameof(Hit)); // эмиссия сигнала Hit()
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true); //setdeferred это безопасный вариант set, не возбуждающий ошибки
    }
    public void Start(Vector2 pos)
    {
        Position = pos;
        Show();
        GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
    }
}