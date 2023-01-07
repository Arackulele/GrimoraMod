using DiskCardGame;

namespace GrimoraMod;

public static class GrimoraSaveUtil
{
	public static bool IsGrimoraModRun => SaveDataRelatedPatches.IsGrimoraModRun;

	public static bool IsNotGrimoraModRun => SaveDataRelatedPatches.IsNotGrimoraModRun;
}
