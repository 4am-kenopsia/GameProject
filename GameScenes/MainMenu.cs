using Godot;
using System;

namespace MapGame
{
	public partial class MainMenu : Control
	{
		// Called when the node enters the scene tree for the first time.
		private TextureButton _continueButton;
		private TextureButton _newGameButton;
		private TextureButton _settingsButton;
		private TextureButton _creditsButton;
		private TextureButton _yesButton;
		private TextureButton _noButton;
		private TextureRect _confirmationPopup;
		private ColorRect _settingsBackdrop;
		private SettingsPopup _settingsPopup;
		private PackedScene _settingsPopupScene;
		private ConfigFile _saveData;

		public override void _Ready()
		{
			_continueButton = GetNode<TextureButton>("VBoxContainer/ContinueButton");
			_newGameButton = GetNode<TextureButton>("VBoxContainer/NewGameButton");
			_settingsButton = GetNode<TextureButton>("VBoxContainer/SettingsButton");
			_creditsButton = GetNode<TextureButton>("VBoxContainer/CreditsButton");
			_yesButton = GetNode<TextureButton>("ConfirmationPopup/YesButton");
			_noButton = GetNode<TextureButton>("ConfirmationPopup/NoButton");
			_confirmationPopup = GetNode<TextureRect>("ConfirmationPopup");
			
			_settingsPopupScene = GD.Load<PackedScene>("res://Elements/SettingsObject/SettingsPopup.tscn");

			_continueButton.Pressed += OnContinueButtonPressed;
			_newGameButton.Pressed += OnNewGameButtonPressed;
			_settingsButton.Pressed += OnSettingsButtonPressed;
			_creditsButton.Pressed += OnCreditsButtonPressed;
			_yesButton.Pressed += OnYesButtonPressed;
			_noButton.Pressed += OnNoButtonPressed;

			_saveData = new ConfigFile();
			Error err = _saveData.Load("res://SaveData/SaveData.cfg");
			if (err != Error.Ok)
			{
				_continueButton.Disabled = true;
				GD.Print("No Savedata found");
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
		private void OnSettingsButtonPressed()
		{
			SoundPlayer.Instance.PlayMenuButtonSound();
			_settingsPopup = _settingsPopupScene.Instantiate<SettingsPopup>();
			AddChild(_settingsPopup);
		}
		private void OnCreditsButtonPressed()
		{
			SoundPlayer.Instance.PlayMenuButtonSound();
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
}