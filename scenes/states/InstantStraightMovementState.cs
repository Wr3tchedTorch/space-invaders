using Godot;
using SpaceInvaders.Assets.Scripts.Interfaces;
using SpaceInvaders.Scenes.Components;
using System;

namespace SpaceInvaders.Scenes.States;

public partial class InstantStraightMovementState : Node, IState
{
    public Node2D Parent { get; set; }

    private IMover ParentMover { get; set; } = null;    

    public void Enter()
    {
        if (Parent is not IMover)
        {
            throw new ArgumentException($"{nameof(InstantStraightMovementState)}: {nameof(Parent)} must be of type {nameof(IMover)}.");
        }
        ParentMover = (IMover)Parent;
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
