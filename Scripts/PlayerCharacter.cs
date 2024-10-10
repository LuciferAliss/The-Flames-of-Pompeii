using Godot;
using System;

public partial class PlayerCharacter : CharacterBody2D
{
	[Export]
	public float Speed = 300.0f;
	public const float JumpVelocity = -450.0f;
	private AnimatedSprite2D Animated;

    public override void _Ready()
    {
        Animated = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		if (Input.IsActionJustPressed("Up") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
			Animated.Play("Jump");
		}

		var Direction = Input.GetAxis("Left", "Right");
		if (Direction == 1 || Direction == -1)
		{
			velocity.X = Direction * Speed;
			if (velocity.Y == 0)
			{
				Animated.Play("Run");
			}
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			if (velocity.Y == 0)
			{
				Animated.Play("Idle");
			}
		}

		if(Direction == -1)
		{
			Animated.FlipH = true;
		} 
		else if (Direction == 1)
		{
			Animated.FlipH = false;
		}

		if (velocity.Y > 7)
		{
			Animated.Play("Fall");
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
