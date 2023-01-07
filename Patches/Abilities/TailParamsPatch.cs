using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod;

[HarmonyPatch(typeof(TailParams))]
public class TailParamsPatch
{
	[HarmonyPrefix, HarmonyPatch(nameof(TailParams.GetDefaultTail))]
	public static bool ChangeDefaultTailForGrimora(CardInfo info, ref CardInfo __result)
	{
		if (GrimoraSaveUtil.IsNotGrimoraModRun)
		{
			return true;
		}
		
		CardInfo cardByName = GrimoraPlugin.AllGrimoraModCards.Single(cardInfo => cardInfo.name == GrimoraPlugin.NameRotTail);
		CardModificationInfo item = new CardModificationInfo
		{
			nameReplacement = string.Format(Localization.Translate("{0} Limb"), info.DisplayedNameLocalized)
		};
		cardByName.Mods.Add(item);
		__result = cardByName;
		return false;
	}
}
