using Godot;
using System;

namespace SpaceInvaders.Scenes.Components;

public partial class OffScreenDestroyComponent : VisibleOnScreenNotifier2D
{
    [Export] Node2D NodeToFree { get; set; } = null!;

    public override void _Ready()
    {
        ScreenExited += () => NodeToFree.QueueFree();
    }
}
