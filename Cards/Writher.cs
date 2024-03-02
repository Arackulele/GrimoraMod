namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameWrither = $"{GUID}_Writher";

	private void Add_Card_Writher()
	{
		CardBuilder.Builder
		 .SetAsRareCard()
		 .SetAbilities(LooseLimb.ability)
		 .SetBaseAttackAndHealth(3, 1)
		 .SetBoneCost(6)
		 .SetDescription("A FALLEN GIANTS SPINE THAT IS TRYING TO FIND A NEW HOST. REGRETTABLY, MOST CREATURES ARE QUITE SMALL.")
		 .SetNames(NameWrither, "Writher")
		 .SetTail(NameWritherTail)
		 .Build();
	}
}
