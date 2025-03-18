using Godot;
using Godot.Collections;
using System;

namespace MapGame
{
	[GlobalClass]
	public partial class EventData : Resource
	{
        [Export] public bool isEventOneTime;
		[Export] public string EventID;
		[Export] public string EventTitle;
		[Export] public string EventDesc;
		[Export] public string[] EventOptions = null;
		[Export] public Dictionary EventDictionary = new();
		
	}
}
