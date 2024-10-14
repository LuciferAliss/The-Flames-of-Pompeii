using Godot;
using System;
using System.Reflection;

public partial class Skeleton : Mob, ObjectMove
{
	bool chase = false;
	Vector2 playerPosition;

	public override void _Ready()
	{
		Animated = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		Signals.Instance.PlayerPositionUpdate += SetPlayerPosition;
		Speed = 150;
	}
	public override void _PhysicsProcess(double delta)
	{
		Move(delta);
	}

	public void SetPlayerPosition(Vector2 newPlayerPosition)
	{
		playerPosition = newPlayerPosition;
	}

	public void Move(double delta)
	{
		Vector2 velocity = Velocity;

		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		var Direction = (playerPosition - this.Position).Normalized();

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
		chase = true;
	}

	public void NoDetectPlayer(Node2D player)
	{
		chase = false;
	}
}
