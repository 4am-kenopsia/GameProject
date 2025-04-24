using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;

namespace MapGame
{
	public partial class EventWindow : Control
	{
		//[Signal] public delegate void OptionButtonPressedEventHandler(int optionIndex);
		[Signal] public delegate void EventAnsweredEventHandler();

		private RichTextLabel _eventTitle = null;
		private RichTextLabel _eventContent = null;
		private EventData _currentEvent = null;
		private Dictionary _eventDictionary = null;
		private TextureRect _eventIcon = null;
		private string[] _eventOptions = null;
		private TextureRect _eventWindow;
		private string _currentEventID = "";
		
		private AnimationPlayer _animationPlayer;

		private float _offset;
		private float _centerX;
		private float _centerY;
		private Vector2 _targetPosition;
		private Vector2 _startingPosition;
		

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			_eventWindow = GetNode<TextureRect>("TextureRect");
			_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
			
			_centerX = SaveData.Instance._viewPortSize.X / 2;
			_centerY = SaveData.Instance._viewPortSize.Y / 2;
			_offset = SaveData.Instance._viewPortSize.Y;
			
			_startingPosition = new Vector2(_centerX - 850, _offset);
			_eventWindow.Position = _startingPosition;
			_targetPosition = new Vector2(_startingPosition.X, _centerY - 440);
			
			string _eventNumber = GD.RandRange(1, 1).ToString();
			if (SaveData.Instance._currentDay == 0)
			{
				_eventNumber = $"T{SaveData.Instance._currentTurn}";
				GD.Print(SaveData.Instance._currentTurn);
			}
			else if (SaveData.Instance._gameOver == true)
			{
				_eventNumber = "E1";
			}
			else if (SaveData.Instance._currentDay == 5)
			{
				_eventNumber = "E2";
			}
			
			LoadEventData(_eventNumber);
			
			_animationPlayer.Play("newevent");
			Tween tween = CreateTween();
			tween.TweenProperty(_eventWindow, "position", _targetPosition, 0.5f)
				.SetTrans(Tween.TransitionType.Spring)
				.SetEase(Tween.EaseType.Out);
		}

		public void LoadEventData(string eventNumber)
		{
			_currentEvent = ResourceLoader.Load<EventData>($"res://Code/EventData/TurnEvents/Event{eventNumber}.tres");
			_currentEventID = eventNumber.ToString();
			_eventDictionary = _currentEvent.EventDictionary;
			
			_eventTitle = GetNode<RichTextLabel>("TextureRect/TurnEventTitle");
			_eventContent = GetNode<RichTextLabel>("TextureRect/TurnEventContent");
			_eventIcon = GetNode<TextureRect>("TextureRect/TurnEventIcon");
			
			_eventIcon.Texture = (Texture2D)GD.Load(_currentEvent.EventPicture);
			
			// Get language
			switch (SaveData.Instance._language)
			{
				case "EN":
					GD.Print("ENG");
					_eventTitle.Text = _currentEvent.EventTitleEN;
					_eventContent.Text = _currentEvent.EventDescEN;
					_eventOptions = _currentEvent.EventOptionsEN;
					break;
				case "FI":
					GD.Print("FI");
					_eventTitle.Text = _currentEvent.EventTitleFI;
					_eventContent.Text = _currentEvent.EventDescFI;
					_eventOptions = _currentEvent.EventOptionsFI;
					break;
			}
			
			SetupOptionButton(1, "OptionContainer1", _eventOptions.Length > 0);
			SetupOptionButton(2, "OptionContainer2", _eventOptions.Length > 1);
			SetupOptionButton(3, "OptionContainer3", _eventOptions.Length > 2);
			SetupOptionButton(4, "OptionContainer4", _eventOptions.Length > 3);
		}

		private void SetupOptionButton(int index, string containerName, bool isVisible)
		{
			if (isVisible)
			{
				var optionButtonContainer = GetNode<OptionButton>($"TextureRect/OptionsContainer/{containerName}/OptionButton{index}");
				var optionButton = optionButtonContainer.GetNode<TextureButton>($"TextureButton");
				var optionButtonText = optionButtonContainer.GetNode<Label>($"Label");
				
				var optionButtonIcon = optionButtonContainer.GetNode<TextureRect>($"TextureRect");

				optionButton.Pressed += () => OnOptionButtonPressed(index);
				optionButtonText.Text = _eventOptions[index - 1];
				EventOutcomeData outcome = (EventOutcomeData)_currentEvent.EventDictionary[_currentEventID + "_" + index];
				switch (outcome.OptionSeverity)
				{
					case EventOutcomeData.Severity.Low:
						optionButtonIcon.Texture = (Texture2D)GD.Load("res://Assets/Resources/small_change.png");
						break;
					case EventOutcomeData.Severity.Medium:
						optionButtonIcon.Texture = (Texture2D)GD.Load("res://Assets/Resources/medium_change.png");
						break;
					case EventOutcomeData.Severity.High:
						optionButtonIcon.Texture = (Texture2D)GD.Load("res://Assets/Resources/large_change.png");
						break;
				}
				optionButtonContainer.Visible = true;
			}
		}

		public void OnOptionButtonPressed(int optionIndex)
		{
			OptionButton(optionIndex);
		}
		public async Task OptionButton(int optionIndex)
		{
			SoundPlayer.Instance.PlayEventButtonSound();
			
			string outcomeKey = _currentEventID + "_" + optionIndex;
			EventOutcomeData outcome = (EventOutcomeData)_eventDictionary[outcomeKey];
			
			Tween tween = CreateTween();
			
			tween.TweenProperty(_eventWindow, "position", _startingPosition, 0.3f)
				.SetTrans(Tween.TransitionType.Cubic)
				.SetEase(Tween.EaseType.Out);

			_animationPlayer.Play("begonevent");
			await ToSignal(tween, "finished");
			
			ResourceManager.Instance.HandleOptionOutcomes(outcome);
			EmitSignal(SignalName.EventAnswered);
		}
	}
}
