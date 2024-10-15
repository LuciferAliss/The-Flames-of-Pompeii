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
    public abstract void Dead();
}

class MoveStateSkeleton : StateMobs
{
	public MoveStateSkeleton(Skeleton skeleton) : base(skeleton) { }

    public override void Move()
    {
        Vector2 velocity = skeleton.Velocity;

		if (!skeleton.IsOnFloor())
		{
			velocity += skeleton.GetGravity() * (float)skeleton.delta;
		}

		var Direction = (skeleton.playerPosition - skeleton.Position).Normalized();

		if (skeleton.chase && Math.Abs(skeleton.playerPosition.X - skeleton.Position.X) > 100)
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
		} 
		else
		{
			skeleton.Animated.FlipH = false;
		}

		skeleton.Velocity = velocity;
		skeleton.MoveAndSlide();
    }

    public override void Attack()
    {
        throw new NotImplementedException();
    }

    public override void Hit()
    {
        if(skeleton.checkDamage)
		{
			skeleton.checkDamage = false;
		}
    }

    public override void Dead()
    {
        throw new NotImplementedException();
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
		skeleton.ChangeState(new MoveStateSkeleton(skeleton));
    }

    public override void Dead()
    {
    }
}

public partial class Skeleton : Mobs, ObjectMove
{
	public bool chase = false;
    public StateMobs stateMob;
    public double delta;
    public Vector2 playerPosition;
	public bool checkDamage = false; 
	int Damage = 20;

	public override void _Ready()
	{
		Animated = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		stateMob = new MoveStateSkeleton(this);
		Signals.Instance.PlayerPositionUpdate += SetPlayerPosition;
		Speed = 150;
		Health = 100;
	}
	public override void _PhysicsProcess(double delta)
	{
		this.delta = delta;
		Move(delta);
	}

	public void ChangeState(StateMobs newState) 
    {
        stateMob = newState;
    }

	public void SetPlayerPosition(Vector2 newPlayerPosition)
	{
		playerPosition = newPlayerPosition;
	}

	public void Move(double delta)
	{
		stateMob.Move();
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
}
