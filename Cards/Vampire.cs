namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameVampire = $"{GUID}_Vampire";

	private void Add_Card_Vampire()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(BloodGuzzler.ability)
			.SetBaseAttackAndHealth(2, 1)
			.SetBoneCost(6)
			.SetDescription("THE VAMPIRE IS A CREATURE OF LEGENDS. IN REALITY, LITTLE BLOOD TO DRINK AFTER EVERYONE HAS DIED.")
			.SetNames(NameVampire, "Vampire")
			.Build();
	}
}
