using System;

namespace SpaceInvaders.Assets.Scripts.Exceptions;

public class ResourceNullException : Exception
{
    public ResourceNullException(string resourceName)
        : base($"Resource '{resourceName}' is null or not set.")
    {
    }
}
