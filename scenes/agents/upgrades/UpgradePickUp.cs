using Godot;
using SpaceInvaders.Assets.Scripts.Exceptions;
using SpaceInvaders.Assets.Scripts.Interfaces;
using SpaceInvaders.Scenes.Autoloads;
using System;

namespace SpaceInvaders.Scenes.Agents.Upgrades;

public partial class UpgradePickUp : Area2D
{
    [Export] public Resource UpgradeResource { get; set; } = null!;

    private bool pickedUp = false;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;        
    }

    private void OnBodyEntered(Node2D body)
    {
        if (pickedUp)
        {
            return;
        }

        pickedUp = true;

        if (UpgradeResource is IWeapon)
        {
            GameEvents.Instance.EmitSignal(GameEvents.SignalName.WeaponUpgradePickedUp, UpgradeResource);
        }
        else if (UpgradeResource is IBullet)
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