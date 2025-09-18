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
        var totalNumberOfInvaders = level * EnemiesIncreasePerLevel;
        var enemiesRowCount = (int)Mathf.Round(totalNumberOfInvaders * 0.3125f);
        var enemiesColCount = (int)Mathf.Round(totalNumberOfInvaders * 0.6875f);
        currentNumberOfInvaders = totalNumberOfInvaders;

        GD.Print($"Total number of enemies: {totalNumberOfInvaders}");
        GD.Print($"Rows: {enemiesRowCount}, Column: {enemiesColCount}\n");

        InvaderFactory.SpawnInvaders(enemiesRowCount, enemiesColCount);
        EnemiesNavigator.StartMoving();
    }

    private void OnLevelStarted()
    {    
        GD.PrintRich($"[color=green][b]Level number {GameData.Instance.CurrentLevel} is starting![/b][/color]");

        SpawnEnemiesForTheLevel(GameData.Instance.CurrentLevel);
    }

    private void OnInvaderDied()
    {
        currentNumberOfInvaders--;

        if (currentNumberOfInvaders <= 0)
        {
            GameEvents.Instance.EmitSignal(GameEvents.SignalName.LevelEnded);
        }
    }    
}

