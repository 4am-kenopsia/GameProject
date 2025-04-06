using Godot;
using System;

namespace MapGame
{

	public partial class GUI : Control
	{
		[Signal] public delegate void TurnButtonPressedEventHandler();

		private static Label _turnLabel;
		private static Label _magicLabel;
		private static Label _happinessLabel;
		private static Label _tokensLabel;
		private static ColorRect _gameMenuOverlay;
		private static TextureButton _menuButton;
		private static TextureButton _continueButton;
		private static TextureButton _idkButton;
		private static TextureButton _saveAndQuitButton;
		private static Label _gameOver;
		private bool _isMenuOpen = false;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			_turnLabel = GetNode<Label>("TurnUI/TurnLabel");
			_magicLabel = GetNode<Label>("SideUI/MagicUI/MagicLabel");
			_happinessLabel = GetNode<Label>("SideUI/HappinessUI/HappinessLabel");
			_tokensLabel = GetNode<Label>("SideUI/TokensUI/TokensLabel");
			_gameOver = GetNode<Label>("GameOver");
			
			_menuButton = GetNode<TextureButton>("SideUI/MenuButton");
			
			_gameMenuOverlay = GetNode<ColorRect>("GameMenuOverlay");
			
			_continueButton = GetNode<TextureButton>("GameMenuOverlay/GameMenuPanel/ButtonsContainer/ContinueButton");
			_idkButton = GetNode<TextureButton>("GameMenuOverlay/GameMenuPanel/ButtonsContainer/IdkButton");
			_saveAndQuitButton = GetNode<TextureButton>("GameMenuOverlay/GameMenuPanel/ButtonsContainer/SaveAndQuitButton");
			
			_menuButton.Pressed += OnMenuButtonPressed;
			_continueButton.Pressed += OnContinueButtonPressed;
			_idkButton.Pressed += OnIdkButtonPressed;
			_saveAndQuitButton.Pressed += OnSaveAndQuitButtonPressed;
			UpdateLabels();
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			if (SaveData.Instance._currentHappiness == 0)
			{
				//_gameOver.Show();
				//MainMenu();

			}
		}

		public static void UpdateLabels()
		{
			_magicLabel.Text = SaveData.Instance._currentMagic.ToString();
			_happinessLabel.Text = SaveData.Instance._currentHappiness.ToString() + "%";
			_tokensLabel.Text = SaveData.Instance._currentTokens.ToString();
			_turnLabel.Text = "Day " + SaveData.Instance._currentDay.ToString() + ", Turn " + SaveData.Instance._currentTurn.ToString();
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
				SoundPlayer.Instance.PlayNextTurnSound();
				_isMenuOpen = true;
				_gameMenuOverlay.Visible = true;
			}
		}
		private void OnContinueButtonPressed()
		{
			SoundPlayer.Instance.PlayNextTurnSound();
			_gameMenuOverlay.Visible = false;
			_isMenuOpen = false;
		}
		private void OnIdkButtonPressed()
		{
			SoundPlayer.Instance.PlayNextTurnSound();
			GD.Print("Bleh");
		}
		private void OnSaveAndQuitButtonPressed()
		{
			SoundPlayer.Instance.PlayNextTurnSound();
			SaveData.Instance.SaveGame();
			SceneTransition.Instance.TransitionToScene("res://GameScenes/MainMenu.tscn");
		}
	}
}
