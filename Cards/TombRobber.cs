namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameTombRobber = "GrimoraMod_TombRobber";

	private void Add_TombRobber()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(ActivatedDrawSkeletonGrimora.ability)
			.SetBaseAttackAndHealth(0, 1)
			.SetDescription("NOTHING... NOTHING AGAIN... NO TREASURE IS LEFT ANYMORE.")
			.SetNames(NameTombRobber, "Tomb Robber")
			.Build();
	}
}
