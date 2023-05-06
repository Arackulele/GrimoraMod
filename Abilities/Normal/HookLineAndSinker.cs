using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Helpers.Extensions;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class HookLineAndSinker : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;

	private static bool _playedDialogueGrimoraGiantHooked;
	
	public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer) => Card.HasOpposingCard();

	public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
	{
		PlayableCard targetCard = Card.OpposingCard();

		AudioController.Instance.PlaySound3D(
			"angler_use_hook",
			MixerGroup.TableObjectsSFX,
			targetCard.transform.position,
			1f,
			0.1f
		);
		yield return new WaitForSeconds(0.51f);

		if (targetCard.IsGrimoraGiant())
		{
			yield return targetCard.TakeDamage(2, Card);
			
			if (!_playedDialogueGrimoraGiantHooked)
			{
				yield return new WaitForSeconds(0.25f);
				yield return TextDisplayer.Instance.ShowUntilInput($"OH DEAR! LOOKS LIKE YOU HAVE HOOKED SOMETHING OUT OF {targetCard.Info.DisplayedNameLocalized}!");
				_playedDialogueGrimoraGiantHooked = true;
			}

			if (ViewManager.Instance.CurrentView != View.Default)
			{
				yield return new WaitForSeconds(0.2f);
				ViewManager.Instance.SwitchToView(View.Default);
				yield return new WaitForSeconds(0.2f);
			}
			yield return CardSpawner.Instance.SpawnCardToHand(NameBoneLordsHorn.GetCardInfo());

			yield break;
		}

		//Anchored Cards cannot be hooked
		if (targetCard.HasAbility(Anchored.ability))
		{
			targetCard.Anim.StrongNegationEffect();

			yield break;
		}

			if (targetCard.NotDead())
		{
			GrimoraPlugin.Log.LogInfo($"[HookLineAndSinker] Hooked card {targetCard.GetNameAndSlot()}, moving to slot [{Card.Slot.Index}]");
			targetCard.SetIsOpponentCard(Card.Slot.IsOpponentSlot());
			yield return Card.Slot.AssignCardToSlot(targetCard, 0.33f);
			if (targetCard.FaceDown)
			{
				targetCard.SetFaceDown(false);
				targetCard.UpdateFaceUpOnBoardEffects();
			}

			yield return new WaitForSeconds(0.66f);
		}
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_HookLineAndSinker()
	{
		const string rulebookDescription =
			"When [creature] perishes, the creature in the opposing slot is dragged onto the owner's side of the board.";

		AbilityBuilder<HookLineAndSinker>.Builder
		 .SetRulebookDescription(rulebookDescription)
			.SetPixelIcon(AssetUtils.GetPrefab<Sprite>("hook_pixel"))
		 .Build();
	}
}
