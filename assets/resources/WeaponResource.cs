using Godot;
using System;

namespace SpaceInvaders.Assets.Resources;

public partial class WeaponResource : Resource
{
    [Export(PropertyHint.File, ".tscn")] public string BulletScenePath { get; set; }
    [Export] public float Damage { get; set; }
    [Export] public float BulletSpeed { get; set; }
    [Export] public float FireRate { get; set; }
}
