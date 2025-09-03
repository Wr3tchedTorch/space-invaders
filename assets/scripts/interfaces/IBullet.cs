using Godot;
using SpaceInvaders.Assets.Resources.Bullet;
using SpaceInvaders.Scenes.Components;

namespace SpaceInvaders.Assets.Scripts.Interfaces;

public interface IBullet : IAttacker, IPhysicsAgent
{
    public BulletResource BulletResource { get; set; }
    public StateMachine StateMachine { get; set; }
    public Callable GetDirection { get; set; }

    public void SetCollisionLayerValue(int layer, bool v);
    public void SetCollisionMaskValue(int layerNumber, bool value);
}