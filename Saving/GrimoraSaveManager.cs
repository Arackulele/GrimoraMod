using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod.Saving;

[HarmonyPatch]
public class GrimoraSaveManager
{
	public static GrimoraSaveFile CurrentSaveFile;
	
	public static void CreateNewSaveFile()
	{
		if (File.Exists(SaveFilePath))
		{
			File.Delete(SaveFilePath);
		}

		CurrentSaveFile = new GrimoraSaveFile();
		CurrentSaveFile.Initialize();
	}
	
	public static void ResetStandardRun()
	{
		CurrentSaveFile.Initialize();
		CurrentSaveFile.NewStandardRun();
	}
	
	#region Patches
	public static string SaveFilePath => SaveManager.SaveFolderPath + "GrimoraSaveFile.gwsave";
	
	[HarmonyPatch(typeof(SaveManager), "SaveToFile")]
	[HarmonyPostfix]
	public static void SaveToFile()
	{
		File.WriteAllText(SaveFilePath, SaveManager.ToJSON(CurrentSaveFile));
	}


	[HarmonyPatch(typeof(SaveManager), "LoadFromFile", new Type[] { })]
	[HarmonyPrefix]
	public static bool SaveManager_LoadFromFile()
	{
		if (File.Exists(SaveFilePath))
		{
			string json = File.ReadAllText(SaveFilePath);
			CurrentSaveFile = SaveManager.FromJSON<GrimoraSaveFile>(json);
		}
		else
		{
			CreateNewSaveFile();
		}

		return true;
	}

	[HarmonyPatch(typeof(RunState), "Run", MethodType.Getter)]
	[HarmonyPrefix]
	public static bool RunState_Run(ref RunState __result)
	{
		if (GrimoraSaveUtil.IsGrimora)
		{
			if (SaveFile.IsAscension)
			{
				__result = CurrentSaveFile.AscensionSaveData.currentRun;
			}
			else
			{
				__result = CurrentSaveFile.CurrentRun;
			}
			
			return false;
		}

		return true;
	}

	[HarmonyPatch(typeof(SaveFile), "CurrentDeck", MethodType.Getter)]
	[HarmonyPrefix]
	public static bool SaveFile_CurrentDeck(ref DeckInfo __result)
	{
		if (GrimoraSaveUtil.IsGrimora)
		{
			__result = CurrentSaveFile.CurrentRun.playerDeck;
			return false;
		}

		return true;
	}

	[HarmonyPatch(typeof(AscensionSaveData), "Data", MethodType.Getter)]
	[HarmonyPrefix]
	public static bool AscensionSaveData_Data(ref AscensionSaveData __result)
	{
		if (GrimoraSaveUtil.IsGrimora)
		{
			__result = CurrentSaveFile.AscensionSaveData;
			return false;
		}

		return true;
	}

	[HarmonyPatch(typeof(GrimoraSaveData), "Data", MethodType.Getter)]
	[HarmonyPrefix]
	public static bool GrimoraSaveData_Data(ref GrimoraSaveData __result)
	{
		if (GrimoraSaveUtil.IsGrimora)
		{				
			GrimoraRunState currentRun = null;
			if (SaveFile.IsAscension)
			{
				currentRun = (GrimoraRunState)CurrentSaveFile.AscensionSaveData.currentRun;
			}
			else
			{
				currentRun = CurrentSaveFile.CurrentRun;
			}
			
			__result = currentRun.boardData;
			return false;
		}

		return true;
	}
	#endregion
}
