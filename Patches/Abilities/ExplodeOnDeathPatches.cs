using System.Collections;
using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod;

[HarmonyPatch(typeof(ExplodeOnDeath))]
public class ExplodeOnDeathPatches
{
	[HarmonyPostfix, HarmonyPatch(nameof(ExplodeOnDeath.BombCard))]
	public static IEnumerator CorrectExceptionIfHooked(
		IEnumerator enumerator,
		ExplodeOnDeath __instance,
		PlayableCard target,
		PlayableCard attacker
	)
	{
		if (target.IsNull())
		{
			yield return __instance.ExplodeFromSlot(attacker.Slot);
		}
		else
		{
			yield return enumerator;
		}
	}
}
