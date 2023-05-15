namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameBonePrince = $"{GUID}_BonePrince";

	private void Add_Card_BonePrince()
	{
		CardBuilder.Builder
			.SetBaseAttackAndHealth(2, 1)
			.SetBoneCost(1)
			.SetDescription("IF I AM TRAPPED, SO WILL YOU. YOU CANNOT END THIS.")
			.SetEvolve(NameBonelord, 1)
			.SetNames(NameBonePrince, "Bone Prince")
			.Build();
	}
}
