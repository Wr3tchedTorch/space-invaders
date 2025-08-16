using System;
using SpaceInvaders.Assets.Resources.Weapon;

namespace SpaceInvaders.Assets.Scripts.Interfaces;

public interface IWeapon
{
    public WeaponResource WeaponResource { get; set; }
}
