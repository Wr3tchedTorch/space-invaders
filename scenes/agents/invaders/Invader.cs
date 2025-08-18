using Godot;
using SpaceInvaders.assets.scripts.interfaces;
using SpaceInvaders.Assets.Resources.Invader;
using SpaceInvaders.Scenes.Components;
using System;

namespace SpaceInvaders.Scenes.Agents.Invaders;

public partial class Invader : Area2D, IEnemy
{
    [ExportGroup("Dependencies")]
    [Export] public HealthComponent HealthComponent { get; private set; } = null!;

    public InvaderResource InvaderResource { get; set; } = null!;

    public override void _Ready()
    {
        HealthComponent.InitialHealth = InvaderResource.Health;
        HealthComponent.Died += OnDied;
    }

    private void OnDied()
    {
        QueueFree();
    }
}
