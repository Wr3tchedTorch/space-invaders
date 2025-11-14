using Godot;
using SpaceInvaders.Assets.Scripts.Exceptions;
using System;

namespace SpaceInvaders.Scenes.Components;

public partial class HealthComponent : Node
{
    [Signal] public delegate void DiedEventHandler();
    [Signal] public delegate void DamageTakenEventHandler();

    [ExportGroup("Dependencies")]
    [Export] public ProgressBar? HealthBar { get; set; } = null;
    [Export] public Timer? InvincibilityTimer { get; set; } = null;

    public float? InitialHealth
    {
        get => _initialHealth;
        set
        {
            _initialHealth = value;

            if (InitialHealth == null)
            {
                throw new InitialHealthNullException();
            }

            if (HealthBar != null)
            {
                HealthBar.MaxValue = InitialHealth.Value;
                HealthBar.Value = HealthBar.MaxValue;
            }
            CurrentHealth = InitialHealth.Value;
        }
    }
    public float CurrentHealth
    {
        get => _currentHealth;
        private set
        {
            _currentHealth = value;

            if (HealthBar != null)
            {
                HealthBar.Value = CurrentHealth;
            }

            if (CurrentHealth <= 0 && !isDead)
            {
                EmitSignal(SignalName.Died);
                isDead = true;
            }
        }
    }

    private float _currentHealth;
    private float? _initialHealth = null;
    private bool isDead = false;
    private bool canTakeDamage = true;

    public override void _Ready() 
    {
        if (InvincibilityTimer != null)
        {
            InvincibilityTimer.Timeout += () => canTakeDamage = true;
        }
    }

    public void TakeDamage(float Damage)
    {        
        if (!canTakeDamage)
        {
            return;
        }

        if (InitialHealth == null)
        {
            throw new InitialHealthNullException();
        }
        CurrentHealth -= Mathf.Max(Damage, 0);
        EmitSignal(SignalName.DamageTaken);

        if (InvincibilityTimer != null)
        {
            canTakeDamage = false;
            InvincibilityTimer.Start();
        }
    }
}
