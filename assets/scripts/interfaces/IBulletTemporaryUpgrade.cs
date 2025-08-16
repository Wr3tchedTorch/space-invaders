using System;
using SpaceInvaders.Assets.Scripts.Interfaces;

namespace SpaceInvaders.Assets.Scripts.Interfaces;

public interface IBulletTemporaryUpgrade : IBulletUpgrade
{
    public float Duration { get; set; }
}
