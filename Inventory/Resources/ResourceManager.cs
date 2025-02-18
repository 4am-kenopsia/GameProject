using Godot;
using System;

namespace MapGame
{
    public partial class ResourceManager : Node
    {
        [Export] public int _startingMainResource = 10000;
        [Export] public int _startingHappiness = 100;
        [Export] public int _startingThirdResource = 5;
        [Export] public int _currentMainResource = 0;
        [Export] public int _currentHappiness = 0;
        [Export] public int _currentThirdResource = 0;
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
            _currentHappiness = targetAmount;
        }
        public void SetThirdResource(int targetAmount)
        {
            _currentThirdResource = targetAmount;
        }
        public void IncreaseMainResource(int increase)
        {
            _currentMainResource =+ increase;
        }
        public void IncreaseHappiness(int increase)
        {
            _currentHappiness =+ increase;
        }
        public void IncreaseThirdResource(int increase)
        {
            _currentThirdResource =+ increase;
        }
        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            Instance = this;
            IncreaseHappiness(-100);
            
        }
        
    }
}