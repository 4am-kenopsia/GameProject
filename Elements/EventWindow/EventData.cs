using Godot;
using Godot.Collections;
using System;

namespace MapGame
{
	[GlobalClass]
	public partial class EventData : Resource
	{
        [Export] public bool isEventOneTime;
        [Export(PropertyHint.File, "*.png")] public string EventPicture;
		[Export] public string EventID;
		[Export] public string EventTitle;
		[Export(PropertyHint.MultilineText)] public string EventDesc;
		[Export] public string[] EventOptions = null;
        [Export] public string[] EventOutcomeHints = null;
		[Export] public Dictionary EventDictionary = new();
		
	}
}
