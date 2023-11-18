using System.Collections;
using System.Diagnostics.CodeAnalysis;
using DiskCardGame;
using GBC;
using GrimoraMod.Saving;
using HarmonyLib;
using InscryptionAPI.Card;
using InscryptionAPI.Helpers.Extensions;
using Pixelplacement;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UIElements;
using static GrimoraMod.GrimoraPlugin;
namespace GrimoraMod;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class GrimoraCardMergeSequencer : CardMergeSequencer
{
	public static GrimoraCardMergeSequencer Instance => FindObjectOfType<GrimoraCardMergeSequencer>();

	private ConfirmStoneButton confirmStone;

	private static readonly int ShowSkull = Animator.StringToHash("show_skull");
	private static readonly int HideSkull = Animator.StringToHash("hide_skull");

	private static readonly int Exit = Animator.StringToHash("exit");

	private GameObject NodePrefab;

	bool hasdoneSlotSetup = false;

	GameObject hostSlotParent;

	GameObject sacSlotParent;

	private GameObject backgroundshrooms;

	public GameObject GrimoraRightHandBossSkull
	{
		get => GrimoraAnimationController.Instance.bossSkull;
		set => GrimoraAnimationController.Instance.bossSkull = value;
	}

	public GameObject GrimoraRightWrist => GameObject.Find("Grimora_RightWrist");

	public IEnumerator StartSequence()
	{
		Log.LogDebug("started Card merge Sequence");

		ExplorableAreaManager.Instance.HangingLight.color = GameColors.instance.brightLimeGreen;
		GameObject.Find("BoardLight_Cards").GetComponent<Light>().color = GameColors.instance.brightLimeGreen;



		GrimoraRightHandBossSkull = Instantiate(AssetConstants.BossSkullMycologists, GrimoraRightWrist.transform);

		var bossSkullTransform = GrimoraRightHandBossSkull.transform;

		bossSkullTransform.localPosition = new Vector3(-0.0044f, 0.18f, -0.042f);
		bossSkullTransform.localRotation = Quaternion.Euler(85.85f, 227.76f, 262.77f);
		bossSkullTransform.localScale = new Vector3(0.14f, 0.14f, -0.14f);

		yield return new WaitForSeconds(0.1f);
		GrimoraAnimationController.Instance.ShowBossSkull();
		GrimoraAnimationController.Instance.SetHeadTrigger("show_skull");
		yield return new WaitForSeconds(0.2f);

		ViewManager.Instance.SwitchToView(View.BossCloseup, false, true);

		yield return new WaitForSeconds(0.7f);

		ChangeDialogueSpeaker("mycologist");
		yield return ChangeMycologistEyes(true, true);

		yield return TextDisplayer.Instance.ShowUntilInput("HELLO?", emotion: Emotion.Surprise);

		ChangeDialogueSpeaker("mycologist");
		yield return ChangeMycologistEyes(false);
		yield return TextDisplayer.Instance.ShowUntilInput("We survived, but... but...");

		ChangeDialogueSpeaker("mycologistside");
		yield return ChangeMycologistEyes(true);
		yield return TextDisplayer.Instance.ShowUntilInput("Our tools did not.");

		backgroundshrooms = UnityEngine.Object.Instantiate(ResourceBank.Get<GameObject>("Prefabs/Environment/TableEffects/GiantMushroomEffects"));

		yield return new WaitForSeconds(0.5f);

		hostSlot.Disable();
		sacrificeSlot.Disable();
		Singleton<TableRuleBook>.Instance.SetOnBoard(onBoard: true);

		ChangeDialogueSpeaker("mycologist");
		yield return ChangeMycologistEyes(false);
		yield return TextDisplayer.Instance.ShowUntilInput("Our talents still remain, but..");

		ChangeDialogueSpeaker("mycologistside");
		yield return ChangeMycologistEyes(true);
		yield return TextDisplayer.Instance.ShowUntilInput("We must do things differently.");


		NodePrefab = GameObject.Instantiate(AssetConstants.MycologistHands);
		NodePrefab.transform.parent = stoneCircleAnim.gameObject.transform;
		NodePrefab.transform.localPosition = new Vector3(-0.0756f, 0.6481f, 0.2106f);
		NodePrefab.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

		stoneCircleAnim.gameObject.SetActive(true);

		hostSlotParent = new GameObject();
		sacSlotParent = new GameObject();

		hostSlotParent.transform.parent = NodePrefab.transform;
		sacSlotParent.transform.parent = NodePrefab.transform;

		if (hasdoneSlotSetup == false)
		{ 

		hostSlot.gameObject.transform.parent = hostSlotParent.transform;
		sacrificeSlot.gameObject.transform.parent = sacSlotParent.transform;
			hasdoneSlotSetup = true;
		}

		hostSlotParent.transform.localPosition = new Vector3(1.5367f, -7.1543f, -1.5f);
		hostSlotParent.transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);

