using Godot;
using System;

namespace SpaceInvaders.Assets.Resources;

[GlobalClass]
public partial class InvaderResource : Resource
{
    [Export] public Texture Sprite { get; set; }
    [Export] public float Width { get; set; }
}
