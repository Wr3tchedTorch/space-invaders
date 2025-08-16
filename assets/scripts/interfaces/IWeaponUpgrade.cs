using System;

namespace SpaceInvaders.Assets.Scripts.Interfaces;

public interface IWeaponUpgrade
{
    public void ApplyUpgrade(IWeapon weapon);
}
