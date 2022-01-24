using System.Collections;
using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public class PayEnergyForWyvern : ActivatedAbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;
	public override int EnergyCost => 3;

	public override IEnumerator Activate()
	{
		yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(CardLoader.GetCardByName("ara_Wyvern"), 0.25f);
		yield break;
	}

	public static NewAbility CreatePayEnergyForWyvern()
		{
		const string rulebookDescription =
			"Pay 3 Energy for [creature] to summon a Wyvern in your Hand. " +
			"More Wyverns, wyverns everywhere.";

		return ApiUtils.CreateAbility<PayEnergyForWyvern>(
			GrimoraPlugin.AllSpriteAssets.Single(spr => spr.name == "PayBonesForWyvern").texture,
			nameof(PayEnergyForWyvern),
			rulebookDescription,
		5,
		activated: true
		);
	}
}
