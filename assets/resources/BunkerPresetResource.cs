using Godot;
using System;

namespace SpaceInvaders.Assets.Resources;

[GlobalClass]
public partial class BunkerPresetResource : Resource
{
	[Export]
	public float Gap { get; set; } = 3;
	[Export]
	public int BunkerCount { get; set; } = 5;
}
