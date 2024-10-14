using Godot;
using System;
using System.Threading.Tasks;

public class Expectation
{
	public bool InputAttack1 {get; private set; } = false;
	public bool InputAttack2 {get; private set; } = false;
	public bool InputAttack3 {get; private set; } = false;
	
	public void InputButtonAttack1()
	{
		InputAttack1 = true;
	}    
	
	public void NotInputButtonAttack1()
	{
		InputAttack1 = false;
	}  

	public void InputButtonAttack2()
	{
		InputAttack2 = true;
	}    
	
	public void NotInputButtonAttack2()
	{
		InputAttack2 = false;
	} 
	
	public void InputButtonAttack3()
	{
		InputAttack3 = true;
	}    
	
	public void NotInputButtonAttack3()
	{
		InputAttack3 = false;
	} 
}

public abstract class StatePlayer
{
	protected PlayerCharacter player;

	public StatePlayer(PlayerCharacter player) 
	{
		this.player = player;
	}

	public abstract void Move();
	public abstract void Attack1();
	public abstract void Attack2();
	public abstract void Attack3();
}

class MoveStatePlayer : StatePlayer
{
	public MoveStatePlayer(PlayerCharacter player) : base(player) { }
    public override void Move()
    {
		Vector2 velocity = player.Velocity;

		if (!player.IsOnFloor())
		{
			velocity += player.GetGravity() * (float)player.delta;
		}


        if (Input.IsActionJustPressed("Up") && player.IsOnFloor())
		{
			velocity.Y = PlayerCharacter.JumpVelocity;
			player.animatedPlayer.Play("Jump");
		}

		var Direction = Input.GetAxis("Left", "Right");
		if (Direction == 1 || Direction == -1)
		{
			velocity.X = Direction * player.Speed;
			if (velocity.Y == 0)
			{
				player.animatedPlayer.Play("Run");
			}
		}
		else
		{
			velocity.X = Mathf.MoveToward(player.Velocity.X, 0, player.Speed);
			if (velocity.Y == 0)
			{
				player.animatedPlayer.Play("Idle");
			}
		}

		if(Direction == -1)
		{
			player.Animated.FlipH = true;
		} 
		else if (Direction == 1)
		{
			player.Animated.FlipH = false;
		}

		if (velocity.Y > 7)
		{
			player.animatedPlayer.Play("Fall");
		}

		player.Velocity = velocity;
		player.MoveAndSlide();
    }

	public override void Attack1()
	{
		if (Input.IsActionJustPressed("Attack"))
		{
			player.ChangeState(new Attack1StatePlayer(player));
		}
	}

	public override void Attack2()
	{
	}

	public override void Attack3()
	{
	}
}

class Attack1StatePlayer : StatePlayer
{
	public Attack1StatePlayer(PlayerCharacter player) : base(player) { }
   
    public override void Move()
    {
	}

    public override void Attack1()
    { 
		Vector2 velocity = player.Velocity;
		if (!player.IsOnFloor())
		{
			velocity += player.GetGravity() * (float)player.delta;
		}

		velocity.X = 0;
		
		if(player.Animated.FlipH)
		{

			velocity.X += -40;
		} 
		else 
		{
			velocity.X += 40;
		}
		
		if (Input.IsActionJustPressed("Attack"))
		{
			player.expectation.InputButtonAttack1();
			return;
		}

		player.animatedPlayer.Play("Attack1");

		player.Velocity = velocity;
		player.MoveAndSlide();
	}

	public override void Attack2()
	{
	}

	public override void Attack3()
	{
	}
}

class Attack2StatePlayer : StatePlayer
{
	public Attack2StatePlayer(PlayerCharacter player) : base(player) { }
   
    public override void Move()
    {
	}

    public override void Attack1()
    { 
	}

	public override void Attack2()
	{
		Vector2 velocity = player.Velocity;
		if (!player.IsOnFloor())
		{
			velocity += player.GetGravity() * (float)player.delta;
		}

		velocity.X = 0;
		
		if(player.Animated.FlipH)
		{
			velocity.X += -40;
		} 
		else 
		{
			velocity.X += 40;
		}

		if (Input.IsActionJustPressed("Attack"))
		{
			player.expectation.InputButtonAttack2();
			return;
		}

		player.animatedPlayer.Play("Attack2");
		
		player.Velocity = velocity;
		player.MoveAndSlide();
	}

	public override void Attack3()
	{
	}
}

class Attack3StatePlayer : StatePlayer
{
	public Attack3StatePlayer(PlayerCharacter player) : base(player) { }

	public override void Move()
    {
	}

    public override void Attack1()
    { 
	}

	public override void Attack2()
	{
	}

	public override void Attack3()
	{
		Vector2 velocity = player.Velocity;
		if (!player.IsOnFloor())
		{
			velocity += player.GetGravity() * (float)player.delta;
		}

		velocity.X = 0;
		
		if(player.Animated.FlipH)
		{
			velocity.X += -40;
		} 
		else 
		{
			velocity.X += 40;
		}

		player.animatedPlayer.Play("Attack3");

		player.Velocity = velocity;
		player.MoveAndSlide();
	}
}

public partial class PlayerCharacter : CharacterBody2D
{
	[Export] public float Speed = 300.0f;
	[Export] public int Health = 100;
	public const float JumpVelocity = -450.0f;
	public double delta { get; private set; }
	public static CharacterBody2D Instance { get; private set; }
	public AnimatedSprite2D Animated { get; private set; }
	public AnimationPlayer animatedPlayer { get; private set; }
	StatePlayer state;
	public bool combo1 {get; set; } = false;
	public bool combo2 {get; set; } = false;
	public Expectation expectation = new();


    public override void _Ready()
    {
		Instance = this;
        Animated = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		Input.MouseMode = Input.MouseModeEnum.Hidden;
		animatedPlayer.AnimationFinished += FinishedAnimation;

		state = new MoveStatePlayer(this);
	}

    public override void _PhysicsProcess(double delta)
	{
		this.delta = delta; 
		state.Move();
    	state.Attack1();
		state.Attack2();
		state.Attack3();
		Signals.Instance.EmitPositionUpdate(Position);
	}	

	public void ChangeState(StatePlayer newState)
    {
        state = newState;
    }

	public void Combo1()
	{
		combo1 = true;
	}

	public void Combo2()
	{
		combo2 = true;
	}

	public void FinishedAnimation(StringName NameAnime)
	{
		if (NameAnime == "Attack1")
		{
			if(combo1 && expectation.InputAttack1)
			{
				ChangeState(new Attack2StatePlayer(this));
				return;
			}
			combo1 = false;
			ChangeState(new MoveStatePlayer(this));
		}
		else if (NameAnime == "Attack2")
		{
			combo1 = false;
			expectation.NotInputButtonAttack1();
			if(combo2 && expectation.InputAttack2)
			{
				ChangeState(new Attack3StatePlayer(this));
				return;
			}
			combo2 = false;
			ChangeState(new MoveStatePlayer(this));
		}
		else if (NameAnime == "Attack3")
		{
			combo2 = false;
			expectation.NotInputButtonAttack2();
			ChangeState(new MoveStatePlayer(this));
		}
	}
}
