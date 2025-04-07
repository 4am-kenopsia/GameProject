using Godot;
using System;

namespace MapGame
{
	public partial class MarkerContainer : MapObject
	{
		// Signal declarations (only one 'public')
		[Signal] public delegate void ButtonPressedEventHandler(); 
		[Signal] public delegate void PopUpEventAnsweredEventHandler(EventOutcomeData outcome);
		[Signal] public delegate void PopUpEventOpenedEventHandler();
		
		[Export] private PackedScene popUp;
		private TextureButton _button;
		public Island TargetIsland { get; set; }  // Island reference

		public override void _Ready()
		{
			_button = GetNode<TextureButton>("Pin");
			_button.Pressed += OnButtonPressed;
		}

		public override void OnButtonPressed()
		{
			if (GameScene.isEventRunning) return;
			
			EmitSignal(SignalName.ButtonPressed);
			SpawnPopUp();
		}
		
		private void SpawnPopUp()
		{
			var popUpInstance = popUp.Instantiate<PopupWindow>();
			popUpInstance.TargetIsland = this.TargetIsland; // Pass island info
			popUpInstance.PopUpEventAnswered += OnPopUpEventAnswered;
			// Get viewport size
			Vector2 viewportSize = GetViewport().GetVisibleRect().Size;
			
			

			// Center in viewport
			//popUpInstance.Position = viewportSize / 2 - popUpInstance.Size / 2;
			popUpInstance.Position = new Vector2(((viewportSize.X - 200) / 2), (viewportSize.Y / 2)); 
			
			AddChild(popUpInstance);
			popUpInstance.TopLevel = true; 
			EmitSignal(SignalName.PopUpEventOpened);
		}

		private void OnPopUpEventAnswered(EventOutcomeData outcome)
		{
			// Handle island-specific outcomes
			if (outcome.IsIslandSpecific)
			{
				outcome.TargetIsland = this.TargetIsland;
			}
			ResourceManager.Instance.HandleOptionOutcomes(outcome);
			EmitSignal(SignalName.PopUpEventAnswered, outcome);
			QueueFree();
		}
		
	}
}
