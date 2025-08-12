using System.Data;
using Godot;
using SpaceInvaders.Assets.Resources;

namespace SpaceInvaders.Assets.Scripts.Interfaces;

public interface IBullet
{
    public BulletResource BulletResource { get; set; }
    public Vector2 GlobalPosition { get; set; }
}