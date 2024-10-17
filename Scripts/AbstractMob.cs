using Godot;
using System;

interface IMove
{
    void Move();
}

interface IAttack
{
    void Attack();
}

public abstract partial class Mobs : CharacterBody2D
{
	public float Health;
    public float Speed;
    public AnimatedSprite2D Animated;
    public AnimationPlayer animationPlayer;
}
