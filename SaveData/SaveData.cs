using Godot;
using System;

namespace MapGame
{
	public partial class SaveData : Node
	{
		[Export] public int _currentTurn = 0;
		[Export] public int _currentDay = 0;
		[Export] public float _currentHappiness = 0;
		[Export] public float _currentMagic = 0;
		[Export] public float _currentTokens = 0;
		[Export] public float _currentSalary = 0;
		[Export] public float _currentMagicMultiplier = 0;
		[Export] public float _currentHappinessMultiplier = 0;

		[Export] public string[] _eventPool; // Something like this? 
		public static SaveData Instance
		{
			get;
			private set;
		}
		
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			Instance = this;
			
		}
		
		public void IncreaseTurn()
		{
			_currentTurn += 1;
		}
		public void Reset()
		{
			_currentTurn = 0;
			_eventPool = new string[0]; // Reset the event pool to an empty array
		}
		
		public void SaveGame()
		{
			var _saveFile = new ConfigFile();

			_saveFile.SetValue("Save1","Turn", _currentTurn);
			_saveFile.SetValue("Save1", "Turn", _currentDay);
			_saveFile.SetValue("Save1","Magic", _currentMagic);
			_saveFile.SetValue("Save1","Happiness", _currentHappiness);
			_saveFile.SetValue("Save1","Tokens", _currentTokens);
			_saveFile.SetValue("Save1","Salary", _currentSalary);
			_saveFile.SetValue("Save1","MagicMultiplier", _currentMagicMultiplier);
			_saveFile.SetValue("Save1","HappinessMultiplier", _currentHappinessMultiplier);

			_saveFile.Save("res://SaveData/SaveData.cfg");
		}
		public void LoadGame(ConfigFile _saveData)
		{
			foreach (String save in _saveData.GetSections())
			{
				_currentTurn = (int)_saveData.GetValue(save, "Turn");
				_currentDay = (int)_saveData.GetValue(save, "Magic");
				_currentMagic = (float)_saveData.GetValue(save, "Magic");
				_currentHappiness = (float)_saveData.GetValue(save, "Happiness");
				_currentTokens = (float)_saveData.GetValue(save, "Tokens");
				_currentSalary = (float)_saveData.GetValue(save, "Salary");
				_currentMagicMultiplier = (float)_saveData.GetValue(save, "MagicMultiplier");
				_currentHappinessMultiplier = (float)_saveData.GetValue(save, "HappinessMultiplier");
			}
		}
		
	}
}
