using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceInvaders.Scenes.Managers;

public partial class MarkerManager : Node
{
    [Signal] public delegate void MarkersSpawnedEventHandler(Marker2D[] markers);

    [ExportGroup("Configuration")]
    [Export]
    public int MarkersCount
    {
        get => _markersCount;
        set
        {
            _markersCount = value;

            if (!_initialized)
            {
                return;
            }
            SpawnMarkers();
        }
    }
    [Export]
    public float HGap
    {
        get => _hGap;
        set
        {
            _hGap = value;

            if (!_initialized)
            {
                return;
            }
            SpawnMarkers();
        }
    }
    [Export]
    public float MaxAngle
    {
        get => _maxAngle;
        set
        {
            _maxAngle = value;

            if (!_initialized)
            {
                return;
            }
            SpawnMarkers();
        }
    }

    [ExportGroup("Dependencies")]
    [Export] public Marker2D CenterMarker { get; set; } = null!;
    [Export] public Node2D MarkersParent { get; set; } = null!;

    private float TotalWidth => (MarkersCount - 1) * HGap;
    private Vector2 CenterPosition => CenterMarker.Position;

    private int _markersCount;
    private float _hGap;
    private float _maxAngle;
    private bool _initialized = false;

    public override void _Ready()
    {
        Callable.From(SpawnMarkers).CallDeferred();
        Callable.From(() => _initialized = true).CallDeferred();
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

        if (MarkersCount == 1)
        {
            return 0f; // Single marker stays centered
        }

        var t = (float)index / (MarkersCount-1);
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
