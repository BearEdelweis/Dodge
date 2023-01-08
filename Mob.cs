using Godot;
using System;

public class Mob : RigidBody2D
{
    private AnimatedSprite animSprite;
    private string[] mobTypes;
    private int typeIndex;

    public override void _Ready()
    {
        animSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        mobTypes = animSprite.Frames.GetAnimationNames();
        //typeIndex = Convert.ToInt32(GD.Randi() % mobTypes.Length);
        typeIndex = RandomRange(0, mobTypes.Length);
        animSprite.Animation = mobTypes[typeIndex];
    }
    private int RandomRange(int fromValue, int toValue)
    {
        Random random = new Random();
        int result = random.Next(fromValue, toValue);
        return result;
    }
    public void OnVisibilityNotifier2DScreenExited()
    {
        QueueFree();
    }
}