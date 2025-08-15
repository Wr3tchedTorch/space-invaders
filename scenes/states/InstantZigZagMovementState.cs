using Godot;
using SpaceInvaders.Assets.Scripts.Interfaces;
using System;

namespace SpaceInvaders.Scenes.States;

public partial class InstantZigZagMovementState : Node, IState
{
    [Signal] public delegate void TurningEventHandler(Vector2 dir);

    [ExportGroup("Dependencies")]
    [Export] private Timer TurnTimer { get; set; }
    [Export] private float TurnAmountDegs { get; set; }

    public Node2D Parent { get; set; }

    private IMover ParentMover { get; set; } = null;    
    private Vector2 Direction { get; set; }

    private Vector2 direction;
    private bool firstTimeTurning = true;

    private double currentTimerDelay;
    private double initialTimerDelay;

    public void Enter()
    {
        if (Parent is not IMover)
        {
            throw new ArgumentException($"{nameof(InstantStraightMovementState)}: {nameof(Parent)} must be of type {nameof(IMover)}.");
        }
        ParentMover = (IMover)Parent;

        initialTimerDelay = TurnTimer.WaitTime;
        TurnTimer.OneShot = true;
        TurnTimer.Timeout += () =>
        {
            GD.Print($"{nameof(currentTimerDelay)} {currentTimerDelay}");

            TurnAmountDegs *= -1;
            EmitSignal(SignalName.Turning, direction);
        
            TurnTimer.Start(initialTimerDelay * 2);
        };
        TurnTimer.Start();
    }

    public void Exit()
    {
    }

    public void PhysicsUpdate(float delta)
    {
        direction = (Vector2)ParentMover.GetDirection.Call();
        var angle = direction.Angle() + Mathf.DegToRad(TurnAmountDegs);
        direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).Normalized();

        ParentMover.Velocity = direction * ParentMover.Speed * delta;
        ParentMover.Move();
    }

    public void Update(float delta)
    {
    }
}
