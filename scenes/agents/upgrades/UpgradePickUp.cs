using Godot;
using SpaceInvaders.Assets.Scripts.Exceptions;
using SpaceInvaders.Assets.Scripts.Interfaces;
using SpaceInvaders.Scenes.Autoloads;
using System;
using System.Runtime.CompilerServices;

namespace SpaceInvaders.Scenes.Agents.Upgrades;

public partial class UpgradePickUp : CharacterBody2D
{
    private readonly StringName DisappearAnimationName = "disappear";

    [ExportGroup("Dependencies")]
    [Export] public Resource UpgradeResource { get; set; } = null!;
    [Export] public Area2D Area2D { get; set; } = null!;
    [Export] public AnimationPlayer AnimationPlayer { get; set; } = null!;

    [ExportGroup("Configuration")]
    [Export] public float DelayBeforeDisappearing { get; set; } = 5;

    private bool pickedUp = false;

    private double timePassedSinceSpawn;

    private double AnimationStartDelay => DelayBeforeDisappearing * 0.3f;

    public override void _Ready()
    {
        Area2D.BodyEntered += OnBodyEntered;

        var disappearTimer = new Timer()
        {
            OneShot = true,
            Autostart = false,
            WaitTime = DelayBeforeDisappearing
        };
        AddChild(disappearTimer);
        disappearTimer.Timeout += () =>
        {
            if (!pickedUp)
            {
                QueueFree();
            }
        };
        disappearTimer.Start();
    }

    public override void _Process(double delta)
    {
        timePassedSinceSpawn += delta;

        if (timePassedSinceSpawn >= AnimationStartDelay)
        {
            PlayDisappearAnimation(timePassedSinceSpawn);
        }        
    }

    private void PlayDisappearAnimation(double timePassedSinceSpawn)
    {
        if (!AnimationPlayer.IsPlaying())
        {
            AnimationPlayer.Play(DisappearAnimationName);
        }
        AnimationPlayer.SpeedScale = (float)(timePassedSinceSpawn / AnimationStartDelay);
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