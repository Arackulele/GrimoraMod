using DiskCardGame;
using static DiskCardGame.CardAppearanceBehaviour;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameDeathKnellBell = $"{GUID}_DeathKnell_Bell";

	private void Add_Card_DeathKnellBell()
	{
		CardBuilder.Builder
			.SetBaseAttackAndHealth(0, 1)
			.SetNames(NameDeathKnellBell, "Chime")
			.Build();
	}
}
