using Godot;
using SpaceInvaders.Assets.Resources.Bullet;
using System;

namespace SpaceInvaders.Assets.Resources.Weapon;

[GlobalClass]
public partial class WeaponResource : Resource
{
    [Export] public float FireRateDelay { get; set; }
    [Export] public BulletResource BulletResource { get; set; }
}
