using Godot;
using System;
using System.Reflection;

public abstract class StateMobs
{
	protected Skeleton skeleton;

	public StateMobs(Skeleton skeleton) 
	{
		this.skeleton = skeleton;
	}

	public abstract void Move();
	public abstract void Attack();
    public abstract void Hit();
    public abstract void Death();
}

class MoveStateSkeleton : StateMobs
{
	public MoveStateSkeleton(Skeleton skeleton) : base(skeleton) { }

    public override void Move()
    {
        Vector2 velocity = skeleton.Velocity;

		if (!skeleton.IsOnFloor())
		{
			velocity += skeleton.GetGravity() * (float)skeleton.skeletonDelta;
		}

		var Direction = (skeleton.playerPosition - skeleton.Position).Normalized();

		if (skeleton.chase && Math.Abs(skeleton.playerPosition.X - skeleton.Position.X) > 60)
		{
			velocity.X = Direction.X * skeleton.Speed;
			skeleton.animationPlayer.Play("Run");
 		}
		else 
		{
			velocity.X = 0;	
			skeleton.animationPlayer.Play("Idle");
		}
		
		if(Direction.X < 0)
		{
			
			skeleton.Animated.FlipH = true;
			skeleton.AttackDirection.RotationDegrees = 180;
		} 
		else
		{
			skeleton.Animated.FlipH = false;
			skeleton.AttackDirection.RotationDegrees = 0;
		}

		skeleton.Velocity = velocity;
		skeleton.MoveAndSlide();
    }

    public override void Attack()
    {
    }

    public override void Hit()
    {
        if(skeleton.checkDamage)
		{
			skeleton.checkDamage = false;
		}
    }

    public override void Death()
    {
    }
}

class AttackStateSkeleton : StateMobs
{
	public AttackStateSkeleton(Skeleton skeleton) : base(skeleton) { }
	
	public override void Move()
    {
    }

    public override void Attack()
	{
		skeleton.animationPlayer.Play("Attack");
    }

    public override void Hit()
    {
    }

    public override void Death()
    {
    }
}

class HitStateSkeleton : StateMobs
{
	public HitStateSkeleton(Skeleton skeleton) : base(skeleton) { }
	
	public override void Move()
    {
    }

    public override void Attack()
	{
    }

    public override void Hit()
    {
		skeleton.animationPlayer.Play("Hit");
    }

    public override void Death()
    {
    }
}

class DeathStateSkeleton : StateMobs
{
	public DeathStateSkeleton(Skeleton skeleton) : base(skeleton) { }
	
	public override void Move()
    {
    }

    public override void Attack()
	{
    }

    public override void Hit()
    {
    }

    public override void Death()
    {
		skeleton.animationPlayer.Play("Death");
    }
}

public partial class Skeleton : Mobs, IMove, IAttack
{
	public bool chase = false;
    public StateMobs stateMob;
    public double skeletonDelta;
    public Vector2 playerPosition;
	public bool checkDamage = false; 
	float Damage = 20f;
	public Node2D AttackDirection;

	public override void _Ready()
	{
		Animated = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		stateMob = new MoveStateSkeleton(this);
		Signals.Instance.PlayerPositionUpdate += SetPlayerPosition;
		Signals.Instance.PlayerAttack += OnDamageReceived;
		Speed = 150;
		Health = 100;
		AttackDirection = GetNode<Node2D>("AttackDirection");
	}
	public override void _PhysicsProcess(double delta)
	{
		skeletonDelta = delta;
		Move();
		Attack();
		stateMob.Hit();
		stateMob.Death();
	}

	public void ChangeState(StateMobs newState) 
    {
        stateMob = newState;
    }

	public void OnDamageReceived(float playerDamage)
	{
		Health -= playerDamage;
		if (Health <= 0)
		{
			ChangeState(new DeathStateSkeleton(this));
		}
		else 
		{
			ChangeState(new HitStateSkeleton(this));
		}
	}

	public void SetPlayerPosition(Vector2 newPlayerPosition)
	{
		playerPosition = newPlayerPosition;
	}

	public void Move()
	{
		stateMob.Move();
	}
	public void Attack()
	{
		stateMob.Attack();
	}

	public void DetectPlayer(Node2D player)
	{
		chase = true;
	}

	public void NoDetectPlayer(Node2D player)
	{
		chase = false;
	}

	public void OnHitBox(Area2D area)
	{
		Signals.Instance.EmitEnemyAttack(Damage);
	}

	public void OnAttackRange(Node2D player)
	{
		ChangeState(new AttackStateSkeleton(this));
	}

	public void FinishedAnimation(StringName NameAnime)
	{
		if(NameAnime == "Death")
		{
			QueueFree();
		}

		Area2D AttackRange = GetNode<Area2D>("AttackDirection/AttackRange");
		if (AttackRange.OverlapsBody(PlayerCharacter.Instance))
		{
			ChangeState(new AttackStateSkeleton(this));
			return;
		}

		ChangeState(new MoveStateSkeleton(this));
	}
}
