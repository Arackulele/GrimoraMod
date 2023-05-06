using System.Collections;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public class GrimoraRandomAbility : AbilityBehaviour
{
	public static Ability ability;
	public override Ability Ability => ability;

	public override bool RespondsToDrawn() => true;

	public override IEnumerator OnDrawn()
	{
		(PlayerHand.Instance as PlayerHand3D).MoveCardAboveHand(Card);
		yield return Card.FlipInHand(AddMod);
		yield return LearnAbility(0.5f);
	}

	public override bool RespondsToResolveOnBoard() => !Card.Status.hiddenAbilities.Contains(Ability);

	public override IEnumerator OnResolveOnBoard()
	{
		Card.Anim.PlayTransformAnimation();
		AddMod();
		yield return new WaitForSeconds(0.5f);
		yield return LearnAbility(0.5f);
	}

	private void AddMod()
	{
		Card.Status.hiddenAbilities.Add(Ability);
		CardModificationInfo cardModificationInfo = new CardModificationInfo(ChooseAbility());
		CardModificationInfo cardModificationInfo2 = Card.TemporaryMods.Find(x => x.HasAbility(Ability))
																							?? Card.Info.Mods.Find(x => x.HasAbility(Ability));

		if (cardModificationInfo2.IsNotNull())
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
			randomAbility = ElectricChairLever.AbilitiesMajorRisk.GetRandomItem();
			num2++;
			if (num2 > 100)
			{
				return Ability.Sharp;
			}
		}

		GrimoraPlugin.Log.LogDebug($"[GrimoraRandomAbility] Random ability chosen [{randomAbility}]");
		return randomAbility;
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_GrimoraRandomAbility()
	{
		const string rulebookDescription = "When [creature] is drawn, this sigil is replaced with another sigil at random.";
		AbilityBuilder<GrimoraRandomAbility>.Builder
		 .SetIcon(AbilitiesUtil.LoadAbilityIcon(Ability.RandomAbility.ToString()))
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName("Random Ability")
		 .SetPixelIcon(AssetUtils.GetPrefab<Sprite>("random_pixel"))
		 .Build();
	}
}
