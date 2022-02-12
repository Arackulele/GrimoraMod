using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameObol = "GrimoraMod_Obol";

	private void Add_Obol()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.Reach, Ability.Sharp)
			.SetBaseAttackAndHealth(0, 3)
			.SetBoneCost(3)
			.SetDescription("The key to everything, somehow")
			.SetNames(NameObol, "Ancient Obol")
			.Build()
		);
	}
}
