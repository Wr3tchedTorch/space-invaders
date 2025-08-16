using Godot;
using SpaceInvaders.Assets.Resources.Bullet;
using SpaceInvaders.Assets.Resources.Weapon;
using SpaceInvaders.Assets.Scripts.Exceptions;
using SpaceInvaders.Assets.Scripts.Interfaces;
using SpaceInvaders.Scenes.Autoloads;
using SpaceInvaders.Scenes.Factories;
using SpaceInvaders.Scenes.Levels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace SpaceInvaders.Scenes.Agents.Weapons;

public partial class WeaponComponent : Node
{    
    [Export] public WeaponResource WeaponResource { get; private set; }

    public Callable GetDirection { get; set; }

    [ExportCategory("Dependencies")]
    [Export] private Timer FireRateTimer { get; set; }
    [Export] private Marker2D BulletSpawnMarker { get; set; }

    private List<IBulletUpgrade> BulletUpgrades { get; set; } = [];

    private bool canShoot = true;
    private bool isShooting = true;

    public override void _Ready()
    {
        FireRateTimer.Autostart = false;
        FireRateTimer.OneShot = true;
        FireRateTimer.WaitTime = WeaponResource.FireRateDelay;
        FireRateTimer.Timeout += () => canShoot = true;

        GameEvents.Instance.UpgradePickedUp += OnUpgradePickedUp;
    }

    private void OnUpgradePickedUp(Resource upgrade)
    {
        if (upgrade is not IBulletUpgrade)
        {
            throw new InvalidBulletUpgradeException(upgrade.ResourcePath);
        }
        BulletUpgrades.Add((IBulletUpgrade) upgrade);
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

    private void Shoot()
    {
        canShoot = false;
        FireRateTimer.Start();

        var bulletPosition = BulletSpawnMarker.GlobalPosition;
        var bullet = BulletFactory.SpawnBullet(bulletPosition, (BulletResource) WeaponResource.BulletResource.Duplicate());
        bullet.GetDirection = GetDirection;

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
    }

    private async void WaitAndRemove(IBulletTemporaryUpgrade temporaryUpgrade)
    {
        await ToSignal(GetTree().CreateTimer(temporaryUpgrade.Duration), "timeout");

        BulletUpgrades.Remove(temporaryUpgrade);
    }
}
