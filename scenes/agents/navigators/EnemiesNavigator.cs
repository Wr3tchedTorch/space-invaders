using Godot;
using SpaceInvaders.Assets.Scripts.interfaces;
using SpaceInvaders.Scenes.Agents.Invaders;
using System;
using System.Linq;

namespace SpaceInvaders.Scenes.Navigators;

public partial class EnemiesNavigator : Node2D
{
    [Export] public float MovementPixelIncrement { get; set; }
    [Export] public float DelayBetweenMovements { get; set; }
    [Export] public Timer MovementTimer { get; set; } = null!;

    private Vector2 Direction = Vector2.Right;

    private float _timeSinceLastSwitch = float.MaxValue;

    private bool _moved = false;
    private bool _moving = true;

    public override void _Ready()
    {
        var enemies = GetChildren().OfType<Invader>();

        foreach (var enemy in enemies)
        {
            enemy.ReachedScreenBorder += SwitchDirection;
        }

        MovementTimer.Timeout += () =>
        {
            if (!_moving)
            {
                return;
            }
            Move();
        };
        MovementTimer.OneShot = false;
        MovementTimer.Start(DelayBetweenMovements);
    }

    private void Move()
    {
        if (_moved) return;
        GD.Print("Moving enemies");

        GlobalPosition += Direction * MovementPixelIncrement;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_moved)
        {
            _timeSinceLastSwitch = 0f;
            _moved = false;
        }
        _timeSinceLastSwitch += (float)delta;
    }

    public void SwitchDirection()
    {
        if (_timeSinceLastSwitch < 0.1f) return;

        _moving = false;

        GlobalPosition += new Vector2(0, MovementPixelIncrement);

        Direction = new Vector2(-Direction.X, Direction.Y);
        _moved = true;

        MovementTimer.Start(DelayBetweenMovements);
        _moving = true;
    }
}
