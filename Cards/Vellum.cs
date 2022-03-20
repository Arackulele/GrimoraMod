using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameVellum = $"{GUID}_Vellum";

	private void Add_Vellum()
	{
		CardBuilder.Builder
			.SetBaseAttackAndHealth(0, 2)
			.SetNames(NameVellum, "Vellum")
			.SetTraits(Trait.Pelt)
			.Build();
	}
}
