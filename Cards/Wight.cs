using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameWight = $"{GUID}_Wight";

	private void Add_Wight()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.CorpseEater)
			.SetBaseAttackAndHealth(2, 1)
			.SetBoneCost(5)
			// .SetDescription("THIS GELID SPECTER ENVELOPS WOULD-BE ATTACKERS IN AN ICY MIST.")
			.SetNames(NameWight, "Wight")
			.Build();
	}
}
