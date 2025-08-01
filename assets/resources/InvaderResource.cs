using Godot;
using System;

namespace SpaceInvaders.Assets.Resources;

[GlobalClass]
public partial class InvaderResource : Resource
{
    [Export] public Texture Sprite { get; set; }
    [Export(PropertyHint.File, ".tscn")] public string ScenePath { get; set; }
    [Export] public float Width { get; set; }
    [Export] public float Height { get; set; }
}
