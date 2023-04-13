using DiskCardGame;
using UnityEngine;
namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameObol = $"{GUID}_Obol";

	private void Add_Card_Obol()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.Reach, Ability.Sharp)
			.SetBaseAttackAndHealth(0, 3)
			.SetBoneCost(3)
			.SetDescription("THE KEY TO EVERYTHING, SOMEHOW.")
			.SetNames(NameObol, "Ancient Obol")
			.Build().pixelPortrait = GrimoraPlugin.AllSprites.Find(o => o.name == "obol_pixel");
	}
}
