using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod
{
	[HarmonyPatch(typeof(CardDisplayer), "SetTextColors")]
	public class CardDisplayerPatches
	{
		public static bool Prefix(ref CardDisplayer __instance, CardRenderInfo renderInfo, PlayableCard playableCard)
		{
			if (!SaveManager.saveFile.IsGrimora)
			{
				return true;
			}

			if (playableCard is not null)
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
}