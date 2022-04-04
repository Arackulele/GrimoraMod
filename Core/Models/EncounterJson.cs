using DiskCardGame;

namespace GrimoraMod;

[Serializable]
public class EncounterJson
{
	public string id;

	public List<List<string>> turns;

	public EncounterJson()
	{
	}

	public EncounterJson(string id, List<List<string>> turns)
	{
		this.id = id;
		this.turns = turns;
	}

	public List<List<EncounterBlueprintData.CardBlueprint>> BuildBlueprints()
	{
		List<List<EncounterBlueprintData.CardBlueprint>> list = new();
		foreach (var turn in turns)
		{
			List<EncounterBlueprintData.CardBlueprint> internalList = new();
			foreach (var s in turn)
			{
				internalList.Add(s.CreateCardBlueprint());
			}
			list.Add(internalList);
		}

		return list;
	}
}
