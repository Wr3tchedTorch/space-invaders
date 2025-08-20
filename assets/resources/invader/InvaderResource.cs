using Godot;
using SpaceInvaders.Assets.Resources.Weapon;
using System;

namespace SpaceInvaders.Assets.Resources.Invader;

[GlobalClass]
public partial class InvaderResource : Resource
{
    [ExportGroup("Attributes")]
    [Export] public float Health;
    [Export] public int ScoreValue;

    [ExportGroup("Configuration")]
    [Export(PropertyHint.File, ".tscn")] public string ScenePath { get; set; } = null!;
    [Export] public WeaponResource WeaponResource { get; set; } = null!;
    [Export] public float Width { get; set; }
    [Export] public float Height { get; set; }
}
