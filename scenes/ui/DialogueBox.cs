using Godot;
using SpaceInvaders.Scenes.Autoloads;
using System;

namespace SpaceInvaders.Scenes.UI;

public partial class DialogueBox : Control
{
    private Label NameLabel { get; set;} = null!;
    private Label DialogueLabel { get; set;} = null!;

    public override void _Ready()
    {
        Visible = false;

        NameLabel = GetNode<Label>("%Name");
        DialogueLabel = GetNode<Label>("%Dialogue");

        if (NameLabel == null)
        {
            GD.PrintErr("NameLabel is null in DialogueBox");        
        }
        if (DialogueLabel == null)
        {
            GD.PrintErr("DialogueLabel is null in DialogueBox");        
        }

        GameEvents.Instance.EndedDialogue += OnEndedDialogue;
        GameEvents.Instance.Talked += OnTalked;
    }

    public void Talk(string name, string text)
    {
        Visible = true;

        NameLabel.Text = name;
        DialogueLabel.Text = text;
    }

    public void CloseDialogueBox()
    {
        Visible = false;
    }

    private void OnTalked(string name, string text)
    {
        Talk(name, text);
    }

    private void OnEndedDialogue()
    {
        GD.Print("Dialogue ended, closing dialogue box...");
        CloseDialogueBox();
    }
}

