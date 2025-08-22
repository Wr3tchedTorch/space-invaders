using Godot;
using SpaceInvaders.Assets.Scripts.interfaces;
using SpaceInvaders.Assets.Resources.Invader;
using SpaceInvaders.Scenes.Components;
using SpaceInvaders.Scenes.Levels;
using System;

namespace SpaceInvaders.Scenes.Agents.Invaders;

public partial class Invader : Area2D, IEnemy
{
    [ExportGroup("Dependencies")]
    [Export] public HealthComponent HealthComponent { get; private set; } = null!;
    [Export] public WeaponComponent WeaponComponent { get; private set; } = null!;

    public InvaderResource InvaderResource { get; set; } = null!;

    public override void _Ready()
    {
        HealthComponent.InitialHealth = InvaderResource.Health;
        HealthComponent.Died += OnDied;

        WeaponComponent.Shooted += OnShooted;
        WeaponComponent.WeaponResource = InvaderResource.WeaponResource;
        WeaponComponent.GetDirection = Callable.From(GetDirection);

        Callable.From(WaitAndAttack).CallDeferred();
    }

    private void OnDied()
    {
        QueueFree();
    }

    private Vector2 GetDirection()
    {
        return Vector2.Down;
    }

    private async void WaitAndAttack()
    {
        var randomOffset = GameWorld.Rng.NextDouble() * InvaderResource.AttackDelay;
        var randomSign = GameWorld.Rng.Next(0, 1) * 2 - 1;

        await ToSignal(GetTree().CreateTimer(InvaderResource.AttackDelay + randomOffset * randomSign), "timeout");
        WeaponComponent.StartShooting();
    }
    
    private void OnShooted()
    {
        WeaponComponent.StopShooting();
        WaitAndAttack();
    }
}
