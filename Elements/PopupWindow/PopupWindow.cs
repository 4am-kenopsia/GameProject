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

		private Button _button1;
		private Button _button2;
		private Panel _panel;
		private Label _description;
		private TextureRect _img;
		private int _popUpEventNumber;
		private string _currentEventID;
		private Texture2D _newTexture;
		private Dictionary _eventDictionary = null;
		//private PopupEventData _currentEvent = null;
		//public int _eventsHappened { get; set; } = 0;
		
		

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			_popUpEventNumber = GD.RandRange(1, 4);
			
			_button1 = GetNode<Button>("Panel/Button");
			_button2 = GetNode<Button>("Panel/Button2");
			_panel = GetNode<Panel>("Panel");
			_img = GetNode<TextureRect>("Panel/img");
			_description = GetNode<Label>("Panel/Description");
			LoadData();
			
			TweenSelf();
			TweenLabel();
			TweenButton();
			

			// Connect the button's "pressed" signal to local methods
			_button1.Pressed += OnButton1Pressed;
			_button2.Pressed += OnButton2Pressed;
			
		}
		
		private void LoadData() 
		{
			// Load the .tres file
			var eventData = ResourceLoader.Load<EventData>("res://Code/EventData/PopupEvents/TestEvent" + _popUpEventNumber + ".tres");
			_eventDictionary = eventData.EventDictionary;
			_currentEventID = _popUpEventNumber.ToString();
			
			// var eventData = ResourceLoader.Load<PopupEventData>("res://Code/PopupData/PopupEvents/TestEvent" + _eventsHappened + ".tres");
			GD.Print(eventData);
			if (eventData != null)
			{
				// Update the Label with the data
				_eventLabel.Text = $"{eventData.EventTitle}";
				_description.Text = $"{eventData.EventDesc}";
				if (!string.IsNullOrEmpty(eventData.EventPicture))
				{
					_newTexture = GD.Load<Texture2D>(eventData.EventPicture);
					GD.Print("pic:" + eventData.EventPicture);
					_img.Texture = _newTexture;
				}
				// Example of accessing other properties
				_button1.SetText(eventData.EventOptions[0]);
				_button2.SetText(eventData.EventOptions[1]);
				if (eventData.EventOptions != null)
				{
					foreach (var option in eventData.EventOptions)
					{
						GD.Print($"Option: {option}");
						
					}
				}

				if (eventData.EventDictionary != null)
				{
					foreach (var key in eventData.EventDictionary.Keys)
					{
						GD.Print($"Key: {key}, Value: {eventData.EventDictionary[key]}");
					}
				}
			}
			else
			{
				GD.PrintErr("Failed to load PopupEventData resource.");
			}
		
		}

		private void OnButton1Pressed()
		{
			// Emit the custom signal
			string outcomeKey = _currentEventID + "_" + 1;
			var outcome = _eventDictionary[outcomeKey];
			ResourceManager.Instance.HandleOptionOutcomes((EventOutcomeData)outcome);
			EmitSignal(SignalName.ButtonPressed1);
			GD.Print("lol1");
			
			//GD.Print(_eventsHappened);
			Visible = false;
			
	
		}

		private void OnButton2Pressed()
		{
			string outcomeKey = _currentEventID + "_" + 2;
			var outcome = _eventDictionary[outcomeKey];
			ResourceManager.Instance.HandleOptionOutcomes((EventOutcomeData)outcome);
			
			EmitSignal(SignalName.ButtonPressed2);
			GD.Print("lol2");
			Visible = false;
			
			
			
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
