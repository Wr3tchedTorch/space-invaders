using System;
using Godot;
using SpaceInvaders.Assets.Scripts.Interfaces;
using SpaceInvaders.Assets.Resources.Weapon;

namespace SpaceInvaders.Assets.Resources.Upgrades.Weapons;

[GlobalClass]
public partial class WeaponSwitchUpgradeResource : Resource, IWeaponUpgrade, IDrop
{
    [Export] public WeaponResource WeaponResource { get; set; } = null!;
    [Export] public double DelayBeforeSwitchingBack { get; set; }
    [Export(PropertyHint.File, ".tscn")] public string ScenePath { get; set; } = null!;

    public void ApplyUpgrade(IWeapon weapon)
    {
        weapon.SwitchToTemporaryWeapon((WeaponResource) WeaponResource.Duplicate(), DelayBeforeSwitchingBack);
    }
}
