using Godot;
using SpaceInvaders.Assets.Scripts.Interfaces;
using System;

namespace SpaceInvaders.Assets.Resources.Bullet.Upgrade;

[GlobalClass]
public partial class BulletDamageUpgradeResource : Resource, IBulletUpgrade
{
    [Export] public float DamageUpgradeAmount { get; set; }

    public void ApplyUpgrade(IBullet bullet)
    {
        bullet.BulletResource.Damage += DamageUpgradeAmount;
    }    
}
