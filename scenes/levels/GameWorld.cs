using Godot;
using SpaceInvaders.Scenes.Autoloads;
using System;

namespace SpaceInvaders.Scenes.Levels;

public partial class GameWorld : Node2D
{
	[Export] public int Seed { get; private set; } = (int)new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();

	[Export] private bool SkipCutscene { get; set; } = false;

	public static Random Rng { get; private set; } = null!;
	public static bool SkippingCutscene { get; private set; } = false;

	public override void _Ready()
	{
		Rng = new Random(Seed);
		GD.Print(Seed);

		SkippingCutscene = SkipCutscene;

		AddToGroup(nameof(GameWorld));
	}

	public static void EnableSlowMotion(float slowMotionTimeScale)
	{
		Engine.TimeScale = slowMotionTimeScale;
	}
}
