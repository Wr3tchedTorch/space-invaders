using System;

namespace SpaceInvaders.Assets.Scripts.Exceptions;

public class InvalidPhysicsMaskException : Exception
{
    public InvalidPhysicsMaskException(int index) : base($"Invalid physics mask: {index}. It must be between 1 and 32.")
    {
    }
}
