using Godot;
using System;

public partial class DadInvader : Node2D
{
    private readonly StringName DefaultAnimationName = "default";

    [Export]
    private AnimatedSprite2D AnimatedSprite2D { get; set; } = null!;

    public void StartAnimating()
    {
        AnimatedSprite2D.Play(DefaultAnimationName);
    }
}
