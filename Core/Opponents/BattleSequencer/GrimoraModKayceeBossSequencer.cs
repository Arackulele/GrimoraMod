using System.Collections;
using DiskCardGame;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class GrimoraModKayceeBossSequencer : GrimoraModBossBattleSequencer
{
	private bool _playedDialogueSubmerge = false;

	private bool _playedDialoguePossessive = false;

	public override Opponent.Type BossType => BaseBossExt.KayceeOpponent;

	public override EncounterData BuildCustomEncounter(CardBattleNodeData nodeData)
	{
		return new EncounterData
		{
			opponentType = BossType
		};
	}

	public override bool RespondsToUpkeep(bool playerUpkeep)
	{
		return playerUpkeep;
	}

	private int _freezeCounter = 2;

	public override IEnumerator OnUpkeep(bool playerUpkeep)
	{
		var playerCardsWithAttacks
			= BoardManager.Instance.GetPlayerCards(pCard => pCard.Attack > 0 && !pCard.FaceDown);

		_freezeCounter += playerCardsWithAttacks.Count;
		Log.LogWarning($"[Kaycee] Freeze counter [{_freezeCounter}]");

		if (playerCardsWithAttacks.IsNotEmpty())
		{
			if (_freezeCounter >= 5)
			{
				ViewManager.Instance.SwitchToView(View.Board, lockAfter: true);
				yield return TextDisplayer.Instance.ShowUntilInput(
					$"Y-Your strikes are only making me {"c-colder".Blue()}!"
				);
				yield return TextDisplayer.Instance.ShowUntilInput(
					$"IT'S TIME FOR YOUR CARDS TO FREEZE! {"CHILLED".Blue()} TO THE BONE!"
				);
				foreach (var playableCard in playerCardsWithAttacks)
				{
					var modInfo = CreateModForFreeze(playableCard);
					playableCard.Info.iceCubeParams = new IceCubeParams { creatureWithin = playableCard.Info };
					if (playableCard.HasAbility(Ability.Submerge))
					{
						if (!_playedDialogueSubmerge)
						{
							yield return TextDisplayer.Instance.ShowUntilInput(
								$"{playableCard.Info.displayedName.Blue()} MIGHT HAVE SOME DIFFICULTY SUBMERGING IF IT'S FROZEN SOLID!"
							);
							_playedDialogueSubmerge = true;
						}

						RemoveAbilityFromThisCard(playableCard, modInfo);
					}
					else if (playableCard.HasAbility(Possessive.ability))
					{
						if (!_playedDialoguePossessive)
						{
							yield return TextDisplayer.Instance.ShowUntilInput(
								$"{playableCard.Info.displayedName.Blue()} CAN'T POSSESS ANYTHING IF IT CAN'T MOVE!"
							);
							_playedDialoguePossessive = true;
						}

						RemoveAbilityFromThisCard(playableCard, modInfo);
					}
					else
					{
						playableCard.AddTemporaryMod(modInfo);
					}

					playableCard.Anim.PlayTransformAnimation();
					yield return new WaitForSeconds(0.05f);
					playableCard.RenderCard();
					_freezeCounter = 0;
				}
			}
		}

		var draugrCards
			= BoardManager.Instance.GetOpponentCards(pCard => pCard.InfoName().Equals(NameDraugr));
		Log.LogDebug($"[KayceeSequencer] Draugr cards found [{draugrCards.GetDelimitedString()}]");
		if (draugrCards.Count >= 2)
		{
			ViewManager.Instance.SwitchToView(View.Board);
			yield return TextDisplayer.Instance.ShowUntilInput(
				$"ALL THIS {"ICE".BrightBlue()} IS TAKING UP TOO MUCH SPACE!"
			);
			foreach (var card in draugrCards)
			{
				yield return card.Die(false);
				yield return new WaitForSeconds(0.1f);
			}
		}

		ViewManager.Instance.SwitchToView(View.Default);
		ViewManager.Instance.Controller.LockState = ViewLockState.Unlocked;
	}

	private CardModificationInfo CreateModForFreeze(PlayableCard playableCard)
	{
		int attack = playableCard.Attack == 0
			? 0
			: -playableCard.Attack;
		var modInfo = new CardModificationInfo
		{
			attackAdjustment = attack,
			healthAdjustment = 1 - playableCard.Health,
			abilities = new List<Ability> { Ability.IceCube },
			negateAbilities = new List<Ability> { Ability.Submerge, Possessive.ability }
		};

		return modInfo;
	}

	private void RemoveAbilityFromThisCard(PlayableCard playableCard, CardModificationInfo modInfo)
	{
		CardInfo cardInfoClone = playableCard.Info.Clone() as CardInfo;
		cardInfoClone.Mods.Add(modInfo);
		playableCard.SetInfo(cardInfoClone);
	}
}
