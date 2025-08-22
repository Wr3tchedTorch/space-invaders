using Godot;
using SpaceInvaders.Assets.Scripts.Interfaces;
using SpaceInvaders.Scenes.Components;
using System;

namespace SpaceInvaders.Scenes.States;

public partial class InstantStraightMovementState : Node, IState
{
    public Node2D Parent { get; set; } = null!;

    private IMover? ParentMover { get; set; } = null;

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
        if (Parent == null || !IsInstanceValid(Parent) || ParentMover == null)
        {
            return;
        }
        try
        {
            var dir = (Vector2)ParentMover.GetDirection.Call();
            ParentMover.Velocity = dir * ParentMover.Speed * delta;

            ParentMover.Move(ParentMover.Velocity.Normalized().Angle());
        }
        catch (Exception e)
        {
            GD.PrintErr($"{nameof(InstantStraightMovementState)}: {e.Message}. {Parent.Name} | {IsInstanceValid(Parent)} | {Parent.IsQueuedForDeletion()}");
            throw;
        }
    }

    public void Update(float delta)
    {
    }
}
