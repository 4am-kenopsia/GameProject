using Godot;
using System;

namespace MapGame
{
    public partial class ResourceManager : Node
    {
        [Export] public int _startingMainResource = 10000;
        [Export] public int _startingHappiness = 100;
        [Export] public int _currentMainResource = 0;
        [Export] public int _currentHappiness = 0;
        int num = 10;
        public static ResourceManager Instance
        {
            get;
            private set;
        }
        
        
        public void SetMainResource(int targetAmount)
        {
            _currentMainResource = targetAmount;
        }
        public void SetHappiness(int targetAmount)
        {
            
        }
        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            Instance = this;
            _currentHappiness = 5555;
        }
    }
}