using Godot;
using SpaceInvaders.Assets.Resources.Weapon;
using SpaceInvaders.Scenes.Autoloads;
using System;

namespace SpaceInvaders.Scenes.UI;

public partial class Gui : Control
{
	[Export]
	private Label PlayerWeaponLabel { get; set; } = null!;
	[Export]
	private WeaponResource InitialPlayerWeapon { get; set; } = null!;

	public override void _Ready()
	{
		PlayerWeaponLabel.Text = InitialPlayerWeapon.Name;

		GameEvents.Instance.WeaponChanged += OnWeaponChanged;
	}

	private void OnWeaponChanged(WeaponResource newWeapon)
	{
		if (GameData.Instance.IsGameOver)
        {
            return;
        }

		PlayerWeaponLabel.Text = newWeapon.Name;
	}
}
