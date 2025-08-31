using Godot;
using SpaceInvaders.Assets.Scripts.Exceptions;
using SpaceInvaders.Assets.Scripts.Interfaces;
using SpaceInvaders.Scenes.Autoloads;
using System;

namespace SpaceInvaders.Scenes.Agents.Upgrades;

public partial class UpgradePickUp : CharacterBody2D
{
    [ExportGroup("Dependencies")]
    [Export] public Resource UpgradeResource { get; set; } = null!;
    [Export] public Area2D Area2D { get; set; } = null!;

    private bool pickedUp = false;

    public override void _Ready()
    {
        Area2D.BodyEntered += OnBodyEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        var velocity = Velocity;
        velocity += GetGravity();
        Velocity = velocity;
        MoveAndSlide();
    }

    private void OnBodyEntered(Node2D body)
    {
        if (pickedUp)
        {
            return;
        }

        pickedUp = true;

        if (UpgradeResource is IWeaponUpgrade)
        {
            GameEvents.Instance.EmitSignal(GameEvents.SignalName.WeaponUpgradePickedUp, UpgradeResource);
        }
        else if (UpgradeResource is IBulletUpgrade)
        {
            GameEvents.Instance.EmitSignal(GameEvents.SignalName.BulletUpgradePickedUp, UpgradeResource);
        }
        else
        {
            throw new InvalidUpgradeTypeException(UpgradeResource.ResourceName);
        }
        QueueFree();
    }    
}

public enum UpgradeType
{
    Bullet,
    Weapon    
}