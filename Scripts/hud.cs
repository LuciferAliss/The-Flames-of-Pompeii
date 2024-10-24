using Godot;
using System;

public partial class hud : CanvasLayer
{
	private TextureProgressBar HealthBar;
	public override void _Ready()
	{
		HealthBar = GetNode<TextureProgressBar>("Control/HealthBar");
		Signals.Instance.PlayerHealthChanged += UpdateHealthBar;
	}

	public override void _Process(double delta)
	{
	}

	public override void _ExitTree()
    {
        Signals.Instance.PlayerHealthChanged -= UpdateHealthBar;
    }

	private void UpdateHealthBar(float HP)
	{
		HealthBar.Value = HP;
	}
}
