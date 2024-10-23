using Godot;
using System;

public partial class Level : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Signals.Instance.KillLevel += KillLevel;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void _ExitTree()
    {
		GD.Print("frwe");
        Signals.Instance.KillLevel -= KillLevel;
    }

    public void KillLevel()
	{
		var newScene = (PackedScene)GD.Load("res://Scenes//MainMenu.tscn");
		var currentScene = GetTree().CurrentScene;
		GetNode<Node2D>("Mobs").QueueFree();
		GetNode<CanvasLayer>("HUD").QueueFree();
		GetNode<Node2D>("Player").QueueFree();
		currentScene.QueueFree();

		var nextSceneInstance = newScene.Instantiate();	
		GetTree().Root.AddChild(nextSceneInstance);
		GetTree().CurrentScene = nextSceneInstance;
	}
}
