using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceInvaders.Scenes.Managers;

public partial class MarkerManager : Node
{
    [Signal] public delegate void MarkersSpawnedEventHandler(Marker2D[] markers);

    [ExportGroup("Configuration")]
    [Export] public int MarkersCount { get; set; }
    [Export] public float HGap { get; set; }
    [Export] public float MaxAngle { get; set; }

    [ExportGroup("Dependencies")]
    [Export] public Marker2D CenterMarker { get; set; } = null!;
    [Export] public Node2D MarkersParent { get; set; } = null!;

    private float TotalWidth => (MarkersCount - 1) * HGap;
    private Vector2 CenterPosition => CenterMarker.Position;

    public override void _Ready()
    {
        Callable.From(SpawnMarkers).CallDeferred();
    }

    private void SpawnMarkers()
    {
        List<Marker2D> markers = [];
        for (int i = 0; i < MarkersCount; i++)
        {
            var position = GetMarkerPosition(i);
            var angle = GetMarkerAngle(i);

            var marker = SpawnMarker(position, angle);
            markers.Add(marker);
        }
        EmitSignal(SignalName.MarkersSpawned, markers.ToArray());
    }

    private Vector2 GetMarkerPosition(int index)
    {
        if (index < 0 || index >= MarkersCount)
        {
            throw new IndexOutOfRangeException();
        }
        float startX = CenterPosition.X - TotalWidth / 2;

        var position = Vector2.Zero;
        position.X = startX + index * HGap;
        position.Y = CenterPosition.Y;
        
        return position;
    }

    private float GetMarkerAngle(int index)
    {
        if (index < 0 || index >= MarkersCount)
        {
            throw new IndexOutOfRangeException();
        }

        var t = (MarkersCount == 1) ? 0.5f : (float)index / MarkersCount;
        var angle = -MaxAngle + t * (2 * MaxAngle);

        return angle;
    }

    private Marker2D SpawnMarker(Vector2 position, float rotation)
    {
        var marker = new Marker2D()
        {
            Position = position,
            RotationDegrees = rotation
        };
        MarkersParent.AddChild(marker);
        return marker;
    }

    private Texture2D CreateDebugTexture(int size = 16)
    {
        var image = Image.CreateEmpty(size, size, false, Image.Format.Rgba8);
        image.Fill(Colors.Red); // Fill with red so it’s visible
        var tex = ImageTexture.CreateFromImage(image);
        return tex;
    }
}
