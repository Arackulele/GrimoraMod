using System.Collections;
using DiskCardGame;
using GrimoraMod.Extensions;
using Sirenix.Utilities;
using InscryptionAPI.Helpers.Extensions;

namespace GrimoraMod;

public class Sculptor : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;


	public override bool RespondsToResolveOnBoard() => true;

	private CardModificationInfo _modInfo;
	public override IEnumerator OnResolveOnBoard()
	{
		yield return PreSuccessfulTriggerSequence();
		ViewManager.Instance.SwitchToView(View.Board);


		


		foreach (var i in Card.Slot.GetAdjacentSlots(true))
		{
			GrimoraPlugin.Log.LogDebug("1");
			if (i != null && i.Card != null && i.Card.AllAbilities().Count < 5)
			{
				GrimoraPlugin.Log.LogDebug("2");
				List<Ability> abilitiesToAdd = Card.AllAbilities();
				GrimoraPlugin.Log.LogDebug("3");
				if (abilitiesToAdd.Contains(Sculptor.ability)) abilitiesToAdd.Remove(Sculptor.ability);
				GrimoraPlugin.Log.LogDebug("4");
				if (abilitiesToAdd != null)
				{
					_modInfo = new CardModificationInfo
					{
						abilities = new List<Ability>(abilitiesToAdd)
					};

					_modInfo.abilities = abilitiesToAdd;
					i.Card.AddTemporaryMod(_modInfo);
					i.Card.Anim.PlayTransformAnimation();
					i.Card.UpdateStatsText();
					i.Card.RenderCard();
				};
			}

		}
		LearnAbility();
		yield break;
	}

}

public partial class GrimoraPlugin
{
	public void Add_Ability_Sculptor()
	{
		const string rulebookDescription =
			"When [creature] is played, it carves its Sigils into adjacent Cards.";

		AbilityBuilder<Sculptor>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName("Sculptor")
		 .Build();
	}
}
