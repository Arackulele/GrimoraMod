using System.Collections;
using APIPlugin;
using DiskCardGame;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class ActivatedEnergyDrawWyvern : ActivatedAbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;
	public override int EnergyCost => 3;

	public override IEnumerator Activate()
	{
		yield return CardSpawner.Instance.SpawnCardToHand(NameWyvern.GetCardInfo());
	}

	public static NewAbility Create()
	{
		const string rulebookDescription = "Pay 3 Energy for [creature] to summon a Wyvern in your hand.";

		return ApiUtils.CreateAbility<ActivatedEnergyDrawWyvern>
			(rulebookDescription, "Screeching Call", true);
	}
}
