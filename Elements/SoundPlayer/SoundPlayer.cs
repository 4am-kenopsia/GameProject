using Godot;
using System;
using System.Threading.Tasks;

namespace MapGame
{
	public partial class SoundPlayer : Node
	{
		public AudioStreamPlayer _ambiencePlayer;
		public AudioStreamPlayer _sfxPlayer;
		public AudioStreamPlayer _sfxPlayer2;
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
			_sfxPlayer.VolumeDb = -10;
			_sfxPlayer2.VolumeDb = -10;
			_ambiencePlayer.VolumeDb = -10;
		}
		public void PlayAmbience()
		{
			_ambiencePlayer.VolumeDb = -80;
			AudioStream audioStream = (AudioStream)GD.Load("res://Assets/Sound/AmbientLoop.wav");
			_ambiencePlayer.Stream = audioStream;
			_ambiencePlayer.Play();
			
			Tween tween = CreateTween();
			tween.TweenProperty(_ambiencePlayer, "volume_db", SaveData.Instance._ambienceVolume, 4f)
				.SetTrans(Tween.TransitionType.Sine)
				.SetEase(Tween.EaseType.InOut);
		}
		public async Task PlayDayEndSound()
		{
			_soundTransition.PlayBackwards("soundtransition");
			await WaitForSoundToFinish(_ambiencePlayer);
			_ambiencePlayer.VolumeDb = -80;
		}
		public void PlayPopUpSound()
		{
			_sfxPlayer.VolumeDb = SaveData.Instance._effectsVolume;
			AudioStream audioStream = (AudioStream)GD.Load("res://Assets/Sound/PopUpSound.wav");
			_sfxPlayer.Stream = audioStream;
			_sfxPlayer.Play();
		}
		public void PlayPopUpSpawnSound()
		{
			_sfxPlayer.VolumeDb = SaveData.Instance._effectsVolume;
			AudioStream audioStream = (AudioStream)GD.Load("res://Assets/Sound/PopUpSpawnSound.mp3");
			_sfxPlayer.Stream = audioStream;
			_sfxPlayer.Play();
		}
		public void PlayEventButtonSound()
		{
			_sfxPlayer.VolumeDb = SaveData.Instance._effectsVolume;
			AudioStream audioStream = (AudioStream)GD.Load("res://Assets/Sound/EventButtonSound.mp3");
			_sfxPlayer.Stream = audioStream;
			_sfxPlayer.Play();
		}
		public void PlayNextTurnSound()
		{
			_sfxPlayer.VolumeDb = SaveData.Instance._effectsVolume;
			AudioStream audioStream = (AudioStream)GD.Load("res://Assets/Sound/TurnButtonSound.mp3");
			_sfxPlayer.Stream = audioStream;
			_sfxPlayer.Play();
		}
		public void PlayTicking()
		{
			_sfxPlayer2.VolumeDb = SaveData.Instance._effectsVolume;
			AudioStream audioStream = (AudioStream)GD.Load("res://Assets/Sound/TurnTickingSound.mp3");
			_sfxPlayer2.Stream = audioStream;
			_sfxPlayer2.Play();
			_soundTransition.Play("ticking");
			_sfxPlayer2.VolumeDb = SaveData.Instance._effectsVolume;
		}
		
		public void PlayToGameSound()
		{
			_sfxPlayer2.VolumeDb = SaveData.Instance._effectsVolume;
			AudioStream audioStream = (AudioStream)GD.Load("res://Assets/Sound/NewGameSound.mp3");
			_sfxPlayer2.Stream = audioStream;
			_sfxPlayer2.Play();
		}
		public void PlayMenuButtonSound()
		{
			_sfxPlayer.VolumeDb = SaveData.Instance._effectsVolume;
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
		
		private async Task WaitForSoundToFinish(AudioStreamPlayer player)
		{
			var tcs = new TaskCompletionSource<bool>();
			
			void OnSoundFinished()
			{
				tcs.SetResult(true);
			}
			player.Finished += OnSoundFinished;
			await tcs.Task;
			player.Finished -= OnSoundFinished;
		}
	}
}
