using Godot;
using System;

public partial class Signals : Node
{
	[Signal]
    public delegate void PlayerPositionUpdateEventHandler(Vector2 newPosition);
	[Signal]
	public delegate void EnemyAttackEventHandler(int enemyDamage);
	
	public static Signals Instance { get; private set; }

	public override void _Ready()
	{
		Instance = this;
	}

    public void EmitPositionUpdate(Vector2 position)
    {
        EmitSignal(nameof(PlayerPositionUpdate), position);
    }

	public void EmitEnemyAttack(int enemyDamage)
    {
        EmitSignal(nameof(EnemyAttack), enemyDamage);
    }
}