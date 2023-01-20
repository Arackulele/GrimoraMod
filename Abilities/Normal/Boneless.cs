using DiskCardGame;

namespace GrimoraMod;

public class Boneless : AbilityBehaviour
{
	public const string RulebookName = "Boneless";

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
		 .SetRulebookName(Boneless.RulebookName)
		 .Build();
	}
}
