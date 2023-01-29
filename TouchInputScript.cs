using Godot;
using System;

public class TouchInputScript : Node
{
    public Vector2 dragSpeed;

    public override void _Ready()
    {
        dragSpeed = Vector2.Zero;
    }
    public override void _Input(InputEvent iE)
    {
        if(iE is InputEventScreenDrag sD)
        {
            dragSpeed = sD.Speed;
        }
    }
    public void OnHUDStartGame()
    {
        dragSpeed = Vector2.Zero;
    }
}
