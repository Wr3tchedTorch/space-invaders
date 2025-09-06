using System;
using Godot;
using SpaceInvaders.Assets.Scripts.Interfaces;
using SpaceInvaders.Assets.Scripts.Exceptions;
using SpaceInvaders.Assets.Scripts.Interfaces;

namespace SpaceInvaders.Assets.Resources.Upgrades.Bullets;

[GlobalClass]
public partial class BulletGhostUpgradeResource : Resource, IBulletTemporaryUpgrade, IDrop
{
    [ExportGroup("Configuration")]
    [Export] public int[] MasksToBeIgnored { get; set; } = [];
    [Export] public int[] LayersToBeIgnored { get; set; } = [];
    [Export] public float Duration { get; set; } = 5.0f;
    [Export(PropertyHint.File, ".tscn")] public string ScenePath { get; set; } = null!;

    public void ApplyUpgrade(IBullet bullet)
    {
        foreach (var mask in MasksToBeIgnored)
        {
            if (mask <= 0)
            {
                throw new InvalidLayerException(mask);
            }
            bullet.SetCollisionMaskValue(mask, false);
        }
        foreach (var layer in LayersToBeIgnored)
        {
            if (layer <= 0)
            {
                throw new InvalidLayerException(layer);
            }
            bullet.SetCollisionLayerValue(layer, false);
        }
    }
}
