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
		 .SetDescription("Any attempts to strike at it will only make it shed it's prickly spine as a painful decoy.")
		 .SetNames(NameWrither, "Writher")
		 .SetTail(NameWritherTail)
		 .Build();
	}
}
