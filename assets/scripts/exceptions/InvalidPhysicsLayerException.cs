using System;

namespace SpaceInvaders.Assets.Scripts.Exceptions;

public class InvalidPhysicsLayerException : Exception
{
    public InvalidPhysicsLayerException(int index) : base($"Invalid physics layer: {index}. It must be between 1 and 32.")
    {
    }
}
