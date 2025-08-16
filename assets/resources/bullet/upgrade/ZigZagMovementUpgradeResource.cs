using Godot;
using SpaceInvaders.Assets.Scripts.Interfaces;
using SpaceInvaders.Scenes.States;
using System;

namespace SpaceInvaders.Assets.Resources.Bullet.Upgrade;

[GlobalClass]
public partial class ZigZagMovementUpgradeResource : Resource, IBulletTemporaryUpgrade
{
    [Export] public float UpgradeDuration { get; set; }
    [Export] public float Duration { get; set; }

    public void ApplyUpgrade(IBullet bullet)
    {
        Callable.From(() => bullet.StateMachine.SwitchTo(nameof(InstantZigZagMovementState))).CallDeferred();
    }
}
