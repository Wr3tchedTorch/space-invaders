using Godot;
using SpaceInvaders.Assets.Resources.Bullet;
using SpaceInvaders.Scenes.Components;

namespace SpaceInvaders.Assets.Scripts.Interfaces;

public interface IBullet : IAttacker
{
    public BulletResource BulletResource { get; set; }
    public StateMachine StateMachine { get; set; }
    public Vector2 GlobalPosition { get; set; }
    public Callable GetDirection { get; set; }
}