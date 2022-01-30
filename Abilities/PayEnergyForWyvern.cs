using System.Collections;
using APIPlugin;
using DiskCardGame;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class PayEnergyForWyvern : ActivatedAbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;
	public override int EnergyCost => 3;

	public override IEnumerator Activate()
	{
		yield return CardSpawner.Instance.SpawnCardToHand(CardLoader.GetCardByName(NameWyvern), 0.25f);
		yield break;
	}

	public static NewAbility Create()
	{
		const string rulebookDescription =
			"Pay 3 Energy for [creature] to summon a Wyvern in your hand. " +
			"More wyverns. Wyverns everywhere.";

		return ApiUtils.CreateAbility<PayEnergyForWyvern>(rulebookDescription, activated: true);
	}
}
