using DiskCardGame;

namespace GrimoraMod;

public class GiantStrikeEnraged : GiantStrike
{
	public new static Ability ability;

	public override Ability Ability => ability;

	private void Awake()
	{
		Card.Anim.PlayTransformAnimation();

		Card.StatsLayer.SetEmissionColor(GameColors.Instance.red);
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_GiantStrikeEnraged()
	{
		const string rulebookDescription = "[creature] will strike each opposing space.";

		AbilityBuilder<GiantStrikeEnraged>.Builder
		 .FlipIconIfOnOpponentSide()
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName("Enraged Giant")
		 .Build();
	}
}
