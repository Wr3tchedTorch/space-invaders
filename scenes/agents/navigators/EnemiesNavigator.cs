using Godot;
using SpaceInvaders.Scenes.Autoloads;
using System;

namespace SpaceInvaders.Scenes.Navigators;

public partial class EnemiesNavigator : Node2D
{
    [Signal] public delegate void MovedEventHandler();

    [Export] public float HorizontalMovementIncrement { get; set; }
    [Export] public float VerticalMovementIncrement { get; set; }

    [Export] public float MaxDelayBetweenMovements { get; set; }
    [Export] public float DelayDecreaseBetweenRows { get; set; }
    [Export] public Timer MovementTimer { get; set; } = null!;

    public float CurrentDelayBetweenMovements
    {
        get => _currentDelayBetweenMovements;
        set
        {
            _currentDelayBetweenMovements = value;

            MovementTimer.WaitTime = Mathf.Max(value, 0.05f);
        }
    }
    private float _currentDelayBetweenMovements;

    private Vector2 Direction = Vector2.Right;
    private Vector2 initialPosition;

    public override void _Ready()
    {
        initialPosition = GlobalPosition;

        GameEvents.Instance.GameOver += MovementTimer.Stop;
        GameEvents.Instance.LevelEnded += Reset;

        CurrentDelayBetweenMovements = MaxDelayBetweenMovements;

        MovementTimer.WaitTime = MaxDelayBetweenMovements;
        MovementTimer.Timeout += () =>
        {
            Move();

            EmitSignal(SignalName.Moved);
        };
    }

    public void StartMoving()
    {
        MovementTimer.Start();
    }

    private void Reset()
    {        
        MovementTimer.Stop();
        MovementTimer.WaitTime = MaxDelayBetweenMovements;
        CurrentDelayBetweenMovements = MaxDelayBetweenMovements;
        Direction = Vector2.Right;
        GlobalPosition = initialPosition;
    }

    private void Move()
    {
        GlobalPosition += Direction * HorizontalMovementIncrement;
    }

    public void OnRightWallEntered(Area2D _)
    {
        if (Mathf.Sign(Direction.X) == 1)
        {
            Direction = new Vector2(-Mathf.Abs(Direction.X), Direction.Y);

            GoDown();
        }
    }

    public void OnLeftWallEntered(Area2D _)
    {
        if (Mathf.Sign(Direction.X) == -1)
        {
            Direction = new Vector2(Mathf.Abs(Direction.X), Direction.Y);

            GoDown();
        }
    }

    private void GoDown()
    {
        CurrentDelayBetweenMovements -= DelayDecreaseBetweenRows;
        GlobalPosition += new Vector2(0, VerticalMovementIncrement);
    }
}
