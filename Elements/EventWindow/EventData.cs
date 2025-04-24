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
		[Export] public string EventTitleEN;
		[Export] public string EventTitleFI;
		[Export(PropertyHint.MultilineText)] public string EventDescEN;
		[Export(PropertyHint.MultilineText)] public string EventDescFI;
		[Export] public string[] EventOptionsEN = null;
		[Export] public string[] EventOptionsFI = null;
		[Export] public Dictionary EventDictionary = new();
		
	}
}
