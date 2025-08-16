using Godot;

namespace SpaceInvaders.Scenes.Autoloads;

public partial class GameEvents : Node
{
    [Signal] public delegate void UpgradePickedUpEventHandler(Resource upgrade);

    public static GameEvents Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
    }
}
