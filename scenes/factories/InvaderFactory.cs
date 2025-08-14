using Godot;
using SpaceInvaders.Assets.Resources.Invader;
using SpaceInvaders.Scenes.Navigators;
using System.Linq;
using System;

namespace SpaceInvaders.Scenes.Factories;

public partial class InvaderFactory : Node
{
    [Export] private int Columns { get; set; } = 11;
    [Export] private int Rows { get; set; } = 5;
    [Export] private float HGap { get; set; } = 10;
    [Export] private float VGap { get; set; } = 10;
    [Export] private EnemiesNavigator EnemiesNavigator { get; set; }

    [Export] private Godot.Collections.Array<InvaderResource> invaders = [];

    private Vector2 CurrentPosition { get; set; }

    public override void _Ready()
    {
        SpawnInvaders();
    }

    private void SpawnInvaders()
    {
        var cellWidth = invaders.Max(i => i.Width);
        var cellHeight = invaders.Max(i => i.Height);
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Columns; col++)
            {
                var initialPosition = EnemiesNavigator.GlobalPosition;
                var enemyResource = GetEnemyResource(row);
                var enemyPosition = GetGridPosition(initialPosition, row, col, cellWidth, cellHeight);

                var enemy = SpawnEnemy(enemyResource.ScenePath, enemyPosition);
            }
        }
        PositionEnemiesNavigator(cellWidth);
    }

    private void PositionEnemiesNavigator(float cellWidth)
    {
        var hPos = (Columns-1) * cellWidth;
        hPos += (Columns-1) * HGap;
        hPos = hPos / 2 + EnemiesNavigator.GlobalPosition.X;

        EnemiesNavigator.GlobalPosition = new Vector2(EnemiesNavigator.GlobalPosition.X - hPos, EnemiesNavigator.GlobalPosition.Y);
    }

    private Vector2 GetGridPosition(Vector2 initialPosition, int row, int column, float cellWidth, float cellHeight)
    {
        var hPos = column * cellWidth;
        var vPos = row * cellHeight;

        hPos += column * HGap;
        vPos += row * VGap;

        return new Vector2(hPos + initialPosition.X, vPos + initialPosition.Y);
    }

    private InvaderResource GetEnemyResource(int row)
    {
        if (row == 0)
        {
            return invaders[0];
        }
        if (row < 3)
        {
            return invaders[1];
        }
        return invaders[2];
    }

    public Node2D SpawnEnemy(string scenePath, Vector2 position)
    {
        var scene = GD.Load<PackedScene>(scenePath);
        var enemy = scene.Instantiate<Node2D>();
        enemy.Position = position;

        EnemiesNavigator.AddChild(enemy);
        return enemy;
    }
}
