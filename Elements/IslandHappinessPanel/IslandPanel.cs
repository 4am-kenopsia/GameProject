using Godot;
using System;

public partial class IslandPanel : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Modulate = new Color(1, 1, 1, 0);
		
		Tween tween = CreateTween();
		
		tween.TweenProperty(this, "modulate:a", 1.0f, 0.3f)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.Out);
			
		tween.TweenInterval(1.8f);
		
		tween.TweenProperty(this, "modulate:a", 0.0f, 0.3f)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.In);
		
		tween.TweenCallback(Callable.From(() => QueueFree()));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
