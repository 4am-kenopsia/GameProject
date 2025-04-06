using Godot;
using System;
using System.Threading.Tasks;

namespace MapGame
{
	public partial class SceneTransition : CanvasLayer
	{
		private AnimationPlayer _animationPlayer;
		private TextureRect _topBar;
		private TextureRect _bottomBar;
		private ColorRect _inputBlocker;
		public static SceneTransition Instance
		{
			get;
			private set;
		}
		public override void _Ready()
		{
			Instance = this;
			_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
			_topBar = GetNode<TextureRect>("TopBar");
			_bottomBar = GetNode<TextureRect>("BottomBar");
			_inputBlocker = GetNode<ColorRect>("InputBlocker");

		}
		public async Task TransitionToScene(string targetScene)
		{
			_inputBlocker.Visible = true;
			if (GetTree().CurrentScene.Name == "GameScene")
			{
				SoundPlayer.Instance.PlayDayEndSound();
			}
			_topBar.Visible = true;
			_bottomBar.Visible = true;
			_animationPlayer.Play("tv");
			await WaitForAnimationToFinish();
			GetTree().ChangeSceneToFile(targetScene);
			_animationPlayer.PlayBackwards("tv");
			if (targetScene == "res://GameScenes/GameScene.tscn")
			{
				SoundPlayer.Instance.PlayToGameSound();
			}
			await WaitForAnimationToFinish();
			_topBar.Visible = false;
			_bottomBar.Visible = false;
			_inputBlocker.Visible = false;
		}
		private async Task WaitForAnimationToFinish()
		{
			var tcs = new TaskCompletionSource<bool>();
			
			void OnAnimationFinished(StringName animationName)
			{
				if (animationName == "tv")
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