using Godot;
using SpaceInvaders.Scenes.Autoloads;
using System;

namespace SpaceInvaders.Scenes.Agents.Upgrades;

public partial class UpgradePickUp : Area2D
{
    [Export] private Resource UpgradeResource;

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

        GameEvents.Instance.EmitSignal(GameEvents.SignalName.UpgradePickedUp, UpgradeResource);

        QueueFree();
    }
}
