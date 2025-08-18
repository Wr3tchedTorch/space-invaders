using System;
using Godot;
using SpaceInvaders.Assets.Scripts.Interfaces;
using SpaceInvaders.Scenes.Components;

namespace SpaceInvaders.Assets.Scripts.Exceptions;

public class InvalidAttackerException : Exception
{
    public InvalidAttackerException() : base($"The {nameof(Node)} attacking this {nameof(HurtboxComponent)} is not a valid `{nameof(IAttacker)}`")
    {        
    }
}
