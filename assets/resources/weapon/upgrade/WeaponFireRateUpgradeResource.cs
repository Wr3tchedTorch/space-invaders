using Godot;
using SpaceInvaders.Assets.Scripts.Interfaces;
using System;

namespace SpaceInvaders.Assets.Resources.Weapon.Upgrade;

[GlobalClass]
public partial class WeaponFireRateUpgradeResource : Resource, IWeaponUpgrade
{
    [Export] public float FireRateUpgradeInSec { get; set; }

    public void ApplyUpgrade(IWeapon weapon)
    {
        weapon.WeaponResource.FireRateDelay -= FireRateUpgradeInSec;

        weapon.WeaponResource.FireRateDelay = Mathf.Max(weapon.WeaponResource.FireRateDelay, 0.01f);
    }
}
