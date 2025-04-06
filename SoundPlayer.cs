using Godot;
using System;
using System.Threading.Tasks;

namespace MapGame
{
	public partial class SoundPlayer : Node
	{
		private AudioStreamPlayer _ambiencePlayer;
		private AudioStreamPlayer _sfxPlayer;
		private AudioStreamPlayer _sfxPlayer2;
		private AnimationPlayer _soundTransition;
		public static SoundPlayer Instance
		{
			get;
			private set;
		}
		public override void _Ready()
		{
			Instance = this;
			_soundTransition = GetNode<AnimationPlayer>("SoundTransition");
			_sfxPlayer = GetNode<AudioStreamPlayer>("SfxPlayer");
			_sfxPlayer2 = GetNode<AudioStreamPlayer>("SfxPlayer2");
			_ambiencePlayer = GetNode<AudioStreamPlayer>("AmbiencePlayer");
		}
		public void PlayAmbience()
		{
			_ambiencePlayer.VolumeDb = -10;
			AudioStream audioStream = (AudioStream)GD.Load("res://Assets/Sound/AmbientLoop.wav");
			_ambiencePlayer.Stream = audioStream;
			_ambiencePlayer.Play();
			_soundTransition.Play("soundtransition");
		}
		public void PlayDayEndSound()
		{
			_soundTransition.PlayBackwards("soundtransition");
		}
		public void PlayPopUpSound()
		{
			AudioStream audioStream = (AudioStream)GD.Load("res://Assets/Sound/PopUpSound.wav");
			_sfxPlayer.Stream = audioStream;
			_sfxPlayer.Play();
		}
		public void PlayEventButtonSound()
		{
			AudioStream audioStream = (AudioStream)GD.Load("res://Assets/Sound/EventButtonSound.mp3");
			_sfxPlayer.Stream = audioStream;
			_sfxPlayer.Play();
		}
		public void PlayNextTurnSound()
		{
			AudioStream audioStream = (AudioStream)GD.Load("res://Assets/Sound/TurnButtonSound.mp3");
			_sfxPlayer.Stream = audioStream;
			_sfxPlayer.Play();
		}
		public void PlayTicking()
		{
			AudioStream audioStream = (AudioStream)GD.Load("res://Assets/Sound/TurnTickingSound.mp3");
			_sfxPlayer2.Stream = audioStream;
			_sfxPlayer2.Play();
			_soundTransition.Play("ticking");
			_sfxPlayer2.VolumeDb = 0;
		}
		public void PlayToGameSound()
		{
			_sfxPlayer2.VolumeDb = 0;
			AudioStream audioStream = (AudioStream)GD.Load("res://Assets/Sound/NewGameSound.mp3");
			_sfxPlayer2.Stream = audioStream;
			_sfxPlayer2.Play();
		}
		public void PlayMenuButtonSound()
		{
			AudioStream audioStream = (AudioStream)GD.Load("res://Assets/Sound/MenuButtonSound.mp3");
			_sfxPlayer.Stream = audioStream;
			_sfxPlayer.Play();
		}
		public void PlaySound()
		{
			AudioStream audioStream = (AudioStream)GD.Load("res://Assets/Sound/");
			_sfxPlayer.Stream = audioStream;
			_sfxPlayer.Play();
		}
		
		private async Task WaitForSoundToFinish()
		{
			var tcs = new TaskCompletionSource<bool>();
			
			void OnSoundFinished()
			{
				tcs.SetResult(true);
			}
			_sfxPlayer.Finished += OnSoundFinished;
			await tcs.Task;
			_sfxPlayer.Finished -= OnSoundFinished;
		}
	}
}