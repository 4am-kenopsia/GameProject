using Godot;
using System;

namespace MapGame
{
	[GlobalClass]
	public partial class EventOutcomeData : Resource
	{
		[ExportGroup("Resource changes")]
		[Export(PropertyHint.Range, "-100,100,")] public int HappinessChange;
		[Export] public int MainResourcehange;
		[Export] public int ThirdResourceChange;

		[ExportSubgroup("Bonus changes")]
		[Export] public string unlockEvent;
		[Export] public string unlockEvent2;
		[Export] public bool SecretMode2;
        
        [ExportSubgroup("Happiness Multipliers")]
        [Export] public bool ChangeHappinessPosMultiplier;
		[Export] public float HappinessPosMultiplier;
        [Export] public bool ChangeHappinessNegMultiplier;
        [Export] public float HappinessNegMultiplier;
        
        [ExportSubgroup("Main Resource Multipliers")]
        [Export] public bool ChangeMainResourcePosMultiplier;
		[Export] public float MainResourcePosMultiplier;
        [Export] public bool ChangeMainResourceNegMultiplier;
        [Export] public float MainResourceNegMultiplier;
	}
}