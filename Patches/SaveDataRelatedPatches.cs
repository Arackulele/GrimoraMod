using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Ascension;
using InscryptionAPI.Saves;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch]
public class SaveDataRelatedPatches
{
	public const string AscensionSaveKey = "CopyOfGrimoraAscensionSave";
	public const string RegularSaveKey = "CopyOfGrimoraSave";
	public const string IsGrimoraRunKey = "IsGrimoraRun";

	public static string SaveKey
	{
		get
		{
			if (SceneLoader.ActiveSceneName == "Ascension_Configure")
				return AscensionSaveKey;

			if (SceneLoader.ActiveSceneName == SceneLoader.StartSceneName)
				return RegularSaveKey;

			return SaveFile.IsAscension ? AscensionSaveKey : RegularSaveKey;
		}
	}

	public static bool IsNotGrimoraModRun => !IsGrimoraModRun;

	public static bool IsGrimoraModRun
	{
		get => ModdedSaveManager.SaveData.GetValueAsBoolean(GUID, IsGrimoraRunKey);
		set => ModdedSaveManager.SaveData.SetValue(GUID, IsGrimoraRunKey, value);
	}
}
