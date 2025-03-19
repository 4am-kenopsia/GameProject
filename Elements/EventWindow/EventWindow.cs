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
		private TextureButton _optionButton1 = null;
		private TextureButton _optionButton2 = null;
		private TextureButton _optionButton3 = null;
		private TextureButton _optionButton4 = null;
		private OptionButton _optionButton1Container = null;
		private OptionButton _optionButton2Container = null;
		private OptionButton _optionButton3Container = null;
		private OptionButton _optionButton4Container = null;
		private RichTextLabel _optionButton1Text = null;
		private RichTextLabel _optionButton2Text = null;
		private RichTextLabel _optionButton3Text = null;
		private RichTextLabel _optionButton4Text = null;
		private EventData _currentEvent = null;
		private Dictionary _eventDictionary = null;
		
		private string _currentEventID = "";

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			int _eventNumber = GD.RandRange(1, 4);
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

			SetupOptionButton(1, "OptionContainer1", _currentEvent.EventOptions.Length > 0);
			SetupOptionButton(2, "OptionContainer2", _currentEvent.EventOptions.Length > 1);
			SetupOptionButton(3, "OptionContainer3", _currentEvent.EventOptions.Length > 2);
			SetupOptionButton(4, "OptionContainer4", _currentEvent.EventOptions.Length > 3);
		}

		private void SetupOptionButton(int index, string containerName, bool isVisible)
		{
			if (isVisible)
			{
				var optionButton = GetNode<TextureButton>($"OptionsContainer/{containerName}/OptionButton{index}/TextureButton");
				var optionButtonText = GetNode<RichTextLabel>($"OptionsContainer/{containerName}/OptionButton{index}/RichTextLabel");
				var optionButtonContainer = GetNode<OptionButton>($"OptionsContainer/{containerName}/OptionButton{index}");

				optionButton.Pressed += () => OnOptionButtonPressed(index);
				optionButtonText.Text = _currentEvent.EventOptions[index - 1];
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
