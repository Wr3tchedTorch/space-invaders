using System;
using Godot;
using SpaceInvaders.Assets.Resources.Invader;

namespace SpaceInvaders.Assets.Scripts.Interfaces;

public interface IEnemy
{
    public InvaderResource InvaderResource { get; set; }
}
