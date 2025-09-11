using System;
using Godot;
using SpaceInvaders.Assets.Resources.Bullet;
using SpaceInvaders.Assets.Scripts.Extensions;
using SpaceInvaders.Assets.Scripts.Interfaces;
using SpaceInvaders.Scenes.Components;

namespace SpaceInvaders.Scenes.Agents.Bullets;

public partial class Laser : Area2D, IBullet, IMover
{
    [ExportGroup("Dependencies")]
    [Export] public VelocityComponent VelocityComponent { get; set; } = null!;
    [Export] public StateMachine StateMachine { get; set; } = null!;    

    public BulletResource BulletResource { get; set; } = null!;
    private float rotationOffset;

    public Callable GetDirection
    {
        get => _getDirection;
        set
        {
            _getDirection = value;

            direction = (Vector2)GetDirection.Call();

            _getDirection = new Callable(this, MethodName.GetBulletDirection);
        }
    }

    public Vector2 Velocity { get; set; }
    public float Speed
    {
        get => BulletResource.Speed;
        set => BulletResource.Speed = value;
    }

    public float Damage { get => BulletResource.Damage; }

    private Vector2 direction;
    private Callable _getDirection;

    public override void _Ready()
    {
        StateMachine.Enter();

        AreaEntered += OnAreaEntered;
        BodyEntered += OnBodyEntered;

        rotationOffset = Rotation;
    }

    private Vector2 GetBulletDirection()
    {
        return direction.Rotated(rotationOffset);
    }

    protected virtual void OnBodyEntered(Node2D body)
    {
        Callable.From(QueueFree).CallDeferred();
    }

    protected virtual void OnAreaEntered(Area2D area)
    {
        Callable.From(QueueFree).CallDeferred();
    }

    public void Move(float angle)
    {
        GlobalPosition += Velocity;
    }

    public void SetPhysicsLayer(uint layer)
    {
        this.ClearPhysicsLayers();

        SetCollisionLayerValue((int) layer, true);
    }

    public void SetPhysicsMask(uint mask)
    {
        this.ClearPhysicsMasks();
                
        SetCollisionMaskValue((int) mask, true);
    }
}
