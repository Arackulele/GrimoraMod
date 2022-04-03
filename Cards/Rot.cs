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
			.SetDescription("It's a wonder this one has stayed together! Though it may lose that arm if it tries to flee...")
			.SetNames(NameRot, "Rot")
			.Build()
			;
	}
}
