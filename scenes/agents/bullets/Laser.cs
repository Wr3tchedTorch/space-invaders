using Godot;
using SpaceInvaders.Assets.Resources.Bullet;
using SpaceInvaders.Assets.Scripts.Interfaces;
using SpaceInvaders.Scenes.Components;

namespace SpaceInvaders.Scenes.Agents.Bullets;

public partial class Laser : Area2D, IBullet, IMover, IAttacker
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

    public float Damage { get => BulletResource.Damage; }

    public override void _Ready()
    {
        GetDirection = new Callable(this, MethodName.GetMovementDirection);

        StateMachine.Enter();
    }

    public void Move(float angle)
    {
        GlobalPosition += Velocity;

        Rotation = angle - Mathf.Pi/2;
    }

    public static Vector2 GetMovementDirection()
    {
        return Vector2.Up;
    }
}
