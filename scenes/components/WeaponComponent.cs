using Godot;
using SpaceInvaders.Assets.Resources.Bullet;
using SpaceInvaders.Assets.Resources.Weapon;
using SpaceInvaders.Assets.Scripts.Exceptions;
using SpaceInvaders.Assets.Scripts.Interfaces;
using SpaceInvaders.Scenes.Autoloads;
using SpaceInvaders.Scenes.Factories;
using SpaceInvaders.Scenes.Levels;
using System.Collections.Generic;
using System;

namespace SpaceInvaders.Scenes.Components;

public partial class WeaponComponent : Node, IWeapon
{
    [Signal] public delegate void ShootedEventHandler();

    [ExportGroup("Configuration")]
    [Export] public WeaponResource WeaponResource
    {
        get => _weaponResource;
        set
        {
            _weaponResource = value;

            if (WeaponResource != null)
            {
                UpdateAttributes();
            }
        }
    }
    [Export]
    private uint BulletPhysicsLayer
    {
        get;
        set;
    }
    [Export] 
    private uint BulletPhysicsMask
    {
        get;
        set;
    }

    public Callable GetDirection { get; set; }

    [ExportCategory("Dependencies")]
    [Export] private Timer FireRateTimer { get; set; } = null!;
    [Export] private Marker2D BulletSpawnMarker { get; set; } = null!;

    private List<IBulletUpgrade> BulletUpgrades { get; set; } = [];

    private bool canShoot = true;
    private bool isShooting = false;

    private WeaponResource _weaponResource = null!;

    public override void _Ready()
    {
        FireRateTimer.Autostart = false;
        FireRateTimer.OneShot = true;
        FireRateTimer.Timeout += () => canShoot = true;

        if (WeaponResource != null)
        {
            UpdateAttributes();
        }

        if (BulletPhysicsLayer <= 0)
        {
            throw new InvalidPhysicsLayerException((int)BulletPhysicsLayer);
        }
        if (BulletPhysicsMask <= 0)
        {
            throw new InvalidPhysicsMaskException((int)BulletPhysicsMask);
        }        

        GameEvents.Instance.BulletUpgradePickedUp += OnBulletUpgradePickedUp;
        GameEvents.Instance.WeaponUpgradePickedUp += OnWeaponUpgradePickedUp;
    }
    
    public override void _Process(double delta)
    {
        if (!isShooting || !canShoot)
        {
            return;
        }
        Shoot();
    }

    public void StartShooting()
    {
        isShooting = true;
    }

    public void StopShooting()
    {
        isShooting = false;
    }

    private void UpdateAttributes()
    {
        if (WeaponResource == null)
        {
            throw new ResourceNullException(nameof(WeaponResource));
        }
        if (FireRateTimer == null)
        {
            return;
        }
        FireRateTimer.WaitTime = WeaponResource.FireRateDelay;
        WeaponResource.FireRateDelayChanged += () => { FireRateTimer.WaitTime = WeaponResource.FireRateDelay; };
    }

    private void OnWeaponUpgradePickedUp(Resource upgrade)
    {
        if (upgrade is not IWeaponUpgrade)
        {
            throw new InvalidUpgradeTypeException(upgrade.ResourcePath, nameof(IWeaponUpgrade));
        }
        ((IWeaponUpgrade)upgrade).ApplyUpgrade(this);
        GD.Print($"upgrade picked up: {upgrade.ResourcePath}");
    }

    private void OnBulletUpgradePickedUp(Resource upgrade)
    {
        if (upgrade is not IBulletUpgrade)
        {
            throw new InvalidUpgradeTypeException(upgrade.ResourcePath, nameof(IBulletUpgrade));
        }
        BulletUpgrades.Add((IBulletUpgrade) upgrade);
    }

    private void Shoot()
    {
        canShoot = false;
        FireRateTimer.Start();

        var bulletPosition = BulletSpawnMarker.GlobalPosition;
        var bullet = BulletFactory.SpawnBullet(bulletPosition, (BulletResource)WeaponResource.BulletResource.Duplicate());
        bullet.GetDirection = GetDirection;

        bullet.SetPhysicsLayer(BulletPhysicsLayer);
        bullet.SetPhysicsMask(BulletPhysicsMask);

        foreach (var upgrade in BulletUpgrades)
        {
            if (upgrade is IBulletTemporaryUpgrade temporaryUpgrade)
            {
                WaitAndRemove(temporaryUpgrade);
            }
            upgrade.ApplyUpgrade(bullet);
        }

        var gameWorld = GetTree().GetFirstNodeInGroup(nameof(GameWorld));
        gameWorld.AddChild((Node2D)bullet);

        EmitSignal(SignalName.Shooted);
    }

    private async void WaitAndRemove(IBulletTemporaryUpgrade temporaryUpgrade)
    {
        await ToSignal(GetTree().CreateTimer(temporaryUpgrade.Duration), "timeout");

        BulletUpgrades.Remove(temporaryUpgrade);
    }
}
