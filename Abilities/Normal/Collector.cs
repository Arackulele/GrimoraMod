using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using UnityEngine;

namespace GrimoraMod;

public class Collector : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;

	public const string RulebookName = "Collector";

	public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer) => true;



		public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
		{
		yield return base.PreSuccessfulTriggerSequence();
		Singleton<ViewManager>.Instance.SwitchToView(View.Consumables);


		if (RunState.Run.consumables.Count() < 3) { 

			RunState.Run.consumables.Add(GrimoraPlugin.AllGrimoraItems.GetRandomItem().name);
		Singleton<ItemsManager>.Instance.UpdateItems();
		}
		else Card.Anim.StrongNegationEffect();

		Singleton<ViewManager>.Instance.SwitchToView(View.Default);
		yield return base.LearnAbility(0.4f);
		yield break;
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_Collector()
	{
		const string rulebookDescription =
			"When [creature] dies, gain a consumable item, if you have a free slot.";

		AbilityBuilder<Collector>.Builder
		 .SetPixelIcon(AssetUtils.GetPrefab<Sprite>("relichoarder2"))
		 .SetRulebookDescription(rulebookDescription)
		 .Build();
	}
}
