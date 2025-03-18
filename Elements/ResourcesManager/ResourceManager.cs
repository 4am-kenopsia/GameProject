using Godot;
using System;

namespace MapGame
{
	public partial class ResourceManager : Node
	{
		[Export] public float _startingMainResource = 10000;
		[Export] public float _startingHappiness = 100; //Percentage 0-100%
		[Export] public float _startingThirdResource = 5;
		[Export] public float _currentMainResource = 10000;
		[Export] public float _currentHappiness = 100;
		[Export] public float _currentThirdResource = 5;
        [Export] public float _mainResourceNegMultiplier = 1;
        [Export] public float _mainResourcePosMultiplier = 1;
        [Export] public float _happinessNegMultiplier = 1;
        [Export] public float _happinessPosMultiplier = 1;
		public static ResourceManager Instance
		{
			get;
			private set;
		}
		
		public void ResetResources()
        {
            SetMainResource(_startingMainResource);
            SetHappiness(_startingHappiness);
            SetThirdResource(_startingThirdResource);
        }
		public void SetMainResource(float targetAmount)
		{
			_currentMainResource = targetAmount;
		}
		public void SetHappiness(float targetAmount)
		{
			_currentHappiness = targetAmount;
		}
		public void SetThirdResource(float targetAmount)
		{
			_currentThirdResource = targetAmount;
		}
		public void IncreaseMainResource(int increase)
		{
			_currentMainResource += _mainResourcePosMultiplier * increase;
		}
		public void DecreaseMainResource(int decrease)
		{
			_currentMainResource += _mainResourceNegMultiplier * decrease;
		}
		public void IncreaseHappiness(int increase)
		{
            if (_currentHappiness + _happinessPosMultiplier * increase > 100)
            {
                _currentHappiness = 100;
                return;
            }
			_currentHappiness += _happinessPosMultiplier * increase;
		}
		public void DecreaseHappiness(int decrease)
		{
            if (_currentHappiness + _happinessNegMultiplier * decrease < 0)
            {
                _currentHappiness = 0;
            }
			_currentHappiness += _happinessNegMultiplier * decrease;
		}
        public void IncreaseThirdResource(int increase)
		{
			_currentThirdResource += increase;
		}
		public void DecreaseThirdResource(int decrease)
		{
			_currentThirdResource += decrease;
		}
        
        public void HandleOptionOutcomes(EventOutcomeData outcome)
		{
            if (outcome.HappinessChange >= 0)
            {
                IncreaseHappiness(outcome.HappinessChange);
            }
            else if (outcome.HappinessChange < 0)
            {
                DecreaseHappiness(outcome.HappinessChange);
            }
			GD.Print(_currentHappiness);
            
            
            if (outcome.MainResourcehange >= 0)
            {
                IncreaseHappiness(outcome.MainResourcehange);
            }
            else if (outcome.MainResourcehange < 0)
            {
                DecreaseHappiness(outcome.MainResourcehange);
            }
			GD.Print(_currentMainResource);
            
            
            if (outcome.ThirdResourceChange >= 0)
            {
                IncreaseThirdResource(outcome.ThirdResourceChange);
            }
            else if (outcome.ThirdResourceChange < 0)
            {
                DecreaseThirdResource(outcome.ThirdResourceChange);
            }
			GD.Print(_currentThirdResource);
            
            if (outcome.ChangeHappinessPosMultiplier)
            {
                _happinessPosMultiplier = outcome.HappinessPosMultiplier;
            }
            
            if (outcome.ChangeHappinessNegMultiplier)
            {
                _happinessNegMultiplier = outcome.HappinessNegMultiplier;
            }
            
            if (outcome.ChangeMainResourcePosMultiplier)
            {
                _mainResourcePosMultiplier = outcome.MainResourcePosMultiplier;
            }
            
            if (outcome.ChangeMainResourceNegMultiplier)
            {
                _mainResourceNegMultiplier = outcome.MainResourceNegMultiplier;
            }
            

		}
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			Instance = this;
			
		}
		
	}
}
