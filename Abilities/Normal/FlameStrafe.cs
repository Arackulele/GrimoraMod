using System.Collections;
using DiskCardGame;
using InscryptionAPI.Helpers.Extensions;
using Sirenix.Utilities;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class FlameStrafe : Strafe
{
	public const string RulebookName = "Flame Strafe";

	public static Ability ability;

	public override Ability Ability => ability;

	public override IEnumerator PostSuccessfulMoveSequence(CardSlot oldSlot)
	{
		if (oldSlot.Card.SafeIsUnityNull())
		{
			yield return oldSlot.CreateCardInSlot(NameFlames.GetCardInfo());
		}
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_FlameStrafe()
	{
		const string rulebookDescriptionEnglish =
			"Whenever [creature] moves, it leaves a trail of Embers. The warmth of the Embers shall enlighten nearby cards.";
		const string rulebookDescriptionChinese =
			"[creature]移动时，会留下余烬。余烬的温度可以点燃相邻的卡牌。";
		string rulebookDescription = Localization.CurrentLanguage == Language.ChineseSimplified ? rulebookDescriptionChinese : rulebookDescriptionEnglish;

		AbilityBuilder<FlameStrafe>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName(FlameStrafe.RulebookName)
		 .Build();
	}
}
