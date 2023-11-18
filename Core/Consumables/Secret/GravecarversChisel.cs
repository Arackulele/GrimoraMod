using DiskCardGame;
using EasyFeedback.APIs;
using InscryptionAPI.Items;
using InscryptionAPI.Items.Extensions;
using Pixelplacement;
using System;
using System.Collections;
using System.Resources;
using System.Security.Policy;
using UnityEngine;
using  static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod.Consumables;

public class GravecarversChisel : ConsumableItem
{

		public override bool ExtraActivationPrerequisitesMet()
		{
		if (GrimoraModBattleSequencer.cardsThatHaveDiedThisMatch.Count == 0) return false;
		else return true;
		}


		public override void OnExtraActivationPrerequisitesNotMet()
	{
		base.OnExtraActivationPrerequisitesNotMet();
		this.PlayShakeAnimation();
	}

	public override IEnumerator ActivateSequence()
	{
		Debug.Log("Using SliveredBank");

		Singleton<ViewManager>.Instance.SwitchToView(View.DeckSelection, immediate: false, lockAfter: true);
		SelectableCard selectedCard = null;
		yield return Singleton<BoardManager>.Instance.CardSelector.SelectCardFrom(GrimoraModBattleSequencer.cardsThatHaveDiedThisMatch, (Singleton<CardDrawPiles>.Instance as CardDrawPiles3D).Pile, delegate (SelectableCard x)
		{
			selectedCard = x;
		});
		Tween.Position(selectedCard.transform, selectedCard.transform.position + Vector3.back * 4f, 0.1f, 0f, Tween.EaseIn);
		UnityEngine.Object.Destroy(selectedCard.gameObject, 0.1f);
	}


	public static ConsumableItemData NewGravecarversChisel(GameObject Model)
	{

		Texture2D HahaL = new Texture2D(70, 80);
		ConsumableItemData data = ConsumableItemManager.New(GUID, "Gravecarvers Chisel", "The Gravecarvers chisel, select any card that has died previously to add to your ahnd.", HahaL, typeof(GravecarversChisel), Model)
		.SetLearnItemDescription("A tool left behind by a mysterious creator, did he carve the gravebards statue, too?");


		data.rulebookCategory = AbilityMetaCategory.GrimoraRulebook;

		return data;
	}
}
