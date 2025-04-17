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
		private Tween _tween;
		
		[Export] private PackedScene popUp;
		private TextureButton _button;
		private AnimationPlayer _player;
		public Island TargetIsland { get; set; }  // Island reference
		private GUI _gui;

		public override void _Ready()
		{
			this.Modulate = Colors.Transparent;
			_button = GetNode<TextureButton>("Pin");
			_player = GetNode<AnimationPlayer>("AnimationPlayer");
			TweenFadeIn();
			
			_button.Pressed += OnButtonPressed;
			GetNode<GUI>("/root/GameScene/GUI").MenuToggled += (is_open) => {
			GetNode<TextureButton>("Pin").Disabled = is_open;
			};


		}


		public override void OnButtonPressed()
		{
			 if (GameScene.isEventRunning || GetNode<TextureButton>("Pin").Disabled) 
	   		 return;
			if (GameScene.isEventRunning) return;
			
			EmitSignal(SignalName.ButtonPressed);
			SpawnPopUp();
		}
		
		private async void SpawnPopUp()
		{
			SoundPlayer.Instance.PlayPopUpSound();
			TweenFadeOut();
			await ToSignal(_tween, Tween.SignalName.Finished);
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
