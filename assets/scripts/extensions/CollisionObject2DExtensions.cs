using System;
using Godot;

namespace SpaceInvaders.Assets.Scripts.Extensions;

public static class CollisionObject2DExtensions
{
    public static readonly int NumberOfLayers = 32;

    public static void ClearPhysicsLayers(this CollisionObject2D collisionObject)
    {
        for (int i = 1; i <= NumberOfLayers; i++)
        {
            collisionObject.SetCollisionLayerValue(i, false);
        }
    }

    public static void ClearPhysicsMasks(this CollisionObject2D collisionObject)
    {
        for (int i = 1; i <= NumberOfLayers; i++)
        {
            collisionObject.SetCollisionMaskValue(i, false);
        }
    }
}
