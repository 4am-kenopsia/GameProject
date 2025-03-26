using Godot;
using System;

namespace MapGame
{
	[GlobalClass]
	public partial class EventOutcomeData : Resource
	{
		[ExportGroup("Resource changes")]
		[Export(PropertyHint.Range, "-100,100,")] public int HappinessChange;
		[Export] public int MagicChange;
		[Export] public int TokensChange;

		[ExportSubgroup("Bonus changes")]
		[Export] public string unlockEvent;
		[Export] public string unlockEvent2;
		[Export] public bool SecretMode2;

		[ExportSubgroup("Multipliers")]
		[Export] public float HappinessMultiplier;
		[Export] public float MagicMultiplier;

	}
}
