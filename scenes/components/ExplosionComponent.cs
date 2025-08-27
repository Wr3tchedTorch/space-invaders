using Godot;
using System;

namespace SpaceInvaders.Scenes.Components;

public partial class ExplosionComponent : Node
{
    [Signal] public delegate void ExplosionFinishedEventHandler();

    [ExportGroup("Explosion Attributes")]
    [Export] private float Duration { get; set; } = 1f;

    [ExportGroup("Explosion Configuration")]
    [Export] private Area2D ExplosionArea { get; set; } = null!;

    public void TriggerExplosion()
    {
        CreateExplosionCollision();
    }

    private async void CreateExplosionCollision()
    {
        ExplosionArea.SetDeferred("monitorable", true);
        ExplosionArea.Visible = true;

        await ToSignal(GetTree().CreateTimer(Duration), "timeout");
        ExplosionArea.SetDeferred("monitorable", false);

        EmitSignal(SignalName.ExplosionFinished);
    }
}
