using Godot;
using System;

public partial class Signals : Node
{
	[Signal]
    public delegate void PlayerPositionUpdateEventHandler(Vector2 newPosition);
	[Signal]
	public delegate void EnemyAttackEventHandler(float enemyDamage);
	[Signal]
	public delegate void PlayerAttackEventHandler(float playerDamage);
	[Signal]
	public delegate void PlayerHealthChangedEventHandler(float HP);
	[Signal]
	public delegate void KillLevelEventHandler();
	[Signal]
	public delegate void PlayerRegenerationHealthEventHandler(double HP);
	[Signal]
	public delegate void RegenerationHealthAbilityEventHandler();
	[Signal]
	public delegate void PowerAbilityEventHandler();
	[Signal]
	public delegate void AccelerationAbilityEventHandler();
	[Signal]
	public delegate void VampirismAbilityEventHandler();
	[Signal]
	public delegate void DeathPlayerEventHandler();
	[Signal]
	public delegate void FinishLevelEventHandler();

	public static Signals Instance { get; private set; }

	public override void _Ready()
	{
		Instance = this;
	}

    public void EmitPositionUpdate(Vector2 position)
    {
        EmitSignal(nameof(PlayerPositionUpdate), position);
    }

	public void EmitEnemyAttack(float enemyDamage)
    {
        EmitSignal(nameof(EnemyAttack), enemyDamage);
    }

	public void EmitPlayerAttack(float playerDamage)
    {
        EmitSignal(nameof(PlayerAttack), playerDamage);
    }

	public void EmitPlayerHealthChanged(float HP)
    {
        EmitSignal(nameof(PlayerHealthChanged), HP);
    }

	public void EmitKillLevel()
    {
        EmitSignal(nameof(KillLevel));
    }

	public void EmitPlayerRegenerationHealth(double HP)
	{
		EmitSignal(nameof(PlayerRegenerationHealth), HP);
	}

	public void EmitRegenerationHealthAbility()
	{
		EmitSignal(nameof(RegenerationHealthAbility));
	}

	public void EmitPowerAbility()
	{
		EmitSignal(nameof(PowerAbility));
	}

	public void EmitAccelerationAbility()
	{
		EmitSignal(nameof(AccelerationAbility));
	}

	public void EmitVampirismAbility()
	{
		EmitSignal(nameof(VampirismAbility));
	}

	public void EmitDeathPlayer()
	{
		EmitSignal(nameof(DeathPlayer));
	}

    public void EmitFinishLevel()
    {
        EmitSignal(nameof(FinishLevel));
    }
}