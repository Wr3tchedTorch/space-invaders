using Godot;
using System;

namespace SpaceInvaders.Scenes.Components;

public partial class VelocityComponent : Node
{
    public Vector2 Velocity { get; private set; }

    public Callable GetDirection { get; set; }
    public float Speed { get; set; }

    public override void _Ready()
    {
        base._Ready();
    }

    public Vector2 GetVelocity(float delta)
    {
        var direction = (Vector2)GetDirection.Call();

        Velocity += direction * Speed * (float)delta;

        return Velocity;
    }
}
