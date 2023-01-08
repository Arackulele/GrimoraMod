using HarmonyLib;

namespace GrimoraMod;

[HarmonyPatch]
public class SaveDataRelatedPatches
{
	public static bool IsNotGrimoraModRun => !IsGrimoraModRun;
	
	private static bool _isGrimoraModRun;
	private static bool _lastRunWasGrimoraModRun;

	public static bool IsGrimoraModRun
	{
		get => _isGrimoraModRun;
		set => _isGrimoraModRun = value;
	}

	public static bool LastRunWasGrimoraModRun
	{
		get => _lastRunWasGrimoraModRun;
		set => _lastRunWasGrimoraModRun = value;
	}
}
