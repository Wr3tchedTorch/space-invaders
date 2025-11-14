using System;
using Godot;
using SpaceInvaders.Assets.Resources.Weapon;

namespace SpaceInvaders.Scenes.Autoloads;

public partial class GameEvents : Node
{
    #region Player
    [Signal] public delegate void BulletUpgradePickedUpEventHandler(Resource upgrade);
    [Signal] public delegate void WeaponUpgradePickedUpEventHandler(Resource upgrade);
    [Signal] public delegate void WeaponChangedEventHandler(WeaponResource newWeapon);
    #endregion

    #region Gameplay
    [Signal] public delegate void LevelEndedEventHandler();
    [Signal] public delegate void LevelStartedEventHandler();
    [Signal] public delegate void GameOverEventHandler();
    #endregion


    [Signal] public delegate void PacificInvaderDiedEventHandler();
    [Signal] public delegate void InvaderDiedEventHandler();
    
    #region Dialogue
    [Signal] public delegate void TalkedEventHandler(string name, string text);
    [Signal] public delegate void EndedDialogueEventHandler();
    #endregion

    public static GameEvents Instance { get; private set; } = null!;

    public override void _Ready()
    {
        Instance = this;
    }
}
