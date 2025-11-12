using Godot;
using SpaceInvaders.Scenes.Autoloads;
using System;

namespace SpaceInvaders.Scenes.Levels;

public partial class GameWorld : Node2D
{
	[Export] public int Seed { get; private set; } = (int)new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();

	public static Random Rng { get; private set; } = null!;

	public override void _Ready()
	{
		Rng = new Random(Seed);
		GD.Print(Seed);

		AddToGroup(nameof(GameWorld));
	}
}
