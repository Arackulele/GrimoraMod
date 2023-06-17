using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using UnityEngine;

namespace GrimoraMod;

public class Bounty : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;

	public const string RulebookName = "Pirates Bounty";

	private const string DefaultAbilityName = "ability_Bounty";

	public int stored = 0;

	private Texture GetIconTexture => AssetUtils.GetPrefab<Texture>($"{DefaultAbilityName}_{stored}");

	public override bool RespondsToTurnEnd(bool playerTurnEnd)
		{
				return playerTurnEnd;
		}

	public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer) => true;

		public override IEnumerator OnTurnEnd(bool playerTurnEnd)
		{
		if (ResourcesManager.Instance.PlayerEnergy > 0 && stored < 8)
		{

			yield return PreSuccessfulTriggerSequence();

			yield return new WaitForSeconds(0.2f);
			ViewManager.Instance.SwitchToView(View.Default);
			yield return new WaitForSeconds(0.2f);
			yield return ResourcesManager.Instance.SpendEnergy(1);
			yield return new WaitForSeconds(0.6f);
			stored += 2;

			ViewManager.Instance.SwitchToView(View.Board);
			Card.Anim.StrongNegationEffect();
			Card.RenderInfo.OverrideAbilityIcon(ability, GetIconTexture);
			Card.RenderCard();

			yield return new WaitForSeconds(0.3f);
			yield return LearnAbility(0.2f);

		}

		}

		public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
		{
		yield return base.PreSuccessfulTriggerSequence();
		if (stored > 0) yield return ResourcesManager.Instance.AddBones(stored - 1, Card.slot);
		yield return base.LearnAbility(0.4f);
		yield break;
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_Bounty()
	{
		const string rulebookDescription =
			"[creature] takes 1 Soul from you at the end of every turn and stores it as 2 Bones, up to a maximum of 8. "
		+ "When [creature] dies, you gain all soul contained in it.";

		AbilityBuilder<Bounty>.Builder
			.SetPixelIcon(AssetUtils.GetPrefab<Sprite>("bounty_pixel"))
		 .SetRulebookDescription(rulebookDescription)
		 .Build();
	}
}
