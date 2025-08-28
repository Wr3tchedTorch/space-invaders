using System;
using Godot;
using SpaceInvaders.Assets.Resources.Invader;

namespace SpaceInvaders.Assets.Scripts.interfaces;

public interface IEnemy
{
    public InvaderResource InvaderResource { get; set; }
}
