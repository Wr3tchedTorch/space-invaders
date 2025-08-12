using Godot;
using System;

namespace SpaceInvaders.Assets.Resources;

[GlobalClass]
public partial class BulletResource : Resource
{
    [Export] public float Damage { get; set; }
    [Export] public float Speed { get; set; }
    [Export(PropertyHint.File, ".tscn")] public string ScenePath { get; set; }
}
