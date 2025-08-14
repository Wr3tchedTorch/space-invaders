using Godot;
using SpaceInvaders.Assets.Scripts.Extensions;
using SpaceInvaders.Scenes.Agents.Weapons;
using System;

namespace SpaceInvaders.Scenes.Agents.Players;

public partial class Player : CharacterBody2D
{
    private readonly StringName LeftArrowAction = "left";
    private readonly StringName RightArrowAction = "right";

    [Export] public float Speed { get; set; }
    [Export] private CollisionShape2D CollisionShape2D { get; set; }
    [Export] private WeaponComponent WeaponComponent { get; set; }

    private float endBorder;
    private float startBorder;
    private float spriteWidth;

    public override void _Ready()
    {
        spriteWidth = CollisionShape2D.Shape.GetRect().Size.X;

        CalculateScreenBounds();
    }

    public override void _PhysicsProcess(double delta)
    {
        var dir = Input.GetAxis(LeftArrowAction, RightArrowAction);

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
}
