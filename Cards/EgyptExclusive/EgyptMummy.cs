using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameEgyptMummy = $"{GUID}_EgyptMummy";

	private void Add_Card_EgyptMummy()
	{
		CardBuilder.Builder
			.SetAbilities(Boneless.ability)
			.SetBaseAttackAndHealth(1, 1)
			.SetNames(NameEgyptMummy, "Old Mummy")
			.SetBoneCost(1)
			.Build();
	}
}
