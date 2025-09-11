using System;
using SpaceInvaders.Assets.Resources.Weapon;

namespace SpaceInvaders.Assets.Scripts.Interfaces;

public interface IWeapon
{
    public WeaponResource PrimaryWeaponResource { get; set; }

    public void SwitchToTemporaryWeapon(WeaponResource weaponResource, double timeBeforeSwitchingBack);
    public void IncrementFireRatePercentage(float amount);
    public void AddCannon(int count);
}
