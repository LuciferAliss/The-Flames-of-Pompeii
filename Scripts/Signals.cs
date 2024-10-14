using Godot;
using System;

public partial class Signals : Node
{
	[Signal]
    public delegate void PlayerPositionUpdateEventHandler(Vector2 newPosition);
	[Signal]
	public delegate void PlayerAttackEventHandler(double PlayerDamage);
	public static Signals Instance { get; private set; }

	public override void _Ready()
	{
		Instance = this;
	}

    public void EmitPositionUpdate(Vector2 position)
    {
        EmitSignal(nameof(PlayerPositionUpdate), position);
    }
}