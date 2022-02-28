using System.Collections;
using APIPlugin;
using DiskCardGame;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class ActivatedEnergyDrawWyvern : ActivatedAbilityBehaviour
{
	public static readonly NewAbility NewAbility = Create();

	public static Ability ability;

	private const int ENERGY_COST = 2;

	public override Ability Ability => ability;

	public override int EnergyCost => ENERGY_COST;

	public override IEnumerator Activate()
	{
		yield return CardSpawner.Instance.SpawnCardToHand(NameWyvern.GetCardInfo());
	}

	public static NewAbility Create()
	{
		string rulebookDescription = $"Pay {ENERGY_COST} Energy for [creature] to summon a Wyvern in your hand.";

		return ApiUtils.CreateAbility<ActivatedEnergyDrawWyvern>(rulebookDescription, "Screeching Call", true);
	}
}
