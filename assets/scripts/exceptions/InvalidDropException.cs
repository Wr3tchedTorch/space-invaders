using System;

namespace SpaceInvaders.Assets.Scripts.Exceptions;

public class InvalidDropException : ArgumentException
{
    public InvalidDropException(string dropId) : base($"The drop {dropId} is not a valid IDrop.")
    {
    }
}
