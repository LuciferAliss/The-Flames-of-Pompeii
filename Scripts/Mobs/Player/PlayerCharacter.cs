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
	public abstract void Hit();
	public abstract void Death();
	public abstract void Regeneration();
	public abstract void Power();
	public abstract void Acceleration();
}

class MoveStatePlayer : StatePlayer
{
	public MoveStatePlayer(PlayerCharacter player) : base(player) { }
    public override void Move()
    {
		Vector2 velocity = player.Velocity;

		if (!player.IsOnFloor())
		{
			velocity += player.GetGravity() * (float)player.deltaPlayer;
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
			player.DamageBox.RotationDegrees = 180;
		} 
		else if (Direction == 1)
		{
			player.Animated.FlipH = false;
			player.DamageBox.RotationDegrees = 0;
		}

		if (velocity.Y > 0)
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

	public override void Hit()
	{
	}

	public override void Death()
	{
	}

    public override void Regeneration()
    {

		if (Input.IsActionJustPressed("AbilityRegeneration") && player.GetNode<hud>("HUD").CooldownAbility[0] == false)
		{
			player.ChangeState(new RegenerationState(player));
		}
    }

	public override void Power()
    {
		if (Input.IsActionJustPressed("PowerAbility") && player.GetNode<hud>("HUD").CooldownAbility[1] == false && player.GetNode<hud>("HUD").useAbility[0] == false)
		{
			player.ChangeState(new PowerState(player));
		}
	}

	public override void Acceleration()
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
			velocity += player.GetGravity() * (float)player.deltaPlayer;
		}

		velocity.X = 0;
		
		if(player.Animated.FlipH)
		{

			velocity.X += -20;
		} 
		else 
		{
			velocity.X += 20;
		}
		
		if (Input.IsActionJustPressed("Attack"))
		{
			player.expectation.InputButtonAttack1();
			return;
		}

		player.DamageMultiplier = 1.2f;
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

	public override void Hit()
	{
	}

	public override void Death()
	{
	}

	public override void Regeneration()
    {
    }

	public override void Power()
    {
	}

	public override void Acceleration()
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
			velocity += player.GetGravity() * (float)player.deltaPlayer;
		}

		velocity.X = 0;
		
		if(player.Animated.FlipH)
		{
			velocity.X += -20;
		} 
		else 
		{
			velocity.X += 20;
		}

		if (Input.IsActionJustPressed("Attack"))
		{
			player.expectation.InputButtonAttack2();
			return;
		}

		player.DamageMultiplier = 1.5f;
		player.animatedPlayer.Play("Attack2");
		
		player.Velocity = velocity;
		player.MoveAndSlide();
	}

	public override void Attack3()
	{
	}

	public override void Hit()
	{
	}

	public override void Death()
	{
	}

	public override void Regeneration()
    {
    }

	public override void Power()
    {
	}

	public override void Acceleration()
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
			velocity += player.GetGravity() * (float)player.deltaPlayer;
		}

		velocity.X = 0;
		
		if(player.Animated.FlipH)
		{
			velocity.X += -20;
		} 
		else 
		{
			velocity.X += 20;
		}

		player.DamageMultiplier = 2f;
		player.animatedPlayer.Play("Attack3");

		player.Velocity = velocity;
		player.MoveAndSlide();
	}

	public override void Hit()
	{
	}

	public override void Death()
	{
	}

	public override void Regeneration()
    {
    }

	public override void Power()
    {
	}

	public override void Acceleration()
	{
	}
}

class HitStatePlayer : StatePlayer
{
	public HitStatePlayer(PlayerCharacter player) : base(player) { }

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
	}

	public override void Hit()
	{	
		player.animatedPlayer.Play("Hit");
	}

	public override void Death()
	{
	}

	public override void Regeneration()
    {
    }

	public override void Power()
    {
	}

	public override void Acceleration()
	{
	}
}

class DeathStatePlayer : StatePlayer
{
	public DeathStatePlayer(PlayerCharacter player) : base(player) { }

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
	}

	public override void Hit()
	{
	}

	public override void Death()
	{
		Vector2 velocity = player.Velocity;
		velocity.X = 0; 

		if (!player.IsOnFloor())
		{
			velocity += player.GetGravity() * (float)player.deltaPlayer;
		}

		player.Velocity = velocity;
		player.MoveAndSlide();

		player.animatedPlayer.Play("Death");
	}

	public override void Regeneration()
    {
    }

	public override void Power()
    {
	}

	public override void Acceleration()
	{
	}
}

