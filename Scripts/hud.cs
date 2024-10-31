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
		Signals.Instance.AccelerationAbility += Acceleration;
		Signals.Instance.VampirismAbility += Vampirism; 
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
		UseAcceleration(delta);
		CooldownAcceleration(delta);
		UseVampirism(delta);
		CooldownVampirism(delta);
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

			while (HealthBar.Value < targetHP) // в отдельный метод надо будет пихнуть
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
			var RegenerationLabel = GetNode<Label>("Control/AbilityContainer/HBoxContainerAbility/VBoxContainerRegen/RegenerationLabel");
			RegenerationLabel.Modulate = new Color(1, 1, 1, 1);

			currentCooldown[0] -= (float)delta;
			RegenerationLabel.Text = ((int) currentCooldown[0]).ToString();

			if (currentCooldown[0] <= 0)
			{
				CooldownAbility[0] = false;
				AbilityRegeneration.Modulate = new Color(0.2f, 0.2f, 0.2f);
				RegenerationLabel.Modulate = new Color(1, 1, 1, 0);
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
			var PowerLabel = GetNode<Label>("Control/AbilityContainer/HBoxContainerAbility/VBoxContainerPower/PowerLabel");
			PowerLabel.Modulate = new Color(1, 0.5f, 0, 1);

			currentTimeUseAbility[0] -= (float)delta;
			PowerLabel.Text = ((int) currentTimeUseAbility[0]).ToString();

			if (currentTimeUseAbility[0] <= 0)
			{
				useAbility[0] = false;
				CooldownAbility[1] = true;
				currentCooldown[1] = cooldownTime[1];
				AbilityPower.Modulate = new Color(0.2f, 0.2f, 0.2f);
				PowerLabel.Modulate = new Color(1, 1, 1, 1);
			}
		}
	}

	private void CooldownPower(double delta)
	{
		if (CooldownAbility[1])
		{
			var AbilityPower = GetNode<TextureRect>("Control/AbilityContainer/HBoxContainerAbility/VBoxContainerPower/PowerAbility");
			var PowerLabel = GetNode<Label>("Control/AbilityContainer/HBoxContainerAbility/VBoxContainerPower/PowerLabel");
			PowerLabel.Modulate = new Color(1, 1, 1, 1);

			currentCooldown[1] -= (float)delta;
			PowerLabel.Text = ((int) currentCooldown[1]).ToString();

			if (currentCooldown[1] <= 0)
			{
				CooldownAbility[1] = false;
				AbilityPower.Modulate = new Color(1, 1, 1);
				PowerLabel.Modulate = new Color(1, 1, 1, 0);
			}
		}
	}

	public void Acceleration()
	{
		useAbility[1] = true;
		currentTimeUseAbility[1] = timeUseAbility[1];
		GetNode<TextureRect>("Control/AbilityContainer/HBoxContainerAbility/VBoxContainerAcceleration/AccelerationAbility").Modulate = new Color(1.7f, 1.7f, 1.7f);
	}

	private void UseAcceleration(double delta)
	{
		if (useAbility[1])
		{
			var AccelerationAbility = GetNode<TextureRect>("Control/AbilityContainer/HBoxContainerAbility/VBoxContainerAcceleration/AccelerationAbility");
			var AccelerationLabel = GetNode<Label>("Control/AbilityContainer/HBoxContainerAbility/VBoxContainerAcceleration/AccelerationLabel");
			AccelerationLabel.Modulate = new Color(0.678431f, 0.847059f, 0.901961f, 1);

			currentTimeUseAbility[1] -= (float)delta;
			AccelerationLabel.Text = ((int) currentTimeUseAbility[1]).ToString();

			if (currentTimeUseAbility[1] <= 0)
			{
				useAbility[1] = false;
				CooldownAbility[2] = true;
				currentCooldown[2] = cooldownTime[2];
				AccelerationAbility.Modulate = new Color(0.2f, 0.2f, 0.2f);
				AccelerationLabel.Modulate = new Color(1, 1, 1, 1);
			}
		}
	}

	private void CooldownAcceleration(double delta)
	{
		if (CooldownAbility[2])
		{
			var AccelerationAbility = GetNode<TextureRect>("Control/AbilityContainer/HBoxContainerAbility/VBoxContainerAcceleration/AccelerationAbility");
			var AccelerationLabel = GetNode<Label>("Control/AbilityContainer/HBoxContainerAbility/VBoxContainerAcceleration/AccelerationLabel");
			AccelerationLabel.Modulate = new Color(1, 1, 1, 1);

			currentCooldown[2] -= (float)delta;
			AccelerationLabel.Text = ((int) currentCooldown[2]).ToString();

			if (currentCooldown[2] <= 0)
			{
				CooldownAbility[2] = false;
				AccelerationAbility.Modulate = new Color(1, 1, 1);
				AccelerationLabel.Modulate = new Color(1, 1, 1, 0);
			}
		}
	}

	public void Vampirism()
	{
		useAbility[2] = true;
		currentTimeUseAbility[2] = timeUseAbility[2];
		GetNode<TextureRect>("Control/AbilityContainer/HBoxContainerAbility/VBoxContainerVampir/VampirismAbility").Modulate = new Color(1.7f, 1.7f, 1.7f);
	}

	private void UseVampirism(double delta)
	{
		if (useAbility[2])
		{
			var VampirismAbility = GetNode<TextureRect>("Control/AbilityContainer/HBoxContainerAbility/VBoxContainerVampir/VampirismAbility");
			var VampirismLabel = GetNode<Label>("Control/AbilityContainer/HBoxContainerAbility/VBoxContainerVampir/VampirismLabel");
			VampirismLabel.Modulate = new Color(0.627451f, 0.12549f, 0.941176f, 1);

			currentTimeUseAbility[2] -= (float)delta;
			VampirismLabel.Text = ((int) currentTimeUseAbility[2]).ToString();

			if (currentTimeUseAbility[2] <= 0)
			{
				useAbility[2] = false;
				CooldownAbility[3] = true;
				currentCooldown[3] = cooldownTime[3];
				VampirismAbility.Modulate = new Color(0.2f, 0.2f, 0.2f);
				VampirismLabel.Modulate = new Color(1, 1, 1, 1);
			}
		}
	}

	private void CooldownVampirism(double delta)
	{
		if (CooldownAbility[3])
		{
			var VampirismAbility = GetNode<TextureRect>("Control/AbilityContainer/HBoxContainerAbility/VBoxContainerVampir/VampirismAbility");
			var VampirismLabel = GetNode<Label>("Control/AbilityContainer/HBoxContainerAbility/VBoxContainerVampir/VampirismLabel");
			VampirismLabel.Modulate = new Color(1, 1, 1, 1);

			currentCooldown[3] -= (float)delta;
			VampirismLabel.Text = ((int) currentCooldown[3]).ToString();

			if (currentCooldown[3] <= 0)
			{
				CooldownAbility[3] = false;
				VampirismAbility.Modulate = new Color(1, 1, 1);
				VampirismLabel.Modulate = new Color(1, 1, 1, 0);
			}
		}
	}
}
