using Godot;
using System;

public partial class Menu : Control
{
	// Called when the node enters the scene tree for the first time.
	private TextureButton _playButton;
	private TextureButton _langButton;
	private TextureButton _creditsButton;
	public override void _Ready()
	{
		
		_playButton = GetNode<TextureButton>("Control/VBoxContainer/playButton");
		_langButton = GetNode<TextureButton>("Control/VBoxContainer/languageButton");
		_creditsButton = GetNode<TextureButton>("Control/VBoxContainer/creditsButton");
		
		_playButton.Pressed += OnPlayButtonPressed;
		_langButton.Pressed += OnLanguageButtonPressed;
		_creditsButton.Pressed += OnCreditsButtonPressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void OnPlayButtonPressed()
	{
		
		 GetTree().ChangeSceneToFile("res://GameScenes/GameScene.tscn");
		
	}
	private void OnLanguageButtonPressed()
	{
		var _langText = GetNode<Label>("Control/VBoxContainer/languageButton/Label2");
		_langText.Text = "Not implemented";
		 
		
	}
	private void OnCreditsButtonPressed()
	{
		var _creditsText = GetNode<Label>("Control/VBoxContainer/creditsButton/Label3");
		_creditsText.Text = "Not implemented";
		 
		
	}
}
