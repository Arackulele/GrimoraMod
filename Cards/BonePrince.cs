using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameBonePrince = $"{GUID}_BonePrince";

	private void Add_BonePrince()
	{
		CardBuilder.Builder
			.SetBaseAttackAndHealth(2, 1)
			.SetBoneCost(1)
			.SetDescription("MY, WHAT A LOVELY PRINCE!")
			.SetNames(NameBonePrince, "Bone Prince")
			.Build();
	}
}
