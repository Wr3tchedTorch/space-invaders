using System;
using System.Numerics;
using Godot;

namespace SpaceInvaders.Assets.Scripts.Extensions;

public static class ViewportExtensions
{
    public static Rect2 GetGlobalBoundsCoordinates(this Viewport view)
    {
        var view_rect = view.GetVisibleRect();
        var camera = view.GetCamera2D();

        var startBorderX = camera.GlobalPosition.X - view_rect.Size.X;
        var endBorderX = camera.GlobalPosition.X + view_rect.Size.X;

        var startBorderY = camera.GlobalPosition.Y - view_rect.Size.Y;
        var endBorderY = camera.GlobalPosition.Y + view_rect.Size.Y;

        var rect = new Rect2(startBorderX, startBorderY, endBorderX - startBorderX, endBorderY - startBorderY);

        return rect;
    }        
}
