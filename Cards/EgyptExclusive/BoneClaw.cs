using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameBoneclaw = $"{GUID}_Boneclaw";

	private void Add_Card_Boneclaw()
	{
		CardBuilder.Builder
			.SetAbilities(Slasher.ability)
			.SetBaseAttackAndHealth(2, 2)
			.SetNames(NameBoneclaw, "Boneclaw")
			.SetBoneCost(7)
			.Build();
	}
}
