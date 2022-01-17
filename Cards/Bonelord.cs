using APIPlugin;
using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameBonelord = "ara_Bonelord";

	private void AddAra_Bonelord()
	{
		NewCard.Add(CardBuilder.Builder
			.AsRareCard()
			.WithAbilities(Ability.Deathtouch)
			.WithBaseAttackAndHealth(5, 10)
			.WithBonesCost(6)
			.WithDescription("Lord of Bones, Lord of Bones, answer our call.")
			.WithEnergyCost(6)
			.WithNames(NameBonelord, "The Bone Lord")
			.WithPortrait(Resources.BoneLord)
			.Build()
		);
	}
}