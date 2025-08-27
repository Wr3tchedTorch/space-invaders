using Godot;
using SpaceInvaders.Assets.Scripts.Interfaces;
using System;

namespace SpaceInvaders.Scenes.Agents.Bullets;

public partial class ExplosionArea : Area2D, IAttacker
{
    public float Damage { get; set; }
    public float Radius { get; set; }
}
