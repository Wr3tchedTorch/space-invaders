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
    [Export] public Marker2D MarkersCenter { get; set; } = null!;
    [Export] public Node2D MarkersParent { get; set; } = null!;

    private float TotalWidth => MarkersCount * HGap;
    private Vector2 CenterPosition => MarkersCenter.GlobalPosition;

    public override void _Ready()
    {
        SpawnMarkers();
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
        if (index < 0)
        {
            throw new IndexOutOfRangeException();
        }

        var position = Vector2.Zero;
        position.X = CenterPosition.X + HGap * index;
        if (index == 0)
        {
            position.X = CenterPosition.X - TotalWidth/2;
        }
        position.Y = CenterPosition.Y;
        return position;
    }

    private float GetMarkerAngle(int index)
    {
        if (index < 0)
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
            GlobalPosition = position,
            Rotation = rotation
        };
        MarkersParent.AddChild(MarkersParent);
        return marker;
    }
}
