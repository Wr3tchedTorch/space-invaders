using Godot;
using SpaceInvaders.Assets.Scripts.Interfaces;
using SpaceInvaders.Assets.Scripts.Interfaces;
using System;

namespace SpaceInvaders.Assets.Resources.Upgrades.Weapons;

[GlobalClass]
public partial class WeaponFireRateUpgradeResource : Resource, IWeaponUpgrade, IDrop
{
    [Export] public float FireRateUpgradeInSec { get; set; }
    [Export(PropertyHint.File, ".tscn")] public string ScenePath { get; set; } = null!;

    public void ApplyUpgrade(IWeapon weapon)
    {
        weapon.WeaponResource.FireRateDelay -= FireRateUpgradeInSec;

        weapon.WeaponResource.FireRateDelay = Mathf.Max(weapon.WeaponResource.FireRateDelay, 0.001f);
    }
}
