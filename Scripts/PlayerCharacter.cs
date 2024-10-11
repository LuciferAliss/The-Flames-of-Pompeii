using Godot;
using System;

public partial class PlayerCharacter : CharacterBody2D, ObjectMove
{
	[Export] public float Speed = 300.0f;
	[Export] public int Health = 100;
	public const float JumpVelocity = -450.0f;
	public static CharacterBody2D Instance { get; private set; }
	private AnimatedSprite2D Animated;

    public override void _Ready()
    {
		Instance = this;
        Animated = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		Input.MouseMode = Input.MouseModeEnum.Hidden;
    }

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
