using DiskCardGame;

namespace GrimoraMod;

public class Raider : StrikeAdjacentSlots
{
	public static Ability ability;

	public override Ability Ability => ability;

	protected override Ability StrikeAdjacentAbility => ability;
}

public partial class GrimoraPlugin
{
	public void Add_Ability_Raider()
	{
		const string rulebookDescription = "[creature] will strike its adjacent slots, except other Raiders.";

		AbilityBuilder<Raider>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .Build();
	}
}
