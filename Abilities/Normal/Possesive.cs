using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using UnityEngine;

namespace GrimoraMod;

public class Possessive : AbilityBehaviour
{
	public static Ability ability;
	public override Ability Ability => ability;

	public override bool RespondsToSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
	{
		return attacker && attacker.Slot == Card.OpposingSlot();
	}

	public override IEnumerator OnSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
	{
		attacker.Anim.StrongNegationEffect();
		yield break;
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_Possessive()
	{
		const string rulebookDescription =
			"[creature] cannot be attacked from the opposing slot. "
		+ "The opposing slot, if possible, instead attacks one of its adjacent friendly cards.";

		AbilityBuilder<Possessive>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .SetPixelIcon(AssetUtils.GetPrefab<Sprite>("posessive_pixel"))
		 .Build();
	}
}
