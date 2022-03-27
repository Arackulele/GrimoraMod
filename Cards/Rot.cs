namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameRot = $"{GUID}_Rot";

	private void Add_Card_Rot()
	{
		CardBuilder.Builder
			//Removed for now as its not fun, maybe at a special event later
			.SetAsNormalCard()
			.SetAbilities(LooseLimb.ability)
			.SetBaseAttackAndHealth(1, 2)
			.SetBoneCost(5)
			// .SetDescription("A SENSE OF DREAD CONSUMES YOU AS YOU REALIZE YOU ARE NOT ALONE IN THESE WOODS.")
			.SetNames(NameRot, "Rot")
			.Build()
			;
	}
}
