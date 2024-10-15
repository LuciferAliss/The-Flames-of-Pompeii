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

public abstract partial class Mobs : CharacterBody2D
{
	public int Health;
    public float Speed;
    public AnimatedSprite2D Animated;
    public AnimationPlayer animationPlayer;
}
