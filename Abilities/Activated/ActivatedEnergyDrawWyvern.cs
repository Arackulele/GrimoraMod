using System.Collections;
using DiskCardGame;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class ActivatedEnergyDrawWyvern : ActivatedAbilityBehaviour
{
	private const int ENERGY_COST = 2;

	public const string RulebookName = "Screeching Call";

	public static Ability ability;
	public override Ability Ability => ability;

	public override int EnergyCost => ENERGY_COST;

	public override IEnumerator Activate()
	{
		var info = Card.Info;
		info.Mods = new(info.Mods);
		yield return CardSpawner.Instance.SpawnCardToHand(info);
	}
}

public partial class GrimoraPlugin
{
	public static void Add_Ability_ActivatedEnergyDrawWyvern()
	{
		const string rulebookDescription = "Pay 2 Energy for [creature] to summon a copy in your hand.";

		AbilityBuilder<ActivatedEnergyDrawWyvern>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName(ActivatedEnergyDrawWyvern.RulebookName)
		 .Build();
	}
}
