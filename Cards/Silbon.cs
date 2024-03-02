using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameSilbon = $"{GUID}_Silbon";

	private void Add_Card_Silbon()
	{
		CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(InvertedStrike.ability, Ability.Strafe)
			.SetBaseAttackAndHealth(3, 2)
			.SetBoneCost(6)
			.SetDescription("A SKILLED HUNTER. DON'T TRUST YOUR EARS WHEN YOU HEAR ITS HAUNTING WHISTLE.")
			.SetNames(NameSilbon, "Silbon")
			.Build().pixelPortrait=GrimoraPlugin.AllSprites.Find(o=>o.name=="pixel_silbon");
	}
}
