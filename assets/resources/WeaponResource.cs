using Godot;
using System;

namespace SpaceInvaders.Assets.Resources;

[GlobalClass]
public partial class WeaponResource : Resource
{
    [Export] public float FireRateDelay { get; set; }
    [Export] public BulletResource BulletResource { get; set; }
}
