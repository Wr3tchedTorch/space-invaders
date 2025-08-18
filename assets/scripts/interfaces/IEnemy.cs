using System;
using SpaceInvaders.Assets.Resources.Invader;

namespace SpaceInvaders.assets.scripts.interfaces;

public interface IEnemy
{
    public InvaderResource InvaderResource { get; set; }
}
