using Godot;
using System;

namespace MapGame
{
	[GlobalClass]
	public partial class EventOutcomeData : Resource
	{
		[ExportGroup("Resource changes")]
		[Export] public int HappinessChange;
		[Export] public int MainResourcehange;
		[Export] public int ThirdResourceChange;

		[ExportSubgroup("Bonus changes")]
		[Export] public string unlockEvent;
		[Export] public string unlockEvent2;
		[Export] public bool SecretMode2;
		[Export] public float HappinessMultiplier;
		[Export] public float MainResourceMultiplier;
	}
}
