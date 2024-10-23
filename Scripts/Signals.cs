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
}