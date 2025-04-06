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
	private TextureButton _yesButton;
	private TextureButton _noButton;
	private TextureRect _confirmationPopup;
	private ConfigFile _saveData;
	
	public override void _Ready()
	{
		_continueButton = GetNode<TextureButton>("VBoxContainer/ContinueButton");
		_newGameButton = GetNode<TextureButton>("VBoxContainer/NewGameButton");
		_langButton = GetNode<TextureButton>("VBoxContainer/LanguageButton");
		_creditsButton = GetNode<TextureButton>("VBoxContainer/CreditsButton");
		_yesButton = GetNode<TextureButton>("ConfirmationPopup/YesButton");
		_noButton = GetNode<TextureButton>("ConfirmationPopup/NoButton");
		_confirmationPopup = GetNode<TextureRect>("ConfirmationPopup");
		
		_continueButton.Pressed += OnContinueButtonPressed;
		_newGameButton.Pressed += OnNewGameButtonPressed;
		_langButton.Pressed += OnLanguageButtonPressed;
		_creditsButton.Pressed += OnCreditsButtonPressed;
		_yesButton.Pressed += OnYesButtonPressed;
		_noButton.Pressed += OnNoButtonPressed;

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
		SoundPlayer.Instance.PlayMenuButtonSound();
		SaveData.Instance.LoadGame(_saveData);
		SceneTransition.Instance.TransitionToScene("res://GameScenes/GameScene.tscn");
	}
	private void OnNewGameButtonPressed()
	{
		SoundPlayer.Instance.PlayMenuButtonSound();
		_confirmationPopup.Visible = true;
	}
	private void OnLanguageButtonPressed()
	{
		SoundPlayer.Instance.PlayMenuButtonSound();
		var _langText = GetNode<Label>("VBoxContainer/LanguageButton/LanguageLabel");
		_langText.Text = "Not implemented";
	}
	private void OnCreditsButtonPressed()
	{
		SoundPlayer.Instance.PlayMenuButtonSound();
		var _creditsText = GetNode<Label>("VBoxContainer/CreditsButton/CreditsLabel");
		_creditsText.Text = "Not implemented";
	}
	private void OnYesButtonPressed()
	{
		_confirmationPopup.Visible = false;
		ResourceManager.Instance.ResetResources();
		SaveData.Instance.SaveGame();
		SoundPlayer.Instance.PlayMenuButtonSound();
		SceneTransition.Instance.TransitionToScene("res://GameScenes/GameScene.tscn");
	}
	
	private void OnNoButtonPressed()
	{
		SoundPlayer.Instance.PlayMenuButtonSound();
		_confirmationPopup.Visible = false;
	}
}
