using Godot;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace MapGame
{
	public partial class GameScene : Node
	{
		private PackedScene _eventWindowScene;
		private EventWindow _eventWindow;
		private TextureButton _turnButton;
		private TextureRect _newDayIndicator;
		private Label _newDayLabel;
		private AnimationPlayer _animationPlayer;
		public static bool isEventRunning = false;
		[Export] private PackedScene markerScene;
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{	
			markerScene = GD.Load<PackedScene>("res://Elements/PopupMarker/MarkerContainer.tscn");
			_turnButton = GetNode<TextureButton>("UI/SideUI/TurnButton");
			_newDayIndicator = GetNode<TextureRect>("UI/NewDayIndicator");
			_newDayLabel = GetNode<Label>("UI/NewDayIndicator/NewDayLabel");
			_animationPlayer = GetNode<AnimationPlayer>("UI/AnimationPlayer");
			
			_turnButton.Pressed += OnTurnButtonPressed;
			SoundPlayer.Instance.PlayAmbience();
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}
		

		public void CreateTurnEvent()
		{
			GD.Print("meow");
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
			NextTurn();
		}
		public async Task NextTurn()
		{
			if (isEventRunning)
			{
				GD.Print("Event is already running");
				return;
			}
			isEventRunning = true;
			SoundPlayer.Instance.PlayNextTurnSound();
			GD.Print("TurnButton Pressed");
			if (SaveData.Instance._currentTurn == 3)
			{
				SceneTransition.Instance.TransitionToScene("res://GameScenes/DayEndScene.tscn");
				return;
			}
			SoundPlayer.Instance.PlayTicking();
			SaveData.Instance.IncreaseTurn();
			GUI.UpdateLabels();
			GD.Print("Turn: " + SaveData.Instance._currentTurn);
			_newDayLabel.Text = "TURN " + SaveData.Instance._currentTurn;
			_animationPlayer.Play("newday");

			await WaitForAnimationToFinish();
			
			CreateTurnEvent();
			CreatePopUpMarker();
		}
		
		public void OnEventAnswered()
		{
			GUI.UpdateLabels();
			GD.Print("Event answered received - resetting flag");
			RemoveChild(_eventWindow);
			isEventRunning = false;
			
		}
		public void OnPopUpEventAnswered(EventOutcomeData outcome)
		{
			ResourceManager.Instance.HandleOptionOutcomes(outcome);
			GUI.UpdateLabels();
			
					 // Print happiness for all islands
			var allHappiness = ResourceManager.Instance.GetAllIslandHappiness();
			foreach (var entry in allHappiness)
			{
				GD.Print($"Island {(int)entry.Key + 1} Happiness: {entry.Value}%");
			}
			isEventRunning = false;
			
		}
		public void OnPopUpEventOpened()
		{
			isEventRunning = true;
			
		}
		//private void CreatePopUpMarker()
		//{
			//// Instance the child scene
			//
			//MarkerContainer markerInstance = (MarkerContainer)markerScene.Instantiate(); 
			//markerInstance.PopUpEventAnswered += OnPopUpEventAnswered;
			//markerInstance.PopUpEventOpened += OnPopUpEventOpened;
//
			//// Generate a random position within the scene
			//Vector2 randomPosition = new Vector2(
				//(float)GD.RandRange(0, 1405),
				//(float)GD.RandRange(0, 854)
			//);
			 //
			//// Set the position of the child instance
			//markerInstance.Position = randomPosition;
//
			//// Add the child instance to the scene
			//
			//AddChild(markerInstance);
	//
		//}
		private void CreatePopUpMarker()
		{
			// Instance the marker
			MarkerContainer markerInstance = (MarkerContainer)markerScene.Instantiate(); 
			markerInstance.PopUpEventAnswered += OnPopUpEventAnswered;
			markerInstance.PopUpEventOpened += OnPopUpEventOpened;

			// Select a random island
			RandomNumberGenerator rng = new();
			rng.Randomize();
			Island randomIsland = (Island)rng.RandiRange(0, 7); // 0-7 for 8 islands
			
			// Get position on island
			Vector2 randomPosition = GetRandomPositionInsideIsland(randomIsland);
			
			// Set position and island reference
			markerInstance.Position = randomPosition;
			markerInstance.TargetIsland = randomIsland; // Add this property to MarkerContainer

			AddChild(markerInstance);
			
			// Optional: Affect island happiness
			ResourceManager.Instance.ChangeIslandHappiness(randomIsland, -5); // Small happiness penalty
		}
		private Vector2 GetRandomPositionInsideIsland(Island island)
		{
			// Get the Area2D node for the selected island
			string nodePath = $"TextureRect/{island}";
			Area2D islandArea = GetNode<Area2D>(nodePath);
			
			// Get all collision polygons for this island
			var polygons = islandArea.GetChildren()
								   .OfType<CollisionPolygon2D>()
								   .ToList();
			
			if (polygons.Count == 0)
			{
				GD.PrintErr($"No collision polygons found for {island}");
				return islandArea.GlobalPosition;
			}

			// Select a random polygon
			RandomNumberGenerator rng = new();
			rng.Randomize();
			CollisionPolygon2D selectedPolygon = polygons[rng.RandiRange(0, polygons.Count - 1)];
			Vector2[] polygonPoints = selectedPolygon.Polygon;

			// Calculate bounds
			Vector2 min = polygonPoints[0];
			Vector2 max = polygonPoints[0];
			
			foreach (Vector2 point in polygonPoints)
			{
				min = new Vector2(Mathf.Min(min.X, point.X), Mathf.Min(min.Y, point.Y));
				max = new Vector2(Mathf.Max(max.X, point.X), Mathf.Max(max.Y, point.Y));
			}

			// Try random points
			for (int i = 0; i < 100; i++)
			{
				Vector2 localPoint = new(
					rng.RandfRange(min.X, max.X),
					rng.RandfRange(min.Y, max.Y)
				);
				
				if (Geometry2D.IsPointInPolygon(localPoint, polygonPoints))
				{
					return selectedPolygon.ToGlobal(localPoint);
				}
			}
			
			return islandArea.GlobalPosition;
		}
		private async Task WaitForAnimationToFinish()
		{
			var tcs = new TaskCompletionSource<bool>();
			
		void OnAnimationFinished(StringName animationName)
			{
				if (animationName == "newday")
				{
					tcs.SetResult(true);
				}
			}
			_animationPlayer.AnimationFinished += OnAnimationFinished;
			await tcs.Task;
			_animationPlayer.AnimationFinished -= OnAnimationFinished;
		}

	}
}
