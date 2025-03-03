using Godot;
using System;

namespace MapGame
{
	public partial class TestScene : Node
	{
		[Export] private PackedScene childScene; // The scene of the child instance you want to spawn
		[Export] private PackedScene popUp;
		private Label _mainResourceLabel;
		private Label _happinessLabel;
		private Label _thirdResourceLabel;
		[Signal] public delegate void ShitBoughtEventHandler();
		private Button _button;

		
		
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			GD.Print(ResourceManager.Instance._currentHappiness);
			childScene = GD.Load<PackedScene>("res://Elements/PopupMarker/MarkerContainer.tscn");
			popUp = GD.Load<PackedScene>("res://Elements/PopupWindow/PopupWindow.tscn");
			_mainResourceLabel = GetNode<Label>("HBoxContainer/MainResourceLabel");
			_happinessLabel = GetNode<Label>("HBoxContainer/HappinessLabel");
			_thirdResourceLabel = GetNode<Label>("HBoxContainer/ThirdResourceLabel");
			SpawnChild();
			UpdateResourceLabels();
			_button = GetNode<Button>("Panel/BuyShit");

			// Connect the button's "pressed" signal to a local method
			_button.Pressed += BuyBuff;
			
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}
		private void SpawnChild()
		{
			
			if (childScene == null)
		{
			GD.PrintErr("Child scene is not assigned!");
			return;
		}

		// Instance the child scene
		MarkerContainer childInstance = (MarkerContainer)childScene.Instantiate(); 
		childInstance.ButtonPressed += OnChildButtonPressed;

		// Generate a random position within the scene
		Vector2 randomPosition = new Vector2(
			(float)GD.RandRange(0, 1405),
			(float)GD.RandRange(0, 854)
		);

		// Set the position of the child instance
		childInstance.Position = randomPosition;

		// Add the child instance to the scene
		AddChild(childInstance);
	}
		private void OnChildButtonPressed()
	{
		GD.Print("Button in the child scene was pressed!");
		SpawnPopUp();
		// Handle the button press event here
	}
	private void OnRightButtonPressed()
	{
		GD.Print("Button right pressed!");
		UpdateResourceLabels();
		// Handle the button press event here
	}
		private void OnLeftButtonPressed()
	{
		GD.Print("Button left pressed!");
		UpdateResourceLabels();
		// Handle the button press event here
	}
	private void SpawnPopUp()
	{
		PopupWindow popUpInstance = (PopupWindow)popUp.Instantiate();
		popUpInstance.ButtonPressed2 += OnRightButtonPressed;
		popUpInstance.ButtonPressed1 += OnLeftButtonPressed;
		ShitBought += popUpInstance.OnShitBought;
		Vector2 middleBottom = new Vector2( GetViewport().GetVisibleRect().Size.X / 2 , GetViewport().GetVisibleRect().Size.Y );
		popUpInstance.Position = middleBottom;
		AddChild(popUpInstance);
		GetNode<Panel>("Panel").Visible = true;
		
	}
	
	private void UpdateResourceLabels()
	{
		_mainResourceLabel.Text = $"Main Resource: {ResourceManager.Instance._currentMainResource}";
		_happinessLabel.Text = $"Happiness: {ResourceManager.Instance._currentHappiness}";
		_thirdResourceLabel.Text = $"Third Resource: {ResourceManager.Instance._currentThirdResource}";
	}
		private void BuyBuff()
	{
		// Emit the custom signal
		ResourceManager.Instance.DecreaseMainResource(300);
		UpdateResourceLabels();
		EmitSignal(SignalName.ShitBought);
		GD.Print("lol1");
	}
	}
}
