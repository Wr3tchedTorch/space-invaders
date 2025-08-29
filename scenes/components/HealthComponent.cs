using Godot;
using SpaceInvaders.Assets.Scripts.Exceptions;
using System;

namespace SpaceInvaders.Scenes.Components;

public partial class HealthComponent : Node
{
    [Signal] public delegate void DiedEventHandler();

    [ExportGroup("Dependencies")]
    [Export] public ProgressBar? HealthBar { get; set; } = null;

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
                HealthBar.Value = CurrentHealth;

            if (CurrentHealth <= 0)
            {
                EmitSignal(SignalName.Died);
            }
        }
    }

    private float _currentHealth;
    private float? _initialHealth = null;

    public void TakeDamage(float Damage)
    {
        if (InitialHealth == null)
        {
            throw new InitialHealthNullException();
        }
        CurrentHealth -= Mathf.Max(Damage, 0);
    }
}
