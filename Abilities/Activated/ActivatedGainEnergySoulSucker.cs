using System.Collections;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public class ActivatedGainEnergySoulSucker : ActivatedAbilityBehaviour
{
	public const string RulebookName = "Soul Sucker";
	
	private const string DefaultAbilityName = "ability_ActivatedGainEnergySoulSucker";
	
	public static Ability ability;

	public override Ability Ability => ability;

	private Texture GetIconTexture => AssetUtils.GetPrefab<Texture>($"{DefaultAbilityName}_{kills}");

	public int kills = 0;

	public override bool CanActivate() => kills > 0 && ResourcesManager.Instance.PlayerEnergy < 6;

	public override IEnumerator Activate()
	{
		ViewManager.Instance.SwitchToView(View.Scales);
		yield return new WaitForSeconds(0.25f);
		yield return ResourcesManager.Instance.AddMaxEnergy(1);
		yield return ResourcesManager.Instance.AddEnergy(1);
		UpdateKillCountAndRerenderCard(--kills);
		yield return new WaitForSeconds(0.5f);
		ViewManager.Instance.SwitchToView(View.Default);
	}

	public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer) 
		=> Card.OnBoard && card.OpponentCard;

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
		Card.RenderInfo.OverrideAbilityIcon(ability, GetIconTexture);
		Card.RenderCard();
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_SoulSucker()
	{
		const string rulebookDescription =
			"When an opponent creature perishes, [creature] will store 1 soul energy, up to 4. Activating this sigil will add 1 soul energy to your current energy counter.";

		AbilityBuilder<ActivatedGainEnergySoulSucker>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName(ActivatedGainEnergySoulSucker.RulebookName)
		 .Build();
	}
}
