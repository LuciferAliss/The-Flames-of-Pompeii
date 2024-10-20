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
	protected float Health;
    protected float Speed;
    protected AnimatedSprite2D animated;
    protected AnimationPlayer animationPlayer;

    public void ChangeAnimate(string anim)
    {
        animationPlayer.Play(anim);
    }

    public void ChangeFlip(bool check)
    {
        animated.FlipH = check;
    }

    public float GetSpeed()
    {
        return Speed;
    }
}
