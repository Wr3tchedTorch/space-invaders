using Godot;
using SpaceInvaders.Assets.Scripts.interfaces;
using SpaceInvaders.Assets.Resources.Invader;
using SpaceInvaders.Scenes.Components;
using System;

namespace SpaceInvaders.Scenes.Agents.Invaders;

public partial class Invader : Area2D, IEnemy
{
    [ExportGroup("Dependencies")]
    [Export] public HealthComponent HealthComponent { get; private set; } = null!;
    [Export] public WeaponComponent WeaponComponent { get; private set; } = null!;

    public InvaderResource InvaderResource { get; set; } = null!;

    public override void _Ready()
    {
        HealthComponent.InitialHealth = InvaderResource.Health;
        HealthComponent.Died += OnDied;

        WeaponComponent.WeaponResource = InvaderResource.WeaponResource;
        WeaponComponent.GetDirection = new Callable(this, MethodName.GetDirection);
    }

    private void OnDied()
    {
        QueueFree();
    }

    private Vector2 GetDirection()
    {
        return Vector2.Down;
    }
}
