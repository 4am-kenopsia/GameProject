using Godot;
using System;

namespace MapGame
{
    public partial class TestScene : Node
    {
        [Export] private string _popupMarkerPath = "res://Elements/Popup Marker/MarkerContainer.tscn";
        private PackedScene _popupMarkerScene = null;
        private PopupMarker _popupMarker = null;
        
        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            GD.Print(ResourceManager.Instance._currentMainResource);
            AddMarker();
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
        }
        public void AddMarker()
        {
            if (_popupMarker != null)
            {
                _popupMarker.QueueFree();
                _popupMarker = null;
            }
            if (_popupMarkerScene == null)
            {
                _popupMarkerScene = ResourceLoader.Load<PackedScene>(_popupMarkerPath);
            }
            _popupMarker = _popupMarkerScene.Instantiate<PopupMarker>();
            AddChild(_popupMarker);
            Vector2I tempPos;
            tempPos.X = 960;
            tempPos.Y = 540;
            _popupMarker.SetPos(tempPos);
        }
    }
}