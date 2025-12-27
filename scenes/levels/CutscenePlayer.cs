using Godot;
using SpaceInvaders.Scenes.Autoloads;
using System;

namespace SpaceInvaders.Scenes.Levels;

public partial class CutscenePlayer : AnimationPlayer
{
    [Export]
    private StringName CutsceneAnimationName { get; set; } = "intro_scene";        

    public override void _Ready() 
    {
        Callable.From(SkipCutscene).CallDeferred();        

        GameEvents.Instance.CutsceneStarted += () => 
        {
            if (IsPlaying())
            {
                return;
            }
            Play(CutsceneAnimationName);
        };
        AnimationFinished += SetupLevelStart;
    }

    private void SkipCutscene()
    {
        if (!GameWorld.SkippingCutscene)
        {
            return;
        }
        SetupLevelStart(CutsceneAnimationName);
    }

    private void SetupLevelStart(StringName animationName)
    {
        if (animationName != CutsceneAnimationName)
        {
            return;
        }
        GameData.Instance.CurrentLevel++;
        GameEvents.Instance.EmitSignal(GameEvents.SignalName.PacificInvaderDied);        
        StartLevel();
    }    

    private async void StartLevel()
    {
        await ToSignal(GetTree().CreateTimer(2.0f), "timeout");
        GameEvents.Instance.EmitSignal(GameEvents.SignalName.LevelStarted);
    }
}
