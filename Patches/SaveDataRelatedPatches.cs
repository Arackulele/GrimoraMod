using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Ascension;
using InscryptionAPI.Saves;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch]
public class SaveDataRelatedPatches
{
	private static bool _isGrimoraModRun;
	private static bool _lastRunWasGrimoraModRun;
	public static bool IsNotGrimoraModRun => !IsGrimoraModRun;

	public static bool IsGrimoraModRun
	{
		get => _isGrimoraModRun;
		set
		{
			_isGrimoraModRun = value; 
			Log.LogInfo("[SaveDataRelatedPatches] _isGrimoraModRun set to " + value);
		}
	}

	public static bool LastRunWasGrimoraModRun
	{
		get => _lastRunWasGrimoraModRun;
		set
		{
			_lastRunWasGrimoraModRun = value; 
			Log.LogInfo("[SaveDataRelatedPatches] _lastRunWasGrimoraModRun set to " + value);
		}
	}
}
