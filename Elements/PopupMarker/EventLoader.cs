using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using MapGame;

public partial class EventLoader : Node
{
	public Dictionary<Island, List<EventData>> IslandEvents = new();

	public override void _Ready()
	{
		foreach (Island island in Enum.GetValues(typeof(Island)))
		{
			LoadIslandEvents(island);
		}
	}

	private void LoadIslandEvents(Island island)
	{
		var islandEvents = new List<EventData>();
		string islandPath = $"res://Code/EventData/PopupEvents/Island{(int)island+1}";
		
		using var dir = DirAccess.Open(islandPath);
		if (dir != null)
		{
			dir.ListDirBegin();
			string fileName = dir.GetNext();
			while (fileName != "")
			{
				if (!dir.CurrentIsDir() && fileName.EndsWith(".tres"))
				{
					var eventData = GD.Load<EventData>($"{islandPath}/{fileName}");
					if (eventData != null) islandEvents.Add(eventData);
				}
				fileName = dir.GetNext();
			}
		}

		IslandEvents[island] = islandEvents;
	}

	public EventData GetRandomEventForIsland(Island island)
	{
		if (IslandEvents.TryGetValue(island, out var events) && events.Count > 0)
		{
			return events[new Random().Next(events.Count)];
		}
		
		return CreateDefaultEvent(island);
	}

	private EventData CreateDefaultEvent(Island island)
	{
		return new EventData {
			EventTitle = $"Island {(int)island + 1} Event",
			EventDesc = "A default event description",
			EventOptions = new string[] { "Continue", "Cancel" }, // Explicit string array
			EventDictionary = new Godot.Collections.Dictionary()
		};
	}
}
