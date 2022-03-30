namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameWrither = $"{GUID}_Writher";

	private void Add_Card_Writher()
	{
		CardBuilder.Builder
			//Removed for now as its not fun, maybe at a special event later
			.SetAsRareCard()
			.SetAbilities(LooseLimb.ability)
			.SetBaseAttackAndHealth(3, 1)
			.SetBoneCost(7)
			// .SetDescription("A SENSE OF DREAD CONSUMES YOU AS YOU REALIZE YOU ARE NOT ALONE IN THESE WOODS.")
			.SetNames(NameWrither, "Writher")
			.Build()
			;
	}
}
