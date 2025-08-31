using Godot;
using System;

namespace SpaceInvaders.Scenes.Agents.Objects;

public partial class Bunker : Node2D
{
    private static StringName DefaultAnimationName = "default";

    [Export] private AnimatedSprite2D AnimatedSprite2D { get; set; } = null!;
    [Export] private Timer HitDelayTimer { get; set; } = null!;    

    private int CurrentHitCount
    {
        get => _currentHitCount;
        set
        {
            if (value >= AnimatedSprite2D.SpriteFrames.GetFrameCount(DefaultAnimationName))
            {
                QueueFree();
                return;
            }
            _currentHitCount = value;
            AnimatedSprite2D.Frame = value;
        }
    }

    private int _currentHitCount;
    private bool canTakeHit = true;

    public override void _Ready()
    {
        CurrentHitCount = 0;

        HitDelayTimer.Timeout += () => { canTakeHit = true; };
    }

    public void TakeHit(float _)
    {
        if (!canTakeHit)
        {
            return;
        }
        CurrentHitCount++;

        canTakeHit = false;
        HitDelayTimer.Start();
    }
}