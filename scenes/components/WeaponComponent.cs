using Godot;
using SpaceInvaders.Assets.Resources.Bullet;
using SpaceInvaders.Assets.Resources.Weapon;
using SpaceInvaders.Assets.Scripts.Exceptions;
using SpaceInvaders.Assets.Scripts.Interfaces;
using SpaceInvaders.Scenes.Factories;
using SpaceInvaders.Scenes.Levels;
using System.Collections.Generic;
using System;
using SpaceInvaders.Scenes.Agents.Players;

namespace SpaceInvaders.Scenes.Components;

public partial class WeaponComponent : Node, IWeapon
{
    private readonly int BulletDefaultLayer = 9;
    private readonly int BulletDefaultMask = 8;

    [Signal] public delegate void ShootedEventHandler();
    [Signal] public delegate void CannonAddedEventHandler(int count);
    [Signal] public delegate void CannonRemovedEventHandler(int count);

    [ExportGroup("Configuration")]
    [Export]
    public WeaponResource PrimaryWeaponResource { get; set; } = null!;
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

    public List<IBulletUpgrade> BulletUpgrades { get; private set; } = [];

    [ExportCategory("Dependencies")]
    [Export] private Timer FireRateTimer { get; set; } = null!;
    [Export] private Marker2D[] BulletSpawnMarkers { get; set; } = [];

    private WeaponResource CurrentWeaponResource
    {
        get => _currentWeaponResource;
        set
        {
            _currentWeaponResource = value;

            if (CurrentWeaponResource != null)
            {
                UpdateAttributes();
            }
        }
    }

    private bool canShoot = true;
    private bool isShooting = false;

    private WeaponResource _currentWeaponResource = null!;

    private float CurrentFireRateUpgrade
    {
        get => _currentFireRateUpgrade;
        set
        {
            _currentFireRateUpgrade = value;

            UpdateAttributes();
        }
    }

    private float _currentFireRateUpgrade = 0;

    public override void _Ready()
    {
        FireRateTimer.Autostart = false;
        FireRateTimer.OneShot = true;
        FireRateTimer.Timeout += () => canShoot = true;

        CurrentWeaponResource = PrimaryWeaponResource;

        if (PrimaryWeaponResource != null)
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
    }

    public override void _Process(double delta)
    {
        if (GetOwner<Node2D>() is Player)
        {
            GD.Print($"Fire Rate: {CurrentWeaponResource.FireRateDelay} | {CurrentWeaponResource.MaxFireRateDelay} | {CurrentFireRateUpgrade} %");
        }

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

    public void AddUpgradeWaitAndRemove(IBulletTemporaryUpgrade temporaryUpgrade)
    {
        BulletUpgrades.Add(temporaryUpgrade);
        WaitAndRemove(temporaryUpgrade);        
    }

    public async void SwitchToTemporaryWeapon(WeaponResource weaponResource, double timeBeforeSwitchingBack)
    {        
        CurrentWeaponResource = weaponResource;

        await ToSignal(GetTree().CreateTimer(timeBeforeSwitchingBack), "timeout");
        CurrentWeaponResource = PrimaryWeaponResource;
    }

    public void ChangeBulletSpawnMarkers(Marker2D[] toMarkers)
    {
        BulletSpawnMarkers = toMarkers;
    }

    public void AddCannon(int count)
    {
        EmitSignal(SignalName.CannonAdded, count);
    }

    public void RemoveCannon(int count)
    {
        if (BulletSpawnMarkers.Length - count <= 0)
        {
            GD.PrintErr($"{nameof(WeaponComponent)}: Can't remove more markers.");
        }
        EmitSignal(SignalName.CannonRemoved, count);
    }

    private void UpdateAttributes()
    {
        if (CurrentWeaponResource == null)
        {
            throw new ResourceNullException(nameof(CurrentWeaponResource));
        }
        if (!IsInstanceValid(FireRateTimer))
        {
            return;
        }
        var percentage = CurrentWeaponResource.MaxFireRateDelay * _currentFireRateUpgrade / 100;
        CurrentWeaponResource.FireRateDelay = CurrentWeaponResource.MaxFireRateDelay - percentage;
        FireRateTimer.WaitTime = CurrentWeaponResource.FireRateDelay;
    }

    private void Shoot()
    {
        if (BulletSpawnMarkers.Length == 0)
        {
            return;
        }

        canShoot = false;
        FireRateTimer.Start();

        foreach (var marker in BulletSpawnMarkers)
        {
            var bulletPosition = marker.GlobalPosition;
            var bullet = BulletFactory.SpawnBullet(bulletPosition, (BulletResource)CurrentWeaponResource.BulletResource.Duplicate());
            bullet.GetDirection = GetDirection;
            bullet.Rotation = marker.Rotation;

            bullet.SetPhysicsLayer(BulletPhysicsLayer);
            bullet.SetPhysicsMask(BulletPhysicsMask);

            bullet.SetCollisionLayerValue(BulletDefaultLayer, true);
            bullet.SetCollisionMaskValue(BulletDefaultMask, true);

            foreach (var upgrade in BulletUpgrades)
            {
                upgrade.ApplyUpgrade(bullet);
            }

            var gameWorld = GetTree().GetFirstNodeInGroup(nameof(GameWorld));
            gameWorld.AddChild((Node2D)bullet);            
        }
        EmitSignal(SignalName.Shooted);
    }    

    private async void WaitAndRemove(IBulletTemporaryUpgrade temporaryUpgrade)
    {
        await ToSignal(GetTree().CreateTimer(temporaryUpgrade.Duration), "timeout");

        BulletUpgrades.Remove(temporaryUpgrade);
    }

    public void IncrementFireRatePercentage(float percentage)
    {        
        CurrentFireRateUpgrade += percentage;
    }
}
