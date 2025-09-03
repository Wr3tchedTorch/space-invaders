using System;

namespace SpaceInvaders.Assets.Scripts.Exceptions;

public class InvalidLayerException : ArgumentException
{
    public InvalidLayerException(int layerNumber) : base($"The number {layerNumber} is not valid for a layer.")
    {
    }
}
