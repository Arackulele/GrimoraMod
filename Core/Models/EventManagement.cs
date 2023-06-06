using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Guid;
using InscryptionAPI.Saves;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch]
public class EventManagement
{
	public static StoryEvent HasReachedTable = GuidManager.GetEnumValue<StoryEvent>(GUID, "HasReachedTable");

	public static bool HasLoadedIntoModBefore
	{
		get => ModdedSaveManager.SaveData.GetValueAsBoolean(GUID, "HasLoadedIntoModBefore");
		set => ModdedSaveManager.SaveData.SetValue(GUID, "HasLoadedIntoModBefore", value);
	}

	public static bool HasLearnedMechanicBoneyard
	{
		get => ModdedSaveManager.SaveData.GetValueAsBoolean(GUID, "Boneyard");
		set => ModdedSaveManager.SaveData.SetValue(GUID, "Boneyard", value);
	}

	public static bool HasLearnedMechanicCardRemoval
	{
		get => ModdedSaveManager.SaveData.GetValueAsBoolean(GUID, "CardRemoval");
		set => ModdedSaveManager.SaveData.SetValue(GUID, "CardRemoval", value);
	}

	public static bool HasLearnedMechanicElectricChair
	{
		get => ModdedSaveManager.SaveData.GetValueAsBoolean(GUID, "ElectricChair");
		set => ModdedSaveManager.SaveData.SetValue(GUID, "ElectricChair", value);
	}

	public static bool HasLearnedMechanicHammerSmashes
	{
		get => ModdedSaveManager.SaveData.GetValueAsBoolean(GUID, "HammerSmashes");
		set => ModdedSaveManager.SaveData.SetValue(GUID, "HammerSmashes", value);
	}

	public static bool HasSeenCredits
	{
		get => ModdedSaveManager.SaveData.GetValueAsBoolean(GUID, "HasSeenCredits");
		set => ModdedSaveManager.SaveData.SetValue(GUID, "HasSeenCredits", value);
	}

	public static bool HasBeatenSkullStorm
	{
		get => ModdedSaveManager.SaveData.GetValueAsBoolean(GUID, "HasBeatenSkullStorm");
		set => ModdedSaveManager.SaveData.SetValue(GUID, "HasBeatenSkullStorm", value);
	}

	public static readonly StoryEvent[] GrimoraAscensionSaveEvents =
	{
		HasReachedTable,
	};

	[HarmonyPrefix, HarmonyPatch(typeof(StoryEventsData), nameof(StoryEventsData.SetEventCompleted))]
	public static bool GrimoraAscensionStoryComplete(StoryEvent storyEvent, bool saveToFile = false, bool sendAnalytics = true)
	{
		Log.LogInfo($"[StoryEventsData.SetEventCompleted] " +
		            $"\nEvent [{storyEvent}] " +
		            $"\nSaveToFile [{saveToFile}] " +
		            $"\nIsAscension? [{SaveFile.IsAscension}]" +
		            $"\nIsGrimoraRun? [{GrimoraSaveUtil.IsGrimoraModRun}]"
		);
		if (SaveFile.IsAscension && GrimoraSaveUtil.IsGrimoraModRun && GrimoraAscensionSaveEvents.Contains(storyEvent))
		{
			Log.LogInfo($"[StoryEventsData.SetEventCompleted] story event [{storyEvent}] exists in list, setting to true");
			ModdedSaveManager.SaveData.SetValue(GUID, $"StoryEvent_{storyEvent}", true);
			return false;
		}

		return true;
	}

	[HarmonyPrefix, HarmonyPatch(typeof(StoryEventsData), nameof(StoryEventsData.EventCompleted))]
	public static bool GrimoraAscensionStoryData(ref bool __result, StoryEvent storyEvent)
	{
		if (SaveFile.IsAscension && GrimoraSaveUtil.IsGrimoraModRun)
		{
			if (GrimoraAscensionSaveEvents.Contains(storyEvent))
			{
				__result = ModdedSaveManager.SaveData.GetValueAsBoolean(GUID, $"StoryEvent_{storyEvent}");
				Log.LogInfo($"[StoryEventsData.EventCompleted] story event [{storyEvent}] exists in list, bool value [{__result}]");
				return false;
			}
		}

		return true;
	}
}
