using Godot;
using System;

namespace SpaceInvaders.Assets.Resources.Bullet;

[GlobalClass]
public partial class BulletResource : Resource
{
    [ExportGroup("Attributes")]
    [Export] public float Damage { get; set; }
    [Export] public float Speed { get; set; }
    [Export] public int Penetration { get; set; }

    [ExportGroup("Configuration")]
    [Export(PropertyHint.File, ".tscn")] public string ScenePath { get; set; } = null!;
}
