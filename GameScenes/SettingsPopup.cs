using Godot;
using System;

namespace MapGame
{
	public partial class SettingsPopup : Control
	{
		private Slider _ambienceSlider;
		private Slider _effectsSlider;
		private TextureButton _closeSettings;
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			_ambienceSlider = GetNode<Slider>("SettingsPanel/HBoxContainer/SliderContainer/AmbienceSlider");
			_effectsSlider = GetNode<Slider>("SettingsPanel/HBoxContainer/SliderContainer/EffectsSlider");
			_closeSettings = GetNode<TextureButton>("SettingsPanel/CloseSettings");
			
			_ambienceSlider.DragEnded += OnAmbienceSliderDragEnded;
			_effectsSlider.DragEnded += OnEffectsSliderDragEnded;
			_closeSettings.Pressed += OnCloseSettingsPressed;
			
			_ambienceSlider.Value = SaveData.Instance._ambienceVolume;
			_effectsSlider.Value = SaveData.Instance._effectsVolume;
			
			
			Position = new Vector2(0f, SaveData.Instance._viewPortSize.Y);
			
			Tween tween = CreateTween();
			tween.TweenProperty(this, "position", new Vector2(0f, 0f), 0.5f)
				.SetTrans(Tween.TransitionType.Sine)
				.SetEase(Tween.EaseType.Out);
		}


		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}
		
		public void OnAmbienceSliderDragEnded(bool ValueChanged)
		{
			if (ValueChanged)
			{
				SaveData.Instance._ambienceVolume = (float)_ambienceSlider.Value;
			}
			SoundPlayer.Instance._ambiencePlayer.VolumeDb = (float)_ambienceSlider.Value;
			GD.Print(_ambienceSlider.Value);
		}
		public void OnEffectsSliderDragEnded(bool ValueChanged)
		{
			if (ValueChanged)
			{
				SaveData.Instance._effectsVolume = (float)_effectsSlider.Value;
			}
			SoundPlayer.Instance._sfxPlayer.VolumeDb = (float)_effectsSlider.Value;
			SoundPlayer.Instance._sfxPlayer2.VolumeDb = (float)_effectsSlider.Value;
			SoundPlayer.Instance.PlayMenuButtonSound();
		}
		public void OnCloseSettingsPressed()
		{
			SoundPlayer.Instance.PlayNextTurnSound();
			Tween tween = CreateTween();
			tween.TweenProperty(this, "position", new Vector2(0.0f, SaveData.Instance._viewPortSize.Y), 0.3f)
				.SetTrans(Tween.TransitionType.Sine)
				.SetEase(Tween.EaseType.In);
				
			tween.TweenCallback(Callable.From(() => QueueFree()));
		}
	}
}