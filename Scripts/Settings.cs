using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Settings : Node2D
{
	public override void _Ready()
	{
		LoadSettingsGame(GetDictionary());
	}

	public override void _Process(double delta)
	{
	}

	private void ChangeSceneToMainMenu()
	{
		GlobalAudio.Instance.PlaySoundEffects("ButtonPressed");
		var newScene = (PackedScene)GD.Load("res://Scenes//MainMeun.tscn");
        var currentScene = GetTree().CurrentScene;
        var nextSceneInstance = newScene.Instantiate();
        
		GetTree().Root.AddChild(nextSceneInstance);
		GetTree().CurrentScene = nextSceneInstance;
		currentScene.QueueFree();
	}

	private void OutputButtonSound()
	{
		GlobalAudio.Instance.PlaySoundEffects("ButtonMapping");
	}	

	private void PermissionSelected(int id)
	{
		GlobalAudio.Instance.PlaySoundEffects("ButtonPressed");
		Vector2I PermissionSize = new Vector2I(1920, 1080);;
		switch (id)
		{
			case 0:
				PermissionSize = new Vector2I(1920, 1080);
				DisplayServer.WindowSetSize(PermissionSize);
				break;	
			case 1:
				PermissionSize = new Vector2I(1600, 900);
				DisplayServer.WindowSetSize(PermissionSize);
				break;	
			case 2:
				PermissionSize = new Vector2I(1280, 720);
				DisplayServer.WindowSetSize(PermissionSize);
				break;	
		}
		string Permission = $"{PermissionSize.X.ToString()}x{PermissionSize.Y.ToString()}";
		SaveSettingsGame(GetDictionary(), "Permission", Permission);
	}
	private void UseFullScreenMode()
	{
		GlobalAudio.Instance.PlaySoundEffects("ButtonPressed");
		bool FullScreenModePosition = bool.Parse(GetDictionary()["Fullscreen"]);
		var FullScreenModeButton = GetNode<TextureButton>("BackgroundSettings/Screen/FullScreenModeButton");
		if(FullScreenModePosition)
		{
			FullScreenModePosition = false;
			FullScreenModeButton.TextureNormal = (Texture2D)GD.Load("res://Images/Settings/CheckBoxFalse.png");
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
		}
		else 
		{
			FullScreenModePosition = true;
			FullScreenModeButton.TextureNormal = (Texture2D)GD.Load("res://Images/Settings/CheckBoxTrue.png");
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
		}
		SaveSettingsGame(GetDictionary(), "Fullscreen", FullScreenModePosition.ToString());
		LoadSettingsGame(GetDictionary());
	}

	private void ChangeBrightness(float brightness)
	{
		GlobalBrightness.Instance.ChangeBrightness(brightness);
		SaveSettingsGame(GetDictionary(), "GlobalBrightness", brightness.ToString());
	}

	private void ChangeMusicVolume(float volume)
	{
		GlobalAudio.Instance.ChangeVolumeMusic(volume);
		SaveSettingsGame(GetDictionary(),"VolumeMusic", volume.ToString());
	}

	private void ChangeSoundEffectsVolume(float volume)
	{
		GlobalAudio.Instance.ChangeVolumeSoundEffects(volume);
		SaveSettingsGame(GetDictionary(),"VolumeSoundEffects", volume.ToString());
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
		var PermissionSelectedButton = GetNode<OptionButton>("BackgroundSettings/Screen/PermissionSelectedButton");
		switch (int.Parse(Settings["Permission"].Split("x").ToArray()[0]))
		{
			case 1920:
				PermissionSelectedButton.Select(0);
				PermissionSelected(0);
				break;
			case 1600:
				PermissionSelectedButton.Select(1);
				PermissionSelected(1);
				break;
			case 1280:
				PermissionSelectedButton.Select(2);
				PermissionSelected(2);
				break;
		}
		
		bool FullScreenModePosition = bool.Parse(Settings["Fullscreen"]);
		var FullScreenModeButton = GetNode<TextureButton>("BackgroundSettings/Screen/FullScreenModeButton");
		if (FullScreenModePosition) FullScreenModeButton.TextureNormal = (Texture2D)GD.Load("res://Images/Settings/CheckBoxTrue.png");
		else FullScreenModeButton.TextureNormal = (Texture2D)GD.Load("res://Images/Settings/CheckBoxFalse.png");

		float GlobalBrightnessValue = float.Parse(Settings["GlobalBrightness"]);
		GlobalBrightness.Instance.ChangeBrightness(GlobalBrightnessValue);
		var BrightnessScrollBar = GetNode<HScrollBar>("BackgroundSettings/Screen/BrightnessScrollBar");
		BrightnessScrollBar.Value = GlobalBrightnessValue;

		float GlobalVolumeMusic = float.Parse(Settings["VolumeMusic"]);
		GlobalAudio.Instance.ChangeVolumeMusic(GlobalVolumeMusic);
		var MusicScrollBar = GetNode<HScrollBar>("BackgroundSettings/Sound/MusicScrollBar");
		MusicScrollBar.Value = GlobalVolumeMusic;

		float GlobalVolumeSoundEffects = float.Parse(Settings["VolumeSoundEffects"]);
		GlobalAudio.Instance.ChangeVolumeSoundEffects(GlobalVolumeSoundEffects);
		var SoundEffectsScrollBar = GetNode<HScrollBar>("BackgroundSettings/Sound/SoundEffectsScrollBar");
		SoundEffectsScrollBar.Value = GlobalVolumeSoundEffects;

	}
	
	private void SaveSettingsGame(Dictionary<string, string> Settings, string NameSetting, string ValueSetting)
	{
		string NewSettings = ""; 
		foreach (var Setting in Settings)
		{
			if (Setting.Key == NameSetting)
			{
				string key = Setting.Key;
				string value = ValueSetting;
				NewSettings += $"{key}={value}\n";
			}
			else
			{
				string key = Setting.Key;
				string value = Setting.Value;
				NewSettings += $"{key}={value}\n";
			}
		}
		NewSettings = NewSettings.TrimEnd('\n');
        var settingsFile = FileAccess.Open("res://Save//SaveSettngsGame.txt", FileAccess.ModeFlags.Write);
        settingsFile.StoreString(NewSettings);
        settingsFile.Close();
	} 
}
