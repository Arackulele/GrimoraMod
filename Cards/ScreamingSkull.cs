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
			.SetBoneCost(2)
			.SetDescription("ONLY PAIN, NOTHING ELSE IS FELT BY THIS SKELETAL HEAD. WHAT A PITY.")
			.SetNames(NameScreamingSkull, "Screaming Skull")
			.Build()
		);
	}
}
