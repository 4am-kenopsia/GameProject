using Godot;
using System;

namespace MapGame
{
	[GlobalClass]
	public partial class TurnEventOutcomeData : Resource
	{
		[ExportGroup("Resource changes")]
		[Export] public int HappinessChange;
		[Export] public int MainResourcehange;
		[Export] public int ThirdResourceChange;

		[ExportSubgroup("Bonus changes")]
		[Export] public bool SecretMode1;
		[Export] public bool SecretMode2;
		[Export] public int SecretMultiplier;
	}
}
