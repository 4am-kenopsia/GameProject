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
		private ColorRect _shadeOverlay;
		private SettingsPopup _settingsPopup;
		private PackedScene _settingsPopupScene;
		private TextureButton _closeSettings;
		private ConfigFile _saveData;

		public override void _Ready()
		{
			GD.Print("--");
			GD.Print("Menu scene loaded");
			
			_continueButton = GetNode<TextureButton>("VBoxContainer/ContinueButton");
			_newGameButton = GetNode<TextureButton>("VBoxContainer/NewGameButton");
			_settingsButton = GetNode<TextureButton>("VBoxContainer/SettingsButton");
			_creditsButton = GetNode<TextureButton>("VBoxContainer/CreditsButton");
			_yesButton = GetNode<TextureButton>("ConfirmationPopup/YesButton");
			_noButton = GetNode<TextureButton>("ConfirmationPopup/NoButton");
			_confirmationPopup = GetNode<TextureRect>("ConfirmationPopup");
			_shadeOverlay = GetNode<ColorRect>("ShadeOverlay");
			
			_settingsPopupScene = GD.Load<PackedScene>("res://Elements/SettingsObject/SettingsPopup.tscn");

			_continueButton.Pressed += OnContinueButtonPressed;
			_newGameButton.Pressed += OnNewGameButtonPressed;
			_settingsButton.Pressed += OnSettingsButtonPressed;
			_creditsButton.Pressed += OnCreditsButtonPressed;
			_yesButton.Pressed += OnYesButtonPressed;
			_noButton.Pressed += OnNoButtonPressed;
			
			_shadeOverlay.Color = new Color(0.404f, 0.192f, 0.357f, 0f);

			_saveData = new ConfigFile();
			Error err = _saveData.Load("res://SaveData/SaveData.cfg");
			if (err != Error.Ok)
			{
				_continueButton.Disabled = true;
				GD.Print("Err: No Savedata found");
			}
		}

		

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}
		private void OnContinueButtonPressed()
		{
			GD.Print("Continue pressed");
			
			SoundPlayer.Instance.PlayMenuButtonSound();
			SaveData.Instance.LoadGame(_saveData);
			SceneTransition.Instance.TransitionToScene("res://GameScenes/GameScene.tscn");
		}
		private void OnNewGameButtonPressed()
		{
			GD.Print("New game pressed");
			SoundPlayer.Instance.PlayMenuButtonSound();
			_confirmationPopup.Visible = true;
			_shadeOverlay.Visible = true;
			Tween tween = CreateTween();
				
			tween.TweenProperty(_shadeOverlay, "color", new Color(0.404f, 0.192f, 0.357f, 0.251f), 0.3f)
				.SetTrans(Tween.TransitionType.Linear)
				.SetEase(Tween.EaseType.Out);
		}
		private void OnSettingsButtonPressed()
		{
			GD.Print("Settings pressed");
			
			_shadeOverlay.Visible = true;
			SoundPlayer.Instance.PlayMenuButtonSound();
			Tween tween = CreateTween();
				
			tween.TweenProperty(_shadeOverlay, "color", new Color(0.404f, 0.192f, 0.357f, 0.251f), 0.3f)
				.SetTrans(Tween.TransitionType.Linear)
				.SetEase(Tween.EaseType.Out);
			_settingsPopup = _settingsPopupScene.Instantiate<SettingsPopup>();
			AddChild(_settingsPopup);
			_closeSettings = GetNode<TextureButton>("SettingsPopup/SettingsPanel/CloseSettings");
			_closeSettings.Pressed += OnCloseSettingsPressed;
		}
		private void OnCreditsButtonPressed()
		{
			GD.Print("Credits pressed");
			
			SoundPlayer.Instance.PlayMenuButtonSound();
		}
		private void OnYesButtonPressed()
		{
			GD.Print("Confirmed new game");
			
			_confirmationPopup.Visible = false;
			ResourceManager.Instance.ResetResources();
			SaveData.Instance.SaveGame();
			SoundPlayer.Instance.PlayMenuButtonSound();
			SceneTransition.Instance.TransitionToScene("res://GameScenes/GameScene.tscn");
		}

		private void OnNoButtonPressed()
		{
			GD.Print("Denied new game");
			
			SoundPlayer.Instance.PlayMenuButtonSound();
			_confirmationPopup.Visible = false;
			Tween tween = CreateTween();
			
			tween.TweenProperty(_shadeOverlay, "color", new Color(0.404f, 0.192f, 0.357f, 0f), 0.3f)
				.SetTrans(Tween.TransitionType.Sine)
				.SetEase(Tween.EaseType.InOut);
			tween.TweenCallback(Callable.From(() => _shadeOverlay.Visible = false));
		}
		private void OnCloseSettingsPressed()
		{
			Tween tween = CreateTween();
			
			tween.TweenProperty(_shadeOverlay, "color", new Color(0.404f, 0.192f, 0.357f, 0f), 0.3f)
				.SetTrans(Tween.TransitionType.Sine)
				.SetEase(Tween.EaseType.InOut);
			tween.TweenCallback(Callable.From(() => _shadeOverlay.Visible = false));
		}
	}
}
