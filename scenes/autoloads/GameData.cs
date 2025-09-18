using Godot;
using System;

namespace SpaceInvaders.Scenes.Autoloads;

public partial class GameData : Node
{
    public static GameData Instance { get; private set; } = null!;

    public int CurrentLevel { get; set; } = 0;
    public int HighScore { get; set; } = 0;
    public int Score { get; set; } = 0;

    public override void _Ready()
    {
        Instance = this;
    }
}
