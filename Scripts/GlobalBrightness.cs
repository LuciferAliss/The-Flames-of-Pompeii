using Godot;
using System;

public partial class GlobalBrightness : Node
{
	private WorldEnvironment Brightness;
	public static GlobalBrightness Instance { get; private set; }
	
	public override void _Ready()
	{
		Instance = this;
		Brightness = GetNode<WorldEnvironment>("Brightness");
	}

	public override void _Process(double delta)
	{
	}

	public void ChangeBrightness(float brightness)
	{
		Brightness.Environment.AdjustmentBrightness = brightness;
	}
}
