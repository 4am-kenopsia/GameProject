using Godot;
using System;

namespace MapGame
{
	public partial class MarkerContainer : MapObject
	{
		[Signal] public delegate void ButtonPressedEventHandler(); 
		[Signal] public delegate void PopUpEventAnsweredEventHandler();
		[Signal] public delegate void PopUpEventOpenedEventHandler();
		[Export] private PackedScene popUp;
		//private int _timesOpened = 1;

		private TextureButton _button;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			// Get the button node
			_button = GetNode<TextureButton>("Pin");

			// Connect the button's "pressed" signal to a local method
			_button.Pressed += OnButtonPressed;
		}

		public override void OnButtonPressed()
		{
			if (GameScene.isEventRunning) 
			{
				return;
			}
			// Emit the custom signal
			EmitSignal(SignalName.ButtonPressed);
			SpawnPopUp();
			GD.Print("pressed marker");
		}
		
		private void SpawnPopUp()
		{
		PopupWindow popUpInstance = (PopupWindow)popUp.Instantiate();
		//popUpInstance._eventsHappened = _timesOpened;
		popUpInstance.TopLevel = true;
		popUpInstance.ButtonPressed2 += OnRightButtonPressed;
		popUpInstance.ButtonPressed1 += OnLeftButtonPressed;
		//ShitBought += popUpInstance.OnShitBought;
		Vector2 middleBottom = new Vector2( GetViewport().GetVisibleRect().Size.X / 2 , GetViewport().GetVisibleRect().Size.Y );
		popUpInstance.Position = middleBottom;
		AddChild(popUpInstance);
		EmitSignal(SignalName.PopUpEventOpened);
		
		//_timesOpened = _timesOpened + 1;
		//GetNode<Panel>("Panel").Visible = true;
	
		}
	
		public void OnRightButtonPressed()
		{
			QueueFree();
			EmitSignal(SignalName.PopUpEventAnswered);
			
		}
		public void OnLeftButtonPressed()
		{
			QueueFree();
			EmitSignal(SignalName.PopUpEventAnswered);
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}
	}
}
