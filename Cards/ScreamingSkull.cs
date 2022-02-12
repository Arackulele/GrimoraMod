using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameScreamingSkull = "GrimoraMod_ScreamingSkull";

	private void Add_ScreamingSkull()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(AreaOfEffectStrike.ability)
			.SetBaseAttackAndHealth(1, 1)
			.SetBoneCost(5)
			.SetDescription("Only pain, nothing else is felt by this Skeletal head.What a pity")
			.SetNames(NameScreamingSkull, "Screaming Skull")
			.Build()
		);
	}
}
