using Godot;
using System;

namespace MapGame
{
    public partial class GameScene : Node
    {
        private PackedScene _eventWindowScene;
        private EventWindow _eventWindow;
        private TextureButton _turnButton;
        private bool _isEventRunning = false;
        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _turnButton = GetNode<TextureButton>("UI/SideUI/TurnButtonContainer/TurnButton");
            _turnButton.Pressed += OnTurnButtonPressed;
            
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
        }

        public void CreateTurnEvent()
        {
            GD.Print("meow");
            // Temp tässä
            _eventWindowScene = ResourceLoader.Load<PackedScene>("res://Elements/EventWindow/EventWindow.tscn");
            if (_eventWindowScene == null)
            {
                GD.PrintErr("Event window scenepath missing.");
                return;
            }
            _eventWindow = _eventWindowScene.Instantiate<EventWindow>();
            AddChild(_eventWindow);
            _eventWindow.EventAnswered += OnEventAnswered;
            _isEventRunning = true;
        }
        public void OnTurnButtonPressed()
        {
            if (_isEventRunning)
            {
                GD.Print("Event is already running");
                return;
            }
            GD.Print("TurnButton Pressed");
            SaveData.Instance.IncreaseTurn();
            GUI.UpdateLabels();
            GD.Print("Turn: " + SaveData.Instance._currentTurn);
            CreateTurnEvent();
        }
        public void OnEventAnswered()
        {
            GUI.UpdateLabels();
            RemoveChild(_eventWindow);
            _isEventRunning = false;
        }
    }
}