using Godot;
using SpaceInvaders.Scenes.Autoloads;
using SpaceInvaders.Scenes.Components;
using System;

namespace SpaceInvaders.Scenes.Agents.Invaders;

public partial class PacificInvader : Area2D
{
    private readonly StringName DefaultAnimationName = "default";
    private readonly StringName DeathAnimationName = "death";

    [Export]
    private string InvaderName { get; set; } = "Pacific Invader";
    [Export]
    private float InitialHealth { get; set; } = 100f;    
    [Export]
    private HealthComponent HealthComponent { get; set; } = null!;
    [Export]
    private AnimatedSprite2D AnimatedSprite2D { get; set; } = null!;
    [Export]
    private Timer LevelStartTimer { get; set; } = null!;

    public override void _Ready()
    {
        HealthComponent.InitialHealth = InitialHealth;
        HealthComponent.Died += OnDied;

        GD.Print("Hey... I didn't expect to see anyone here... How are you doing bro?");

        LevelStartTimer.Timeout += StartLevel;
    }

    public void Attacked(float _)
    {
        GD.Print("What are you doing? Please don't hurt me!");

        if (AnimatedSprite2D.IsPlaying() == false)
        {
            AnimatedSprite2D.Play(DefaultAnimationName);
        }
    }
    
    public void OnDied()
    {
        GD.Print("Ahhh... I can't believe this is happening... Say goodbye to my family...");

        AnimatedSprite2D.Play(DeathAnimationName);
        AnimatedSprite2D.AnimationFinished += () => 
        {
            AnimatedSprite2D.Visible = false;
            LevelStartTimer.Start();

            GameData.Instance.CurrentLevel++;
            GameEvents.Instance.EmitSignal(GameEvents.SignalName.PacificInvaderDied);
        };
    }

    private void StartLevel()
    {
        if (IsQueuedForDeletion())
        {
            return;
        }

        QueueFree();
        
        GameEvents.Instance.EmitSignal(GameEvents.SignalName.LevelStarted);
    }

}
