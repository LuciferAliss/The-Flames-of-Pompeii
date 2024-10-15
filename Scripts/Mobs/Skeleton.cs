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
			skeleton.Animated.Play("Run");
 		}
		else 
		{
			velocity.X = 0;	
			skeleton.Animated.Play("Idle");
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
        // if(skeleton.checkDamage)
		// {
			
		// 	skeleton.checkDamage = false;
		// }
    }

    public override void Dead()
    {
        throw new NotImplementedException();
    }
}

// class HitStateSkeleton : Skeleton
// {
// 	public MoveStateSkeleton(Skeleton skeleton) : base(skeleton) { }
	
// 	public override void Move()
//     {
//     }

//     public override void Attack()
//     {
//         throw new NotImplementedException();
//     }

//     public override void Hit()
//     {
//     }

//     public override void Dead()
//     {
//         throw new NotImplementedException();
//     }
// }

public partial class Skeleton : Mobs, ObjectMove
{
	public bool chase = false;
    public StateMobs stateMob;
    public double delta;
    public Vector2 playerPosition;
	public bool checkDamage = false; 

	public override void _Ready()
	{
		Animated = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		stateMob = new MoveStateSkeleton(this);
		Signals.Instance.PlayerPositionUpdate += SetPlayerPosition;
		Signals.Instance.EnemyHurAttack += OnDamageReceived;
		Speed = 150;
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

	public void OnDamageReceived(int Damage)
	{
		checkDamage = true;
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
}
