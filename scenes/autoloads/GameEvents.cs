using System;
using Godot;
using SpaceInvaders.Assets.Resources.Weapon;

namespace SpaceInvaders.Scenes.Autoloads;

public partial class GameEvents : Node
{
    [Signal] public delegate void BulletUpgradePickedUpEventHandler(Resource upgrade);
    [Signal] public delegate void WeaponUpgradePickedUpEventHandler(Resource upgrade);
    [Signal] public delegate void WeaponChangedEventHandler(WeaponResource newWeapon);
    [Signal] public delegate void LevelEndedEventHandler();
    [Signal] public delegate void LevelStartedEventHandler();
    [Signal] public delegate void InvaderDiedEventHandler();
    [Signal] public delegate void GameOverEventHandler();

    public static GameEvents Instance { get; private set; } = null!;

    public override void _Ready()
    {
        Instance = this;
    }
}
