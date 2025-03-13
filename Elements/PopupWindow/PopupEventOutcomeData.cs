using Godot;
using System;

namespace MapGame
{
	[GlobalClass]
	public partial class PopupEventOutcomeData : Resource
	{
		[ExportGroup("Resource changes")]
		[Export(PropertyHint.Range, "-1000,1000,")] public int HappinessChange;
		[Export(PropertyHint.Range, "-10000,10000,")] public int MainResourcehange;
		[Export(PropertyHint.Range, "-10,10,")] public int ThirdResourceChange;

		[ExportSubgroup("Bonus changes")]
		[Export] public bool SecretMode1;
		[Export] public bool SecretMode2;
		[Export] public int SecretMultiplier;
	}
}
