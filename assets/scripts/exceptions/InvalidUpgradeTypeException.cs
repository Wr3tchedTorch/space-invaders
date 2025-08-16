using System;

namespace SpaceInvaders.Assets.Scripts.Exceptions;

public class InvalidUpgradeTypeException : ArgumentException
{
    public InvalidUpgradeTypeException(string resourceName, string upgradeType) : base($"The resource `{resourceName}` is not a valid {upgradeType}.")
    {
    }
}
