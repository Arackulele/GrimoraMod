using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameBloodySack = $"{GUID}_BloodySack";

	private void Add_Card_BloodySack()
	{
		CardBuilder.Builder
		 .SetAsNormalCard()
		 .SetAbilities(Ability.DrawRandomCardOnDeath)
		 .SetBaseAttackAndHealth(0, 2)
		 .SetBoneCost(2)
		 .SetDescription("This sack is full of body parts. Maybe you'll find something useful in there?")
		 .SetNames(NameBloodySack, "Bloody Sack")
		 .Build();
	}
}
