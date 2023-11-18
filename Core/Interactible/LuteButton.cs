using System;
using System.Collections;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public class LuteButton : HighlightedInteractable
{
	private View confirmView = View.CardMergeConfirm;

	private Animator anim;

	private CursorType pressCursorType = CursorType.Point;

	private string activateSoundId = "guitar-strum-74592";

	public bool SelectionConfirmed { get; private set; }

	private void Start()
	{
		base.HighlightCursorType = pressCursorType;
	}

	public IEnumerator WaitUntilConfirmation()
	{
		SelectionConfirmed = false;
		ClearDelegates();
		base.CursorSelectEnded = (Action<MainInputInteractable>)Delegate.Combine(base.CursorSelectEnded, (Action<MainInputInteractable>)delegate
		{
			SelectionConfirmed = true;
		});
		yield return new WaitUntil(() => SelectionConfirmed || (base.Enabled && InputButtons.GetButtonDown(Button.EndTurn)));
		PressButton();
	}

	public void SetStoneInactive()
	{
		anim.gameObject.SetActive(value: false);
	}

	public void Enter()
	{
		ResetAnimTriggers();
		anim.gameObject.SetActive(value: true);
		anim.SetTrigger("enter");
		SetButtonInteractable();
	}

	public void Exit()
	{
		ResetAnimTriggers();
		anim.SetTrigger("exit");
		Disable();
	}

	public void SetButtonInteractable()
	{
		SetEnabled(enabled: true);
		ShowState(State.Interactable);
		Singleton<ViewManager>.Instance.SwitchToView(confirmView, immediate: false, lockAfter: true);
	}

	public void Disable()
	{
		SetEnabled(enabled: false);
		ShowState(State.NonInteractable);
	}

	public void Unpress()
	{
		SetEnabled(enabled: true);
		ShowState(State.Interactable);
	}

	public void PressButton()
	{
		

		AudioController.Instance.PlaySound3D(activateSoundId, MixerGroup.TableObjectsSFX, base.transform.position, 1.4f);
		ShowPressed();
		Disable();
		Singleton<TableVisualEffectsManager>.Instance.ThumpTable(0.2f);
	}

	private void ShowPressed()
	{
		ResetAnimTriggers();

	}

	private void ResetAnimTriggers()
	{

	}

}
