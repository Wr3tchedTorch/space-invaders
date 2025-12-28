using Godot;
using SpaceInvaders.Scenes.Levels;
using System;

public partial class SlowMotionComponent : Node
{
    private const float DefaultTimeScale = 1f;

    [Export] public ProgressBar TimeBreakingBar { get; set; } = null!;
    [Export] public float SlowMotionTimeScale { get; set; } = 0.5f;
    [Export] public float MaxTimeBreakingAmount { get; set; } = 2f;

    private float currentTimeBreakingAmount;
    private bool doingSlowMotion = false;

    private bool isTimeBroken = false;

    public override void _Ready()
    {
        base._Ready();

        Callable.From(() =>
        {
            currentTimeBreakingAmount = MaxTimeBreakingAmount;
            TimeBreakingBar.Value = 0;
            TimeBreakingBar.MaxValue = MaxTimeBreakingAmount;
        }).CallDeferred();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (isTimeBroken && currentTimeBreakingAmount == 0)
        {
            isTimeBroken = false;
            Engine.TimeScale = DefaultTimeScale;
        }

        DecreaseTimeBreakingAmount(delta);

        IncreaseTimeBreakingAmount(delta);
    }

    private void IncreaseTimeBreakingAmount(double delta)
    {
        if (!doingSlowMotion)
        {
            return;
        }
        AddDuration(delta / Engine.TimeScale);

        if (currentTimeBreakingAmount == MaxTimeBreakingAmount)
        {
            BreakTime();
        }
    }

    private void DecreaseTimeBreakingAmount(double delta)
    {
        if (doingSlowMotion || currentTimeBreakingAmount == 0)
        {
            return;
        }
        double multiplier = isTimeBroken ? 1.5 : 5;
        AddDuration(-(delta / (Engine.TimeScale * multiplier)));
    }


    private void BreakTime()
    {
        isTimeBroken = true;
        doingSlowMotion = false;
        Engine.TimeScale = DefaultTimeScale * 2;
    }

    public void EnableSlowMotion()
    {
        CalculateOddsForTimeBreak();
        if (isTimeBroken)
        {
            return;
        }
        doingSlowMotion = true;
        Engine.TimeScale = SlowMotionTimeScale;
    }

    public void DisableSlowMotion()
    {
        if (isTimeBroken)
        {
            return;
        }
        doingSlowMotion = false;

        Engine.TimeScale = DefaultTimeScale;
    }

    private void AddDuration(double delta)
    {
        currentTimeBreakingAmount = Mathf.Min(MaxTimeBreakingAmount, currentTimeBreakingAmount + (float)delta);
        currentTimeBreakingAmount = Mathf.Max(0, currentTimeBreakingAmount);
        TimeBreakingBar.Value = currentTimeBreakingAmount;
    }

    private void CalculateOddsForTimeBreak()
    {
        if (currentTimeBreakingAmount == 0)
        {
            return;
        }
        float max = currentTimeBreakingAmount / MaxTimeBreakingAmount * 100;
        double odds = GameWorld.Rng.NextDouble() * 100;

        if (odds < max)
        {
            return;
        }

        BreakTime();
    }
}
