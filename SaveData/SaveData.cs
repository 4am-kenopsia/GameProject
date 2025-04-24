using Godot;
using Godot.Collections;
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
		[Export] public float _ambienceVolume = -10;
		[Export] public float _effectsVolume = -10;
		[Export] public bool _gameOver = false;
		[Export] public string _language = "EN";
		[Export] public Vector2 _viewPortSize;
		[Export] public Vector2 _gameAreaSize;
		[Export] public Dictionary <Island, float> _islandHappiness = new();

		[Export] public string[] _eventPool; // Something like this? 
		public static SaveData Instance
		{
			get;
			private set;
		}
		public Dictionary<Island, float> GetAllIslandHappiness()
		{
			return _islandHappiness;
		}
		
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			Instance = this;
			_viewPortSize = GetViewport().GetVisibleRect().Size;
			_gameAreaSize = new Vector2(_viewPortSize.X - 220, _viewPortSize.Y);
			GD.Print($"Viewportsize is: {_viewPortSize.X} by {_viewPortSize.Y}");
			GD.Print($"Gamearea size is: {_gameAreaSize.X} by {_gameAreaSize.Y}");
		}

		public override void _Process(double delta)
		{
			_viewPortSize = GetViewport().GetVisibleRect().Size;
		}
		
		public void IncreaseTurn()
		{
			_currentTurn += 1;
		}
		public void IncreaseDay()
		{
			_currentDay += 1;
			_currentTurn = 0;
			ResourceManager.Instance.SetMagicMultiplier(1);
		}
		
		public void SaveGame()	
		{
			var _saveFile = new ConfigFile();

			_saveFile.SetValue("Save1","Turn", 0);
			_saveFile.SetValue("Save1", "Day", _currentDay);
			_saveFile.SetValue("Save1","Magic", _currentMagic);
			_saveFile.SetValue("Save1","Happiness", _currentHappiness);
			_saveFile.SetValue("Save1","Tokens", _currentTokens);
			_saveFile.SetValue("Save1","Salary", _currentSalary);
			_saveFile.SetValue("Save1","MagicMultiplier", _currentMagicMultiplier);
			_saveFile.SetValue("Save1", "IslandHappiness", _islandHappiness);
			_saveFile.SetValue("Save1", "EffectsVolume", _effectsVolume);
			_saveFile.SetValue("Save1", "AmbienceVolume", _ambienceVolume);
			_saveFile.SetValue("Save1","Language", _language);
			_saveFile.SetValue("Save1", "GameOver", _gameOver);

			_saveFile.Save("res://SaveData/SaveData.cfg");
			
			GD.Print("Game saved");
		}
		public void LoadGame(ConfigFile _saveData)
		{
			foreach (String save in _saveData.GetSections())
			{
				_currentTurn = (int)_saveData.GetValue(save, "Turn");
				_currentDay = (int)_saveData.GetValue(save, "Day");
				_currentMagic = (float)_saveData.GetValue(save, "Magic");
				_currentHappiness = (float)_saveData.GetValue(save, "Happiness");
				_currentTokens = (float)_saveData.GetValue(save, "Tokens");
				_currentSalary = (float)_saveData.GetValue(save, "Salary");
				_currentMagicMultiplier = (float)_saveData.GetValue(save, "MagicMultiplier");
				_islandHappiness = (Dictionary<Island, float>)_saveData.GetValue(save, "IslandHappiness");
				_ambienceVolume = (float)_saveData.GetValue(save, "AmbienceVolume");
				_effectsVolume = (float)_saveData.GetValue(save, "EffectsVolume");
				_language = (string)_saveData.GetValue(save, "Language");
				_gameOver = (bool)_saveData.GetValue(save, "GameOver");
				
				GD.Print("Game loaded");
			}
		}
	}
}
