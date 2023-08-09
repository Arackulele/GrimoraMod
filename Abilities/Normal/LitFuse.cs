using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using UnityEngine;

namespace GrimoraMod;

public class LitFuse : ExplodeOnDeath
{
	public const string RulebookName = "Lit Fuse";

	public static Ability ability;

	public override Ability Ability => ability;

	public override bool RespondsToUpkeep(bool playerUpkeep)
	{
		return Card.IsPlayerCard() && playerUpkeep || Card.OpponentCard && !playerUpkeep;
	}

	public override IEnumerator OnUpkeep(bool playerUpkeep)
	{
		if (Card.NotDead())
		{
			ViewManager.Instance.SwitchToView(View.Board, lockAfter: true);
			yield return new WaitForSeconds(0.25f);
			yield return Card.TakeDamage(1, null);
			yield return new WaitForSeconds(0.25f);
			ViewManager.Instance.SetViewUnlocked();
		}
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_LitFuse()
	{
		const string rulebookDescriptionEnglish =
			"[creature] loses 1 health per turn. "
		+ "When [creature] dies, the creature opposing it, as well as adjacent friendly creatures, are dealt 10 damage.";
		const string rulebookDescriptionChinese =
			"[creature]每回合损失1点生命。"
		+ "当它死亡时，正对面的造物以及相邻的友方造物均受到10点伤害。";
		string rulebookDescription = Localization.CurrentLanguage == Language.ChineseSimplified ? rulebookDescriptionChinese : rulebookDescriptionEnglish;

		AbilityBuilder<LitFuse>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName(LitFuse.RulebookName)
		 .Build();
	}
}
