using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameHeadlessHorseman = $"{GUID}_HeadlessHorseman";

	private void Add_Card_HeadlessHorseman()
	{
		CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(Ability.Flying, Ability.Strafe)
			.SetBaseAttackAndHealth(5, 5)
			.SetBoneCost(13)
			.SetDescription("THEY CALL HER THE RIDER OF THE APOCALYPSE. HER FLAMING BLADE CUTS THROUGH THE LIVING AND DEAD ALIKE.")
			.SetNames(NameHeadlessHorseman, "Headless Horseman")
			.Build();
	}
}
