using Godot;
using MapGame;
using System;
using System.IO;

public partial class MainMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	private TextureButton _continueButton;
	private TextureButton _newGameButton;
	private TextureButton _langButton;
	private TextureButton _creditsButton;
	private ConfigFile _saveData;
	
	public override void _Ready()
	{
		_continueButton = GetNode<TextureButton>("VBoxContainer/ContinueButton");
		_newGameButton = GetNode<TextureButton>("VBoxContainer/NewGameButton");
		_langButton = GetNode<TextureButton>("VBoxContainer/LanguageButton");
		_creditsButton = GetNode<TextureButton>("VBoxContainer/CreditsButton");
		
		_continueButton.Pressed += OnContinueButtonPressed;
		_newGameButton.Pressed += OnNewGameButtonPressed;
		_langButton.Pressed += OnLanguageButtonPressed;
		_creditsButton.Pressed += OnCreditsButtonPressed;

		_saveData = new ConfigFile();
		Error err = _saveData.Load("res://SaveData/SaveData.cfg");
		if (err != Error.Ok)
		{
			_continueButton.Disabled = true;
			GD.Print("Vro");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void OnContinueButtonPressed()
	{
		SaveData.Instance.LoadGame(_saveData);
		GetTree().ChangeSceneToFile("res://GameScenes/GameScene.tscn");
	}
	private void OnNewGameButtonPressed()
	{
		ResourceManager.Instance.ResetResources();
		SaveData.Instance.SaveGame();
		GetTree().ChangeSceneToFile("res://GameScenes/GameScene.tscn");
	}
	private void OnLanguageButtonPressed()
	{
		var _langText = GetNode<Label>("VBoxContainer/LanguageButton/LanguageLabel");
		_langText.Text = "Not implemented";
	}
	private void OnCreditsButtonPressed()
	{
		var _creditsText = GetNode<Label>("VBoxContainer/CreditsButton/CreditsLabel");
		_creditsText.Text = "Not implemented";
	}
}
