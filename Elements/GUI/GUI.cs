using Godot;
using System;

namespace MapGame
{

	public partial class GUI : Control
	{
		[Signal] public delegate void MenuToggledEventHandler(bool is_open);
		[Signal] public delegate void TurnButtonPressedEventHandler();
		
		
		
		private PackedScene _settingsPopupScene;
		private SettingsPopup _settingsPopup;
		private static Label _turnLabel;
		private static Label _magicLabel;
		private static TextureRect _magicIcon;
		private static Label _happinessLabel;
		private static TextureRect _happinessIcon;
		private static Label _tokensLabel;
		private static TextureRect _tokensIcon;
		private static ColorRect _gameMenuOverlay;
		private static TextureButton _menuButton;
		private static TextureButton _continueButton;
		private static TextureButton _settingsButton;
		private static TextureButton _saveAndQuitButton;
		private float _centerX;
		private float _centerY;
		private float _offset;
		private static Label _gameOver;
		private bool _isMenuOpen = false;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			_settingsPopupScene = GD.Load<PackedScene>("res://Elements/SettingsObject/SettingsPopup.tscn");
			
			_turnLabel = GetNode<Label>("TurnUI/TurnLabel");
			_magicLabel = GetNode<Label>("SideUI/TextureRect/MagicLabel");
			_happinessLabel = GetNode<Label>("SideUI/TextureRect/HappinessLabel");
			_tokensLabel = GetNode<Label>("SideUI/TextureRect/TokensLabel");
			
			_magicIcon = GetNode<TextureRect>("SideUI/TextureRect/MagicIcon");
			_happinessIcon = GetNode<TextureRect>("SideUI/TextureRect/HappinessIcon");
			_tokensIcon = GetNode<TextureRect>("SideUI/TextureRect/TokensIcon");
			
			_gameOver = GetNode<Label>("GameOver");
			
			_menuButton = GetNode<TextureButton>("SideUI/MenuButton");
			_gameMenuOverlay = GetNode<ColorRect>("GameMenuOverlay");
			_continueButton = GetNode<TextureButton>("ContinueButton");
			_settingsButton = GetNode<TextureButton>("SettingsButton");
			_saveAndQuitButton = GetNode<TextureButton>("SaveAndQuitButton");
			
			_menuButton.Pressed += OnMenuButtonPressed;
			_continueButton.Pressed += OnContinueButtonPressed;
			_settingsButton.Pressed += OnSettingsButtonPressed;
			_saveAndQuitButton.Pressed += OnSaveAndQuitButtonPressed;
			
			_centerX = SaveData.Instance._viewPortSize.X / 2;
			_centerY = SaveData.Instance._viewPortSize.Y / 2;
			
			_offset = -SaveData.Instance._viewPortSize.X - 700;
			_continueButton.Position = new Vector2(_offset, _centerY - 215);
			_settingsButton.Position = new Vector2(_offset, _centerY - 70);
			_saveAndQuitButton.Position = new Vector2(_offset, _centerY + 75);
			
			UpdateLabels();
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			_centerX = SaveData.Instance._viewPortSize.X / 2;
			_centerY = SaveData.Instance._viewPortSize.Y / 2;
			if (SaveData.Instance._currentHappiness == 0)
			{
				//_gameOver.Show();
				//MainMenu();

			}
		}
		
		public void UpdateLabels()
		{
			// Cool little animations by first checking if the values changed.
			UpdateResourceAnimation(_magicLabel, _magicIcon, SaveData.Instance._currentMagic);
			_magicLabel.Text = SaveData.Instance._currentMagic.ToString();
			
			UpdateResourceAnimation(_happinessLabel, _happinessIcon, SaveData.Instance._currentHappiness);
			_happinessLabel.Text = SaveData.Instance._currentHappiness.ToString() + "%";
			
			UpdateResourceAnimation(_tokensLabel, _tokensIcon, SaveData.Instance._currentTokens);
			_tokensLabel.Text = SaveData.Instance._currentTokens.ToString();
			
			_turnLabel.Text = "Day " + SaveData.Instance._currentDay.ToString() + ", Turn " + SaveData.Instance._currentTurn.ToString();
		}
		
