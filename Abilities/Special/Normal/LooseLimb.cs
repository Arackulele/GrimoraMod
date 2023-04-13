using DiskCardGame;

namespace GrimoraMod;

public class LooseLimb : TailOnHit
{
	public static Ability ability;

	public override Ability Ability => ability;
}

public partial class GrimoraPlugin
{
	public void Add_Ability_LooseLimb()
	{
		const string rulebookDescription =
			"When [creature] would be struck, a severed limb is created in its place and this card moves into an adjacent open slot.";

		AbilityBuilder<LooseLimb>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .Build();
	}
}
