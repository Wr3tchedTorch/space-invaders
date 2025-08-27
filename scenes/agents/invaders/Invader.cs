using Godot;
using SpaceInvaders.Assets.Scripts.interfaces;
using SpaceInvaders.Assets.Resources.Invader;
using SpaceInvaders.Scenes.Components;
using SpaceInvaders.Scenes.Levels;
using System;
using SpaceInvaders.Assets.Scripts.Interfaces;
using SpaceInvaders.Assets.Scripts.Exceptions;

namespace SpaceInvaders.Scenes.Agents.Invaders;

public partial class Invader : Area2D, IEnemy
{
    [ExportGroup("Dependencies")]
    [Export] public HealthComponent HealthComponent { get; private set; } = null!;
    [Export] public WeaponComponent WeaponComponent { get; private set; } = null!;

    [ExportGroup("Configuration")]
    [Export] public Resource[] BulletUpgradeResources { get; set; } = [];

    public InvaderResource InvaderResource { get; set; } = null!;

    public override void _Ready()
    {
        HealthComponent.InitialHealth = InvaderResource.Health;
        HealthComponent.Died += OnDied;

        if (InvaderResource.WeaponResource == null)
        {
            throw new ArgumentNullException($"{nameof(Invader)}: {nameof(InvaderResource.WeaponResource)} is null.");
        }

        InvaderResource = (InvaderResource)InvaderResource.Duplicate(true);
        WeaponComponent.WeaponResource = InvaderResource.WeaponResource;
        WeaponComponent.GetDirection = Callable.From(GetDirection);

        foreach (var upgrade in BulletUpgradeResources)
        {
            AddBulletUpgrade(upgrade);
        }

        Callable.From(StartShooting).CallDeferred();
    }

    private void OnDied()
    {
        QueueFree();
    }

    private Vector2 GetDirection()
    {
        return Vector2.Down;
    }

    private async void StartShooting()
    {
        InvaderResource.WeaponResource.FireRateDelay = GetRandomFireRateDelay();

        await ToSignal(GetTree().CreateTimer(InvaderResource.WeaponResource.FireRateDelay), "timeout");
        WeaponComponent.StartShooting();
    }

    private float GetRandomFireRateDelay()
    {
        var halfFireRate = InvaderResource.WeaponResource.FireRateDelay * 0.95f;

        var randomOffset = (float)(GameWorld.Rng.NextDouble() * halfFireRate);
        var randomSign = GameWorld.Rng.Next(0, 2) * 2 - 1;

        return InvaderResource.WeaponResource.FireRateDelay + randomOffset * randomSign;
    }

    private void AddBulletUpgrade(Resource upgrade)
    {
        if (upgrade is not IBulletUpgrade)
        {
            throw new InvalidUpgradeTypeException(upgrade.ResourcePath, nameof(IBulletUpgrade));
        }
        WeaponComponent.BulletUpgrades.Add((IBulletUpgrade) upgrade);
    }    
}
