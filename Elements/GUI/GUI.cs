using Godot;
using System;

namespace MapGame
{

    public partial class GUI : Control
    {
        [Signal] public delegate void TurnButtonPressedEventHandler();

        private static Label _turnLabel;
        private static Label _mainResourceLabel;
        private static Label _happinessLabel;
        private static Label _thirdResourceLabel;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _turnLabel = GetNode<Label>("SideUI/TurnLabel");
            _mainResourceLabel = GetNode<Label>("SideUI/MainResourceLabel");
            _happinessLabel = GetNode<Label>("SideUI/HappinessLabel");
            _thirdResourceLabel = GetNode<Label>("SideUI/ThirdResourceLabel");
            UpdateLabels();
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
        }

        public static void UpdateLabels()
        {
            _mainResourceLabel.Text = ResourceManager.Instance._currentMainResource.ToString();
            _happinessLabel.Text = ResourceManager.Instance._currentHappiness.ToString() + "%";
            _thirdResourceLabel.Text = ResourceManager.Instance._currentThirdResource.ToString();
            _turnLabel.Text = "Turn " + SaveData.Instance._currentTurn.ToString();
        }
    }
}