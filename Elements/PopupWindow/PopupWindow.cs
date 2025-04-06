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


		private Button _button1;
		private Button _button2;
		private Panel _panel;
		private Label _description;
		private TextureRect _img;
		private int _popUpEventNumber;
		private string _currentEventID;
		private Texture2D _newTexture;
		private Dictionary _eventDictionary = null;
		public Island TargetIsland { get; set; }  // Add this property

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
		//private PopupEventData _currentEvent = null;
		//public int _eventsHappened { get; set; } = 0;
		
		

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
{
	// Get UI elements safely
	_panel = GetNode<Panel>("Panel");
	_button1 = _panel.GetNode<Button>("Button");
	_button2 = _panel.GetNode<Button>("Button2");
	_img = _panel.GetNode<TextureRect>("img");
	_description = _panel.GetNode<Label>("Description");
	_eventLabel = _panel.GetNode<Label>("EventLabel");

	// Get event from EventLoader
	var eventLoader = GetNode<EventLoader>("/root/EventLoader");
	var marker = GetParent() as MarkerContainer;
	var eventData = eventLoader.GetRandomEventForIsland(marker?.TargetIsland ?? Island.Island1);
	
	// Initialize event data
	EventData = eventData ?? new EventData();
	
	// Start animations
	TweenSelf();
	TweenLabel();
	TweenButton();
	
	// Connect signals
	_button1.Pressed += OnButton1Pressed;
	_button2.Pressed += OnButton2Pressed;
}
		
		//private void LoadData() 
		//{
			//// Load the .tres file
			//var eventData = ResourceLoader.Load<EventData>("res://Code/EventData/PopupEvents/TestEvent" + _popUpEventNumber + ".tres");
			//_eventDictionary = eventData.EventDictionary;
			//_currentEventID = _popUpEventNumber.ToString();
			//
			//// var eventData = ResourceLoader.Load<PopupEventData>("res://Code/PopupData/PopupEvents/TestEvent" + _eventsHappened + ".tres");
			//GD.Print(eventData);
			//if (eventData != null)
			//{
				//// Update the Label with the data
				//_eventLabel.Text = $"{eventData.EventTitle}";
				//_description.Text = $"{eventData.EventDesc}";
				//if (!string.IsNullOrEmpty(eventData.EventPicture))
				//{
					//_newTexture = GD.Load<Texture2D>(eventData.EventPicture);
					//GD.Print("pic:" + eventData.EventPicture);
					//_img.Texture = _newTexture;
				//}
				//// Example of accessing other properties
				//_button1.SetText(eventData.EventOptions[0]);
				//_button2.SetText(eventData.EventOptions[1]);
				//if (eventData.EventOptions != null)
				//{
					//foreach (var option in eventData.EventOptions)
					//{
						//GD.Print($"Option: {option}");
						//
					//}
				//}
//
				//if (eventData.EventDictionary != null)
				//{
					//foreach (var key in eventData.EventDictionary.Keys)
					//{
						//GD.Print($"Key: {key}, Value: {eventData.EventDictionary[key]}");
					//}
				//}
			//}
			//else
			//{
				//GD.PrintErr("Failed to load PopupEventData resource.");
			//}
		//
		//}
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
				
				_button1.Text = eventData.EventOptions[0];
				_button2.Text = eventData.EventOptions[1];
			}
		}

		private void OnButton1Pressed()
		{
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

		private void OnButton2Pressed()
		{
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
		_button1.Text = eventData.EventOptions[0] ?? "Continue";
		_button2.Text = eventData.EventOptions[1] ?? "Cancel";
	}
	else
	{
		_button1.Text = "Continue";
		_button2.Text = "Cancel";
	}
}


		//public void OnShitBought()
		//{
			//GD.Print("Shit was bought! Updating PopupWindow...");
			//_increase = _increase + 1;
			//// Add logic to update the PopupWindow UI or perform other actions
		//}
		
		
		 private void TweenButton()
	{
		// Tween the button's position
		Tween _tween = GetTree().CreateTween();

		// Tween the button's scale
		_tween.TweenProperty(_button1, "scale", new Vector2(1f, 1f), 0.5f)
			  .SetTrans(Tween.TransitionType.Back) // Add a slight overshoot
			  .SetEase(Tween.EaseType.InOut); // Ease in and out
			
		_tween.TweenProperty(_button2, "scale", new Vector2(1f, 1f), 0.5f)
			  .SetTrans(Tween.TransitionType.Back) // Add a slight overshoot
			  .SetEase(Tween.EaseType.InOut); // Ease in and out

		// Start the tween
		_tween.Play();
	}
	 private void TweenLabel()
	{
		// Tween the button's position
		_eventLabel.VisibleRatio = 0;
		_description.VisibleRatio = 0;
		Tween _tween = GetTree().CreateTween();

		_tween.TweenProperty(_eventLabel, "visible_ratio", 1.0f, 0.5f)
			.SetTrans(Tween.TransitionType.Linear) // Smooth linear transition
			.SetEase(Tween.EaseType.InOut); // Ease in and out for smooth animation
				
		_tween.TweenProperty(_description, "visible_ratio", 1.0f, 0.5f)
			.SetTrans(Tween.TransitionType.Linear) // Smooth linear transition
			.SetEase(Tween.EaseType.InOut); // Ease in and out for smooth animation
			


		// Start the tween
		_tween.Play();
	}
	 private void TweenSelf()
	{
		// Tween the button's position
		Tween _tween = GetTree().CreateTween();

		_tween.TweenProperty(_panel, "scale", new Vector2(1f, 1f), 0.25f)
			.SetTrans(Tween.TransitionType.Back) // Smooth linear transition
			.SetEase(Tween.EaseType.InOut); // Ease in and out for smooth animation


		// Start the tween
		_tween.Play();
	}


		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}
	}
}
