using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using UnityEngine;

namespace GrimoraMod;

public class GrimoraRandomAbility : AbilityBehaviour
{
	public static Ability ability;
	public override Ability Ability => ability;

	public override bool RespondsToDrawn()
	{
		return true;
	}

	public override IEnumerator OnDrawn()
	{
		(PlayerHand.Instance as PlayerHand3D).MoveCardAboveHand(Card);
		yield return Card.FlipInHand(AddMod);
		yield return LearnAbility(0.5f);
	}

	private void AddMod()
	{
		Card.Status.hiddenAbilities.Add(Ability);
		CardModificationInfo cardModificationInfo = new CardModificationInfo(ChooseAbility());
		CardModificationInfo cardModificationInfo2 = Card.TemporaryMods.Find(x => x.HasAbility(Ability));
		if (cardModificationInfo2 == null)
		{
			cardModificationInfo2 = Card.Info.Mods.Find(x => x.HasAbility(Ability));
		}

		if (cardModificationInfo2 != null)
		{
			cardModificationInfo.fromTotem = cardModificationInfo2.fromTotem;
			cardModificationInfo.fromCardMerge = cardModificationInfo2.fromCardMerge;
		}

		Card.AddTemporaryMod(cardModificationInfo);
	}

	private Ability ChooseAbility()
	{
		Ability randomAbility = Ability.RandomAbility;
		int num2 = 0;
		while (randomAbility == Ability.RandomAbility)
		{
			var randomizedList = ElectricChairSequencer.AbilitiesToChoseRandomly.Randomize().ToList();
			randomAbility = randomizedList[UnityEngine.Random.RandomRangeInt(0, randomizedList.Count)];
			num2++;
			if (num2 > 100)
			{
				return Ability.Sharp;
			}
		}

		GrimoraPlugin.Log.LogDebug($"[GrimoraRandomAbility] Random ability chosen [{randomAbility}]");
		return randomAbility;
	}


	public static AbilityManager.FullAbility Create()
	{
		const string rulebookDescription = "When [creature] is drawn, this sigil is replaced with another sigil at random.";
		Texture icon = AbilitiesUtil.LoadAbilityIcon(Ability.RandomAbility.ToString());
		return ApiUtils.CreateAbility<GrimoraRandomAbility>(rulebookDescription, "Random Ability", rulebookIcon: icon);
	}
}
