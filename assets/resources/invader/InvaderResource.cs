using Godot;
using System;

namespace SpaceInvaders.Assets.Resources.Invader;

[GlobalClass]
public partial class InvaderResource : Resource
{
    [ExportGroup("Attributes")]
    [Export] public float Health;

    [ExportGroup("Configuration")]
    [Export(PropertyHint.File, ".tscn")] public string ScenePath { get; set; }
    [Export] public Texture Sprite { get; set; }
    [Export] public float Width { get; set; }
    [Export] public float Height { get; set; }
}
