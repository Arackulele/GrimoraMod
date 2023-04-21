namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameVengefulSpirit = $"{GUID}_VengefulSpirit";

	private void Add_Card_VengefulSpirit()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetBaseAttackAndHealth(1, 1)
			.SetEnergyCost(3)
			.SetDescription("TO NEVER BE SATISFIED, TO ALWAYS WANT MORE. THIS  SPIRIT SHALL NEVER FIND ITS PEACE.")
			.SetNames(NameVengefulSpirit, "Vengeful Spirit")
			.Build().pixelPortrait=GrimoraPlugin.AllSprites.Find(o=>o.name=="pixel_spirit");;
	}
}
