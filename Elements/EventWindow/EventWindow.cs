    using Godot;
using Godot.Collections;
using System;

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
		
		private string _currentEventID = "";

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			int _eventNumber = GD.RandRange(1, 1);
			LoadEventData(_eventNumber);
		}

		public void LoadEventData(int eventNumber)
		{
			_currentEvent = ResourceLoader.Load<EventData>("res://Code/EventData/TurnEvents/TestEvent" + eventNumber + ".tres");
			_currentEventID = eventNumber.ToString();
			_eventDictionary = _currentEvent.EventDictionary;

			_eventTitle = GetNode<RichTextLabel>("TurnEventTitle");
			_eventTitle.Text = _currentEvent.EventTitle;

			_eventContent = GetNode<RichTextLabel>("TurnEventContent");
			_eventContent.Text = _currentEvent.EventDesc;

			SetupOptionButton(2, "OptionContainer2", _currentEvent.EventOptions.Length > 1);
			SetupOptionButton(1, "OptionContainer1", _currentEvent.EventOptions.Length > 0);
			SetupOptionButton(3, "OptionContainer3", _currentEvent.EventOptions.Length > 2);
			SetupOptionButton(4, "OptionContainer4", _currentEvent.EventOptions.Length > 3);
		}

		private void SetupOptionButton(int index, string containerName, bool isVisible)
		{
			if (isVisible)
			{
				var optionButton = GetNode<TextureButton>($"OptionsContainer/{containerName}/OptionButton{index}/TextureButton");
				var optionButtonText = GetNode<Label>($"OptionsContainer/{containerName}/OptionButton{index}/Label");
				var optionButtonContainer = GetNode<OptionButton>($"OptionsContainer/{containerName}/OptionButton{index}");
				var optionButtonIcon = GetNode<TextureRect>($"OptionsContainer/{containerName}/OptionButton{index}/TextureRect");

				optionButton.Pressed += () => OnOptionButtonPressed(index);
				optionButtonText.Text = _currentEvent.EventOptions[index - 1];
				EventOutcomeData outcome = (EventOutcomeData)_currentEvent.EventDictionary[_currentEventID + "_" + index];
				switch (outcome.OptionSeverity)
				{
					case 1:
						optionButtonIcon.Texture = (Texture2D)GD.Load("res://Assets/Resources/small_change.png");
						GD.Print("Low");
						break;
					case 2:
						optionButtonIcon.Texture = (Texture2D)GD.Load("res://Assets/Resources/medium_change.png");
						GD.Print("Med");
						break;
					case 3:
						optionButtonIcon.Texture = (Texture2D)GD.Load("res://Assets/Resources/large_change.png");
						GD.Print("High");
						break;
				}
				optionButtonContainer.Visible = true;
			}
		}

		public void OnOptionButtonPressed(int optionIndex)
		{
			string outcomeKey = _currentEventID + "_" + optionIndex;
			var outcome = _eventDictionary[outcomeKey];
			ResourceManager.Instance.HandleOptionOutcomes((EventOutcomeData)outcome);
			EmitSignal(SignalName.EventAnswered);
		}
	}
}
