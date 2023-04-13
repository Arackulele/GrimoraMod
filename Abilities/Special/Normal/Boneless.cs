using DiskCardGame;

namespace GrimoraMod;

public class Boneless : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;
}

public partial class GrimoraPlugin
{
	public void Add_Ability_Boneless()
	{
		const string rulebookDescription = "[creature] yields no bones upon death.";

		AbilityBuilder<Boneless>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .Build();
	}
}
