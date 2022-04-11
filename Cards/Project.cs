namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameProject = $"{GUID}_Project";

	private void Add_Card_Project()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(ChaosStrike.ability)
			.SetBaseAttackAndHealth(1, 3)
			.SetBoneCost(7)
			.SetDescription("AN EXPERIMENT GONE WRONG, OR RIGHT. IT DEPENDS ON YOUR WORLD VIEW.")
			.SetNames(NameProject, "Project")
			.Build();
	}
}
