using Godot;
using SpaceInvaders.Assets.Scripts.Interfaces;
using SpaceInvaders.Assets.Resources.Invader;
using SpaceInvaders.Scenes.Components;
using SpaceInvaders.Scenes.Levels;
using System;
using SpaceInvaders.Assets.Scripts.Exceptions;
using SpaceInvaders.Scenes.Navigators;
using SpaceInvaders.Scenes.Agents.Upgrades;

namespace SpaceInvaders.Scenes.Agents.Invaders;

public partial class Invader : Area2D, IEnemy
{
    [ExportGroup("Dependencies")]
    [Export] public HealthComponent HealthComponent { get; private set; } = null!;
    [Export] public WeaponComponent WeaponComponent { get; private set; } = null!;
    [Export] public AnimatedSprite2D AnimatedSprite2D { get; set; } = null!;

    [ExportGroup("Configuration")]
    [Export(PropertyHint.Range, "0,100")] public float UpgradeDropChance { get; set; } = 30f;
    [Export] public Resource[] BulletUpgradeResources { get; set; } = [];
    [Export] public Resource[] UpgradeDrops { get; set; } = [];

    public InvaderResource InvaderResource { get; set; } = null!;

    private bool isDead = false;

    public override void _Ready()
    {
        Callable.From(Init).CallDeferred();
    }

    private void Init()
    {
        HealthComponent.InitialHealth = InvaderResource.Health;
        HealthComponent.Died += OnDied;

        if (InvaderResource.WeaponResource == null)
        {
            throw new ArgumentNullException($"{nameof(Invader)}: {nameof(InvaderResource.WeaponResource)} is null.");
        }

        InvaderResource = (InvaderResource)InvaderResource.Duplicate(true);
        WeaponComponent.PrimaryWeaponResource = InvaderResource.WeaponResource;
        WeaponComponent.GetDirection = Callable.From(GetDirection);

        foreach (var upgrade in BulletUpgradeResources)
        {
            AddBulletUpgrade(upgrade);
        }

        Callable.From(StartShooting).CallDeferred();

        var parent = GetParent<EnemiesNavigator>();
        parent.Moved += OnFlockMoved;
    }

    private void OnDied()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;
        var chance = GameWorld.Rng.NextDouble() * 100;
        if (chance <= UpgradeDropChance)
        {
            SpawnDrop();
        }
        QueueFree();
    }

    private void SpawnDrop()
    {
        var rng = GameWorld.Rng.NextDouble();
        var upgradeIndex = (int)Mathf.Round((UpgradeDrops.Length - 1) * rng);
        var dropResource = UpgradeDrops[upgradeIndex];

        if (dropResource is not IDrop)
        {
            throw new InvalidDropException(dropResource.ResourceName);
        }
        var drop = (IDrop)dropResource;

        var scene = GD.Load<PackedScene>(drop.ScenePath);
        var instance = scene.Instantiate<UpgradePickUp>();
        instance.GlobalPosition = GlobalPosition;

        instance.UpgradeResource = dropResource;

        var gameWorld = GetTree().GetFirstNodeInGroup(nameof(GameWorld));
        gameWorld?.CallDeferred("add_child", instance);
    }

    private Vector2 GetDirection()
    {
        return Vector2.Down;
    }

    private async void StartShooting()
    {
        InvaderResource.WeaponResource.MaxFireRateDelay = GetRandomFireRateDelay();

        await ToSignal(GetTree().CreateTimer(InvaderResource.WeaponResource.MaxFireRateDelay), "timeout");
        WeaponComponent.StartShooting();
    }

    private float GetRandomFireRateDelay()
    {
        var halfFireRate = InvaderResource.WeaponResource.MaxFireRateDelay * 0.95f;

        var randomOffset = (float)(GameWorld.Rng.NextDouble() * halfFireRate);
        var randomSign = GameWorld.Rng.Next(0, 2) * 2 - 1;

        return InvaderResource.WeaponResource.MaxFireRateDelay + randomOffset * randomSign;
    }

    private void AddBulletUpgrade(Resource upgrade)
    {
        if (upgrade is not IBulletUpgrade)
        {
            throw new InvalidUpgradeTypeException(upgrade.ResourcePath, nameof(IBulletUpgrade));
        }
        WeaponComponent.BulletUpgrades.Add((IBulletUpgrade)upgrade);
    }

    private void OnFlockMoved()
    {
        if (!IsInstanceValid(AnimatedSprite2D))
        {
            return;
        }

        if (AnimatedSprite2D.Frame == 1)
        {
            AnimatedSprite2D.Frame = 0;
            return;
        }
        AnimatedSprite2D.Frame = 1;
    }
}
