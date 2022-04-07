using System.Runtime.Serialization;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

[DataContract]
public class EncounterJson
{

	private static readonly Dictionary<string, string> SpecialEncounterIdByBossName = new()
	{
		{ "kaycee", "SpecialSequencer_arackulele.inscryption.grimoramod_GrimoraModKayceeBossSequencer" },
		{ "sawyer", "SpecialSequencer_arackulele.inscryption.grimoramod_GrimoraModSawyerBossSequencer" },
		{ "royal", "SpecialSequencer_arackulele.inscryption.grimoramod_GrimoraModRoyalBossSequencer" },
		{ "grimora", "SpecialSequencer_arackulele.inscryption.grimoramod_GrimoraModGrimoraBossSequencer" },
	};

	private static int _customEncounterTally = 0;

	private int GetBossIndex(List<string> splitId)
	{
		foreach (var s in splitId)
		{
			if (SpecialEncounterIdByBossName.Keys.Any(validBossName => s.StartsWith(validBossName, StringComparison.InvariantCultureIgnoreCase)))
			{
				return SpecialEncounterIdByBossName.Keys.ToList().IndexOf(s.ToLowerInvariant());
			}
		}

		return -1;
	}
	
	private static readonly List<string> ValidBlueprintType = new List<string> { "region", "fight" };
	
	private int GetBlueprintTypeIndex(List<string> splitId)
	{
		foreach (var s in splitId)
		{
			if (ValidBlueprintType.Any(valueBlueprintType => s.StartsWith(valueBlueprintType, StringComparison.InvariantCultureIgnoreCase)))
			{
				return ValidBlueprintType.IndexOf(s.ToLowerInvariant());
			}
		}

		return -1;
	}
	
	[DataMember]
	public string id { get; set; }

	[DataMember]
	public List<List<string>> turns { get; set; }
	
	public string bossName;

	public int bossIndex;

	public string blueprintType;
	
	public void SetupOtherFields()
	{
		List<string> splitId = id.Split('_').ToList();
		int bossNameIndex = GetBossIndex(splitId);
		int blueprintTypeIndex = GetBlueprintTypeIndex(splitId);
		if (bossNameIndex == -1 || blueprintTypeIndex == -1)
		{
			throw new Exception("Unable to determine where to place blueprint as the naming scheme is wrong! Please read the readme here: https://github.com/julian-perge/GrimoraMod/blob/main/Creating_Custom_Encounters.md");
		}
		bossIndex = bossNameIndex;
		bossName = SpecialEncounterIdByBossName.Keys.ElementAt(bossNameIndex);
		blueprintType = ValidBlueprintType[blueprintTypeIndex];

		string idTemp = id;
		if (!idTemp.StartsWith("Custom_", StringComparison.OrdinalIgnoreCase))
		{
			idTemp = "Custom_" + id;
		}
		
		id = $"{idTemp}_{++_customEncounterTally}";
	}

	public EncounterBlueprintData BuildEncounter()
	{
		SetupOtherFields();
		var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
		blueprint.name = id;
		blueprint.turns = BuildBlueprints();
		return blueprint;
	}

	public List<List<EncounterBlueprintData.CardBlueprint>> BuildBlueprints()
	{
		List<List<EncounterBlueprintData.CardBlueprint>> list = new();
		foreach (var turn in turns)
		{
			List<EncounterBlueprintData.CardBlueprint> internalList = new();
			foreach (var cardName in turn)
			{
				internalList.Add($"{GrimoraPlugin.GUID}_{cardName}".CreateCardBlueprint());
			}
			list.Add(internalList);
		}

		return list;
	}
}
