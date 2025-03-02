using Godot;
using System;

namespace MapGame
{
    public partial class PopupMarker : MapObject
    {
        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
        }

        public override void OnClick()
        {
            ResourceManager.Instance.IncreaseMainResource(1000);
            GD.Print(ResourceManager.Instance._currentMainResource); //For some reason it only displays 1000 after clicking (so doesn't go up past the first click)
        }
    }
}