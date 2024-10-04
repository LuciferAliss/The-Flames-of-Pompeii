using Godot;
using System;

public partial class MainMeun : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void ChangeSceneToSettings()
	{
		var newScene = (PackedScene)GD.Load("res://Scenes//Settings.tscn");
		var currentScene = GetTree().CurrentScene;
		var nextSceneInstance = newScene.Instantiate();

        GetTree().Root.AddChild(nextSceneInstance);
		GetTree().CurrentScene = nextSceneInstance;
        currentScene.QueueFree();
	}
}
