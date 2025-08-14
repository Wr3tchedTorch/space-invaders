using Godot;
using SpaceInvaders.Assets.Scripts.Extensions;
using SpaceInvaders.Scenes.Agents.Weapons;
using System;

namespace SpaceInvaders.Scenes.Agents.Players;

public partial class Player : CharacterBody2D
{
    public static readonly StringName LeftArrowAction = "left";
    public static readonly StringName RightArrowAction = "right";
    public static readonly StringName AttackAction = "attack";
    
    [Export] public float Speed { get; set; }

    [ExportGroup("Dependencies")]    
    [Export] private CollisionShape2D CollisionShape2D { get; set; }
    [Export] private WeaponComponent WeaponComponent { get; set; }

    private float endBorder;
    private float startBorder;
    private float spriteWidth;

    private Vector2 _direction;

    public override void _Ready()
    {
        WeaponComponent.GetDirection = new Callable(this, MethodName.GetDirection);        

        spriteWidth = CollisionShape2D.Shape.GetRect().Size.X;

        CalculateScreenBounds();
    }

    public override void _Process(double delta)
    {
        HandleWeaponShootingInput();
    }

    public override void _PhysicsProcess(double delta)
    {
        var dir = Input.GetAxis(LeftArrowAction, RightArrowAction);

        _direction.X = dir;

        var canGoRight = GlobalPosition.X + spriteWidth / 2 + 1 < endBorder;
        var canGoLeft = GlobalPosition.X - spriteWidth / 2 - 1 > 0;

        if (dir == -1 && !canGoLeft)
        {
            return;
        }
        if (dir == 1 && !canGoRight)
        {
            return;
        }

        var velocity = Vector2.Right * (dir * Speed);
        Velocity = velocity;
        MoveAndSlide();
    }

    private void CalculateScreenBounds()
    {
        var view = GetViewport();
        var boundsRect = view.GetGlobalBoundsCoordinates();
        endBorder = boundsRect.End.X;
        startBorder = boundsRect.Position.X;
    }

    private void HandleWeaponShootingInput()
    {
        var isAttacking = Input.IsActionPressed(AttackAction);
        if (isAttacking)
        {
            WeaponComponent.StartShooting();
            return;
        }
        WeaponComponent.StopShooting();
    }

    private Vector2 GetDirection()
    {
        return _direction;
    }
}
