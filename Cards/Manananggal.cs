using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameManananggal = $"{GUID}_Manananggal";

	private void Add_Card_Manananggal()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.Flying)
			.SetBaseAttackAndHealth(3, 3)
			.SetBoneCost(8)
			.SetDescription("NO BRUTALITY SATIATES THE MANANANGGAL, WHEN YOU SEE HER SEVER FROM HER TORSO, YOU TOO WILL BE A VICTIM OF THE HUNT.")
			.SetNames(NameManananggal, "Manananggal")
			.Build();
	}
}
