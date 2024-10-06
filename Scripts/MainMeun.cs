using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class MainMeun : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		LoadSettingsGame(GetDictionary());
	}

	public override void _Process(double delta)
	{
	}

	private Dictionary<string, string> GetDictionary()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		var Settings = FileAccess.Open("res://Save//SaveSettngsGame.txt", FileAccess.ModeFlags.Read);

        while (!Settings.EofReached())
        {
            string line = Settings.GetLine();
            var parts = line.Split(new[] { '=' }, 2);

            if (parts.Length == 2)
            {
                string key = parts[0].Trim();
                string value = parts[1].Trim();
                dictionary[key] = value;
            }
        }
		Settings.Close();
		return dictionary;
	}

	private void LoadSettingsGame(Dictionary<string, string> Settings)
	{
		Vector2I PermissionSize;
		List<string> Permission = Settings["Permission"].Split("x").ToList();
 		PermissionSize = new Vector2I(int.Parse(Permission[0]), int.Parse(Permission[1]));
		DisplayServer.WindowSetSize(PermissionSize);
		
		if (bool.Parse(Settings["Fullscreen"])) DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
		else DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);

		float GlobalBrightnessValue = float.Parse(Settings["GlobalBrightness"]);
		var GlobalBrightness = GetNode<WorldEnvironment>("GlobalBrightness");
		GlobalBrightness.Environment.AdjustmentBrightness = GlobalBrightnessValue;
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

	private void CloseGame()
	{
		GetTree().Quit();
	}
}
