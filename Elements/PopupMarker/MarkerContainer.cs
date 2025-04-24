using Godot;
using System;

namespace MapGame
{
	public partial class MarkerContainer : Control
	{
		[Signal] public delegate void PopUpEventAnsweredEventHandler();
		[Signal] public delegate void PopUpEventOpenedEventHandler();
		
		private Tween _tween;
		
		[Export] private PackedScene popUp;
		private TextureButton _pin;
		public Island TargetIsland { get; set; } // Island reference
		private GUI _gui;

		public override void _Ready()
		{
			Modulate = Colors.Transparent;
			_pin = GetNode<TextureButton>("Pin");
			TweenFadeIn();
			
			_pin.Pressed += OnPinPressed;
			
			GetNode<GUI>("/root/GameScene/GUI").MenuToggled += (is_open) => {
			GetNode<TextureButton>("Pin").Disabled = is_open;
			};
		}


		public void OnPinPressed()
		{
			if (GameScene.isEventRunning || GetNode<TextureButton>("Pin").Disabled) return;
			
			SpawnPopUp();
		}
		
		private async void SpawnPopUp()
		{
			SoundPlayer.Instance.PlayPopUpSound();
			TweenFadeOut();
			await ToSignal(_tween, Tween.SignalName.Finished);
			
			PopupWindow popupWindow = popUp.Instantiate<PopupWindow>();
			
			popupWindow.TargetIsland = TargetIsland; // Pass island info
			
			popupWindow.PopUpEventAnswered += OnPopUpEventAnswered;

			// Center in viewport
			//popupWindow.GlobalPosition = new Vector2(0, 0); 
			
			AddChild(popupWindow);
			popupWindow.TopLevel = true; 
			EmitSignal(SignalName.PopUpEventOpened);
		}

		private void OnPopUpEventAnswered()
		{
			EmitSignal(SignalName.PopUpEventAnswered);
			QueueFree();
		}
		
		public void TweenFadeIn()
		{
		// Create a new tween
		_tween = CreateTween();
		
		// Configure and start the tween
		_tween.TweenProperty(this, "modulate", new Color(1, 1, 1, 1), 0.5f)
			.SetTrans(Tween.TransitionType.Quad)
			.SetEase(Tween.EaseType.Out);
		}
		public void TweenFadeOut()
		{
			// Create a new tween
			_tween = CreateTween();
		
			// Configure and start the tween
			_tween.TweenProperty(this, "modulate", Colors.Transparent, 0.25f)
				.SetTrans(Tween.TransitionType.Quad)
				.SetEase(Tween.EaseType.Out);
		}
	}
}
