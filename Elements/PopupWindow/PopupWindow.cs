using Godot;
using Godot.Collections;

namespace MapGame
{

	public partial class PopupWindow : Control
	{
		// Define custom signals
		[Signal]
		public delegate void ButtonPressed1EventHandler();
		
		[Export] private Label _eventLabel; // Assign this in the Godot editor

		[Signal]
		public delegate void ButtonPressed2EventHandler();
		
		[Signal]
		public delegate void PopUpEventAnsweredEventHandler(EventOutcomeData outcome);


		private TextureButton _button1;
		private TextureButton _button2;
		private Label _button1Label;
		private Label _button2Label;
		private TextureRect _panel;
		private Label _description;
		private TextureRect _img;
		private int _popUpEventNumber;
		private string _currentEventID;
		private Texture2D _newTexture;
		private Dictionary _eventDictionary = null;
		public Island TargetIsland { get; set; }  // Add this property
		private Vector2 _originalPosition;
		private Vector2 _targetPosition;
		private Tween _tween;

		private EventData _eventData;
		public EventData EventData
		
		{
			get => _eventData;
			set 
			{
				_eventData = value;
				if (_eventData != null)
				{
					LoadEventData(_eventData);
				}
			}
		}

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
		ColorRect _eater = GetNode<ColorRect>("ColorRect2");
		_eater.CustomMinimumSize = SaveData.Instance._viewPortSize;
		_eater.GlobalPosition = new Vector2(0, 0);
		// Get UI elements safely
		// Store original position
		_targetPosition = new Vector2(0, 0);
		
		// Set initial position (offscreen above)
		_originalPosition = new Vector2(_targetPosition.X, SaveData.Instance._viewPortSize.Y);
		Position = _originalPosition;
		//Animation here
		_panel = GetNode<TextureRect>("Panel");
		_button1 = _panel.GetNode<TextureButton>("HBoxContainer/Button1");
		_button2 = _panel.GetNode<TextureButton>("HBoxContainer/Button2");
		_button1Label = _panel.GetNode<Label>("HBoxContainer/Button1/Label");
		_button2Label = _panel.GetNode<Label>("HBoxContainer/Button2/Label");
		
		_img = _panel.GetNode<TextureRect>("img");
		_description = _panel.GetNode<Label>("Description");
		_eventLabel = _panel.GetNode<Label>("EventLabel");
	
		// Get event from EventLoader
		var eventLoader = GetNode<EventLoader>("/root/EventLoader");
		var marker = GetParent() as MarkerContainer;
		var eventData = eventLoader.GetRandomEventForIsland(marker?.TargetIsland ?? Island.Island1);
		
		// Initialize event data
		EventData = eventData ?? new EventData();
		
		
		// Connect signals
		_button1.Pressed += OnButton1Pressed;
		_button2.Pressed += OnButton2Pressed;
		TweenWindowUp();
		}

		private void LoadData() 
		{
			// Get the EventLoader instance
			var eventLoader = GetNode<EventLoader>("/root/EventLoader");
			
			// Get random event for current island (passed from MarkerContainer)
			var marker = GetParent() as MarkerContainer;
			var eventData = eventLoader.GetRandomEventForIsland(marker?.TargetIsland ?? Island.Island1);
			
			if (eventData != null)
			{
				_eventDictionary = eventData.EventDictionary;
				_currentEventID = eventData.EventID;
				
				// Update UI elements
				_eventLabel.Text = eventData.EventTitle;
				_description.Text = eventData.EventDesc;
				
				if (!string.IsNullOrEmpty(eventData.EventPicture))
				{
					_img.Texture = GD.Load<Texture2D>(eventData.EventPicture);
				}
				
				_button1Label.Text = eventData.EventOptions[0];
				_button2Label.Text = eventData.EventOptions[1];
			}
		}

		private async void OnButton1Pressed()
		{
			SoundPlayer.Instance.PlayEventButtonSound();
			TweenWindowDown();
			await ToSignal(_tween, Tween.SignalName.Finished);
			string outcomeKey = _currentEventID + "_" + 1;
			var outcome = (EventOutcomeData)_eventDictionary[outcomeKey];
			
			// Get parent marker if exists
			var marker = GetParent() as MarkerContainer;
			if (marker != null && outcome.IsIslandSpecific)
			{
				outcome.TargetIsland = marker.TargetIsland;
			}

			
			
			EmitSignal(SignalName.PopUpEventAnswered, outcome);
			EmitSignal(SignalName.ButtonPressed1);
			Visible = false;
		}

		private async void OnButton2Pressed()
		{
			SoundPlayer.Instance.PlayEventButtonSound();
			TweenWindowDown();
			await ToSignal(_tween, Tween.SignalName.Finished);
			string outcomeKey = _currentEventID + "_" + 2;
			var outcome = (EventOutcomeData)_eventDictionary[outcomeKey];
			
			// Get parent marker if exists
			var marker = GetParent() as MarkerContainer;
			if (marker != null && outcome.IsIslandSpecific)
			{
				outcome.TargetIsland = marker.TargetIsland;
			}
			
			EmitSignal(SignalName.PopUpEventAnswered, outcome);
			EmitSignal(SignalName.ButtonPressed2);
			Visible = false;
		}
		


		private void LoadEventData(EventData eventData)
		{
		// Initialize with empty dictionary if null
		_eventDictionary = eventData?.EventDictionary ?? new Godot.Collections.Dictionary();
		_currentEventID = eventData?.EventID ?? "default_event";
	
		// Safe UI updates with null checks
		if (_eventLabel != null)
			_eventLabel.Text = eventData?.EventTitle ?? "New Event";
		
		if (_description != null)
			_description.Text = eventData?.EventDesc ?? "An event occurred";
		// Safe image loading
		if (_img != null && !string.IsNullOrEmpty(eventData?.EventPicture))
		{
			_img.Texture = GD.Load<Texture2D>(eventData.EventPicture) 
						  ?? GD.Load<Texture2D>("res://fallback_texture.png");
		}
		// Safe button options (with array bounds checking)
		if (eventData?.EventOptions != null && eventData.EventOptions.Length >= 2)
		{
			_button1Label.Text = eventData.EventOptions[0] ?? "Continue";
			_button2Label.Text = eventData.EventOptions[1] ?? "Cancel";
		}
		else
		{
			_button1Label.Text = "Continue";
			_button2Label.Text = "Cancel";
		}
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
