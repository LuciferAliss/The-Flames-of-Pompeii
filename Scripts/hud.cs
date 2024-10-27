using Godot;
using System;

public partial class hud : CanvasLayer
{
	private TextureProgressBar HealthBar;
	private float[] cooldownTime = new float[] {60f, 120f, 60f, 240f};
	private float[] currentCooldown = new float[] {0f, 0f, 0f, 0f};
	public bool[] CooldownAbility = new bool[] { false, false, false, false };
	private float[] timeUseAbility = new float[] { 20f, 20f, 15f};
	private float[] currentTimeUseAbility = new float[] { 0f, 0f, 0f};
	public bool[] useAbility = new bool[] { false, false, false };
	public override void _Ready()
	{
		HealthBar = GetNode<TextureProgressBar>("Control/HealthBar");
		Signals.Instance.PlayerHealthChanged += UpdateHealthBar;
		Signals.Instance.RegenerationHealthAbility += Regeneration;
		Signals.Instance.PowerAbility += Power;
		GetNode<Label>("Control/AbilityContainer/HBoxContainerAbility/VBoxContainerRegen/RegenerationLabel").Modulate = new Color(1, 1, 1, 0);
		GetNode<Label>("Control/AbilityContainer/HBoxContainerAbility/VBoxContainerPower/PowerLabel").Modulate = new Color(1, 1, 1, 0);
		GetNode<Label>("Control/AbilityContainer/HBoxContainerAbility/VBoxContainerAcceleration/AccelerationLabel").Modulate = new Color(1, 1, 1, 0);
		GetNode<Label>("Control/AbilityContainer/HBoxContainerAbility/VBoxContainerVampir/VampirismLabel").Modulate = new Color(1, 1, 1, 0);
	}

	public override void _Process(double delta)
	{
		CooldownRegeneration(delta);
		UsePower(delta);
		CooldownPower(delta);
	}

	public override void _ExitTree()
    {
        Signals.Instance.PlayerHealthChanged -= UpdateHealthBar;
    }

	private void UpdateHealthBar(float HP)
	{
		HealthBar.Value = HP;
	}

	public async void Regeneration()
	{
		if (CooldownAbility[0] == false)
		{
			double targetHP = HealthBar.Value + 30;
			targetHP = Math.Min(targetHP, 100);
			var delay = 0.2f;
			double HP;
			
			CooldownAbility[0] = true;
			currentCooldown[0] = cooldownTime[0];
			GetNode<TextureRect>("Control/AbilityContainer/HBoxContainerAbility/VBoxContainerRegen/AbilityRegeneration").Modulate = new Color(0.2f, 0.2f, 0.2f);

			while (HealthBar.Value < targetHP) 
			{
				HP = Math.Min(HealthBar.Value + 3, targetHP);

				var tween = GetTree().CreateTween();
				tween.TweenProperty(HealthBar, "value", HP, delay);

				await ToSignal(tween, "finished");

				if (HealthBar.Value >= 100)
				{
					HealthBar.Value = 100;
					break;
				}
			}

			Signals.Instance.EmitPlayerRegenerationHealth(HealthBar.Value);
		}
	}

	private void CooldownRegeneration(double delta)
	{
		if (CooldownAbility[0])
		{
			var AbilityRegeneration = GetNode<TextureRect>("Control/AbilityContainer/HBoxContainerAbility/VBoxContainerRegen/AbilityRegeneration");
			var RegenerationLable = GetNode<Label>("Control/AbilityContainer/HBoxContainerAbility/VBoxContainerRegen/RegenerationLabel");
			RegenerationLable.Modulate = new Color(1, 1, 1, 1);

			currentCooldown[0] -= (float)delta;
			RegenerationLable.Text = ((int) currentCooldown[0]).ToString();

			if (currentCooldown[0] <= 0)
			{
				CooldownAbility[0] = false;
				AbilityRegeneration.Modulate = new Color(0.2f, 0.2f, 0.2f);
				RegenerationLable.Modulate = new Color(1, 1, 1, 0);
			}
		}
	}

	public void Power()
	{
		useAbility[0] = true;
		currentTimeUseAbility[0] = timeUseAbility[0];
		GetNode<TextureRect>("Control/AbilityContainer/HBoxContainerAbility/VBoxContainerPower/PowerAbility").Modulate = new Color(1.7f, 1.7f, 1.7f);
	}

	private void UsePower(double delta)
	{
		if (useAbility[0])
		{
			var AbilityPower = GetNode<TextureRect>("Control/AbilityContainer/HBoxContainerAbility/VBoxContainerPower/PowerAbility");
			var PowerLable = GetNode<Label>("Control/AbilityContainer/HBoxContainerAbility/VBoxContainerPower/PowerLabel");
			PowerLable.Modulate = new Color(1, 0.5f, 0, 1);

			currentTimeUseAbility[0] -= (float)delta;
			PowerLable.Text = ((int) currentTimeUseAbility[0]).ToString();

			if (currentTimeUseAbility[0] <= 0)
			{
				useAbility[0] = false;
				CooldownAbility[1] = true;
				currentCooldown[1] = cooldownTime[1];
				AbilityPower.Modulate = new Color(0.2f, 0.2f, 0.2f);
				PowerLable.Modulate = new Color(1, 1, 1, 1);
			}
		}
	}

	private void CooldownPower(double delta)
	{
		if (CooldownAbility[1])
		{
			var AbilityPower = GetNode<TextureRect>("Control/AbilityContainer/HBoxContainerAbility/VBoxContainerPower/PowerAbility");
			var PowerLable = GetNode<Label>("Control/AbilityContainer/HBoxContainerAbility/VBoxContainerPower/PowerLabel");
			PowerLable.Modulate = new Color(1, 1, 1, 1);

			currentCooldown[1] -= (float)delta;
			PowerLable.Text = ((int) currentCooldown[1]).ToString();

			if (currentCooldown[1] <= 0)
			{
				CooldownAbility[1] = false;
				AbilityPower.Modulate = new Color(1, 1, 1);
				PowerLable.Modulate = new Color(1, 1, 1, 0);
			}
		}
	}
}
