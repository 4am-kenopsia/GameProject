using Godot;
using System;

namespace MapGame
{
	public partial class GameScene : Node
	{
		private PackedScene _eventWindowScene;
		private EventWindow _eventWindow;
		private TextureButton _turnButton;
		public static bool isEventRunning = false;
		[Export] private PackedScene markerScene;
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{	
			markerScene = GD.Load<PackedScene>("res://Elements/PopupMarker/MarkerContainer.tscn");
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
			isEventRunning = true;
		}
		public void OnTurnButtonPressed()
		{
			if (isEventRunning)
			{
				GD.Print("Event is already running");
				return;
			}
			GD.Print("TurnButton Pressed");
			SaveData.Instance.IncreaseTurn();
			GUI.UpdateLabels();
			GD.Print("Turn: " + SaveData.Instance._currentTurn);
			CreateTurnEvent();
			CreatePopUpMarker();
		}
		public void OnEventAnswered()
		{
            GUI.UpdateLabels();
			RemoveChild(_eventWindow);
			isEventRunning = false;
			
		}
		public void OnPopUpEventAnswered()
		{
			isEventRunning = false;
			
		}
		public void OnPopUpEventOpened()
		{
			isEventRunning = true;
			
		}
		private void CreatePopUpMarker()
		{
			// Instance the child scene
			
			MarkerContainer markerInstance = (MarkerContainer)markerScene.Instantiate(); 
			markerInstance.PopUpEventAnswered += OnPopUpEventAnswered;
			markerInstance.PopUpEventOpened += OnPopUpEventOpened;

			// Generate a random position within the scene
			Vector2 randomPosition = new Vector2(
				(float)GD.RandRange(0, 1405),
				(float)GD.RandRange(0, 854)
			);
			
			// Set the position of the child instance
			markerInstance.Position = randomPosition;

			// Add the child instance to the scene
			
			AddChild(markerInstance);
	
		}

	}
}
