using Godot;
using System;

namespace MapGame
{
	public partial class ResourceManager : Node
	{
		[Export] public int _startingMainResource = 10000;
		[Export] public int _startingHappiness = 100; //Percentage 0-100%
		[Export] public int _startingThirdResource = 5;
		[Export] public int _currentMainResource = 10000;
		[Export] public int _currentHappiness = 100;
		[Export] public int _currentThirdResource = 5;
        [Export] public float _mainResourceMultiplier = 1;
        [Export] public float _happinessMultiplier = 1;
		public static ResourceManager Instance
		{
			get;
			private set;
		}
		
		
		public void SetMainResource(int targetAmount)
		{
			_currentMainResource = targetAmount;
		}
		public void SetHappiness(int targetAmount)
		{
			_currentHappiness = targetAmount;
		}
		public void SetThirdResource(int targetAmount)
		{
			_currentThirdResource = targetAmount;
		}
		public void IncreaseMainResource(int increase)
		{
			_currentMainResource += increase;
		}
		public void IncreaseHappiness(int increase)
		{
			_currentHappiness += increase;
		}
		public void IncreaseThirdResource(int increase)
		{
			_currentThirdResource += increase;
		}
		public void DecreaseMainResource(int decrease)
		{
			_currentMainResource -= decrease;
		}
		public void DecreaseHappiness(int decrease)
		{
			_currentHappiness -= decrease;
		}
		public void DecreaseThirdResource(int decrease)
		{
			_currentThirdResource -= decrease;
		}
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			Instance = this;
			
		}
		
	}
}
