using Godot;
using System;

namespace MapGame
{
    public partial class EventWindow : Control
    {
        [Export] private string _optionButtonScenePath = "res://Elements/EventWindow/EventOptionButton.tscn";
        
        private PackedScene _optionButtonScene = null;
        
        private RichTextLabel _eventTitle = null;
        private RichTextLabel _eventContent = null;
        private OptionButton _optionButton = null;
        private RichTextLabel _optionButtonText = null;
        private CenterContainer _optionContainer = null;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            TurnEventData _currentEvent = GetEvent();
            _eventTitle = GetNode<RichTextLabel>("TurnEventTitle");
            _eventTitle.Text = _currentEvent.EventTitle;
            _eventContent = GetNode<RichTextLabel>("TurnEventContent");
            _eventContent.Text = _currentEvent.EventDesc;
            GD.Print(_eventContent.Text + _eventTitle.Text);
            for (int i = 1; i <= _currentEvent.EventOptions.Length; i++)
            {
                string _currentButton = i.ToString();
                string _buttonContent = _currentEvent.EventOptions[i - 1];
                CreateOptionButton(_currentButton, _buttonContent);
            }
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
        }
        
        public static TurnEventData GetEvent()
        {
            int _eventNumber = 1; /*GD.RandRange(1, 4);*/
            var _turnEvent = ResourceLoader.Load<TurnEventData>("res://Code/EventData/TurnEvents/TestEvent" + _eventNumber + ".tres");
            return _turnEvent;
        }
        //TODO: add button contents string input
        public void CreateOptionButton(string optionNumber, string buttonContent)
        {
            if (_optionButton != null)
            {
                //_optionButton.QueueFree;
                _optionButton = null;
            }
            
            // Checking that the button scene exists
            if (_optionButtonScene == null)
            {
                _optionButtonScene = ResourceLoader.Load<PackedScene>(_optionButtonScenePath);
                if (_optionButtonScene == null)
                {
                    GD.PrintErr("Can't load option button scene!");
                    return;
                }
            }
            
            // Getting the right container to put the button in
            _optionContainer = GetNode<CenterContainer>("OptionsContainer/OptionContainer" + optionNumber);
            
            // Creating the button node
            _optionButton = _optionButtonScene.Instantiate<OptionButton>();
            _optionContainer.AddChild(_optionButton);
            
            // Getting the button scene's RichTextLabel node
            _optionButtonText = GetNode<RichTextLabel>("OptionsContainer/OptionContainer" + optionNumber + "/OptionButton/RichTextLabel");
            
            // Setting the button's text
            _optionButtonText.Text = buttonContent;
            
            // Renames the newly created button
            _optionButton.Name = "OptionButton" + optionNumber;
        }
    }
}