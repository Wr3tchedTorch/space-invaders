using Godot;
using SpaceInvaders.Assets.Resources.Weapon;
using SpaceInvaders.Scenes.Factories;
using SpaceInvaders.Scenes.Levels;
using System;

namespace SpaceInvaders.Scenes.Agents.Weapons;

public partial class WeaponComponent : Node
{
    [Export] public WeaponResource WeaponResource { get; private set; }

    public Callable GetDirection { get; set; }

    [ExportCategory("Dependencies")]
    [Export] private Timer FireRateTimer { get; set; }
    [Export] private Marker2D BulletSpawnMarker { get; set; }

    private bool canShoot = true;
    private bool isShooting = true;

    public override void _Ready()
    {
        FireRateTimer.Autostart = false;
        FireRateTimer.OneShot = true;
        FireRateTimer.WaitTime = WeaponResource.FireRateDelay;
        FireRateTimer.Timeout += () => canShoot = true;
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
        var bullet = BulletFactory.SpawnBullet(bulletPosition, WeaponResource.BulletResource);
        bullet.GetDirection = GetDirection;

        var gameWorld = GetTree().GetFirstNodeInGroup(nameof(GameWorld));
        gameWorld.AddChild((Node2D)bullet);
    }
}
