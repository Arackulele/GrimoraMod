using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameHauntedMirror = $"{GUID}_HauntedMirror";

	private void Add_Card_HauntedMirror()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetSpecialAbilities(SpecialTriggeredAbility.Mirror)
			.SetBaseAttackAndHealth(0, 2)
			.SetEnergyCost(4)
			// .SetDescription("")
			.SetNames(NameHauntedMirror, "Haunted Mirror")
			.SetSpecialStatIcon(SpecialStatIcon.Mirror)
			.Build();
	}
}
