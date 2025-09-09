using System;
using Godot;

namespace SpaceInvaders.Assets.Scripts.Interfaces;

public interface IPhysicsAgent
{
    public Vector2 GlobalPosition { get; set; }
    public float Rotation { get; set; }

    public void SetPhysicsLayer(uint layer);
    public void SetPhysicsMask(uint mask);
}
