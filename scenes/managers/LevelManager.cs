using Godot;
using SpaceInvaders.Assets.Resources.Invader;
using SpaceInvaders.Scenes.Autoloads;
using SpaceInvaders.Scenes.Factories;
using SpaceInvaders.Scenes.Navigators;
using System;

namespace SpaceInvaders.Scenes.Managers;

public partial class LevelManager : Node
{
    [ExportGroup("Configuration")]
    [Export] public InvaderResource[] GameInvaders { get; set; } = [];
    [Export] public int EnemiesIncreasePerLevel = 1;
    [Export] public float EnemiesMovementDelayDecreasePerLevel = 0.2f;
    [Export] public float EnemiesFireRateDelayDecreasePerLevel = 0.2f;

    [ExportGroup("Dependencies")]
    [Export] public InvaderFactory InvaderFactory { get; set; } = null!;
    [Export] public EnemiesNavigator EnemiesNavigator { get; set; } = null!;

    private int currentNumberOfInvaders;

    public override void _Ready()
    {
        GameEvents.Instance.LevelStarted += OnLevelStarted;
        GameEvents.Instance.InvaderDied += OnInvaderDied;
    }

    public void SpawnEnemiesForTheLevel(int level)
    {
        var totalNumberOfEnemies = level * EnemiesIncreasePerLevel;

        var grid = GetGrid(totalNumberOfEnemies);
        currentNumberOfInvaders = totalNumberOfEnemies;

        GD.Print($"Total number of enemies: {currentNumberOfInvaders}");
        GD.Print($"Rows: {grid.Y}, Columns: {grid.X}\n");

        InvaderFactory.SpawnInvaders(grid.Y, grid.X);
        EnemiesNavigator.StartMoving();
    }

    private static Vector2I GetGrid(int totalEnemies)
    {
        int bestRows = 1;
        int bestCols = totalEnemies;

        for (int i = 1; i <= Mathf.Sqrt(totalEnemies); i++)
        {
            if (totalEnemies % i == 0)
            {
                int rows = i;
                int cols = totalEnemies / i;

                if (cols >= rows)
                {
                    bestRows = rows;
                    bestCols = cols;
                }
            }
        }
        return new Vector2I(bestCols, bestRows);
    }


    private void OnLevelStarted()
    {
        GD.PrintRich($"[color=green][b]Level number {GameData.Instance.CurrentLevel} is starting![/b][/color]");

        SpawnEnemiesForTheLevel(GameData.Instance.CurrentLevel);
    }

    private void OnInvaderDied()
    {
        currentNumberOfInvaders--;

        if (currentNumberOfInvaders == 1)
        {
            EnemiesNavigator.CurrentDelayBetweenMovements -= 1;
            return;
        }

        if (currentNumberOfInvaders <= 0)
        {
            GameEvents.Instance.EmitSignal(GameEvents.SignalName.LevelEnded);

            StartLevelWithDelay();
        }
    }

    private async void StartLevelWithDelay()
    {
        await ToSignal(GetTree().CreateTimer(3), "timeout");
        GameData.Instance.CurrentLevel++;
        GameEvents.Instance.EmitSignal(GameEvents.SignalName.LevelStarted);
    }
}

