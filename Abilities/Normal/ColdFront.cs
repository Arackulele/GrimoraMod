using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using UnityEngine;

namespace GrimoraMod;

public class ColdFront : AbilityBehaviour
{
	public const string RulebookName = "Cold Front";

	private static bool _playedDialogueGrimoraGiantFrozen;
	
	public static Ability ability;

	public override Ability Ability => ability;

	public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer) => true;

	public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
	{
		PlayableCard opposingSlotCard = Card.OpposingCard();
		if (opposingSlotCard)
		{
			if (opposingSlotCard.HasTrait(Trait.Giant))
			{
				if (!_playedDialogueGrimoraGiantFrozen)
				{
					yield return new WaitForSeconds(0.25f);
					yield return TextDisplayer.Instance.ShowUntilInput($"THE BITTER CHILL MIGHT HURT, BUT IT WON'T SLOW DOWN {opposingSlotCard.Info.DisplayedNameLocalized}!");
					_playedDialogueGrimoraGiantFrozen = true;
				}
				yield return opposingSlotCard.TakeDamage(4, Card);
			}
			else if (opposingSlotCard.LacksAbility(Ability.IceCube))
			{
				var frozenAway = GrimoraModKayceeBossSequencer.CreateModForFreeze(opposingSlotCard);
				opposingSlotCard.RemoveAbilityFromThisCard(frozenAway);
				opposingSlotCard.Anim.PlayTransformAnimation();
				yield return new WaitForSeconds(0.25f);
			}
			yield break;
		}
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_ColdFront()
	{
		const string rulebookDescriptionEnglish = "When [creature] perishes, the card opposing it is Frozen Away if not already frozen.";
		const string rulebookDescriptionChinese = "[creature]阵亡时，它对面的造物如果没有被冰封，则会被冰封禁锢。无法冰封某些造物。";
		string rulebookDescription = Localization.CurrentLanguage == Language.ChineseSimplified ? rulebookDescriptionChinese : rulebookDescriptionEnglish;

		AbilityBuilder<ColdFront>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName(ColdFront.RulebookName)
		 .Build();
	}
}
