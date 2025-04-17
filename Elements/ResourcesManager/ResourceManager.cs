using Godot;
using System;
using System.Collections.Generic;

namespace MapGame
{
	public enum Island
	{
		Island1,
		Island2,
		Island3,
		Island4,
		Island5,
		Island6,
		Island7,
	}

	public partial class ResourceManager : Node
	{
		[Export] public float _startingMagic = 10000;
		[Export] public float _startingHappiness = 100; // Percentage 0-100%
		[Export] public float _startingTokens = 3;
		[Export] public float _startingSalary = 1000;
		[Export] public float _startingMagicMultiplier = 1;

		public static ResourceManager Instance { get; private set; }

		public void ResetResources()
		{
			SetMagic(_startingMagic);
			SetTokens(_startingTokens);
			SetSalary(_startingSalary);
			SetMagicMultiplier(_startingMagicMultiplier);
			
			// Reset happiness for all islands
			foreach (Island island in Enum.GetValues(typeof(Island)))
			{
				SetIslandHappiness(island, _startingHappiness);
			}
			
			// Set global happiness to average of all islands
			UpdateGlobalHappiness();
		}



		// Fixed missing closing parenthesis here
		public void SetIslandHappiness(Island island, float happiness)
		{
			happiness = Mathf.Clamp(happiness, 0, 100);
			SaveData.Instance._islandHappiness[island] = happiness;
			UpdateGlobalHappiness();
		}

		public float GetIslandHappiness(Island island)
		{
			if (SaveData.Instance._islandHappiness.TryGetValue(island, out float happiness))
			{
				return happiness;
			}
			return _startingHappiness;
		}


		public void ChangeIslandHappiness(Island island, float change)
		{
			float current = GetIslandHappiness(island);
			float newHappiness = Mathf.Clamp(current + change, 0, 100);
			SaveData.Instance._islandHappiness[island] = newHappiness;
			UpdateGlobalHappiness();
		}


		private void UpdateGlobalHappiness()
		{
			if (SaveData.Instance._islandHappiness.Count == 0) return;
			
			float total = 0;
			foreach (var happiness in SaveData.Instance._islandHappiness.Values)
			{
				total += happiness;
			}
			float average = total / SaveData.Instance._islandHappiness.Count;
			SetHappiness(average);
		}


		public void ChangeHappiness(int change)
		{
			float perIslandChange = change / (float)Enum.GetValues(typeof(Island)).Length;
			
			foreach (Island island in Enum.GetValues(typeof(Island)))
			{
				ChangeIslandHappiness(island, perIslandChange);
			}
		}
		
		
		public void SetHappiness(float targetAmount)
		{
			SaveData.Instance._currentHappiness = Mathf.Clamp(targetAmount, 0, 100);
		}
		public void SetMagic(float targetAmount)
		{
			SaveData.Instance._currentMagic = targetAmount;
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






		public void HandleOptionOutcomes(EventOutcomeData outcome)
		{
			if (outcome.HappinessChange != 0)
			{
				if (outcome.IsIslandSpecific && outcome.TargetIsland.HasValue)
				{
					ChangeIslandHappiness(outcome.TargetIsland.Value, outcome.HappinessChange);
				}
				else
				{
					ChangeHappiness(outcome.HappinessChange);
				}
			}

			if (outcome.MagicChange != 0)
			{
				ChangeMagic(outcome.MagicChange);
			}

			if (outcome.TokensChange != 0)
			{
				ChangeTokens(outcome.TokensChange);
			}

			if (outcome.MagicMultiplier != 0)
			{
				ChangeMagicMultiplier(outcome.MagicMultiplier);
			}
		}
		public override void _Ready()
		{
			Instance = this;
		}
	}
}
