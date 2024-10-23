using Godot;
using System;

public partial class hud : CanvasLayer
{
	private TextureProgressBar HealthBar;
	public override void _Ready()
	{
		HealthBar = GetNode<TextureProgressBar>("HealthBar");
		HealthBar.Value = 100;
		Signals.Instance.PlayerHealthChanged += UpdateHealthBar;
	}

	public override void _Process(double delta)
	{
	}

	public override void _ExitTree()
    {
		GD.Print("frwe");
        Signals.Instance.PlayerHealthChanged -= UpdateHealthBar;
    }

	private void UpdateHealthBar(float HP)
	{
		HealthBar.Value = HP;
	}
}
