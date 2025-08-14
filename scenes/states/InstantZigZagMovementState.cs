using Godot;
using SpaceInvaders.Assets.Scripts.Interfaces;
using System;

namespace SpaceInvaders.Scenes.States;

public partial class InstantZigZagMovementState : Node, IState
{
    [ExportGroup("Dependencies")]
    [Export] private Timer TurnTimer { get; set; }

    public Node2D Parent { get; set; }

    private IMover ParentMover { get; set; } = null;

    public void Enter()
    {
        if (Parent is not IMover)
        {
            throw new ArgumentException($"{nameof(InstantStraightMovementState)}: {nameof(Parent)} must be of type {nameof(IMover)}.");
        }
        ParentMover = (IMover)Parent;

        TurnTimer.OneShot = false;
        TurnTimer.Timeout += () =>
        {
            var velocity = ParentMover.Velocity;
            velocity.X *= -1;
            ParentMover.Velocity = velocity;
        };
        TurnTimer.Start();
    }

    public void Exit()
    {
    }

    public void PhysicsUpdate(float delta)
    {        
        ParentMover.Velocity = (Vector2)ParentMover.GetDirection.Call() * ParentMover.Speed * delta;
        ParentMover.Move();
    }

    public void Update(float delta)
    {
    }
}
