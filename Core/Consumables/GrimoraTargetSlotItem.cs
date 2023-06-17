using DiskCardGame;
using EasyFeedback.APIs;
using InscryptionAPI.Card;
using InscryptionAPI.Items;
using InscryptionAPI.Items.Extensions;
using System;
using System.Collections;
using System.Resources;
using System.Security.Policy;
using UnityEngine;
using  static GrimoraMod.GrimoraPlugin;
namespace GrimoraMod.Consumables;

public class GrimoraTargetSlotItem : TargetSlotItem
{
		public override string FirstPersonPrefabId => null;

	public override Vector3 FirstPersonItemPos => new Vector3(0f, -1f, 2.5f);

	public override Vector3 FirstPersonItemEulers => new Vector3(-60f, 0f, 0f);

	public override View SelectionView => View.Board;

	public override CursorType SelectionCursorType => CursorType.Point;

	//Just copied this from Decompiled ILSpy Code,
	//this is just the normal TargetSlotItemSequencer but not using the FirstPersonPrefabIdString, instead using a custom variable

	public GameObject GrimoraFirstPersonPrefab;

	public override IEnumerator ActivateSequence()
	{

		if (this is Mallet) GrimoraFirstPersonPrefab = kopieGameObjects.Find(g => g.name.Contains("MalletPrefabFirstPerson"));
		else if (this is Trowel) GrimoraFirstPersonPrefab = kopieGameObjects.Find(g => g.name.Contains("TrowelPrefabFirstPerson"));
		else if (this is Quill) GrimoraFirstPersonPrefab = kopieGameObjects.Find(g => g.name.Contains("QuillPrefabFirstPerson"));
		else if (this is EmbalmingFluid) GrimoraFirstPersonPrefab = kopieGameObjects.Find(g => g.name.Contains("EmbalmingFluidPrefabFirstPerson"));
		PlayExitAnimation();
		yield return new WaitForSeconds(0.1f);
		Singleton<UIManager>.Instance.Effects.GetEffect<EyelidMaskEffect>().SetIntensity(0.6f, 0.2f);
		Singleton<ViewManager>.Instance.SwitchToView(SelectionView);
		yield return new WaitForSeconds(0.25f);

		GameObject itemnontransform = UnityEngine.Object.Instantiate(GrimoraFirstPersonPrefab);
		Transform firstPersonItem = itemnontransform.transform;


		firstPersonItem.SetParent(Singleton<FirstPersonAnimationController>.Instance.pixelCamera.transform);


		firstPersonItem.localPosition = Vector3.zero;
		firstPersonItem.localRotation = Quaternion.identity;

		firstPersonItem.localPosition = FirstPersonItemPos + Vector3.right * 3f;
		firstPersonItem.localEulerAngles = FirstPersonItemEulers;
		Singleton<InteractionCursor>.Instance.InteractionDisabled = false;
		CardSlot target = null;
		List<CardSlot> validTargets = GetValidTargets();
		MoveItemToPosition(firstPersonItem, validTargets[validTargets.Count - 1].transform.position);
		Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
		yield return Singleton<BoardManager>.Instance.ChooseTarget(GetAllTargets(), validTargets, delegate (CardSlot slot)
		{
			target = slot;
		}, OnInvalidTargetSelected, delegate (CardSlot slot)
		{
			MoveItemToPosition(firstPersonItem, slot.transform.position);
		}, () => Singleton<ViewManager>.Instance.CurrentView != SelectionView || !Singleton<TurnManager>.Instance.IsPlayerMainPhase, SelectionCursorType);
		if (target != null)
		{
			Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Locked;
			Singleton<InteractionCursor>.Instance.InteractionDisabled = true;
			yield return OnValidTargetSelected(target, itemnontransform.gameObject);
		}
		else
		{
			base.ActivationCancelled = true;
		}
		UnityEngine.Object.Destroy(firstPersonItem.gameObject);
		Singleton<UIManager>.Instance.Effects.GetEffect<EyelidMaskEffect>().SetIntensity(0f, 0.2f);
		Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
	}

		public override List<CardSlot> GetAllTargets()
		{
				throw new NotImplementedException();
		}

		public override List<CardSlot> GetValidTargets()
		{
				throw new NotImplementedException();
		}
}
