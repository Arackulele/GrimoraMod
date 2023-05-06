namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameScreamingSkull = $"{GUID}_ScreamingSkull";

	private void Add_Card_ScreamingSkull()
	{
		CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(AreaOfEffectStrike.ability)
			.SetBaseAttackAndHealth(1, 1)
			.SetBoneCost(2)
			.SetDescription("ONLY PAIN, NOTHING ELSE IS FELT BY THIS SKELETAL HEAD. WHAT A PITY.")
			.SetNames(NameScreamingSkull, "Screaming Skull")
			.Build().pixelPortrait=GrimoraPlugin.AllSprites.Find(o=>o.name=="pixel_skull");;
	}
}
