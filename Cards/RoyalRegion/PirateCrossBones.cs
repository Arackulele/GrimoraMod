namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameCrossBones = $"{GUID}_Crossbones";

	private void Add_Card_Crossbones()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Bounty.ability, Anchored.ability)
			.SetBaseAttackAndHealth(0, 2)
			.SetBoneCost(1)
			.SetDescription("A DISTANT RELATIVE OF THE SCREAMING SKULL, HE IS NOW THE SYMBOL OF PIRACY ALL AROUND THE WORLD")
			.SetNames(NameCrossBones, "Crossbones")
			.Build();
	}
}
