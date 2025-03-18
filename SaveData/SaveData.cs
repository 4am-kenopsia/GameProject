using Godot;
using System;

namespace MapGame
{
	public partial class SaveData : Node
	{
        [Export] public int _currentTurn = 0;
        [Export] public string[] _eventPool; // Something like this? 
		public static SaveData Instance
		{
			get;
			private set;
		}
		
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			Instance = this;
			
		}
        
        public void IncreaseTurn()
        {
            _currentTurn += 1;
        }
        
		
	}
}
