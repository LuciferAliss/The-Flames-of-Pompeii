using Godot;
using System;

public partial class DeathSceneManager : Node
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        Signals.Instance.DeathPlayer += ShowDeathScene;
        Signals.Instance.FinishLevel += ShowEndScene;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    private void ChangeToMain()
    {
        GetTree().Paused = false;

        Input.MouseMode = Input.MouseModeEnum.Hidden;

        GlobalAudio.Instance.PlaySoundEffects("ButtonPressed");
        var newScene = (PackedScene)GD.Load("res://Scenes//MainMenu.tscn");
        var currentScene = GetTree().CurrentScene;
        var nextSceneInstance = newScene.Instantiate();

        GetTree().Root.AddChild(nextSceneInstance);
        GetTree().CurrentScene = nextSceneInstance;
        currentScene.QueueFree();
    }

    private void AgainPressed()
    {
        GetTree().Paused = false;

        var deathScene = GetNode<Control>("CanvasLayer/DeathScene");
        deathScene.Hide();

        GlobalAudio.Instance.PlaySoundEffects("ButtonPressed");
        var newScene = (PackedScene)GD.Load("res://Scenes/first_level.tscn");
        var currentScene = GetTree().CurrentScene;
        var nextSceneInstance = newScene.Instantiate();

        GetTree().Root.AddChild(nextSceneInstance);
        GetTree().CurrentScene = nextSceneInstance;
        currentScene.QueueFree();
    }

    public void ShowDeathScene()
    {
        GetTree().Paused = true;

        Input.MouseMode = Input.MouseModeEnum.Visible;

        var deathScene = GetNode<Control>("CanvasLayer/DeathScene");

        deathScene.Show();
    }

    public void ShowEndScene()
    {
        GetTree().Paused = true;

        Input.MouseMode = Input.MouseModeEnum.Visible;

        var endScene = GetNode<Control>("CanvasLayer/FinishScene");

        endScene.Show();
    }

    public override void _ExitTree()
    {
        Signals.Instance.DeathPlayer -= ShowDeathScene;
        Signals.Instance.FinishLevel -= ShowEndScene;
    }
}
