using Godot;
using SpaceInvaders.Assets.Resources.Bullet;
using SpaceInvaders.Assets.Resources.Weapon;
using SpaceInvaders.Assets.Scripts.Exceptions;
using SpaceInvaders.Assets.Scripts.Interfaces;
using SpaceInvaders.Scenes.Factories;
using SpaceInvaders.Scenes.Levels;
using System.Collections.Generic;
using System;

namespace SpaceInvaders.Scenes.Components;

public partial class WeaponComponent : Node, IWeapon
{
    private readonly string PrimaryWeaponResourceName = "primary_weapon";
    private readonly string TemporaryWeaponResourceName = "temporary_weapon";

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
    [Export] public Label? AmmunitionLabel { get; set; }
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

    private int maxAmmo = 0;

    public override void _Ready()
    {
        Callable.From(Init).CallDeferred();
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

    public void AddUpgradeWaitAndRemove(IBulletTemporaryUpgrade temporaryUpgrade)
    {
        BulletUpgrades.Add(temporaryUpgrade);
        WaitAndRemove(temporaryUpgrade);
    }

    public void SwitchToTemporaryWeapon(WeaponResource weaponResource)
    {
        if (AmmunitionLabel != null)
        {
            maxAmmo = weaponResource.Ammunition;
            UpdateAmmoLabel(maxAmmo, maxAmmo);
        }

        CurrentWeaponResource = (WeaponResource)weaponResource.Duplicate();
        CurrentWeaponResource.ResourceName = TemporaryWeaponResourceName;

        FireRateTimer.Stop();
        canShoot = true;
    }

    public void SwitchToPrimaryWeapon()
    {
        CurrentWeaponResource = (WeaponResource)PrimaryWeaponResource.Duplicate();
        CurrentWeaponResource.ResourceName = PrimaryWeaponResourceName;

        UpdateAmmoLabel("∞/∞");

        FireRateTimer.Stop();
        canShoot = true;
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

    private void Init()
    {
        FireRateTimer.Autostart = false;
        FireRateTimer.OneShot = true;
        FireRateTimer.Timeout += () => canShoot = true;

        PrimaryWeaponResource = (WeaponResource)PrimaryWeaponResource.Duplicate();
        PrimaryWeaponResource.ResourceName = PrimaryWeaponResourceName;
        Callable.From(SwitchToPrimaryWeapon).CallDeferred();

        if (BulletPhysicsLayer <= 0)
        {
            throw new InvalidPhysicsLayerException((int)BulletPhysicsLayer);
        }
        if (BulletPhysicsMask <= 0)
        {
            throw new InvalidPhysicsMaskException((int)BulletPhysicsMask);
        }
    }

    private void UpdateAmmoLabel(int ammo, int maxAmmo)
    {
        UpdateAmmoLabel($"{ammo}/{maxAmmo}");
    }

    private void UpdateAmmoLabel(string label)
    {
        if (AmmunitionLabel == null)
        {
            return;
        }
        AmmunitionLabel.Text = label;
        AmmunitionLabel.Visible = true;
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
        CurrentWeaponResource.FireRateDelay = Mathf.Max(CurrentWeaponResource.MaxFireRateDelay - _currentFireRateUpgrade, 0.1f);
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

        if (AmmunitionLabel != null)
        {
            UpdateAmmunition();
        }
    }

    private void UpdateAmmunition()
    {
        if (AmmunitionLabel == null)
        {
            return;
        }

        if (CurrentWeaponResource.ResourceName == PrimaryWeaponResource.ResourceName)
        {
            return;
        }

        CurrentWeaponResource.Ammunition -= BulletSpawnMarkers.Length;
        UpdateAmmoLabel(CurrentWeaponResource.Ammunition, maxAmmo);

        if (CurrentWeaponResource.Ammunition <= 0)
        {
            SwitchToPrimaryWeapon();
        }
    }

    private async void WaitAndRemove(IBulletTemporaryUpgrade temporaryUpgrade)
    {
        await ToSignal(GetTree().CreateTimer(temporaryUpgrade.Duration), "timeout");

        BulletUpgrades.Remove(temporaryUpgrade);
    }

    public void IncrementFireRate(float amount)
    {
        CurrentFireRateUpgrade += amount;
    }
}
