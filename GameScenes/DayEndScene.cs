using Godot;
using System;

namespace MapGame
{

    public partial class DayEndScene : Control
    {
        private TextureButton _continueButton;
        private Label _title;
        private Label _content;
        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _continueButton = GetNode<TextureButton>("TextureRect/TextureButton");
            _title = GetNode<Label>("TextureRect/Label");
            _content = GetNode<Label>("TextureRect/Label2");

            _continueButton.Pressed += OnContinueButtonPressed;
            DayResults();
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
        }

        public void DayResults()
        {
            _title.Text = $"DAY {SaveData.Instance._currentDay} SURVIVED";
            _content.Text = "Salary: " + SaveData.Instance._currentMagicMultiplier + "x " + SaveData.Instance._currentSalary;
            ResourceManager.Instance.ChangeMagic(SaveData.Instance._currentMagicMultiplier * SaveData.Instance._currentSalary);
        }

        public void OnContinueButtonPressed()
        {
			SaveData.Instance.IncreaseDay();
			SaveData.Instance.SaveGame();
			GetTree().ChangeSceneToFile("res://GameScenes/GameScene.tscn");
		}
	}
}