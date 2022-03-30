using System.Collections;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public class ActivatedGainEnergySoulSucker : ActivatedAbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;

	public int kills = 0;

	public override bool CanActivate() => ResourcesManager.Instance.PlayerEnergy > 0;

	public override IEnumerator Activate()
	{
		ViewManager.Instance.SwitchToView(View.Scales, lockAfter: true);
		yield return ResourcesManager.Instance.AddEnergy(1);
		UpdateKillCountAndRerenderCard(--kills);
		yield return new WaitForSeconds(0.25f);
	}

	public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
	{
		return fromCombat && killer && !killer.OpponentCard;
	}

	public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
	{
		if(kills++ < 4)
		{
			UpdateKillCountAndRerenderCard(kills);
		}
		Card.Anim.StrongNegationEffect();
		yield break;
	}

	public void UpdateKillCountAndRerenderCard(int setKillsTo)
	{
		ViewManager.Instance.SwitchToView(View.Board);
		kills = setKillsTo;
		Card.Anim.StrongNegationEffect();
		string iconNumber = setKillsTo == 0 ? "ability_SoulSucker" : $"ability_SoulSucker_{kills}";
		Card.RenderInfo.OverrideAbilityIcon(ability, AssetUtils.GetPrefab<Texture>(iconNumber));
		Card.RenderCard();
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_SoulSucker()
	{
		const string rulebookDescription =
			"Remove 1 soul counter from [creature] to gain 1 soul. When an opposing creature perishes, this card will siphon their soul, up to 4 souls.";

		ApiUtils.CreateAbility<ActivatedGainEnergySoulSucker>(rulebookDescription);
	}
}
