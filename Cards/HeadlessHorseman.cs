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
			.SetDescription("THE APOCALYPSE IS SOON.")
			.SetNames(NameHeadlessHorseman, "Headless Horseman")
			.Build();
	}
}
