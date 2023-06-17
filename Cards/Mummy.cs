namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameMummy = $"{GUID}_Mummy";

	private void Add_Card_Mummy()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetBaseAttackAndHealth(3, 3)
			.SetBoneCost(8)
			.SetDescription("HIS AGE OF GLORY IS LONG GONE, YET IT IS WHAT KEEPS HIM GOING.")
			.SetNames(NameMummy, "Mummy Lord")
			.Build();
	}
}
