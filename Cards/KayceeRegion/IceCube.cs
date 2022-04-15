namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameIceCube = $"{GUID}_IceCube";

	private void Add_Card_IceCube()
	{
		CardBuilder.Builder
		 // .SetAsNormalCard()
		 .SetAbilities(ColdFront.ability)
		 .SetBaseAttackAndHealth(1, 1)
		 .SetBoneCost(5)
		 .SetDescription("Those who strike at it will end up sharing their frosty fate!")
		 .SetNames(NameIceCube, "Ice Cube")
		 .Build();
	}
}
