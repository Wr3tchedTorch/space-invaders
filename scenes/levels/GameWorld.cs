using Godot;
using System;

namespace SpaceInvaders.Scenes.Levels;

public partial class GameWorld : Node2D
{
    public override void _Ready()
    {
        AddToGroup(nameof(GameWorld));
    }
}
