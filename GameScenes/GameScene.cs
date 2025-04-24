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
		
		private Control _currentIslandPanel = null;
		private PackedScene IslandPanelScene;
		
		private TextureButton _turnButton;
		
		private TextureRect _newDayIndicator;
		private Label _newDayLabel;
		
		private AnimationPlayer _animationPlayer;
		
		public static bool isEventRunning = false;
		public bool _isPanelOpen = false;
		public bool _isMenuOpenBool = false;
		
		[Export] private PackedScene markerScene;
		
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			GD.Print("--");
			GD.Print("Game scene loaded");
			GD.Print($"Day: {SaveData.Instance._currentDay}");
			
			_guiInstance = GetNode<GUI>("GUI");
			markerScene = GD.Load<PackedScene>("res://Elements/PopupMarker/MarkerContainer.tscn");
			IslandPanelScene = GD.Load<PackedScene>("res://Elements/IslandHappinessPanel/IslandPanel.tscn");
			_turnButton = GetNode<TextureButton>("GUI/SideUI/TurnButton");
			_newDayIndicator = GetNode<TextureRect>("GUI/NewDayIndicator");
			_newDayLabel = GetNode<Label>("GUI/NewDayIndicator/NewDayLabel");
			_animationPlayer = GetNode<AnimationPlayer>("GUI/AnimationPlayer");
			
			_turnButton.Pressed += OnTurnButtonPressed;
			_guiInstance.MenuToggled += (IsOpen) => {_isMenuOpenBool = IsOpen;};
			SoundPlayer.Instance.PlayAmbience();
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}

		public override void _Input(InputEvent @event)
		{
			if (@event is InputEventScreenTouch touchEvent 
				&& touchEvent.Pressed 
				&& _isPanelOpen == false 
				&& isEventRunning == false 
				&& _currentIslandPanel == null
				&& _isMenuOpenBool == false)
			{
				Vector2 touchPos = touchEvent.Position;
				_ = CheckTouchedIsland(touchPos);
			}
		}
		
		private async Task CheckTouchedIsland(Vector2 touchPosition)
		{
			await ToSignal(GetTree().CreateTimer(0.3), "timeout");
			
			if (isEventRunning == true) return;
			
			Node _map = GetNode<TextureRect>("TextureRect");
			foreach (Node child in _map.GetChildren())
			{
				if (child is Area2D area)
				{
					foreach (Node shape in area.GetChildren())
					{
						if (shape is CollisionPolygon2D polygon && polygon.Polygon != null)
						{
							Vector2 localTouch = polygon.ToLocal(touchPosition);
							
							if (Geometry2D.IsPointInPolygon(localTouch, polygon.Polygon))
							{
								GD.Print($"{localTouch.X}, {localTouch.Y}");
								Control panel = (Control)IslandPanelScene.Instantiate();
								area.AddChild(panel);
								_currentIslandPanel = panel;
								
								GD.Print($"{area.Name} pressed");
								
								Label _panelHappiness = area.GetNode<Label>("IslandPanel/TextureRect/HappinessLabel");
								
								Enum.TryParse(area.Name, out Island islandEnum);
								SaveData.Instance._islandHappiness.TryGetValue(islandEnum, out float happiness);
								GD.Print(happiness);
								
								_panelHappiness.Text = $"{happiness}%";
								
								if (touchPosition.Y - 132 < 0)
								{
									panel.GlobalPosition = new Vector2(touchPosition.X, 132);
								}
								else
								{
									panel.GlobalPosition = touchPosition;
								}
								await ToSignal(GetTree().CreateTimer(1.3), "timeout");
								_currentIslandPanel = null;
								return;
							}
						}
					}
				}
			}
		}
		
		public void CreateTurnEvent()
		{
			GD.Print("Turn event created");
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
			GD.Print("Next turn button pressed");
			
			if (isEventRunning)
			{
				GD.Print("Event is already running");
				return;
			}
			
			SoundPlayer.Instance.PlayNextTurnSound();
			if (SaveData.Instance._currentTurn == 3)
			{
				GD.Print("Ending the day");
				
				SceneTransition.Instance.TransitionToScene("res://GameScenes/DayEndScene.tscn");
				return;
			}
			
			isEventRunning = true;
			SoundPlayer.Instance.PlayTicking();
			SaveData.Instance.IncreaseTurn();
			_guiInstance.UpdateLabels();
			
			GD.Print($"Day: {SaveData.Instance._currentDay}, Turn: {SaveData.Instance._currentTurn}");
			
			_newDayLabel.Text = "TURN " + SaveData.Instance._currentTurn;
			
			Vector2 _startingPosition = new Vector2(-SaveData.Instance._viewPortSize.X - 400, SaveData.Instance._viewPortSize.Y / 9);
			_newDayIndicator.Visible = true;
			_newDayIndicator.Position = _startingPosition;
			Vector2 _point1 = new Vector2((float)(SaveData.Instance._viewPortSize.X / 2 - 400), _startingPosition.Y);
			Vector2 _point2 = new Vector2((float)(SaveData.Instance._viewPortSize.X / 2), _startingPosition.Y);
			Vector2 _endingPosition = new Vector2(SaveData.Instance._viewPortSize.X, _startingPosition.Y);
			
			Tween tween = CreateTween();
			
			tween.TweenProperty(_newDayIndicator,"position", _point1, 0.3f)
				.SetEase(Tween.EaseType.Out)
				.SetTrans(Tween.TransitionType.Sine);
			
			tween.TweenProperty(_newDayIndicator,"position", _point2, 0.5f);
			
			tween.TweenProperty(_newDayIndicator,"position", _endingPosition, 0.3f)
				.SetEase(Tween.EaseType.In)
				.SetTrans(Tween.TransitionType.Sine);
			
			await ToSignal(tween, "finished");
			_newDayIndicator.Visible = false;
			
			CreateTurnEvent();
		}
		
		public async void OnEventAnswered()
		{
			_guiInstance.UpdateLabels();
			GD.Print("Turn event answered received - resetting flag");
			RemoveChild(_eventWindow);
			isEventRunning = false;
			if (SaveData.Instance._currentDay == 0 && SaveData.Instance._currentTurn == 1)
			{
				return;
			}
			
			int randomNumber = GD.RandRange(1, 3);
			GD.Print($"Creating {randomNumber} popup markers");
			for(int i = 0; i < randomNumber; i++)
			{	
				await ToSignal(GetTree().CreateTimer(0.2f), "timeout"); // "SceneTreeTimer.SignalName.Timeout"
				CreatePopUpMarker();
			}
		}
		
		public void OnPopUpEventAnswered()
		{
			_guiInstance.UpdateLabels();
			
			// Print happiness for all islands
			var allHappiness = SaveData.Instance.GetAllIslandHappiness();
			GD.Print("--Island happinesses--");
			foreach (var entry in allHappiness)
			{
				GD.Print($"Island {(int)entry.Key} Happiness: {entry.Value}%");
				if (entry.Value <= 0)
				{
					SaveData.Instance._gameOver = true;
				}
			}
			GD.Print("--");
			
			isEventRunning = false;
		}
		
		public void OnPopUpEventOpened()
		{
			GD.Print("Popup event opened");
			
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
			Island randomIsland = (Island)GD.RandRange(1, 6); // 0-5 for 6 areas
			GD.Print($"Random island: {randomIsland}");
			
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
				GD.PrintErr($"No collision polygons found for Island {island}");
				return islandArea.GlobalPosition;
			}

			// Select a random polygon
			RandomNumberGenerator rng = new();
			rng.Randomize();
			CollisionPolygon2D selectedPolygon = polygons[GD.RandRange(0, polygons.Count - 1)];
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
	}
}
