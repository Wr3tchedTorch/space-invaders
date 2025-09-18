using Godot;
using SpaceInvaders.Scenes.Autoloads;
using System;

namespace SpaceInvaders.Scenes.Levels;

public partial class GameWorld : Node2D
{
    [Export] public int Seed { get; private set; } = (int)new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();

    public static Random Rng { get; private set; } = null!;

    public override void _Ready()
    {
        Rng = new Random(Seed);
        GD.Print(Seed);

        AddToGroup(nameof(GameWorld));
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey eventKey)
        {            
            if (eventKey.Pressed && eventKey.Keycode == Key.N)
            {
                GameData.Instance.CurrentLevel++;

                GameEvents.Instance.EmitSignal(GameEvents.SignalName.LevelStarted);
            }
        }
    }
}
