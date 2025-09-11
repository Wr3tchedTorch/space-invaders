using Godot;
using SpaceInvaders.Assets.Scripts.Interfaces;
using SpaceInvaders.Scenes.Components;
using System;

namespace SpaceInvaders.Assets.Resources.Upgrades.Weapons;

[GlobalClass]
public partial class CannonUpgradeResource : Resource, IWeaponUpgrade, IDrop
{
    [Export] public int Amount { get; set; }
    [Export(PropertyHint.File, ".tscn")] public string ScenePath { get; set; } = null!;

    public void ApplyUpgrade(IWeapon weapon)
    {
        weapon.AddCannon(Amount);
    }
}
