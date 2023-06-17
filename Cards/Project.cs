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
			.SetDescription("AN EXPERIMENT GONE WRONG, IN A FUTILE ATTEMPT TO IMMITATE THE DANSE MACCABRE. THEY DO NOT FUNCTION TOGETHER.")
			.SetNames(NameProject, "Project")
			.Build();
	}
}
