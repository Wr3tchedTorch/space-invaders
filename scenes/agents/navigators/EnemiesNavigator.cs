using Godot;
using System;

namespace SpaceInvaders.Scenes.Navigators;

public partial class EnemiesNavigator : Node2D
{
    [Signal] public delegate void MovedEventHandler();

    [Export] public float HorizontalMovementIncrement { get; set; }
    [Export] public float VerticalMovementIncrement { get; set; }

    [Export] public float DelayBetweenMovements { get; set; }
    [Export] public Timer MovementTimer { get; set; } = null!;

    private Vector2 Direction = Vector2.Right;

    private bool _moving = true;

    public override void _Ready()
    {
        MovementTimer.WaitTime = DelayBetweenMovements;
        MovementTimer.Timeout += () =>
        {
            Move();

            EmitSignal(SignalName.Moved);
        };
        MovementTimer.Start();        
    }

    private void Move()
    {
        GlobalPosition += Direction * HorizontalMovementIncrement;
    }

    public void OnRightWallEntered(Area2D _)
    {
        GD.Print($"Right wall hit. Direction: {Direction}");
        if (Mathf.Sign(Direction.X) == 1)
        {
            Direction = new Vector2(-Mathf.Abs(Direction.X), Direction.Y);
            GlobalPosition += new Vector2(0, VerticalMovementIncrement);
        }
    }

    public void OnLeftWallEntered(Area2D _)
    {
        GD.Print($"Left wall hit. Direction: {Direction}");
        if (Mathf.Sign(Direction.X) == -1)
        {
            Direction = new Vector2(Mathf.Abs(Direction.X), Direction.Y);
            GlobalPosition += new Vector2(0, VerticalMovementIncrement);
        }
    }
}
