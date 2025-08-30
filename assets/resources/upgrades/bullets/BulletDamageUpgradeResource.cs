using Godot;
using SpaceInvaders.assets.scripts.interfaces;
using SpaceInvaders.Assets.Scripts.Interfaces;
using System;

namespace SpaceInvaders.Assets.Resources.Upgrades.Bullets;

[GlobalClass]
public partial class BulletDamageUpgradeResource : Resource, IBulletUpgrade, IDrop
{
    [Export] public float DamageUpgradeAmount { get; set; }
    [Export] public string ScenePath { get; set; } = null!;

    public void ApplyUpgrade(IBullet bullet)
    {
        bullet.BulletResource.Damage += DamageUpgradeAmount;
    }    
}
