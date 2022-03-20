using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameSpectrabbit = $"{GUID}_Spectrabbit";

	private void Add_Spectrabbit()
	{
		CardBuilder.Builder
			.SetBaseAttackAndHealth(0, 1)
			.SetNames(NameSpectrabbit, "Spectrabbit")
			.Build();
	}
}