class RegenerationState : StatePlayer
{
	public RegenerationState(PlayerCharacter player) : base(player) { }

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
	}

	public override void Hit()
	{
	}

	public override void Death()
	{
	}

	public override void Regeneration()
    {
		player.animatedPlayer.Play("AbilityRegeneration");
		Signals.Instance.EmitRegenerationHealthAbility();
	}

	public override void Power()
    {
	}

	public override void Acceleration()
	{
	}
}

class PowerState : StatePlayer
{
	public PowerState(PlayerCharacter player) : base(player) { }

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
	}

	public override void Hit()
	{
	}

	public override void Death()
	{
	}

	public override void Regeneration()
    {
	}

	public override void Power()
    {
		player.animatedPlayer.Play("PowerAbility");
		Signals.Instance.EmitPowerAbility();
	}

	public override void Acceleration()
	{
	}
}

class AccelerationState : StatePlayer
{
	public AccelerationState(PlayerCharacter player) : base(player) { }

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
	}

	public override void Hit()
	{
	}

	public override void Death()
	{
	}

	public override void Regeneration()
    {
	}

	public override void Power()
    {
	}

	public override void Acceleration()
	{
	}
}

public partial class PlayerCharacter : CharacterBody2D
{
	[Export] public float Speed { get; private set; } = 300.0f;
	[Export] public float Health { get; private set; } = 100f;
	[Export] public int DamageBasic { get; private set; } = 10;
	public float DamageMultiplier = 1;
	public float DamageCurrent = 0;
	public const float JumpVelocity = -450.0f;
	public double deltaPlayer { get; private set; }
	public static CharacterBody2D Instance { get; private set; }
	public AnimatedSprite2D Animated { get; private set; }
	public AnimationPlayer animatedPlayer { get; private set; }
	public StatePlayer state { get; private set; }
	public bool combo1 { get; set; } = false;
	public bool combo2 { get; set; } = false;
	public Expectation expectation = new();
	public Node2D DamageBox;

    public override void _Ready()
    {
		Instance = this;
        Animated = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		DamageBox = GetNode<Node2D>("AttackDirection/DamageBox");
		Input.MouseMode = Input.MouseModeEnum.Hidden;
		animatedPlayer.AnimationFinished += FinishedAnimation;
		Signals.Instance.EnemyAttack += OnDamageReceived;
		Signals.Instance.PlayerRegenerationHealth += Regeneration;
		ChangeState(new MoveStatePlayer(this));
		Signals.Instance.EmitPlayerHealthChanged(Health);
		
	}

    public override void _PhysicsProcess(double delta)
	{
		deltaPlayer = delta; 
		state.Move();
    	state.Attack1();
		state.Attack2();
		state.Attack3();
		state.Hit();
		state.Death();
		state.Regeneration();
		state.Power();
		Signals.Instance.EmitPositionUpdate(Position);
		DamageCurrent = DamageBasic * DamageMultiplier;
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
			return;
		}
		else if (NameAnime == "Attack3")
		{
			combo2 = false;
			expectation.NotInputButtonAttack2();
			ChangeState(new MoveStatePlayer(this));
			return;
		}
		else if (NameAnime == "Death")
		{
			Signals.Instance.EmitKillLevel();
			return;
		}
		ChangeState(new MoveStatePlayer(this));
	}
	
	public void OnDamageReceived(float damage)
	{
		Health -= damage;
		Signals.Instance.EmitPlayerHealthChanged(Health);
		if (Health <= 0)
		{
			ChangeState(new DeathStatePlayer(this));
		}
		else
		{
			ChangeState(new HitStatePlayer(this));
		}
	}

	public void OnHitBox(Area2D Mobs)
	{
		if(GetNode<hud>("HUD").useAbility[0])
		{
			DamageCurrent += DamageCurrent / 100 * 30;
		}
		Signals.Instance.EmitPlayerAttack(DamageCurrent);
	}

	public void Regeneration(double HP)
	{
		Health = (float)HP;
	}
}