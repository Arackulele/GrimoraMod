using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Encounters;
using InscryptionAPI.Helpers.Extensions;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class GrimoraModKayceeBossSequencer : GrimoraModBossBattleSequencer
{
	public new static readonly SpecialSequenceManager.FullSpecialSequencer FullSequencer = SpecialSequenceManager.Add(
		GUID,
		nameof(GrimoraModKayceeBossSequencer),
		typeof(GrimoraModKayceeBossSequencer)
	);

	public static readonly List<Ability> AbilitiesThatShouldBeRemovedWhenFrozen = new()
	{
		Ability.DebuffEnemy, Ability.Submerge, HookLineAndSinker.ability, Possessive.ability, Haunter.ability
	};

	private bool _playedDialogueSubmerge = false;

	private bool _playedDialogueHookLineAndSinker = false;

	private bool _playedDialoguePossessive = false;
	
	private bool _playedDialogueStinky = false;

	private int fireballslot = 0;

	private bool fireballnext = false;

	public override Opponent.Type BossType => KayceeBossOpponent.FullOpponent.Id;

	public override bool RespondsToUpkeep(bool playerUpkeep)
	{
		return playerUpkeep;
	}

	private int _freezeCounter = 2;

	public static List<PlayableCard> GetValidCardsForFreezing()
	{
		return BoardManager.Instance.GetPlayerCards(pCard => pCard.Attack > 0 && !pCard.FaceDown && pCard.LacksAbility(Ability.IceCube));
	}

	public override IEnumerator OnUpkeep(bool playerUpkeep)
	{
		var playerCardsWithAttacks = GetValidCardsForFreezing();

		if (AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.ThreePhaseGhouls) && TurnManager.Instance.Opponent.NumLives == 1)
		{

			if (fireballnext == false) fireballnext = true;
			else
			{
				ViewManager.Instance.SwitchToView(View.Board);
				yield return TextDisplayer.Instance.ShowUntilInput($"LETS {"HEAT".Red()} UP SOME CARDS!");
				if (BoardManager.Instance.playerSlots[fireballslot].Card != null)
				{
					yield return BoardManager.Instance.playerSlots[fireballslot].Card.TakeDamage(2, null);
					if (BoardManager.Instance.playerSlots[fireballslot].Card == null) BoardManager.Instance.CreateCardInSlot(NameFlames.GetCardInfo(), BoardManager.Instance.playerSlots[fireballslot]);
				}

				if (BoardManager.Instance.opponentSlots[fireballslot].Card != null) 
				{
					yield return BoardManager.Instance.opponentSlots[fireballslot].Card.TakeDamage(2, null);
					if (BoardManager.Instance.opponentSlots[fireballslot].Card == null) BoardManager.Instance.CreateCardInSlot(NameFlames.GetCardInfo(), BoardManager.Instance.opponentSlots[fireballslot]);

				}


				fireballslot++;

				if (fireballslot >= 4) fireballslot = 0;
				fireballnext = false;
			}




		}
		else	_freezeCounter += playerCardsWithAttacks.Count;
		Log.LogWarning($"[Kaycee] Freeze counter [{_freezeCounter}]");

		if (playerCardsWithAttacks.Any())
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
					yield return CheckCardForAbilitiesThatBreakWhileBeingFrozen(playableCard);

					playableCard.Anim.PlayTransformAnimation();
					yield return new WaitForSeconds(0.05f);
					playableCard.RenderCard();
					_freezeCounter = 0;
				}
			}
		}

		var draugrCards = BoardManager.Instance.GetOpponentCards(pCard => pCard.InfoName().Equals(NameDraugr));
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
		ViewManager.Instance.SetViewUnlocked();
	}

	private IEnumerator CheckCardForAbilitiesThatBreakWhileBeingFrozen(PlayableCard playableCard)
	{
		var modInfo = CreateModForFreeze(playableCard);
		if (playableCard.HasAbility(Ability.Submerge))
		{
			if (!_playedDialogueSubmerge)
			{
				yield return TextDisplayer.Instance.ShowUntilInput(
					$"{playableCard.Info.displayedName.Blue()} MIGHT HAVE SOME DIFFICULTY SUBMERGING IF IT'S FROZEN SOLID!"
				);
				_playedDialogueSubmerge = true;
			}
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
		}
		else if (playableCard.HasAbility(HookLineAndSinker.ability))
		{
			if (!_playedDialogueHookLineAndSinker)
			{
				yield return TextDisplayer.Instance.ShowUntilInput(
					$"{playableCard.Info.displayedName.Blue()} WILL HAVE A HARD TIME HOOKING ANYTHING IF IT'S FROZEN SOLID!"
				);
				_playedDialogueHookLineAndSinker = true;
			}
		}
		else if (playableCard.HasAbility(Ability.DebuffEnemy))
		{
			if (!_playedDialogueStinky)
			{
				yield return TextDisplayer.Instance.ShowUntilInput(
					$"{playableCard.Info.displayedName.Blue()} FINALLY! TO GET RID OF THAT FOUL SMELL!"
				);
				_playedDialogueStinky = true;
			}
		}
		else
		{
			playableCard.AddTemporaryMod(modInfo);
			yield break;
		}

		playableCard.RemoveAbilityFromThisCard(modInfo);
	}

	public static CardModificationInfo CreateModForFreeze(PlayableCard playableCard)
	{
		int attack = playableCard.Attack == 0 ? 0 : -playableCard.Attack;
		var modInfo = new CardModificationInfo(attack, 1 - playableCard.Health)
		{
			negateAbilities = new List<Ability> { Ability.DebuffEnemy, Ability.Submerge, HookLineAndSinker.ability, Possessive.ability, Ability.PermaDeath }
		};
		if (playableCard.LacksAbility(Ability.IceCube))
		{
			modInfo.abilities = new List<Ability> { Ability.IceCube };


			playableCard.Info.iceCubeParams = new IceCubeParams { creatureWithin = playableCard.Info };

			playableCard.GetComponentInChildren<GravestoneRenderStatsLayer>().Material.SetAlbedoTexture(AssetUtils.GetPrefab<Material>("GravestoneFrozen").mainTexture);
			playableCard.RenderCard();
		}

		return modInfo;
	}
}
