using Godot;
using System;

namespace MapGame
{
	public partial class ResourceManager : Node
	{
		[Export] public float _startingMagic = 10000;
		[Export] public float _startingHappiness = 100; // Percentage 0-100%
		[Export] public float _startingTokens = 3;
		[Export] public float _startingSalary = 1000;
		[Export] public float _startingMagicMultiplier = 1;
		[Export] public float _startingHappinessMultiplier = 1;

		public static ResourceManager Instance
		{
			get;
			private set;
		}

		public void ResetResources()
		{
			SetMagic(_startingMagic);
			SetHappiness(_startingHappiness);
			SetTokens(_startingTokens);
			SetSalary(_startingSalary);
			SetMagicMultiplier(_startingMagicMultiplier);
			SetHappinessMultiplier(_startingHappinessMultiplier);
		}

		public void SetMagic(float targetAmount)
		{
			SaveData.Instance._currentMagic = targetAmount;
		}
		public void SetHappiness(float targetAmount)
		{
			SaveData.Instance._currentHappiness = targetAmount;
		}
		public void SetTokens(float targetAmount)
		{
			SaveData.Instance._currentTokens = targetAmount;
		}
		public void SetSalary(float targetAmount)
		{
			SaveData.Instance._currentSalary = targetAmount;
		}
		public void SetMagicMultiplier(float targetAmount)
		{
			SaveData.Instance._currentMagicMultiplier = targetAmount;
		}
		public void SetHappinessMultiplier(float targetAmount)
		{
			SaveData.Instance._currentHappinessMultiplier = targetAmount;
		}


		public void ChangeMagic(float change)
		{
			if (SaveData.Instance._currentMagic + change < 0)
			{
				SaveData.Instance._currentMagic = 0;
				return;
			}
			SaveData.Instance._currentMagic += change;
		}
		public void ChangeHappiness(float change)
		{
			if (SaveData.Instance._currentHappiness + change > 100)
			{
				SaveData.Instance._currentHappiness = 100;
				return;
			}
			else if (SaveData.Instance._currentHappiness + change < 0)
			{
				SaveData.Instance._currentHappiness = 0;
			}
			SaveData.Instance._currentHappiness += change;
		}
		public void ChangeTokens(float change)
		{
			if (SaveData.Instance._currentTokens + change < 0)
			{
				SaveData.Instance._currentTokens = 0;
				return;
			}
			SaveData.Instance._currentTokens += change;
		}
		public void ChangeSalary(float change)
		{
			SaveData.Instance._currentSalary += change;
		}
		public void ChangeMagicMultiplier(float change)
		{
			SaveData.Instance._currentMagicMultiplier += change;
		}
		public void ChangeHappinessMultiplier(float change)
		{
			SaveData.Instance._currentHappinessMultiplier += change;
		}


		public void HandleOptionOutcomes(EventOutcomeData outcome)
		{
			if (outcome.HappinessChange != 0)
			{
				ChangeHappiness(outcome.HappinessChange);
			}

			if (outcome.MagicChange != 0)
			{
				ChangeMagic(outcome.MagicChange);
			}

			if (outcome.TokensChange != 0)
			{
				ChangeTokens(outcome.TokensChange);
			}
			else if (outcome.TokensChange < 0)

			if (outcome.HappinessMultiplier != 0)
			{
				ChangeHappinessMultiplier(outcome.HappinessMultiplier);
			}

			if (outcome.MagicMultiplier != 0)
			{
				ChangeMagicMultiplier(outcome.MagicMultiplier);
			}
			SaveData.Instance.SaveGame();
		}
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			Instance = this;

		}

	}
}
