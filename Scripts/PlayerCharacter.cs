using Godot;
using System;

public partial class PlayerCharacter : CharacterBody2D
{
	[Export]
	public float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;
	private AnimatedSprite2D Animat;

    public override void _Ready()
    {
        Animat = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		
		// Handle Jump.
		if (Input.IsActionJustPressed("Up") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDirection = Input.GetVector("Left", "Right", "Up", "Down");
		if (inputDirection != Vector2.Zero)
		{
			velocity.X = inputDirection.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;

		if (velocity.X != 0)
		{
			Animat.Animation = "Run";
			Animat.FlipV = false;
			// See the note below about the following boolean assignment.
			Animat.FlipH = velocity.X < 0;
		}
		else if (velocity.Y != 0)
		{
			Animat.Animation = "Jump";
			Animat.FlipV = velocity.Y > 0;
		}

		MoveAndSlide();
	}
}
