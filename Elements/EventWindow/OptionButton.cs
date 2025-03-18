using Godot;
using System;

namespace MapGame
{
	public partial class OptionButton : Control
	{
        //[Signal] public delegate void ButtonPressedEventHandler();
        //private TextureButton _button;
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
            //_button = GetNode<TextureButton>("TextureButton");
            //_button.Pressed += OnButtonPressed;
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}
		/*public void OnButtonPressed()
		{
            EmitSignal(SignalName.ButtonPressed);
		}*/
	}
}
