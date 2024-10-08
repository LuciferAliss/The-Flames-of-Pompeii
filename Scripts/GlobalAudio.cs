using Godot;
using System;
using System.Collections.Generic;

public partial class GlobalAudio : Node
{
	public static GlobalAudio Instance { get; private set; }
	private AudioStreamPlayer music;  
	private AudioStreamPlayer soundEffects;  
	private Dictionary<string, string> linkNameSoundEffects;

	public override void _Ready()
	{
		Instance = this;
		music = GetNode<AudioStreamPlayer>("Music");
		soundEffects = GetNode<AudioStreamPlayer>("SoundEffects");
		linkNameSoundEffects = new Dictionary<string, string>()
		{
			{"ButtonMapping", "res://Music//MainMenuANDSettings//ButtonMapping.mp3"},
			{"ButtonPressed", "res://Music//MainMenuANDSettings//ButtonPressed.mp3"},
		};
	}

	public override void _Process(double delta)
	{
	}

	public void ChangeVolumeMusic(float volume)
	{
		AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Music"), volume);
	}

	public void ChangeVolumeSoundEffects(float volume)
	{
		AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("SoundEffects"), volume);
	}

	public void PlaySoundEffects(string NameSoundEffects)
	{
		var audioStream = (AudioStream)GD.Load(linkNameSoundEffects[NameSoundEffects]);
        soundEffects.Stream = audioStream;
        soundEffects.Play();
	}
}
