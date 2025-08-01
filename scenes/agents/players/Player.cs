using Godot;
using System;

namespace SpaceInvaders.Scenes.Agents.Players;

public partial class Player : CharacterBody2D
{
    private readonly StringName LeftArrowAction = "left";
    private readonly StringName RightArrowAction = "right";

    [Export] public float Speed;

    public override void _PhysicsProcess(double delta)
    {
        var dir = Input.GetAxis(LeftArrowAction, RightArrowAction);

        var velocity = Vector2.Right * (dir * Speed);
        Velocity = velocity;
        MoveAndSlide();
    }
}
