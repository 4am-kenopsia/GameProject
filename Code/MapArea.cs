using Godot;
using System;

namespace MapGame
{

    public partial class MapArea : Node2D
    {
        [Export] private int _width = 1920;
        [Export] private int _height = 1080;
        public int Width
        {
            get { return _width; }
        }
        public int Height
        {
            get { return _height; }
        }
        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
        }
        public static bool IsValidMapPos(Vector2I MapPos)
        {
            if(true)
            {
                return true;
            }
            //return false;
        }
    }
}