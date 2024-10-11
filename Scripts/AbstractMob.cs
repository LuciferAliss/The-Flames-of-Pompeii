using Godot;
using System;

interface ObjectMove
{
    void Move(double delta);
}

interface ObjectTakeDamage
{
    void TakeDamage();
    void Die();
}

interface ObjectAttack
{
    void Attack();
}

public abstract partial class Mob : CharacterBody2D
{
	protected int Health;
    protected float Speed;
    AnimatableBody2D Animated;
}
