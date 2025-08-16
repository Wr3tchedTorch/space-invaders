using Godot;
using SpaceInvaders.Assets.Scripts.Interfaces;
using SpaceInvaders.Scenes.Levels;
using System;

namespace SpaceInvaders.Scenes.States;

public partial class InstantZigZagMovementState : Node, IState
{    
    [ExportGroup("Zig Zag Properties")]
    [Export] private float TurnDelay { get; set; }
    [Export] private float TurnAmountDegs { get; set; }

    public Node2D Parent { get; set; }

    private IMover ParentMover { get; set; } = null;

    private Vector2 direction;

    private Timer turnTimer;

    public void Enter()
    {
        if (Parent is not IMover)
        {
            throw new ArgumentException($"{nameof(InstantStraightMovementState)}: {nameof(Parent)} must be of type {nameof(IMover)}.");
        }
        ParentMover = (IMover)Parent;

        CreateTurnTimer();
    }

    public void Exit()
    {
    }

    public void PhysicsUpdate(float delta)
    {
        direction = (Vector2)ParentMover.GetDirection.Call();
        direction = GetRotatedDirection();

        ParentMover.Velocity = direction * ParentMover.Speed * delta;
        ParentMover.Move();
    }

    public void Update(float delta)
    {
    }

    public override void _Notification(int what)
    {
        if (what == NotificationPredelete && IsInstanceValid(turnTimer))
        {
            turnTimer.QueueFree();
        }
    }

    private Vector2 GetRotatedDirection()
    {
        var angle = direction.Angle() + Mathf.DegToRad(TurnAmountDegs);

        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).Normalized();
    }

    private void CreateTurnTimer()
    {
        turnTimer = new Timer
        {
            WaitTime = TurnDelay,
            OneShot = true,
            Autostart = false
        };

        var gameWorld = GetTree().GetFirstNodeInGroup(nameof(GameWorld));
        gameWorld.AddChild(turnTimer);

        turnTimer.Timeout += () =>
        {
            TurnAmountDegs *= -1;

            turnTimer.Start(turnTimer.WaitTime = TurnDelay * 2);
        };
        turnTimer.Start();
    }
}
