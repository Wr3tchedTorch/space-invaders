using System;
using Godot;

namespace SpaceInvaders.Assets.Scripts.Interfaces;

public interface IMover
{
    public float Speed { get; set; }
    public Callable GetDirection { get; set; }
    public Vector2 Velocity { get; set; }
    public void Move(float angle);
}
