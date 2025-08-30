using Godot;
using SpaceInvaders.assets.scripts.interfaces;
using SpaceInvaders.Assets.Scripts.Interfaces;
using SpaceInvaders.Scenes.States;
using System;

namespace SpaceInvaders.Assets.Resources.Upgrades.Bullets;

[GlobalClass]
public partial class BulletZigZagMovementUpgradeResource : Resource, IBulletTemporaryUpgrade, IDrop
{
    [Export] public float Duration { get; set; }
    [Export(PropertyHint.File, ".tscn")] public string ScenePath { get; set; } = null!;

    public void ApplyUpgrade(IBullet bullet)
    {
        Callable.From(() => bullet.StateMachine.SwitchTo(nameof(InstantZigZagMovementState))).CallDeferred();
    }
}
