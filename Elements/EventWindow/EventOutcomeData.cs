using Godot;
using System;

namespace MapGame
{
	[GlobalClass]
	public partial class EventOutcomeData : Resource
	{
		public enum Severity
		{
			Low,
			Medium,
			High
		}
		[Export] public Severity OptionSeverity { get; set; }
		[Export] public bool IsIslandSpecific;
		[ExportGroup("Resource changes")]
		
		[Export] public int MagicChange;
		[Export(PropertyHint.Range, "-100,100,")] public int HappinessChange;

		[Export] public int TokensChange;
		[Export] public float MagicMultiplier;

		[ExportSubgroup("Bonus changes")]
		[Export] public string unlockEvent;
		[Export] public string unlockEvent2;
		[Export] public bool InstantGameOver;

		[ExportGroup("Island Targeting")]
		
		// Not exported - set at runtime
		public Island? TargetIsland { get; set; }
	}
}
