using Godot;
using Godot.Collections;
using System;

namespace MapGame
{
    public partial class EventWindow : Control
    {
        [Signal] public delegate void OptionButton1PressedEventHandler();
        [Signal] public delegate void OptionButton2PressedEventHandler();
        [Signal] public delegate void OptionButton3PressedEventHandler();
        [Signal] public delegate void OptionButton4PressedEventHandler();
        
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
        //private TurnEventOutcomeData _turnEventOutcome = null;
        private TurnEventData _currentEvent = null;
        private Dictionary _eventDictionary = null;
        
        private string _currentEventID = "";

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            // Edit this with the number of events
            int _eventNumber = GD.RandRange(1, 2);
            LoadEventData(_eventNumber);
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
        }
        public void LoadEventData(int eventNumber)
        {
            // Get the event resource file.
            _currentEvent = ResourceLoader.Load<TurnEventData>("res://Code/EventData/TurnEvents/TestEvent" + eventNumber + ".tres");
            _currentEventID = eventNumber.ToString();
            
            // Getting the outcome dictionary.
            _eventDictionary = _currentEvent.EventDictionary;
            
            // Get and set the event window's title.
            _eventTitle = GetNode<RichTextLabel>("TurnEventTitle");
            _eventTitle.Text = _currentEvent.EventTitle;
            
            // Get and set the event window's contents.
            _eventContent = GetNode<RichTextLabel>("TurnEventContent");
            _eventContent.Text = _currentEvent.EventDesc;
            
            // Setting the button's text and visibility and connecting it's signal to a local method.
            if (_currentEvent.EventOptions.Length > 0)
            {
                _optionButton1 = GetNode<TextureButton>("OptionsContainer/OptionContainer1/OptionButton1/TextureButton");
                _optionButton1Text = GetNode<RichTextLabel>("OptionsContainer/OptionContainer1/OptionButton1/RichTextLabel");
                _optionButton1Container = GetNode<OptionButton>("OptionsContainer/OptionContainer1/OptionButton1");
                _optionButton1.Pressed += OnOptionButton1Pressed;
                _optionButton1Text.Text = _currentEvent.EventOptions[0];
                _optionButton1Container.Visible = true;
            }
            
            if (_currentEvent.EventOptions.Length > 1)
            {
                _optionButton2 = GetNode<TextureButton>("OptionsContainer/OptionContainer2/OptionButton2/TextureButton");
                _optionButton2Text = GetNode<RichTextLabel>("OptionsContainer/OptionContainer2/OptionButton2/RichTextLabel");
                _optionButton2Container = GetNode<OptionButton>("OptionsContainer/OptionContainer2/OptionButton2");
                _optionButton2.Pressed += OnOptionButton2Pressed;
                _optionButton2Text.Text = _currentEvent.EventOptions[1];
                _optionButton2Container.Visible = true;
            }
            
            if (_currentEvent.EventOptions.Length > 2)
            {
                _optionButton3 = GetNode<TextureButton>("OptionsContainer/OptionContainer3/OptionButton3/TextureButton");
                _optionButton3Text = GetNode<RichTextLabel>("OptionsContainer/OptionContainer3/OptionButton3/RichTextLabel");
                _optionButton3Container = GetNode<OptionButton>("OptionsContainer/OptionContainer3/OptionButton3");
                _optionButton3.Pressed += OnOptionButton3Pressed;
                _optionButton3Text.Text = _currentEvent.EventOptions[2];
                _optionButton3Container.Visible = true;
            }
            
            if (_currentEvent.EventOptions.Length > 3)
            {
                _optionButton4 = GetNode<TextureButton>("OptionsContainer/OptionContainer4/OptionButton4/TextureButton");
                _optionButton4Text = GetNode<RichTextLabel>("OptionsContainer/OptionContainer4/OptionButton4/RichTextLabel");
                _optionButton4Container = GetNode<OptionButton>("OptionsContainer/OptionContainer4/OptionButton4");
                _optionButton4.Pressed += OnOptionButton4Pressed;
                _optionButton4Text.Text = _currentEvent.EventOptions[3];
                _optionButton4Container.Visible = true;
            }
        }
        public void OnOptionButton1Pressed()
        {
            GD.Print("B1");
            string _outcome = _currentEventID + "_1";
            var _turnEventOutcome = _eventDictionary[_outcome];
            HandleOptionOutcomes((TurnEventOutcomeData)_turnEventOutcome);
        }
        public void OnOptionButton2Pressed()
        {
            string _outcome = _currentEventID + "_2";
            var _turnEventOutcome = _eventDictionary[_outcome];
            HandleOptionOutcomes((TurnEventOutcomeData)_turnEventOutcome);
        }
        public void OnOptionButton3Pressed()
        {
            string _outcome = _currentEventID + "_3";
            var _turnEventOutcome = _eventDictionary[_outcome];
            HandleOptionOutcomes((TurnEventOutcomeData)_turnEventOutcome);
        }
        public void OnOptionButton4Pressed()
        {
            string _outcome = _currentEventID + "_4";
            var _turnEventOutcome = _eventDictionary[_outcome];
            HandleOptionOutcomes((TurnEventOutcomeData)_turnEventOutcome);
        }
        
        public void HandleOptionOutcomes(TurnEventOutcomeData outcome)
        {
            ResourceManager.Instance.IncreaseHappiness(outcome.HappinessChange);
            GD.Print(ResourceManager.Instance._currentHappiness);
            ResourceManager.Instance.IncreaseMainResource(outcome.MainResourcehange);
            GD.Print(ResourceManager.Instance._currentMainResource);
            ResourceManager.Instance.IncreaseThirdResource(outcome.ThirdResourceChange);
            GD.Print(ResourceManager.Instance._currentThirdResource);
        }
    }
}