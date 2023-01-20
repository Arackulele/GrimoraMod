using DiskCardGame;

namespace GrimoraMod;

public class Anchored : AbilityBehaviour
{
	public const string RulebookName = "Anchored";

	public static Ability ability;

	public override Ability Ability => ability;
}

public partial class GrimoraPlugin
{
	public void Add_Ability_Anchored()
	{
		const string rulebookDescription = "[creature] is unaffected by the motion of the ship.";

		AbilityBuilder<Anchored>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName(Anchored.RulebookName)
		 .Build();
	}
}
