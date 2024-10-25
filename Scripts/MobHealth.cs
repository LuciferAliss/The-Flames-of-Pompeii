using Godot;
using System;

public partial class MobHealth : Node2D
{
	private TextureProgressBar HealthBar;

	public override void _Ready()
	{
		HealthBar = GetNode<TextureProgressBar>("HealthBar");
		HealthBar.Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void UpdateHealthBar(float HP)
	{
		HealthBar.Value = HP;
	}
}
