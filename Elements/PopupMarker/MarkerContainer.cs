using Godot;
using System;

namespace MapGame
{
    public partial class MarkerContainer : MapObject
    {
        [Signal] public delegate void ButtonPressedEventHandler();

        private TextureButton _button;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            // Get the button node
            _button = GetNode<TextureButton>("Pin");

            // Connect the button's "pressed" signal to a local method
            _button.Pressed += OnButtonPressed;
        }

        public override void OnButtonPressed()
        {
            // Emit the custom signal
            EmitSignal(SignalName.ButtonPressed);
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
        }
    }
}