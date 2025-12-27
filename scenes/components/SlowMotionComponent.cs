using Godot;
using System;

public partial class SlowMotionComponent : Node
{
    private const float DefaultTimeScale = 1f;

    [Export] private ProgressBar SlowMotionbar { get; set; } = null!;
    [Export] public float SlowMotionTimeScale { get; set; } = 0.5f;
    [Export] public float MaxSlowMotionDuration { get; set; } = 2f;

    private float currentSlowMotionDuration;
    private bool doingSlowMotion = false;

    public override void _Ready() 
    {
        base._Ready();

        currentSlowMotionDuration = MaxSlowMotionDuration;
        SlowMotionbar.Value = MaxSlowMotionDuration;
        SlowMotionbar.MaxValue = MaxSlowMotionDuration;
    }    

    public override void _Process(double delta) 
    {
        base._Process(delta);

        if (!doingSlowMotion && currentSlowMotionDuration < MaxSlowMotionDuration)
        {
            AddDuration(delta/(Engine.TimeScale*5));
            return;
        }
        if (doingSlowMotion)
        {
            AddDuration(-(delta/Engine.TimeScale));
            
            if (currentSlowMotionDuration <= 0)
            {
                DisableSlowMotion();
            }
        }
    }

    public void EnableSlowMotion()
    {
        if (currentSlowMotionDuration <= 0)
        {
            return;
        }
        doingSlowMotion = true;
        
        Engine.TimeScale = SlowMotionTimeScale;
    }

    public void DisableSlowMotion()
    {
        doingSlowMotion = false;

        Engine.TimeScale = DefaultTimeScale;
    }

    private void AddDuration(double delta)
    {
        currentSlowMotionDuration = Mathf.Min(MaxSlowMotionDuration, currentSlowMotionDuration + (float)delta);
        currentSlowMotionDuration = Mathf.Max(0, currentSlowMotionDuration);
        SlowMotionbar.Value = currentSlowMotionDuration;
    }
}
