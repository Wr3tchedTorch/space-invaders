using System;
using Godot;

namespace SpaceInvaders.Scenes.Autoloads;

public partial class GameEvents : Node
{
    [Signal] public delegate void BulletUpgradePickedUpEventHandler(Resource upgrade);
    [Signal] public delegate void WeaponUpgradePickedUpEventHandler(Resource upgrade);    

    public static GameEvents Instance { get; private set; } = null!;

    public static Random Rng { get; } = new Random(DateTime.UtcNow.Millisecond);

    public override void _Ready()
    {
        Instance = this;
    }
}
