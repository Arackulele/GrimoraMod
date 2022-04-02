using System.Collections;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public class ActivatedGainEnergySoulSucker : ActivatedAbilityBehaviour
{
	public const string defaultAbilityName = "ability_ActivatedGainEnergySoulSucker";
	
	public static Ability ability;

	public override Ability Ability => ability;

	public int kills = 0;

	public override bool CanActivate() => kills > 0 && !ResourcesManager.Instance.EnergyAtMax;

	public override IEnumerator Activate()
	{
		ViewManager.Instance.SwitchToView(View.Scales, lockAfter: true);
		yield return new WaitForSeconds(0.25f);
		yield return ResourcesManager.Instance.AddMaxEnergy(1);
		yield return ResourcesManager.Instance.AddEnergy(1);
		UpdateKillCountAndRerenderCard(--kills);
		yield return new WaitForSeconds(0.25f);
		ViewManager.Instance.SetViewUnlocked();
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
		GrimoraPlugin.Log.LogDebug($"[SoulSucker] setting kills to [{setKillsTo}]");
		ViewManager.Instance.SwitchToView(View.Board);
		kills = setKillsTo;
		Card.Anim.StrongNegationEffect();
		string iconNumber = setKillsTo == 0 ? defaultAbilityName : $"{defaultAbilityName}_{kills}";
		Card.RenderInfo.OverrideAbilityIcon(ability, AssetUtils.GetPrefab<Texture>(iconNumber));
		Card.RenderCard();
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_SoulSucker()
	{
		const string rulebookDescription =
			"When an opponent creature perishes, [creature] will store 1 soul energy, up to 4. Activating this sigil will add 1 soul energy to your current energy counter.";

		ApiUtils.CreateAbility<ActivatedGainEnergySoulSucker>(rulebookDescription, "Soul Sucker", true);
	}
}