		sacSlotParent.transform.localPosition = new Vector3(-0.8633f, 4.6384f, 8.9079f);
		sacSlotParent.transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);
		sacSlotParent.transform.rotation = Quaternion.Euler(0f, 90f, 270f);

		sacrificeSlot.transform.GetChild(0).GetComponent<MeshRenderer>().material = AssetConstants.FlyingSelectionSlot;
		hostSlot.transform.GetChild(0).GetComponent<MeshRenderer>().material = AssetConstants.FlyingSelectionSlot;

		SineWaveMovement wave = sacrificeSlot.gameObject.AddComponent<SineWaveMovement>();
		wave.speed = 1;
		wave.xMagnitude = 0.22f;
		wave.yMagnitude = 0;
		wave.zMagnitude = 0;

		SineWaveMovement wave2 = hostSlot.gameObject.AddComponent<SineWaveMovement>();
		wave2.speed = 1;
		wave2.xMagnitude = 0f;
		wave2.yMagnitude = 0.18f;
		wave2.zMagnitude = 0;


		yield return new WaitForSeconds(0.8f);

		ChangeDialogueSpeaker("mycologist");
		yield return ChangeMycologistEyes(false);
		yield return TextDisplayer.Instance.ShowUntilInput("We cannot fuse two into one, but...");

		ChangeDialogueSpeaker("mycologistside");
		yield return ChangeMycologistEyes(true);
		yield return TextDisplayer.Instance.ShowUntilInput("We can swap their souls, their Sigils.");


		yield return pile.SpawnCards(RunState.Run.playerDeck.Cards.Count, 0.5f);
		Singleton<ViewManager>.Instance.Controller.SwitchToControlMode(ViewController.ControlMode.CardMerging);
		Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Locked;
		yield return new WaitForSeconds(0.3f);


		ChangeDialogueSpeaker("mycologist");
		yield return ChangeMycologistEyes(false, true);
		StartCoroutine(TextDisplayer.Instance.ShowThenClear("Choose as you will...", 2f));


		Singleton<ViewManager>.Instance.SwitchToView(View.CardMergeSlots);
		gamepadGrid.enabled = true;

		sacrificeSlot.RevealAndEnable();
		sacrificeSlot.ClearDelegates();
		SelectCardFromDeckSlot selectCardFromDeckSlot = sacrificeSlot;
		selectCardFromDeckSlot.CursorSelectStarted += OnSlotSelected;

		hostSlot.RevealAndEnable();
		hostSlot.ClearDelegates();
		SelectCardFromDeckSlot selectCardFromDeckSlot2 = hostSlot;
		selectCardFromDeckSlot2.CursorSelectStarted += OnSlotSelected;




		yield return confirmStone.WaitUntilConfirmation();
		gamepadGrid.enabled = false;
		hostSlot.Disable();
		sacrificeSlot.Disable();

		yield return new WaitForSeconds(0.4f);

		yield return TextDisplayer.Instance.ShowUntilInput("It is time, it is...");

		ChangeDialogueSpeaker("mycologistside");
		yield return ChangeMycologistEyes(true);
		yield return TextDisplayer.Instance.ShowUntilInput("We shall work once more.");

		ChangeDialogueSpeaker("mycologist");
		yield return ChangeMycologistEyes(false);
		yield return TextDisplayer.Instance.ShowUntilInput("This will get ugly...");

		Singleton<ViewManager>.Instance.SwitchToView(View.TableStraightDown);

		AudioController.Instance.PlaySound3D("mycologist_carnage", MixerGroup.TableObjectsSFX, GrimoraRightHandBossSkull.transform.position);
		yield return new WaitForSeconds(1f);
		Singleton<DiskCardGame.CameraEffects>.Instance.Shake(0.1f, 0.25f);
		//bloodParticles1.gameObject.SetActive(true);
		yield return new WaitForSeconds(0.5f);
		Singleton<DiskCardGame.CameraEffects>.Instance.Shake(0.05f, 0.4f);
		yield return new WaitForSeconds(1f);
		//bloodParticles2.gameObject.SetActive(true);
		Singleton<DiskCardGame.CameraEffects>.Instance.Shake(0.1f, 0.4f);

		List<Ability> HostList = hostSlot.Card.Info.abilities;
		List<Ability> SacList = sacrificeSlot.Card.Info.abilities;

		CardModificationInfo cardModificationInfo = new CardModificationInfo();
		cardModificationInfo = new CardModificationInfo
		{
			abilities = SacList,
			negateAbilities = HostList
			};

		CardModificationInfo cardModificationInfo2 = new CardModificationInfo();
		cardModificationInfo2 = new CardModificationInfo
		{
			abilities = HostList,
			negateAbilities = SacList
		};

		RunState.Run.playerDeck.ModifyCard(hostSlot.Card.Info, cardModificationInfo);
		RunState.Run.playerDeck.ModifyCard(sacrificeSlot.Card.Info, cardModificationInfo2);
		yield return new WaitForEndOfFrame();
		RunState.Run.playerDeck.ModifyCard(hostSlot.Card.Info, CreateCloneCard(hostSlot.Card.Info));

		yield return new WaitForEndOfFrame();

		RunState.Run.playerDeck.ModifyCard(sacrificeSlot.Card.Info, CreateCloneCard(sacrificeSlot.Card.Info));


		hostSlot.Card.Anim.PlayTransformAnimation();
		sacrificeSlot.Card.Anim.PlayTransformAnimation();
		hostSlot.Card.SetInfo(hostSlot.Card.Info);
		hostSlot.Card.SetInteractionEnabled(false);
		sacrificeSlot.Card.SetInfo(sacrificeSlot.Card.Info);
		sacrificeSlot.Card.SetInteractionEnabled(false);
		AudioController.Instance.PlaySound3D("card_blessing", MixerGroup.TableObjectsSFX, hostSlot.transform.position, 1f, 0f, new AudioParams.Pitch(0.6f));
		yield return new WaitForSeconds(0.75f);

		Singleton<ViewManager>.Instance.SwitchToView(View.CardMergeSlots);

		yield return TextDisplayer.Instance.ShowUntilInput("Their bodies and souls, they are...");

		ChangeDialogueSpeaker("mycologistside");
		yield return ChangeMycologistEyes(true);
		yield return TextDisplayer.Instance.ShowUntilInput("Switched.");

		ChangeDialogueSpeaker("mycologist");
		yield return ChangeMycologistEyes(false);
		yield return TextDisplayer.Instance.ShowUntilInput("They lead new lives..");

		ChangeDialogueSpeaker("mycologistside");
		yield return ChangeMycologistEyes(true);
		yield return TextDisplayer.Instance.ShowUntilInput("With new purposes.");

		ChangeDialogueSpeaker("mycologist");
		yield return ChangeMycologistEyes(false);
		yield return TextDisplayer.Instance.ShowUntilInput("Till we meet again, one day... one day...");

		yield return ChangeMycologistEyes(false, true);

		hostSlot.FlyOffCard();
		sacrificeSlot.FlyOffCard();

		yield return new WaitForSeconds(0.8f);

		ViewManager.Instance.SwitchToView(View.Default, false, true);

		Destroy(wave);
		Destroy(wave2);


		yield return OutroSequence();

	}


	MainInputInteractable slotbeingedit;

	private void OnSlotSelected(MainInputInteractable slot)
	{
		slotbeingedit = slot;
		slot.SetEnabled(false);
		((SelectCardFromDeckSlot)slot).ShowState(HighlightedInteractable.State.NonInteractable);
		confirmStone.Exit();
		List<CardInfo> validCards = new List<CardInfo>(GetValidCards());
		((SelectCardFromDeckSlot)slot).SelectFromCards(validCards, OnSelectionEnded, false);
	}

	private new void OnSelectionEnded()
	{
		slotbeingedit.SetEnabled(true);
		((SelectCardFromDeckSlot)slotbeingedit).ShowState(HighlightedInteractable.State.Interactable);
		ViewManager.Instance.SwitchToView(View.Default, false, true);
		if (hostSlot.Card && sacrificeSlot.Card)
		{
			confirmStone.Enter();
		}
		StartCoroutine(teleportcardtoslotpos());
	}

	private IEnumerator teleportcardtoslotpos()
	{
		yield return new WaitForSeconds(0.1f);
		((SelectCardFromDeckSlot)slotbeingedit).Card.gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);


	}

	private IEnumerator OutroSequence()
	{
		//bloodParticles1.gameObject.SetActive(value: false);
		//bloodParticles2.gameObject.SetActive(value: false);


		Log.LogDebug($"Destroying deck pile");
		yield return pile.DestroyCards();

		Log.LogDebug($"Setting Exit trigger");
		stoneCircleAnim.SetTrigger(Exit);

		ExplorableAreaManager.Instance.ResetHangingLightsToZoneColors(0.25f);
		yield return new WaitForSeconds(0.25f);

		Log.LogDebug($"Confirm stone exit");
		confirmStone.Exit();
		yield return new WaitForSeconds(0.75f);

		GlitchOutAssetEffect.GlitchModel(backgroundshrooms.transform);

		GrimoraAnimationController.Instance.GlitchOutBossSkull();
		GrimoraAnimationController.Instance.headAnim.ResetTrigger(ShowSkull);
		GrimoraAnimationController.Instance.SetHeadTrigger("hide_skull");
		GrimoraRightHandBossSkull = null;

		Log.LogDebug($"stoneCircleAnim.gameObject false");
		stoneCircleAnim.gameObject.SetActive(false);

		Log.LogDebug($"confirmStone.SetStoneInactive");
		confirmStone.SetStoneInactive();

		yield return new WaitForSeconds(0.75f);

		ChangeDialogueSpeaker("grimora");

		GameFlowManager.Instance.TransitionToGameState(GameState.Map);
	}

	private static List<CardInfo> GetValidCards()
	{
		List<CardInfo> deckCopy = new List<CardInfo>(RunState.Run.playerDeck.Cards);
		deckCopy.RemoveAll(
										card => card.traits.Contains(Trait.Pelt)
							|| card.traits.Contains(Trait.Terrain)
		);

		if (Instance.hostSlot.Card != null && Instance.hostSlot.Card.Info != null && deckCopy.Contains(Instance.hostSlot.Card.Info)) deckCopy.Remove(Instance.hostSlot.Card.Info);
		if (Instance.sacrificeSlot.Card != null && Instance.sacrificeSlot.Card.Info != null && deckCopy.Contains(Instance.sacrificeSlot.Card.Info)) deckCopy.Remove(Instance.sacrificeSlot.Card.Info);

		return deckCopy;
	}


	public static void CreateSequencerInScene()
	{
		if (SpecialNodeHandler.Instance.SafeIsUnityNull())
		{
			return;
		}

		GameObject cardStatObj = Instantiate(
		ResourceBank.Get<GameObject>("Prefabs/SpecialNodeSequences/CardMerger"),
		SpecialNodeHandler.Instance.transform
	);

		cardStatObj.name = "CardMergeSequencer_Grimora";

		var newSequencer = cardStatObj.AddComponent<GrimoraCardMergeSequencer>();

		var oldMergeSequencer = cardStatObj.GetComponent<CardMergeSequencer>();

		newSequencer.stoneCircleAnim = oldMergeSequencer.stoneCircleAnim;
		newSequencer.gamepadGrid = oldMergeSequencer.gamepadGrid;
		newSequencer.confirmStone = oldMergeSequencer.confirmStone;
		newSequencer.sacrificeSlot = oldMergeSequencer.sacrificeSlot;
		newSequencer.hostSlot = oldMergeSequencer.hostSlot;
		newSequencer.sacrificeSlot.cardSelector.selectableCardPrefab = AssetConstants.GrimoraSelectableCard;
		newSequencer.sacrificeSlot.pile.cardbackPrefab = AssetConstants.GrimoraCardBack;

		newSequencer.pile = oldMergeSequencer.pile;
		newSequencer.pile.cardbackPrefab = AssetConstants.GrimoraCardBack;

		foreach (Transform child in newSequencer.sacrificeSlot.gameObject.transform.parent.transform)
		{
			if (child.name != "SacrificeSlot") GameObject.Destroy(child.gameObject);
		}
		foreach (Transform child in newSequencer.hostSlot.gameObject.transform.parent.transform)
		{
			if (child.name != "HostSlot") GameObject.Destroy(child.gameObject);
		}

		foreach (Transform child in newSequencer.stoneCircleAnim.gameObject.transform)
		{
			if (child.name.Contains("Tall Rock")) GameObject.Destroy(child.gameObject);
		}

		Destroy(newSequencer.hostSlot.gameObject.transform.parent.GetComponent<MeshRenderer>());
		Destroy(newSequencer.hostSlot.gameObject.transform.parent.GetComponent<MeshFilter>());

		Instance.hostSlotParent = new GameObject();
		Instance.sacSlotParent = new GameObject();
	}

	private IEnumerator ChangeMycologistEyes(bool small, bool backon = false)
	{
		Transform Mycoskull = GrimoraRightHandBossSkull.transform.GetChild(0);
		if (small == true && backon == false)
		{
			StartCoroutine(ScaleDownThenDisable(0.2f, Mycoskull.GetChild(0).GetChild(0).gameObject));
			yield return ScaleDownThenDisable(0.2f, Mycoskull.GetChild(0).GetChild(1).gameObject);

			Mycoskull.GetChild(1).GetChild(0).gameObject.SetActive(true);
			Mycoskull.GetChild(1).GetChild(1).gameObject.SetActive(true);

		}
		else if (backon == false)
		{

			Mycoskull.GetChild(0).GetChild(0).gameObject.SetActive(true);
			Mycoskull.GetChild(0).GetChild(1).gameObject.SetActive(true);

			StartCoroutine(ScaleDownThenDisable(0.2f, Mycoskull.GetChild(1).GetChild(0).gameObject));
			yield return ScaleDownThenDisable(0.2f, Mycoskull.GetChild(1).GetChild(1).gameObject);


		}
		else
		{

			Mycoskull.GetChild(0).GetChild(0).gameObject.SetActive(true);
			Mycoskull.GetChild(0).GetChild(1).gameObject.SetActive(true);

			Mycoskull.GetChild(1).GetChild(0).gameObject.SetActive(true);
			Mycoskull.GetChild(1).GetChild(1).gameObject.SetActive(true);

		}

		yield return new WaitForSeconds(0.2f);
	}

	private CardModificationInfo CreateCloneCard(CardInfo cardToCopy)
	{
		CardInfo cardByName = CardLoader.GetCardByName(cardToCopy.name);

		CardModificationInfo cardModificationInfo2 = new CardModificationInfo();
		int currentRandomSeed = SaveManager.SaveFile.GetCurrentRandomSeed();
		float num = UnityEngine.Random.value*2;
		if (num < 0.33f && cardByName.abilities.Count < 5)
		{
			cardModificationInfo2.abilities = new List<Ability>() { ElectricChairLever.AbilitiesMajorRisk.GetRandomItem() };
		}
		else if (num < 0.66f && cardByName.Attack > 0)
		{
			cardModificationInfo2.attackAdjustment = (SeededRandom.Bool(currentRandomSeed++) ? 1 : (-1));
		}
		else if (num < 1f && cardByName.Health > 1)
		{
			int num2 = Mathf.Min(2, cardByName.Health - 1);
			cardModificationInfo2.healthAdjustment = (SeededRandom.Bool(currentRandomSeed++) ? 2 : (-num2));
		}
		return cardModificationInfo2;
	}

	IEnumerator ScaleDownThenDisable(float time, GameObject g)
	{
		float i = 0;
		float rate = 1 / time;
		Vector3 originalScale = g.transform.localScale;

		Vector3 fromScale = g.transform.localScale;
		Vector3 toScale = new Vector3(originalScale.x, 0, originalScale.z);
		while (i < 1)
		{
			i += Time.deltaTime * rate;
			g.transform.localScale = Vector3.Lerp(fromScale, toScale, i);
			yield return 0;
		}

		g.SetActive(false);
		g.transform.localScale = originalScale;
	}

}

