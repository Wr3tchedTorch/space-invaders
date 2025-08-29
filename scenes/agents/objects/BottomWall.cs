using Godot;
using SpaceInvaders.Scenes.Autoloads;
using System;

namespace SpaceInvaders.Scenes.Agents.Objects;

public partial class BottomWall : Area2D
{
    public override void _Ready()
    {
        AreaEntered += _ => { GameEvents.Instance.EmitSignal(GameEvents.SignalName.GameOver); };
        BodyEntered += _ => { GameEvents.Instance.EmitSignal(GameEvents.SignalName.GameOver); };
    }
}
