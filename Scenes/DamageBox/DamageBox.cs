using Godot;
using System;

public partial class DamageBox : Node2D
{
    public override void _Ready()
    {
        GetNode<CollisionShape2D>("HitBox/CollisionShape2D").Disabled = true;
    }
}
