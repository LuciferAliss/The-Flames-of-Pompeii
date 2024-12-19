using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class MainMeun : Node2D
{
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Visible;
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
		GlobalBrightness.Instance.ChangeBrightness(GlobalBrightnessValue);

		float GlobalVolumeMusic = float.Parse(Settings["VolumeMusic"]);
		GlobalAudio.Instance.ChangeVolumeMusic(GlobalVolumeMusic);

		float GlobalVolumeSoundEffects = float.Parse(Settings["VolumeSoundEffects"]);
		GlobalAudio.Instance.ChangeVolumeSoundEffects(GlobalVolumeSoundEffects);
	}

	private void ChangeSceneToSettings()
	{
		GlobalAudio.Instance.PlaySoundEffects("ButtonPressed");
		var newScene = (PackedScene)GD.Load("res://Scenes//Settings.tscn");
		var currentScene = GetTree().CurrentScene;
		var nextSceneInstance = newScene.Instantiate();

        GetTree().Root.AddChild(nextSceneInstance);
		GetTree().CurrentScene = nextSceneInstance;
        currentScene.QueueFree();
	}

	private void StartGame()
	{
		GlobalAudio.Instance.PlaySoundEffects("ButtonPressed");
		var newScene = (PackedScene)GD.Load("res://Scenes/first_level.tscn");
		var currentScene = GetTree().CurrentScene;
		var nextSceneInstance = newScene.Instantiate();

        GetTree().Root.AddChild(nextSceneInstance);
		GetTree().CurrentScene = nextSceneInstance;
        currentScene.QueueFree();
	}

	private void CloseGame()
	{
		GlobalAudio.Instance.PlaySoundEffects("ButtonPressed");
		GetTree().Quit();
	}

	private void OutputButtonSound()
	{
		GlobalAudio.Instance.PlaySoundEffects("ButtonMapping");
	}	
}
