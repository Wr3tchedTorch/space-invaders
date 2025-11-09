using Godot;
using System;

namespace SpaceInvaders.Scenes.UI;

public partial class ReloadBar : Control
{
	[Export]
	public bool HideOnMaxValue { get; set; } = true;
	[Export]
	private TextureProgressBar ProgressBar { get; set; } = null!;

	private Timer? timer;
	private double TimePassed => timer == null ? 0 : (timer.WaitTime - timer.TimeLeft);
	private double GetCurrentValue => timer == null ? ProgressBar.MaxValue : (TimePassed * ProgressBar.MaxValue) / timer.WaitTime;

	public override void _Process(double delta)
	{
		if (ProgressBar.Value == ProgressBar.MaxValue)
		{
			ProgressBar.Visible = !HideOnMaxValue;
			return;
		}
		ProgressBar.Value = GetCurrentValue;
	}

	public void Play(Timer timer)
	{
		this.timer = timer;

		ProgressBar.Value = GetCurrentValue;
		ProgressBar.Visible = true;
	}
}
