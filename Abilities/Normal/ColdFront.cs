using System.Collections;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public class ColdFront : AbilityBehaviour
{
	private static bool _playedDialogueGrimoraGiantFrozen;
	
	public static Ability ability;

	public override Ability Ability => ability;

	public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer) => true;

	public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
	{
		PlayableCard opposingSlotCard = Card.Slot.opposingSlot.Card;
		if (opposingSlotCard)
		{
			if (opposingSlotCard.Info.SpecialAbilities.Contains(GrimoraGiant.FullSpecial.Id))
			{
				if (!_playedDialogueGrimoraGiantFrozen)
				{
					yield return new WaitForSeconds(0.25f);
					yield return TextDisplayer.Instance.ShowUntilInput($"THE BITTER CHILL MIGHT HURT, BUT IT WON'T SLOW DOWN {opposingSlotCard.Info.DisplayedNameLocalized}!");
					_playedDialogueGrimoraGiantFrozen = true;
				}
				yield return opposingSlotCard.TakeDamage(4, Card);
			}
			else
			{
				var frozenAway = GrimoraModKayceeBossSequencer.CreateModForFreeze(opposingSlotCard);
				if (frozenAway.negateAbilities.Exists(opposingSlotCard.HasAbility))
				{
					GrimoraPlugin.Log.LogDebug($"[ColdFront.OnDie] Opposing Card {opposingSlotCard.GetNameAndSlot()} has negated ability, removing...");
					opposingSlotCard.RemoveAbilityFromThisCard(frozenAway);
				}
				else
				{
					GrimoraPlugin.Log.LogDebug($"[ColdFront.OnDie] Opposing Card {opposingSlotCard.GetNameAndSlot()} does not have a negated ability, just adding temp mod.");
					opposingSlotCard.AddTemporaryMod(frozenAway);
				}
				opposingSlotCard.Anim.PlayTransformAnimation();
				yield return new WaitForSeconds(0.25f);
			}
		}
	}
}

public partial class GrimoraPlugin
{
	public void _Ability_ColdFront()
	{
		const string rulebookDescription = "When [creature] perishes, the card opposing it is Frozen Away if not already frozen.";

		ApiUtils.CreateAbility<ColdFront>(rulebookDescription);
	}
}
