using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod;

[HarmonyPatch(typeof(CreateDams))]
public class CreateDamsPatch
{
	[HarmonyPrefix, HarmonyPatch(nameof(CreateDams.SpawnedCardId), MethodType.Getter)]
	public static bool ChangeDialogue(ref string __result)
	{
		if (GrimoraSaveUtil.isNotGrimora)
		{
			return true;
		}

		__result = "Blocked on both sides. No Shipwrecks for the Forgotten Man.";
		return false;
	}

	[HarmonyPrefix, HarmonyPatch(nameof(CreateDams.SpawnedCardId), MethodType.Getter)]
	public static bool ChangeDamsToShipPieces(ref string __result)
	{
		if (GrimoraSaveUtil.isNotGrimora)
		{
			return true;
		}

		__result = GrimoraPlugin.NameShipwreckDams;
		return false;
	}
}
