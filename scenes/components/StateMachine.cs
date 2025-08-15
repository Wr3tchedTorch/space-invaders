using Godot;
using SpaceInvaders.Assets.Scripts.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SpaceInvaders.Scenes.Components;

public partial class StateMachine : Node
{
    public Node2D Parent { get; set; }

    public Dictionary<string, IState> States { get; private set; } = [];

    public IState CurrentState { get; private set; }

    public override void _Ready()
    {
        Parent = GetOwner<Node2D>();

        var children = GetChildren().OfType<IState>();
        foreach (var child in children)
        {
            if (child is Node node)
            {
                child.Exit();
                child.Parent = Parent;
                States.Add(node.Name, child);                
            }
        }        
    }

    public void Enter()
    {
        CurrentState = States.FirstOrDefault().Value;
        CurrentState.Enter();
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
    }

    public void Exit()
    {
        if (CurrentState == null)
        {
            GD.PrintErr("No state is selected.");
            return;
        }
        CurrentState.Exit();
        CurrentState = null;
    }
}
