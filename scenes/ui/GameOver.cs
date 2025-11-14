using Godot;
using SpaceInvaders.Scenes.Autoloads;
using System;

namespace SpaceInvaders.Scenes.UI;

public partial class GameOver : Control
{
    [Export]
    private Button RestartButton { get; set; } = null!;

    public override void _Ready()
    {
        Visible = false;
        RestartButton.Pressed += OnRestartButtonPressed;

        GameEvents.Instance.GameOver += OnGameOver;
    }

    private void OnGameOver()
    {
        Visible = true;
    }

    private void OnRestartButtonPressed()
    {
        GetTree().ReloadCurrentScene();
    }
}
