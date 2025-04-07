using Godot;
using System;

namespace MapGame
{
	public abstract partial class MapObject : Control
	{
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}
		
		public bool SetPos(Vector2I mapPos)
		{
			if (MapArea.IsValidMapPos(mapPos))
			{
				Position = mapPos;
				return true;
			}
			return false;
		}
		public abstract void OnButtonPressed();
	}
}
