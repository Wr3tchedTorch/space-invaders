using Godot;
using SpaceInvaders.Assets.Scripts.Exceptions;
using SpaceInvaders.Assets.Scripts.Interfaces;
using System;

namespace SpaceInvaders.Scenes.Components;

public partial class HurtboxComponent : Area2D
{
    [Signal] public delegate void AttackedEventHandler(float damage);

    public override void _Ready()
    {
        Monitoring = true;
        Monitorable = true;

        AreaEntered += OnAreaEntered;
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is not IAttacker)
        {
            throw new InvalidAttackerException();
        }
        var attacker = (IAttacker)body;
        EmitHurtSignal(attacker);
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is not IAttacker)
        {
            throw new InvalidAttackerException();
        }
        var attacker = (IAttacker)area;
        EmitHurtSignal(attacker);
    }

    private void EmitHurtSignal(IAttacker attacker)
    {
        EmitSignal(SignalName.Attacked, attacker.Damage);
    }
}
