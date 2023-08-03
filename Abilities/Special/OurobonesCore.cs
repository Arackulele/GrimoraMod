using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;

namespace GrimoraMod;

public class OurobonesCore : SpecialCardBehaviour
{
	public static SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility FullSpecial;

	public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer) => true;

	public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
	{
		CardInfo changedInfo = Card.Info.Clone() as CardInfo;
		int additiveCost = (PlayableCard.HasAbility(Ability.Brittle) ? 1 : 2);
		changedInfo.bonesCost += additiveCost;
		changedInfo.baseAttack++;
		changedInfo.baseHealth++;
		changedInfo.Mods = new(Card.Info.Mods);
		if (changedInfo.baseAttack >= 7) AchievementManager.Unlock(GrimoraPlugin.SomethingEnds);
		yield return CardSpawner.Instance.SpawnCardToHand(changedInfo);
	}
}

public partial class GrimoraPlugin
{
	public static void Add_Ability_OurobonesCore()
	{
		ApiUtils.CreateSpecialAbility<OurobonesCore>();
	}
}
