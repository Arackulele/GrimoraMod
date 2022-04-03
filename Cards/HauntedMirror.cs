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
			.SetDescription("I advise against staring into it, you won't like what stares back.")
			.SetEnergyCost(4)
			.SetNames(NameHauntedMirror, "Haunted Mirror")
			.SetSpecialStatIcon(SpecialStatIcon.Mirror)
			.Build();
	}
}
