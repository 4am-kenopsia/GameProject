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
		//[Export(PropertyHint.Enum, "Low,Medium,High")] public string OptionSeverity;

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

		[ExportGroup("Island Targeting")]
		[Export] public bool IsIslandSpecific = false;
		[Export] public int IslandIndex = 0; // 0-7 for your 8 islands
	
		// Not exported - set at runtime
		public Island? TargetIsland { get; set; }
	}
}
