using Godot;
using Godot.Collections;

namespace MapGame
{

	public partial class PopupWindow : Control
	{
		[Signal] public delegate void PopUpEventAnsweredEventHandler();

		private MarkerContainer _parentMarker;
		private TextureRect _panel;
		private Label _eventDescription;
		private int _popUpEventNumber;
		private string _currentEventID;
		private string[] _eventOptions = null;
		private Dictionary _eventDictionary = null;
		public Island TargetIsland { get; set; }
		
		private Vector2 _originalPosition;
		private Vector2 _targetPosition;
		private Tween _tween;

		private EventData _eventData;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			// Get the parent
			_parentMarker = (MarkerContainer)GetParent();
			
			// Set the touch eater nomnom
			ColorRect _eater = GetNode<ColorRect>("ColorRect2");
			_eater.CustomMinimumSize = SaveData.Instance._viewPortSize;
			_eater.GlobalPosition = new Vector2(0, 0);
			
			// Setup the position for the animation
			_targetPosition = new Vector2(0, 0);
			_originalPosition = new Vector2(_targetPosition.X, SaveData.Instance._viewPortSize.Y);
			GlobalPosition = _originalPosition;
			
			// Get the panel of the entire window
			_panel = GetNode<TextureRect>("Panel");
			
			// Decide on which event to display
			int eventNumber = GD.RandRange(1,2);
			string islandNumber = _parentMarker.TargetIsland.ToString();
			GD.Print($"This island's name is: {islandNumber}");
			LoadEventData(eventNumber, islandNumber);
			
			TweenWindowUp();
		}
		
		private void LoadEventData(int eventNumber, string islandNumber)
		{
			_eventData = ResourceLoader.Load<EventData>($"res://Code/EventData/PopupEvents/{islandNumber}/Event{eventNumber}.tres");
			GD.Print($"Loaded event data from: res://Code/EventData/PopupEvents/{islandNumber}/Event{eventNumber}.tres");
			_eventDictionary = _eventData.EventDictionary;
			_currentEventID = _eventData.EventID.ToString();
			
			_eventDescription = GetNode<Label>("Panel/Description");
			
			// Get language
			switch (SaveData.Instance._language)
			{
				case "EN":
					_eventDescription.Text = _eventData.EventDescEN;
					_eventOptions = _eventData.EventOptionsEN;
					break;
				case "FI":
					_eventDescription.Text = _eventData.EventDescFI;
					_eventOptions = _eventData.EventOptionsFI;
					break;
			}

			SetupOptionButton(1);
			SetupOptionButton(2);
		}
		
		private void SetupOptionButton(int index)
		{
			var _button = _panel.GetNode<TextureButton>($"HBoxContainer/Button{index}");
			var _buttonLabel = _button.GetNode<Label>("Label");
			var _buttonIcon = _button.GetNode<TextureRect>("TextureRect");
			
			_button.Pressed += () => OnButtonPressed(index);
			_buttonLabel.Text = _eventOptions[index - 1];
			
			EventOutcomeData outcome = (EventOutcomeData)_eventData.EventDictionary[_currentEventID + "_" + index];
			switch (outcome.OptionSeverity)
			{
				case EventOutcomeData.Severity.Low:
					_buttonIcon.Texture = (Texture2D)GD.Load("res://Assets/Resources/small_change.png");
					break;
				case EventOutcomeData.Severity.Medium:
					_buttonIcon.Texture = (Texture2D)GD.Load("res://Assets/Resources/medium_change.png");
					break;
				case EventOutcomeData.Severity.High:
					_buttonIcon.Texture = (Texture2D)GD.Load("res://Assets/Resources/large_change.png");
					break;
			}
		}
		private void OnButtonPressed(int index)
		{
			ButtonPressed(index);
		}

		private async void ButtonPressed(int index)
		{
			SoundPlayer.Instance.PlayEventButtonSound();
			
			string outcomeKey = _currentEventID + "_" + index;
			EventOutcomeData outcome = (EventOutcomeData)_eventDictionary[outcomeKey];
			
			TweenWindowDown();
			await ToSignal(_tween, Tween.SignalName.Finished);
			
			if (outcome.IsIslandSpecific)
			{
				outcome.TargetIsland = _parentMarker.TargetIsland;
			}

			ResourceManager.Instance.HandleOptionOutcomes(outcome);
			
			EmitSignal(SignalName.PopUpEventAnswered);
			
			Visible = false;
		}

		public void TweenWindowUp()
		{
			// Create a new tween
			_tween = CreateTween();
			
			// Configure and start the tween
			_tween.TweenProperty(this, "position", _targetPosition , 0.5f)
				.SetTrans(Tween.TransitionType.Quad)
				.SetEase(Tween.EaseType.Out);
				
		
		}
		public void TweenWindowDown()
		{
			// Create a new tween
			_tween = CreateTween();
			
			// Configure and start the tween
			_tween.TweenProperty(this, "position", _originalPosition, 0.25f)
				.SetTrans(Tween.TransitionType.Quad)
				.SetEase(Tween.EaseType.Out);
				
			
		}
		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}
	}
}
