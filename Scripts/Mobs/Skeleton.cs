using Godot;
using System;

public partial class Skeleton : Mob, ObjectMove
{
	CharacterBody2D player;
	private AnimatedSprite2D Animated;
	bool chase = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Animated = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		player = PlayerCharacter.Instance;
		Speed = 150;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
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
			Animated.Play("Run");
 		}
		else 
		{
			velocity.X = 0;	
			Animated.Play("Idle");
		}
		
		if(Direction.X < 0)
		{
			Animated.FlipH = true;
		} 
		else
		{
			Animated.FlipH = false;
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
