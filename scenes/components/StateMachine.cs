using Godot;
using SpaceInvaders.Assets.scripts.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceInvaders.Scenes.Components;

public partial class StateMachine : Node
{
    public Node2D Parent { get; set; }

    public Dictionary<string, IState> States { get; private set; } = [];

    public IState CurrentState { get; private set; }

    public override void _Ready()
    {
        var children = GetChildren().OfType<IState>();
        foreach (var child in children)
        {
            if (child is Node node)
            {
                child.Exit();
                RemoveChild(node);
                States.Add(node.Name, child);
            }
        }
        CurrentState = States.FirstOrDefault().Value;
        CurrentState.Enter();
        AddChild((Node)CurrentState);
    }

    public override void _PhysicsProcess(double delta)
    {
        CurrentState.PhysicsUpdate((float)delta);
    }

    public override void _Process(double delta)
    {
        CurrentState.Update((float)delta);
    }

    public void SwitchTo(string stateName)
    {
        if (!States.TryGetValue(stateName, out IState newState))
        {
            GD.PrintErr($"The state {stateName} does not exist.");
            return;
        }

        Exit();

        CurrentState = newState;
        CurrentState.Enter();
        AddChild((Node)CurrentState);
    }

    public void Exit()
    {
        if (CurrentState == null)
        {
            GD.PrintErr("No state is selected.");
            return;
        }
        CurrentState.Exit();
        RemoveChild((Node)CurrentState);
        CurrentState = null;
    }
}
