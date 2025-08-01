using Godot;
using SpaceInvaders.Assets.Resources;
using System;

namespace SpaceInvaders.Scenes.Factories;

public partial class InvaderFactory : Node
{
    [Export] private int Columns { get; set; } = 11;
    [Export] private int Rows { get; set; } = 5;
    [Export] private float HGap { get; set; }
    [Export] private float VGap { get; set; }

    [Export] private Godot.Collections.Array<InvaderResource> invaders = [];

    
}
