using System;

namespace SpaceInvaders.Assets.Scripts.Exceptions;

public class InvalidBulletUpgradeException : ArgumentException
{
    public InvalidBulletUpgradeException(string resourceName) : base($"The resource `{resourceName}` is not a valid bullet upgrade resource.")
    {
    }
}
