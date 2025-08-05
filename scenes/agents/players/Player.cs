using Godot;
using System;

namespace SpaceInvaders.Scenes.Agents.Players;

public partial class Player : CharacterBody2D
{
    private readonly StringName LeftArrowAction = "left";
    private readonly StringName RightArrowAction = "right";

    [Export] public float Speed { get; set; }
    [Export] private CollisionShape2D CollisionShape2D { get; set; }

    private float endBorder;
    private float startBorder;
    private float size;

    public override void _Ready()
    {
        var view = GetViewport();
        var rect = view.GetVisibleRect();
        var camera = view.GetCamera2D();

        size = CollisionShape2D.Shape.GetRect().Size.X;

        startBorder = camera.GlobalPosition.X - rect.Size.X;
        endBorder = camera.GlobalPosition.X + rect.Size.X;
    }

    public override void _PhysicsProcess(double delta)
    {
        var dir = Input.GetAxis(LeftArrowAction, RightArrowAction);

        var canGoRight = GlobalPosition.X + size / 2 + 1 < endBorder;
        var canGoLeft = GlobalPosition.X - size / 2 - 1 > 0;

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
}
