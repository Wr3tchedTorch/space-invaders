using Godot;
using System;

[Tool]
public partial class ExplosionParticles : GpuParticles2D
{
	[Export]
	public float Percentage { get; set; } = 40;

	[Export]
	public double LifeTimeInSeconds
	{
		get => _lifeTime;
		set
		{
			_lifeTime = value;

			if (ProcessMaterial is not ParticleProcessMaterial processMaterial)
			{
				GD.PrintErr("GPUParticles2D must have a ParticleProcessMaterial assigned!");
				return;
			}

			Lifetime = _lifeTime;
			processMaterial.InitialVelocityMax = Percentage / (float)Lifetime;
			GD.Print($"processMaterial.InitialVelocityMax: {processMaterial.InitialVelocityMax}");
		}
	}
	[Export]
	public float InitialVelocity
	{
		get => _initialVelocity;
		set
		{
			_initialVelocity = value;

			if (ProcessMaterial is not ParticleProcessMaterial processMaterial)
			{
				GD.PrintErr("GPUParticles2D must have a ParticleProcessMaterial assigned!");
				return;
			}

			processMaterial.InitialVelocityMax = value;
			LifeTimeInSeconds = value / Percentage;
		}
	}

	private double _lifeTime;
	private float _initialVelocity;

	public override void _Ready()
	{
		ParticleProcessMaterial? processMaterial = ProcessMaterial as ParticleProcessMaterial;
		if (processMaterial == null)
		{
			GD.PrintErr("GPUParticles2D must have a ParticleProcessMaterial assigned!");
			return;
		}

	}

	public override void _Process(double delta)
	{
	}
}
