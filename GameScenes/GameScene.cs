using Godot;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace MapGame
{
	public partial class GameScene : Node
	{
		private GUI _guiInstance;
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
			_guiInstance = GetNode<GUI>("GUI");
			markerScene = GD.Load<PackedScene>("res://Elements/PopupMarker/MarkerContainer.tscn");
			_turnButton = GetNode<TextureButton>("GUI/SideUI/TurnButton");
			_newDayIndicator = GetNode<TextureRect>("GUI/NewDayIndicator");
			_newDayLabel = GetNode<Label>("GUI/NewDayIndicator/NewDayLabel");
			_animationPlayer = GetNode<AnimationPlayer>("GUI/AnimationPlayer");
			
			
			
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
			isEventRunning = true;
			_eventWindowScene = ResourceLoader.Load<PackedScene>("res://Elements/EventWindow/EventWindow.tscn");
			if (_eventWindowScene == null)
			{
				GD.PrintErr("Event window scenepath missing.");
				return;
			}
			_eventWindow = _eventWindowScene.Instantiate<EventWindow>();
			AddChild(_eventWindow);
			_eventWindow.EventAnswered += OnEventAnswered;
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
			
			
			SoundPlayer.Instance.PlayNextTurnSound();
			GD.Print("TurnButton Pressed");
			if (SaveData.Instance._currentTurn == 3)
			{
				SceneTransition.Instance.TransitionToScene("res://GameScenes/DayEndScene.tscn");
				return;
			}
			isEventRunning = true;
			SoundPlayer.Instance.PlayTicking();
			SaveData.Instance.IncreaseTurn();
			_guiInstance.UpdateLabels();
			
			GD.Print("Turn: " + SaveData.Instance._currentTurn);
			
			_newDayLabel.Text = "TURN " + SaveData.Instance._currentTurn;
			
			// Add tween anim
			// wait for tween anim to finish
			
			// await WaitForAnimationToFinish();
			
			CreateTurnEvent();
			
			
			
			
		}
		
		public void OnEventAnswered()
		{
			_guiInstance.UpdateLabels();
			GD.Print("Event answered received - resetting flag");
			RemoveChild(_eventWindow);
			isEventRunning = false;
			Random random = new Random();
			int randomNumber = random.Next(1, 3);
			for(int i = 0; i < randomNumber; i++)
			{
				CreatePopUpMarker();
			}
			
		}
		public void OnPopUpEventAnswered(EventOutcomeData outcome)
		{
			ResourceManager.Instance.HandleOptionOutcomes(outcome);
			_guiInstance.UpdateLabels();
			
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
		private void CreatePopUpMarker()
		{	
			SoundPlayer.Instance.PlayPopUpSpawnSound();
			// Instance the marker
			
			MarkerContainer markerInstance = (MarkerContainer)markerScene.Instantiate(); 
			markerInstance.PopUpEventAnswered += OnPopUpEventAnswered;
			markerInstance.PopUpEventOpened += OnPopUpEventOpened;

			// Select a random island
			RandomNumberGenerator rng = new();
			rng.Randomize();
			Island randomIsland = (Island)rng.RandiRange(0, 5); // 0-5 for 6 areas
			
			// Get position on island
			Vector2 randomPosition = GetRandomPositionInsideIsland(randomIsland);
			
			// Set position and island reference
			markerInstance.Position = randomPosition;
			markerInstance.TargetIsland = randomIsland; // Add this property to MarkerContainer

			AddChild(markerInstance);
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
