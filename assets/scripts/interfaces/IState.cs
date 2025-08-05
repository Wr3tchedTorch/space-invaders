using System;
using Godot;

namespace SpaceInvaders.Assets.scripts.interfaces;

public interface IState
{
    public Node2D Parent { get; set; }

    public void Enter();
    public void Exit();
    public void Update(float delta);
    public void PhysicsUpdate(float delta);
}
