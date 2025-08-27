using Godot;
using System;
using System.Linq;

namespace SpaceInvaders.Scenes.Components;

public partial class ExplosionComponent : Node
{
    [Signal] public delegate void ExplosionFinishedEventHandler();

    [ExportGroup("Explosion Attributes")]
    [Export] private float Duration { get; set; } = 1f;

    [ExportGroup("Explosion Configuration")]
    [Export] private Area2D ExplosionArea { get; set; } = null!;
    [Export] private CollisionShape2D ExplosionShape { get; set; } = null!;

    public void TriggerExplosion()
    {
        CreateExplosionCollision();
    }

    private async void CreateExplosionCollision()
    {
        ExplosionShape.SetDeferred("disabled", false);

        await ToSignal(GetTree().CreateTimer(Duration), "timeout");
        ExplosionShape.SetDeferred("disabled", true);
        EmitSignal(SignalName.ExplosionFinished);        
    }
}