		public async void UpdateResourceAnimation(Label label, TextureRect icon, float currentResource)
		{
			Color animationColor = new Color(0, 0, 0);
			Color defaultColor = new Color(0, 0, 0);
			Vector2 originalPosition = icon.Position;
			float _old = label.Text.Replace("%", "").ToFloat();
			float _new = currentResource;
			
			var settings = GD.Load("res://Themes/UiResourceLabelSettings.tres").Duplicate() as LabelSettings;
			label.LabelSettings = settings;
			
			if (_old - _new == 0)
			{
				return;
			}
			else if (_old - _new < 0)
			{
				animationColor = new Color(0, 1, 0);
			}
			else if (_old - _new > 0)
			{
				animationColor = new Color(1, 0, 0);
			}
			
			Tween tween = CreateTween();
			
			tween.TweenProperty(
				icon,
				"position",
				originalPosition - new Vector2(0, 10f),
				0.15f
			);
			tween.TweenProperty(
				icon,
				"position",
				originalPosition,
				0.15f
			);
			tween.TweenProperty(
				label.LabelSettings,
				"font_color",
				animationColor,
				0.5f
			);
			tween.TweenProperty(
				label.LabelSettings,
				"font_color",
				defaultColor,
				0.5f
			);
		}

		public async void MainMenu()
		{

			await ToSignal(GetTree().CreateTimer(3.0f), Timer.SignalName.Timeout);
			// Reset the resources
			ResourceManager.Instance.ResetResources();

			// Reset the SaveData
			SaveData.Instance.Reset();


			// Load the main menu scene
			GetTree().ChangeSceneToFile("res://GameScenes/MainMenu.tscn");


		}
		
		private void OnMenuButtonPressed()
		{
			if (!_isMenuOpen)
			{
				_isMenuOpen = true;
				 UpdateMenuState(true);
				_gameMenuOverlay.Color = new Color(0.404f, 0.192f, 0.357f, 0f);
				_gameMenuOverlay.Visible = true;
				
				SoundPlayer.Instance.PlayNextTurnSound();
				
				Tween tween = CreateTween();
				
				tween.TweenProperty(_gameMenuOverlay, "color", new Color(0.404f, 0.192f, 0.357f, 0.251f), 0.3f)
					.SetTrans(Tween.TransitionType.Sine)
					.SetEase(Tween.EaseType.InOut);
				
				tween.TweenProperty(_continueButton, "position", new Vector2(_centerX - 300, _centerY - 215), 0.1f)
					.SetTrans(Tween.TransitionType.Sine)
					.SetEase(Tween.EaseType.Out);
				
				tween.Chain()
					.TweenProperty(_settingsButton, "position", new Vector2(_centerX - 300, _centerY - 70), 0.1f)
					.SetTrans(Tween.TransitionType.Sine)
					.SetEase(Tween.EaseType.Out);
				
				tween.Chain()
					.TweenProperty(_saveAndQuitButton, "position", new Vector2(_centerX - 300, _centerY + 75), 0.1f)
					.SetTrans(Tween.TransitionType.Sine)
					.SetEase(Tween.EaseType.Out);
			}
		}
		private void OnContinueButtonPressed()
		{
			SoundPlayer.Instance.PlayNextTurnSound();

			Tween tween = CreateTween();
			//Tween tween2 = CreateTween();
			
			tween.TweenProperty(_gameMenuOverlay, "color", new Color(0.404f, 0.192f, 0.357f, 0f), 0.3f)
				.SetTrans(Tween.TransitionType.Sine)
				.SetEase(Tween.EaseType.InOut);
			tween.TweenCallback(Callable.From(() => _gameMenuOverlay.Visible = false));
			
			tween.TweenProperty(_continueButton, "position", new Vector2(_offset, _centerY - 215), 0.1f)
					.SetTrans(Tween.TransitionType.Sine)
					.SetEase(Tween.EaseType.Out);
				
			tween.Chain()
				.TweenProperty(_settingsButton, "position", new Vector2(_offset, _centerY - 70), 0.1f)
				.SetTrans(Tween.TransitionType.Sine)
				.SetEase(Tween.EaseType.Out);
			
			tween.Chain()
				.TweenProperty(_saveAndQuitButton, "position", new Vector2(_offset, _centerY + 75), 0.1f)
				.SetTrans(Tween.TransitionType.Sine)
				.SetEase(Tween.EaseType.Out);
			
			_isMenuOpen = false;
			UpdateMenuState(false); 
		}
		private void OnSettingsButtonPressed()
		{
			SoundPlayer.Instance.PlayNextTurnSound();
			_settingsPopup = _settingsPopupScene.Instantiate<SettingsPopup>();
			AddChild(_settingsPopup);
			GD.Print("Bleh");
		}
		private void OnSaveAndQuitButtonPressed()
		{
			SoundPlayer.Instance.PlayNextTurnSound();
			SaveData.Instance.SaveGame();
			SceneTransition.Instance.TransitionToScene("res://GameScenes/MainMenu.tscn");
		}
		private void UpdateMenuState(bool is_open)
		{
			EmitSignal(SignalName.MenuToggled, is_open);
		}
	}
}
