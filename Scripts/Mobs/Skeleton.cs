using Godot;
using System;

public partial class Skeleton : Mob, ObjectMove
{
	CharacterBody2D player;
	bool chase = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = PlayerCharacter.Instance;
		Speed = 200;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Move(delta);
	}

	public void Move(double delta)
	{
		Vector2 velocity = Velocity;

		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		var Direction = (player.Position - this.Position).Normalized();

		if (chase)
		{
			velocity.X = Direction.X * Speed;
 		}
		else 
		{
			velocity.X = 0;	
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	public void DetectPlayer(Node2D player)
	{
		if (player.Name == "PlayerCharacter")
		{
			chase = true;
		}
	}

	public void NoDetectPlayer(Node2D player)
	{
		if (player.Name == "PlayerCharacter")
		{
			chase = false;
		}
	}
}
