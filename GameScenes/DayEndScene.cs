using Godot;
using System;
using System.Threading.Tasks;

namespace MapGame
{

	public partial class DayEndScene : Control
	{
		private TextureButton _continueButton;
		private Label _title;
		private Label _content;
		private TextureRect _dayEndPanel;
		private Vector2 _targetPosition;
		private Vector2 _startingPosition;
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			_continueButton = GetNode<TextureButton>("TextureRect/TextureButton");
			_title = GetNode<Label>("TextureRect/Label");
			_content = GetNode<Label>("TextureRect/Label2");
			_dayEndPanel = GetNode<TextureRect>("TextureRect");
			
			_targetPosition = new Vector2(SaveData.Instance._viewPortSize.X / 2 - 760, SaveData.Instance._viewPortSize.Y / 2 - 440);
			_startingPosition = new Vector2(_targetPosition.X, SaveData.Instance._viewPortSize.Y);
			
			_dayEndPanel.Position = _startingPosition;

			_continueButton.Pressed += OnContinueButtonPressed;
			DayResultsAsync();
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}

		public async Task DayResultsAsync()
		{
			await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
			_title.Text = $"END OF DAY {SaveData.Instance._currentDay}";
			Tween tween = CreateTween();
			
			tween.TweenProperty(_dayEndPanel, "position", _targetPosition, 0.5f)
				.SetTrans(Tween.TransitionType.Sine)
				.SetEase(Tween.EaseType.Out);
			
			await ToSignal(tween, "finished");
			
			float _earnings = SaveData.Instance._currentSalary * SaveData.Instance._currentMagicMultiplier;
			
			string _panelText = "Salary: " + SaveData.Instance._currentSalary + 
								"\nMult: " + SaveData.Instance._currentMagicMultiplier + 
								"\n" +
								"\n" + SaveData.Instance._currentMagicMultiplier + " x " + SaveData.Instance._currentSalary + 
								" = " + _earnings;
			_content.Text = "";
			foreach (char c in _panelText)
			{
				_content.Text += c;
				await ToSignal(GetTree().CreateTimer(0.03f), "timeout");
			}
			
			ResourceManager.Instance.ChangeMagic(SaveData.Instance._currentMagicMultiplier * SaveData.Instance._currentSalary);
		}

		public void OnContinueButtonPressed()
		{
			BackToGame();
		}
		public async Task BackToGame()
		{
			Tween tween = CreateTween();
			
			tween.TweenProperty(_dayEndPanel, "position", _startingPosition, 0.2f)
				.SetTrans(Tween.TransitionType.Sine)
				.SetEase(Tween.EaseType.Out);
				
			await ToSignal(tween, "finished");
			
			SaveData.Instance.IncreaseDay();
			SaveData.Instance.SaveGame();
			SceneTransition.Instance.TransitionToScene("res://GameScenes/GameScene.tscn");
		}
	}
}