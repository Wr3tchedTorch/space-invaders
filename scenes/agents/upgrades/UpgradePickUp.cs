using Godot;
using SpaceInvaders.Scenes.Autoloads;
using System;

namespace SpaceInvaders.Scenes.Agents.Upgrades;

public partial class UpgradePickUp : Area2D
{
    [Export] private Resource UpgradeResource;
    [Export] private UpgradeType upgradeType;

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

        switch (upgradeType)
        {
            case UpgradeType.Bullet:
                GameEvents.Instance.EmitSignal(GameEvents.SignalName.BulletUpgradePickedUp, UpgradeResource);
                break;
            case UpgradeType.Weapon:
                GameEvents.Instance.EmitSignal(GameEvents.SignalName.WeaponUpgradePickedUp, UpgradeResource);
                break;
        }

        QueueFree();
    }
}

public enum UpgradeType
{
    Bullet,
    Weapon    
}