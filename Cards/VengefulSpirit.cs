using APIPlugin;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameVengefulSpirit = "GrimoraMod_VengefulSpirit";

	private void Add_VengefulSpirit()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetBaseAttackAndHealth(1, 1)
			.SetEnergyCost(3)
			.SetDescription("Coming for vengeance!")
			.SetNames(NameVengefulSpirit, "Vengeful Spirit")
			.Build()
		);
	}
}
