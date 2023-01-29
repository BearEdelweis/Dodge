using Godot;

public class Player : Area2D
{
    [Signal]
    public delegate void Hit();

    [Export]
    public int speed;

    public Vector2 screenSize;

    private Vector2 velocity;

    private AnimatedSprite animatedSprite;
    private CollisionShape2D collisionShape2D;

    private TouchInputScript touchInputScript;

    public override void _Ready()
    {
        screenSize = GetViewportRect().Size;
        velocity = Vector2.Zero;
        animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");
        touchInputScript = GetParent().GetNode<TouchInputScript>("TouchInputScript");
        Hide();
    }

    public override void _Process(float delta)
    {
        velocity = touchInputScript.dragSpeed;

        //velocity = Vector2.Zero;
        //if (Input.IsActionPressed("ui_up")) velocity.y -= 1;
        //if (Input.IsActionPressed("ui_down")) velocity.y += 1;
        //if (Input.IsActionPressed("ui_left")) velocity.x -= 1;
        //if (Input.IsActionPressed("ui_right")) velocity.x += 1;

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
        Hide(); //disappears after hit
        EmitSignal(nameof(Hit)); // эмиссия сигнала Hit()
        collisionShape2D.SetDeferred("disabled", true); //setdeferred это безопасный вариант set, не возбуждающий ошибки
    }
    public void Start(Vector2 pos)
    {
        Position = pos;
        velocity = Vector2.Zero;
        Show();
        collisionShape2D.Disabled = false;
    }
}