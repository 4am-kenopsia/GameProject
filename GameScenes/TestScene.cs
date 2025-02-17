using Godot;
using System;

namespace MapGame
{
    public partial class TestScene : Node
    {
        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            GD.Print(ResourceManager.Instance._currentHappiness);
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
        }
    }
}