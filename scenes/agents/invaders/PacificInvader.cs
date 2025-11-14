using Godot;
using SpaceInvaders.Scenes.Autoloads;
using SpaceInvaders.Scenes.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpaceInvaders.Scenes.Agents.Invaders;

public partial class PacificInvader : Area2D
{
    private readonly StringName DefaultAnimationName = "default";
    private readonly StringName DeathAnimationName = "death";

    [Export]
    private string InvaderName { get; set; } = "Pacific Invader";
    [Export]
    private float InitialHealth { get; set; } = 125f;
    [Export]
    private HealthComponent HealthComponent { get; set; } = null!;
    [Export]
    private AnimatedSprite2D AnimatedSprite2D { get; set; } = null!;
    [Export]
    private Timer LevelStartTimer { get; set; } = null!;

    private Queue<string> DamageDialogueQueue { get; } = new(
    [
        "Para, isso é falta de educação!",
        "Isso machucou cara, por favor para!",
        "O que você tá fazendo? Vamos conversar numa boa mano.",
        "Já chega, se você continuar eu vou te bater!"
    ]);

    public override void _Ready()
    {
        HealthComponent.InitialHealth = InitialHealth;
        HealthComponent.Died += OnDied;

        LevelStartTimer.Timeout += StartLevel;

        Callable.From(() => Talk("Eae, não esparava ver alguém ai. Quem é você maninho?")).CallDeferred();
    }

    public void Attacked(float _)
    {   
        if (DamageDialogueQueue.Count == 0)
        {
            return;
        }

        Talk(DamageDialogueQueue.Dequeue());

        if (AnimatedSprite2D.IsPlaying() == false)
        {
            AnimatedSprite2D.Play(DefaultAnimationName);
        }
    }

    public async void OnDied()
    {
        Talk("Ahhh... Não acredito que vou morrer assim... Diga adeus a minha familia...");

        await WaitAndCloseDialogue(2.0);

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

    private void Talk(string text)
    {
        GameEvents.Instance.EmitSignal(GameEvents.SignalName.Talked, InvaderName, text);
    }

    private async Task WaitAndCloseDialogue(double seconds)
    {
        await ToSignal(GetTree().CreateTimer(seconds), "timeout");

        GameEvents.Instance.EmitSignal(GameEvents.SignalName.EndedDialogue);            
    }
}
