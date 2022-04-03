using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod;

[HarmonyPatch(typeof(Sharp))]
public class SharpPatches
{
	[HarmonyPostfix, HarmonyPatch(nameof(Sharp.RespondsToTakeDamage))]
	public static void CheckForAttackBeingGreaterThanZero(PlayableCard source, ref bool __result)
	{
		if (source)
		{
			__result &= source.Attack > 0;
		}
		
	}
}
