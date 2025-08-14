using Godot;
using SpaceInvaders.Assets.Scripts.Interfaces;
using SpaceInvaders.Scenes.Components;
using System;

namespace SpaceInvaders.Scenes.States;

public partial class StraightMovementState : Node, IState
{    
    public Node2D Parent { get; set; }

    private IMover ParentMover { get; set; } = null;

    [ExportGroup("Dependencies")]
    [Export] public VelocityComponent VelocityComponent { get; set; }

    public void Enter()
    {
        if (Parent is not IMover)
        {
            throw new ArgumentException($"{nameof(StraightMovementState)}: {nameof(Parent)} must be of type {nameof(IMover)}.");
        }
        ParentMover = (IMover)Parent;

        VelocityComponent.GetDirection = ParentMover.GetDirection;
        VelocityComponent.Speed = ParentMover.Speed;
    }

    public void Exit()
    {
        GD.Print("Exiting");
        if (ParentMover != null)
        {
            ParentMover.Velocity = Vector2.Zero;
        }
    }

    public void PhysicsUpdate(float delta)
    {
        ParentMover.Velocity = VelocityComponent.GetVelocity(delta);
        ParentMover.Move();
    }

    public void Update(float delta)
    {
    }
}
