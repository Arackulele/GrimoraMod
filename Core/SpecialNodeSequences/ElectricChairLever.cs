using DiskCardGame;
using Pixelplacement;
using UnityEngine;

namespace GrimoraMod;

public class ElectricChairLever : HighlightedInteractable
{
	private static readonly Color DarkCellColor = new Color(0, 0.08637799f, 0.1132075f, 1f);
	
	public enum SigilRisk
	{
		Safe,
		Minor,
		Major
	}

	[SerializeField]
	public SigilRisk currentSigilRisk = SigilRisk.Safe;

	[SerializeField]
	private GameObject animHandle;

	[SerializeField]
	private CursorType pressCursorType = CursorType.Rotate;
	
	private readonly List<MeshRenderer> _cellRenderers = new();


	private void Start()
	{
		animHandle = gameObject.transform.Find("Anim").gameObject;
		Transform cells = gameObject.transform.Find("Cells");
		for (int i = 0; i < cells.childCount; i++)
		{
			// CetChild(i) EnergyCell -> Case -> Cell
			_cellRenderers.Add(cells.GetChild(i).GetChild(0).GetChild(0).GetComponent<MeshRenderer>());
		}
		
		SetCellColor(_cellRenderers[1], DarkCellColor);
		SetCellColor(_cellRenderers[2], DarkCellColor);

		HighlightCursorType = pressCursorType;

		CursorSelectStarted = (Action<MainInputInteractable>)Delegate.Combine(
			CursorSelectStarted,
			(Action<MainInputInteractable>)ChangeRisk
		);
	}

	private void ChangeRisk(MainInputInteractable i)
	{
		GrimoraPlugin.Log.LogDebug($"[ChangeRisk] current risk is [{currentSigilRisk}]");
		switch (currentSigilRisk)
		{
			case SigilRisk.Safe:
				// -45 zed rotation to 0
				currentSigilRisk = SigilRisk.Minor;
				SetCellColor(_cellRenderers[1], Color.green);
				Tween.LocalRotation(animHandle.transform, Quaternion.Euler(0, 0, 0), 0.75f, 0.1f, Tween.EaseIn);
				break;
			case SigilRisk.Minor:
				// 0 zed rotation to 45
				currentSigilRisk = SigilRisk.Major;
				SetCellColor(_cellRenderers[2], Color.green);
				Tween.LocalRotation(animHandle.transform, Quaternion.Euler(0, 0, 45), 0.75f, 0.1f, Tween.EaseIn);
				break;
			case SigilRisk.Major:
			default:
				// 45 to -45 zed
				currentSigilRisk = SigilRisk.Safe;
				SetCellColor(_cellRenderers[1], DarkCellColor);
				SetCellColor(_cellRenderers[2], DarkCellColor);
				Tween.LocalRotation(animHandle.transform, Quaternion.Euler(0, 0, -45), 0.75f, 0.1f, Tween.EaseIn);
				break;
		}
	}

	private void SetCellColor(Renderer cell, Color c)
	{
		void FlickerOn()
		{
			cell.material.SetEmissionColor(c);
		}
		void FlickerOff()
		{
			cell.material.SetEmissionColor(DarkCellColor);
		}
		CustomCoroutine.FlickerSequence(FlickerOn, FlickerOff, false, true, 0.05f, 2);
	}
}
