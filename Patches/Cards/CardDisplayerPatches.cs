using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod;

[HarmonyPatch(typeof(CardDisplayer))]
public class CardDisplayerPatches
{
	[HarmonyPrefix, HarmonyPatch(nameof(CardDisplayer.SetTextColors))]
	public static bool PrefixChangeRenderColors(ref CardDisplayer __instance, CardRenderInfo renderInfo,
		PlayableCard playableCard)
	{
		if (GrimoraSaveUtil.isNotGrimora)
		{
			return true;
		}

		if (playableCard)
		{
			int maxHealth = playableCard.MaxHealth;
		}
		else
		{
			int health = renderInfo.baseInfo.Health;
		}

		__instance.SetHealthTextColor((renderInfo.health >= renderInfo.baseInfo.Health)
			? GameColors.Instance.glowSeafoam
			: GameColors.Instance.darkLimeGreen);

		__instance.SetAttackTextColor(GameColors.Instance.glowSeafoam);
		__instance.SetNameTextColor(GameColors.Instance.glowSeafoam);

		return false;
	}
}
