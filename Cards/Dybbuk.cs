namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameDybbuk = $"{GUID}_Dybbuk";

	private void Add_Card_Dybbuk()
	{
		CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(Possessive.ability)
			.SetBaseAttackAndHealth(0, 1)
			.SetBoneCost(3)
			.SetDescription("NO ONE KNOWS WHAT EXACTLY THE DYBBUK IS, SOME SAY IT IS BETTER LEFT UNKNOWN.")
			.SetNames(NameDybbuk, "Dybbuk")
			.Build().pixelPortrait = GrimoraPlugin.AllSprites.Find(o=>o.name=="dybbuk_pixel");
	}
}
