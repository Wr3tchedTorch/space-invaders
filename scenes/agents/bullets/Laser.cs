using Godot;
using SpaceInvaders.Assets.Resources.Bullet;
using SpaceInvaders.Assets.Scripts.Interfaces;
using SpaceInvaders.Scenes.Components;
using SpaceInvaders.Scenes.States;
using System;

namespace SpaceInvaders.Scenes.Agents.Bullets;

public partial class Laser : Area2D, IBullet, IMover
{
    [ExportGroup("Dependencies")]
    [Export] public VelocityComponent VelocityComponent { get; set; }
    [Export] public StateMachine StateMachine { get; set; }

    public BulletResource BulletResource { get; set; }

    public Callable GetDirection { get; set; }
    public Vector2 Velocity { get; set; }
    public float Speed
    {
        get => BulletResource.Speed;
        set => BulletResource.Speed = value;
    }

    public override void _Ready()
    {
        GetDirection = new Callable(this, MethodName.GetMovementDirection);

        StateMachine.Enter();

        GD.Print($"{nameof(BulletResource.Damage)} : {BulletResource.Damage}");
    }

    public void Move(float angle)
    {
        GlobalPosition += Velocity;

        Rotation = angle - Mathf.Pi/2;

        GD.Print(Rotation);
    }

    public static Vector2 GetMovementDirection()
    {
        return Vector2.Up;
    }
}
