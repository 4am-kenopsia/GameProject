using Godot;
using System;

namespace MapGame
{
	public partial class SettingsBackdrop : ColorRect
	{
		private Slider _musicSlider;
		private Slider _ambienceSlider;
		private Slider _effectsSlider;
		private TextureButton _closeSettings;
		private AnimationPlayer _animationPlayer;
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			_musicSlider = GetNode<Slider>("SettingsPanel/SliderContainer/MusicSlider");
			_ambienceSlider = GetNode<Slider>("SettingsPanel/SliderContainer/AmbienceSlider");
			_effectsSlider = GetNode<Slider>("SettingsPanel/SliderContainer/EffectsSlider");
			_closeSettings = GetNode<TextureButton>("SettingsPanel/CloseSettings");
			_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
			
			_musicSlider.DragEnded += OnMusicSliderDragEnded;
			_ambienceSlider.DragEnded += OnAmbienceSliderDragEnded;
			_effectsSlider.DragEnded += OnEffectsSliderDragEnded;
			_closeSettings.Pressed += OnCloseSettingsPressed;
		}


		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}
		
		public void OnMusicSliderDragEnded(bool ValueChanged)
		{
			if (ValueChanged)
			{
				SaveData.Instance._musicVolume = (float)_musicSlider.Value;
			}
		}
		public void OnAmbienceSliderDragEnded(bool ValueChanged)
		{
			if (ValueChanged)
			{
				SaveData.Instance._ambienceVolume = (float)_ambienceSlider.Value;
			}
			SoundPlayer.Instance._ambiencePlayer.VolumeDb = (float)_ambienceSlider.Value;
		}
		public void OnEffectsSliderDragEnded(bool ValueChanged)
		{
			if (ValueChanged)
			{
				SaveData.Instance._effectsVolume = (float)_effectsSlider.Value;
			}
		}
		public void OnCloseSettingsPressed()
		{
			Visible = false;
		}
	}
}