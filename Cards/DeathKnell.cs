using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameDeathKnell = $"{GUID}_DeathKnell";

	private void Add_Card_DeathKnell()
	{
		CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(Ability.CreateBells)
			.SetSpecialAbilities(SpecialTriggeredAbility.BellProximity, SpecialTriggeredAbility.Daus)
			.SetBaseAttackAndHealth(0, 2)
			.SetBoneCost(8)
			.SetDescription("FOR WHOM THE BELL TOLLS?")
			.SetNames(NameDeathKnell, "Death Knell")
			.SetSpecialStatIcon(SpecialStatIcon.Bell)
			.Build().pixelPortrait=GrimoraPlugin.AllSprites.Find(o=>o.name=="deathknell_pixel");
	}
}
