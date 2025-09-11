using Godot;
using SpaceInvaders.Assets.Scripts.Exceptions;
using SpaceInvaders.Assets.Scripts.Extensions;
using SpaceInvaders.Assets.Scripts.Interfaces;
using SpaceInvaders.Scenes.Autoloads;
using SpaceInvaders.Scenes.Components;
using System;

namespace SpaceInvaders.Scenes.Agents.Players;

public partial class Player : CharacterBody2D
{
    public static readonly StringName LeftArrowAction = "left";
    public static readonly StringName RightArrowAction = "right";
    public static readonly StringName AttackAction = "attack";

    [ExportGroup("Attributes")]
    [Export] public float Speed { get; set; }
    [Export] public float MaxHealth { get; set; }

    [ExportGroup("Dependencies")]
    [Export] private CollisionShape2D CollisionShape2D { get; set; } = null!;
    [Export] private WeaponComponent WeaponComponent { get; set; } = null!;
    [Export] private HealthComponent HealthComponent { get; set; } = null!;
    [Export] private ProgressBar HealthBar { get; set; } = null!;

    [Export] private Resource BulletUpgrade { get; set; } = null!;

    private float endBorder;
    private float startBorder;
    private float spriteWidth;

    private Vector2 _direction;

    public override void _Ready()
    {
        GameEvents.Instance.GameOver += Die;

        HealthComponent.Died += OnDied;
        HealthComponent.HealthBar = HealthBar;
        HealthComponent.InitialHealth = 100;

        WeaponComponent.GetDirection = new Callable(this, MethodName.GetDirection);

        spriteWidth = CollisionShape2D.Shape.GetRect().Size.X;

        CalculateScreenBounds();

        GameEvents.Instance.BulletUpgradePickedUp += OnBulletUpgradePickedUp;
        GameEvents.Instance.WeaponUpgradePickedUp += OnWeaponUpgradePickedUp;

        WeaponComponent.BulletUpgrades.Add((IBulletUpgrade)BulletUpgrade);
    }

    public override void _Process(double delta)
    {
        HandleWeaponShootingInput();
    }

    public override void _PhysicsProcess(double delta)
    {
        var dir = Input.GetAxis(LeftArrowAction, RightArrowAction);

        _direction.X = dir;

        var canGoRight = GlobalPosition.X + spriteWidth / 2 + 1 < endBorder;
        var canGoLeft = GlobalPosition.X - spriteWidth / 2 - 1 > 0;

        if (dir == -1 && !canGoLeft)
        {
            return;
        }
        if (dir == 1 && !canGoRight)
        {
            return;
        }

        var velocity = Vector2.Right * dir * Speed;
        Velocity = velocity;
        MoveAndSlide();
    }

    private void CalculateScreenBounds()
    {
        var view = GetViewport();
        var boundsRect = view.GetGlobalBoundsCoordinates();
        endBorder = boundsRect.End.X;
        startBorder = boundsRect.Position.X;
    }

    private void HandleWeaponShootingInput()
    {
        var isAttacking = Input.IsActionPressed(AttackAction);
        if (isAttacking)
        {
            WeaponComponent.StartShooting();
            return;
        }
        WeaponComponent.StopShooting();
    }

    private Vector2 GetDirection()
    {
        return Vector2.Up;
    }

    private void OnWeaponUpgradePickedUp(Resource upgrade)
    {
        if (upgrade is not IWeaponUpgrade)
        {
            throw new InvalidUpgradeTypeException(upgrade.ResourcePath, nameof(IWeaponUpgrade));
        }
        ((IWeaponUpgrade)upgrade).ApplyUpgrade(WeaponComponent);
        GD.Print($"upgrade picked up: {upgrade.ResourcePath}");
    }

    private void OnBulletUpgradePickedUp(Resource upgrade)
    {
        if (upgrade is not IBulletUpgrade)
        {
            throw new InvalidUpgradeTypeException(upgrade.ResourcePath, nameof(IBulletUpgrade));
        }
        if (upgrade is IBulletTemporaryUpgrade temporaryUpgrade)
        {
            WeaponComponent.AddUpgradeWaitAndRemove(temporaryUpgrade);
            return;
        }
        WeaponComponent.BulletUpgrades.Add((IBulletUpgrade)upgrade);
    }

    private void OnAttacked(float damage)
    {
        GD.Print($"Player attacked with {damage} damage.");
    }

    private void OnDied()
    {
        GameEvents.Instance.EmitSignal(GameEvents.SignalName.GameOver);

        Die();
    }

    private void Die()
    {
        QueueFree();
    }
}
