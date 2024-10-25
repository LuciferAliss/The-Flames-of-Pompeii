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
			velocity.X = Direction.X * skeleton.GetSpeed();
			skeleton.ChangeAnimate("Run");
 		}
		else 
		{
			velocity.X = 0;	
			skeleton.ChangeAnimate("Idle");
		}
		
		if(Direction.X < 0)
		{
			
			skeleton.ChangeFlipH(true);
			skeleton.AttackDirection.RotationDegrees = 180;
		} 
		else
		{
			skeleton.ChangeFlipH(false);
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
		skeleton.ChangeAnimate("Attack");
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
		skeleton.ChangeAnimate("Hit");
		
		var velocity = skeleton.Velocity;
		velocity.X = 0;

		if(skeleton.GetFlipH())
		{
			velocity.X += 20;
		}
		else
		{
			velocity.X += -20;
		}

		var tween = skeleton.GetTree().CreateTween();
		tween.Parallel().TweenProperty(skeleton, "velocity", Vector2.Zero, 0.6);

		skeleton.Velocity = velocity;
		skeleton.MoveAndSlide();
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
		skeleton.ChangeAnimate("Death");
    }
}

public partial class Skeleton : Mobs, IMove, IAttack
{
	public bool chase = false;
    private StateMobs stateMob;
    public double skeletonDelta { get; private set; }
    public Vector2 playerPosition;
	public bool checkDamage = false; 
	float Damage = 20f;
	public Node2D AttackDirection;

	public override void _Ready()
	{
		animated = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		stateMob = new MoveStateSkeleton(this);
		Signals.Instance.PlayerPositionUpdate += SetPlayerPosition;
		Signals.Instance.PlayerAttack += OnDamageReceived;
		Speed = 150;
		Health = 100;
		AttackDirection = GetNode<Node2D>("AttackDirection");
		GetNode<TextureProgressBar>("MobHealth/HealthBar").Value = Health;
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
		if (IsPlayerInRange())
    	{
			Health -= playerDamage;
			var HealthBar = GetNode<TextureProgressBar>("MobHealth/HealthBar");
			var TextDamage = GetNode<Label>("MobHealth/DamageText");
			var AnimeText = GetNode<AnimationPlayer>("MobHealth/Animation");
			HealthBar.Value -= playerDamage;
			HealthBar.Visible = true;
			TextDamage.Visible = true;
			TextDamage.Text = playerDamage.ToString();
			AnimeText.Play("TextDamage");
			GD.Print(Health);
			if (Health <= 0)
			{
				ChangeState(new DeathStateSkeleton(this));
			}
			else 
			{
				if (stateMob is HitStateSkeleton)
				{
					animationPlayer.Stop();	
				}
				ChangeState(new HitStateSkeleton(this));
			}
		}
	}

	private bool IsPlayerInRange()
	{
		Area2D AttackRange = GetNode<Area2D>("AttackDirection/AttackRange");
		return AttackRange.OverlapsBody(PlayerCharacter.Instance);
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
		if (stateMob is DeathStateSkeleton) return;
		ChangeState(new AttackStateSkeleton(this));
	}

	public void FinishedAnimation(StringName NameAnime)
	{
		if(NameAnime == "Death")
		{
			QueueFree();
			return;
		}
		
		Area2D AttackRange = GetNode<Area2D>("AttackDirection/AttackRange");
		if (AttackRange.OverlapsBody(PlayerCharacter.Instance))
		{
			ChangeState(new AttackStateSkeleton(this));
			return;
		}

		ChangeState(new MoveStateSkeleton(this));
	}

	public override void _ExitTree()
	{
		Signals.Instance.PlayerPositionUpdate -= SetPlayerPosition;
		Signals.Instance.PlayerAttack -= OnDamageReceived;
	}
}
