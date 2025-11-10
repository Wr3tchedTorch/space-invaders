using Godot;
using SpaceInvaders.Assets.Resources.Bullet;
using System;

namespace SpaceInvaders.Assets.Resources.Weapon;

[GlobalClass]
public partial class WeaponResource : Resource
{
    [Signal] public delegate void FireRateDelayChangedEventHandler();

    [Export]
    public float MaxFireRateDelay
    {
        get => _maxFireRateDelay;
        set
        {
            _maxFireRateDelay = value;

            EmitSignal(SignalName.FireRateDelayChanged);
        }
    }
    [Export] public float FireRateDelay { get; set; }
    [Export] public int Ammunition { get; set; }
    [Export] public BulletResource BulletResource { get; set; } = null!;
    [Export]
    public string? Name { get; set; }

    private float _maxFireRateDelay;
}
