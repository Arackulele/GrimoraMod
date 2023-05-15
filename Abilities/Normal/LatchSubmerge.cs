using DiskCardGame;

namespace GrimoraMod;

public class LatchSubmerge : Latch
{
	public static Ability ability;
	public override Ability Ability => ability;
	public override Ability LatchAbility => Ability.Submerge;
}

public partial class GrimoraPlugin
{
	public void Add_Ability_LatchSubmerge()
	{
		const string rulebookDescription =
			"When [creature] perishes, its owner chooses a creature to gain the Waterborne sigil.";

		AbilityBuilder<LatchSubmerge>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .Build();
	}
}
