using Godot;
using SpaceInvaders.Scenes.Autoloads;
using System;

namespace SpaceInvaders.Scenes.UI;

public partial class ScoreGui : VBoxContainer
{
    private Label scoreLabel = null!;
    private Label currentLevelLabel = null!;

    public override void _Ready()
    {
        Visible = false;

        scoreLabel = GetNode<Label>("%ScoreLabel");
        currentLevelLabel = GetNode<Label>("%CurrentLevelLabel");

        scoreLabel.Text = "Score: 0";
        currentLevelLabel.Text = "Level: 0";

        GameEvents.Instance.LevelEnded += OnLevelEnded;
        GameEvents.Instance.PacificInvaderDied += OnLevelEnded;
        GameEvents.Instance.LevelStarted += OnLevelStarted;
    }

    private void OnLevelStarted()
    {
        Visible = false;
    }

    private void OnLevelEnded()
    {
        currentLevelLabel.Text = $"Level: {GameData.Instance.CurrentLevel}";
        scoreLabel.Text = $"Score: {GameData.Instance.Score}";

        Visible = true;
    }
}
