using System;
using Godot;
using SpaceInvaders.Assets.Resources.Bullet;
using SpaceInvaders.Assets.Scripts.Interfaces;

namespace SpaceInvaders.Scenes.Factories;

public class BulletFactory
{
    public static IBullet SpawnBullet(Vector2 position, BulletResource bulletResource)
    {
        var scene = GD.Load<PackedScene>(bulletResource.ScenePath);
        var bullet = scene.Instantiate<IBullet>();
        bullet.GlobalPosition = position;
        bullet.BulletResource = bulletResource;
        return bullet;
    }
}
