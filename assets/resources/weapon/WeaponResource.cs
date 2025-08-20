using Godot;
using SpaceInvaders.Assets.Resources.Bullet;
using System;

namespace SpaceInvaders.Assets.Resources.Weapon;

[GlobalClass]
public partial class WeaponResource : Resource
{
    [Signal] public delegate void FireRateDelayChangedEventHandler();

    [Export]
    public float FireRateDelay
    {
        get => _fireRateDelay;
        set
        {
            _fireRateDelay = value;

            EmitSignal(SignalName.FireRateDelayChanged);
        }
    }
    [Export] public BulletResource BulletResource { get; set; } = null!;

    private float _fireRateDelay;
}
