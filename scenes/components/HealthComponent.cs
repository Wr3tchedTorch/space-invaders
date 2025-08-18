using Godot;
using System;

namespace SpaceInvaders.Scenes.Components;

public partial class HealthComponent : Node
{
    [ExportGroup("Dependencies")]
    [Export] private ProgressBar progressBar;

    public float InitialHealth { get; set; }
    public float CurrentHealth
    {
        get => _currentHealth;
        private set
        {
            _currentHealth = value;
            progressBar.Value = _currentHealth;
        }
    }

    private float _currentHealth;

    public override void _Ready()
    {
        progressBar.MaxValue = InitialHealth;
        CurrentHealth = InitialHealth;
    }

    public void TakeDamage(float Damage)
    {
        CurrentHealth -= Mathf.Max(Damage, 0);
    }
}
