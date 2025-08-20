using System;
using SpaceInvaders.Scenes.Components;

namespace SpaceInvaders.Assets.Scripts.Exceptions;

public class InitialHealthNullException : Exception
{
    public InitialHealthNullException() : base($"The {nameof(HealthComponent.InitialHealth)} property of {nameof(HealthComponent)} must be set.")
    {        
    }
}
