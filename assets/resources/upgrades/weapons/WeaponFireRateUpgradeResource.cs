using Godot;
using SpaceInvaders.Assets.Scripts.Interfaces;
using System;

namespace SpaceInvaders.Assets.Resources.Upgrades.Weapons;

[GlobalClass]
public partial class WeaponFireRateUpgradeResource : Resource, IWeaponUpgrade, IDrop
{
    [Export] public float FireRateUpgradePercentage { get; set; }
    [Export(PropertyHint.File, ".tscn")] public string ScenePath { get; set; } = null!;

    public void ApplyUpgrade(IWeapon weapon)
    {
        weapon.IncrementFireRatePercentage(FireRateUpgradePercentage);
    }
}
