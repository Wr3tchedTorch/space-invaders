using Godot;
using SpaceInvaders.Scenes.Components;
using System;

namespace SpaceInvaders.Scenes.Agents.Bullets;

public partial class Missile : Laser
{
    [Signal] public delegate void EnemyHitEventHandler();

    [ExportGroup("Dependencies")]
    [Export] public ExplosionComponent ExplosionComponent { get; set; } = null!;
    [Export] public ExplosionArea ExplosionArea { get; set; } = null!;

    public override void _Ready()
    {
        base._Ready();

        ExplosionComponent.ExplosionFinished += OnExplosionFinished;

        ExplosionArea.Damage = BulletResource.Damage;
    }

    protected override void OnBodyEntered(Node2D body)
    {
        StateMachine.Exit();

        EmitSignal(SignalName.EnemyHit);
    }

    protected override void OnAreaEntered(Area2D area)
    {
        StateMachine.Exit();

        EmitSignal(SignalName.EnemyHit);
    }

    private void OnExplosionFinished()
    {
        QueueFree();
    }
}
