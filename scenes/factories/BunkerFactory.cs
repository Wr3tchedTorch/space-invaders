using Godot;
using SpaceInvaders.Assets.Resources;
using SpaceInvaders.Scenes.Autoloads;
using SpaceInvaders.Scenes.Levels;
using System;
using System.Collections.Generic;

namespace SpaceInvaders.Scenes.Factories;

public partial class BunkerFactory : Node
{
	private readonly StringName BunkersParentGroup = "BunkersParent";
	private readonly StringName BunkersGroup = "Bunkers";

	[Export]
	private float BunkerWidth { get; set; } = 32;
	[Export]
	private float Gap { get; set; } = 3;
	[Export]
	private PackedScene BunkerScene { get; set; } = null!;
	[Export]
	private Marker2D CenterMarker { get; set; } = null!;
	[Export]
	private Godot.Collections.Array<BunkerPresetResource> BunkerPresetResources { get; set; } =
	[
		new BunkerPresetResource { BunkerCount = 1, Gap = 0 },
		new BunkerPresetResource { BunkerCount = 2, Gap = 15 },
		new BunkerPresetResource { BunkerCount = 3, Gap = 15 },
		new BunkerPresetResource { BunkerCount = 2, Gap = 30 },
		new BunkerPresetResource { BunkerCount = 3, Gap = 30 },
		new BunkerPresetResource { BunkerCount = 5, Gap = 15 },
		new BunkerPresetResource { BunkerCount = 6, Gap = 10 },
		new BunkerPresetResource { BunkerCount = 10, Gap = 0 },
	];

	public override void _Ready()
	{
		SpawnBunkers(BunkerPresetResources[4]);

		GameEvents.Instance.LevelStarted += OnLevelStarted;
	}

    private void OnLevelStarted()
	{
		var bunkers = GetTree().GetNodesInGroup(BunkersGroup);
		foreach (var bunker in bunkers)
		{
			bunker.QueueFree();
		}
		SpawnBunkers(BunkerPresetResources[GameData.Instance.CurrentLevel % BunkerPresetResources.Count]);
    }

    private void SpawnBunkers(BunkerPresetResource preset)
    {
		Gap = preset.Gap;
		SpawnBunkers(preset.BunkerCount);
    }

	private void SpawnBunkers(int count)
	{
		float totalGap = (count - 1) * Gap;
		float totalWidth = (count * BunkerWidth) + totalGap;
		float startingPos = CenterMarker.GlobalPosition.X - totalWidth / 2 + BunkerWidth/2;
		
		for (int i = 0; i < count; i++)
		{
			var bunkerPos = startingPos + i * (BunkerWidth + Gap);
			var bunker = BunkerScene.Instantiate<Node2D>();
			bunker.Position = new Vector2(bunkerPos, CenterMarker.GlobalPosition.Y);
			bunker.AddToGroup(BunkersGroup);
			GetTree().GetFirstNodeInGroup(BunkersParentGroup).AddChild(bunker);
		}
	}
}