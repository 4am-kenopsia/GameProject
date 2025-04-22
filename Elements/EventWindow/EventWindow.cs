using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;

namespace MapGame
{
	public partial class EventWindow : Control
	{
		[Signal] public delegate void OptionButtonPressedEventHandler(int optionIndex);
		[Signal] public delegate void EventAnsweredEventHandler();

		private RichTextLabel _eventTitle = null;
		private RichTextLabel _eventContent = null;
		private EventData _currentEvent = null;
		private Dictionary _eventDictionary = null;
		private TextureRect _eventIcon = null;
		private AnimationPlayer _animationPlayer;
		private TextureRect _eventWindow;
		private float _offset;
		private float _centerX;
		private float _centerY;
		private Vector2 _targetPosition;
		private Vector2 _startingPosition;
		
		private string _currentEventID = "";

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
			LoadEventData(_eventNumber);
			
			_animationPlayer.Play("newevent");
			
			Tween tween = CreateTween();
			
			tween.TweenProperty(_eventWindow, "position", _targetPosition, 0.5f)
				.SetTrans(Tween.TransitionType.Spring)
				.SetEase(Tween.EaseType.Out);
		}

		public void LoadEventData(string eventNumber)
		{
			_currentEvent = ResourceLoader.Load<EventData>($"res://Code/EventData/TurnEvents/{SaveData.Instance._language}/Event{eventNumber}.tres");
			_currentEventID = eventNumber.ToString();
			_eventDictionary = _currentEvent.EventDictionary;

			_eventTitle = GetNode<RichTextLabel>("TextureRect/TurnEventTitle");
			_eventTitle.Text = _currentEvent.EventTitle;

			_eventContent = GetNode<RichTextLabel>("TextureRect/TurnEventContent");
			_eventContent.Text = _currentEvent.EventDesc;
			
			_eventIcon = GetNode<TextureRect>("TextureRect/TurnEventIcon");
			_eventIcon.Texture = (Texture2D)GD.Load(_currentEvent.EventPicture);
			
			SetupOptionButton(1, "OptionContainer1", _currentEvent.EventOptions.Length > 0);
			SetupOptionButton(2, "OptionContainer2", _currentEvent.EventOptions.Length > 1);
			SetupOptionButton(3, "OptionContainer3", _currentEvent.EventOptions.Length > 2);
			SetupOptionButton(4, "OptionContainer4", _currentEvent.EventOptions.Length > 3);
		}

		private void SetupOptionButton(int index, string containerName, bool isVisible)
		{
			if (isVisible)
			{
				var optionButton = GetNode<TextureButton>($"TextureRect/OptionsContainer/{containerName}/OptionButton{index}/TextureButton");
				var optionButtonText = GetNode<Label>($"TextureRect/OptionsContainer/{containerName}/OptionButton{index}/Label");
				var optionButtonContainer = GetNode<OptionButton>($"TextureRect/OptionsContainer/{containerName}/OptionButton{index}");
				var optionButtonIcon = GetNode<TextureRect>($"TextureRect/OptionsContainer/{containerName}/OptionButton{index}/TextureRect");

				optionButton.Pressed += () => OnOptionButtonPressed(index);
				optionButtonText.Text = _currentEvent.EventOptions[index - 1];
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
			var outcome = _eventDictionary[outcomeKey];
			
			Tween tween = CreateTween();
			
			tween.TweenProperty(_eventWindow, "position", _startingPosition, 0.3f)
				.SetTrans(Tween.TransitionType.Cubic)
				.SetEase(Tween.EaseType.Out);

			_animationPlayer.Play("begonevent");
			await WaitForAnimationToFinish();
			
			ResourceManager.Instance.HandleOptionOutcomes((EventOutcomeData)outcome);
			EmitSignal(SignalName.EventAnswered);
		}
		private async Task WaitForAnimationToFinish()
		{
			var tcs = new TaskCompletionSource<bool>();
			
			void OnAnimationFinished(StringName animationName)
			{
				if (animationName == "begonevent")
				{
					tcs.SetResult(true);
				}
			}
			_animationPlayer.AnimationFinished += OnAnimationFinished;
			await tcs.Task;
			_animationPlayer.AnimationFinished -= OnAnimationFinished;
		}
	}
}
