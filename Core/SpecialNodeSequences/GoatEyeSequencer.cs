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
using static GrimoraMod.GrimoraPlugin;
namespace GrimoraMod;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class GoatEyeSequencer : ManagedBehaviour
{
	public static GoatEyeSequencer Instance => FindObjectOfType<GoatEyeSequencer>();

	private ConfirmStoneButton confirmStone;

	private ConfirmStoneButton rejectStone;

	private GameObject rejectbutton;

	private GameObject confirmbutton;

	MeshRenderer StoneQuad;
	MeshRenderer rejectQuad;

	private List<GameObject> goatEyes = new List<GameObject>();

	public Material defaultMaterialstone;

	public IEnumerator StartSequence()
	{
		Log.LogDebug("started Goat eye sequence");

		yield return new WaitForSeconds(0.3f);

		ExplorableAreaManager.Instance.HangingLight.gameObject.SetActive(false);
		ExplorableAreaManager.Instance.HandLight.gameObject.SetActive(true);

		yield return new WaitForSeconds(0.6f);

		ViewManager.Instance.SwitchToView(View.Default, false, true);

		yield return TextDisplayer.Instance.ShowUntilInput("I FEEL WATCHED SOMEHOW");

		foreach (var g in goatEyes) g.SetActive(true);

		StartCoroutine(makeGoatEyefollowMouse());


		yield return new WaitForSeconds(1f);

		if (!EventManagement.HasLearnedMechanicGoatEye)
		{

			yield return TextDisplayer.Instance.ShowUntilInput("AN ANCIENT ENERGY DEMANDS YOUR SACRIFICE");

		}

		if (AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.NoBones))
		{

			yield return TextDisplayer.Instance.ShowUntilInput("ARE YOU WILLING TO SACRIFICE ONE OF YOUR STARTING-");

			yield return new WaitForSeconds(0.5f);
			yield return TextDisplayer.Instance.ShowUntilInput("WAIT");
			ChallengeActivationUI.TryShowActivation(ChallengeManagement.NoBones);

			ChangeDialogueSpeaker("bonelord");
			yield return new WaitForSeconds(0.5f);

			yield return TextDisplayer.Instance.ShowUntilInput("IT SEEMS YOU BEAR A TERRIBLE CURSE");

			yield return TextDisplayer.Instance.ShowUntilInput("YOU SHALL KEEP THE BOON, BUT FOR A FAR GREATER SACRIFICE");

			yield return TextDisplayer.Instance.ShowUntilInput("CAN YOU GIVE UP YOUR ETERNAL ASHES?");


			ChangeDialogueSpeaker("grimora");
		}

		else yield return TextDisplayer.Instance.ShowUntilInput("ARE YOU WILLING TO SACRIFICE ONE OF YOUR STARTING BONES");


		yield return new WaitForSeconds(0.7f);

		confirmStone.Enter();
		confirmStone.SetColors(GameColors.instance.darkLimeGreen, GameColors.instance.darkLimeGreen, GameColors.instance.limeGreen);
		rejectStone.Enter();
		rejectStone.ResetColors();


		StoneQuad = confirmStone.transform.Find("Quad").GetComponent<MeshRenderer>();
		rejectQuad = rejectStone.transform.Find("Quad").GetComponent<MeshRenderer>();

		StoneQuad.material = AssetConstants.checkmark;
		rejectQuad.material = AssetConstants.cancel;

		defaultMaterialstone = StoneQuad.material;

		StartCoroutine(RejectOffer());

		yield return confirmStone.WaitUntilConfirmation();

		StopCoroutine(RejectOffer());
		rejectStone.PressButton();

		yield return new WaitForSeconds(0.5f);

		if (AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.NoBones)) GrimoraRunState.CurrentRun.riggedDraws.Add("No_Ashes");

		rejectStone.Exit();

		confirmStone.Exit();

		if (!EventManagement.HasLearnedMechanicGoatEye) yield return TextDisplayer.Instance.ShowUntilInput("A BRAVE SOUL");

		yield return TextDisplayer.Instance.ShowUntilInput("THE GODS ANSWER YOUR PRAYERS, NOW CHOOSE");

		List<String> AvailableBoons = new List<String>() { "Boon_StartingDraw", "Boon_TerrainBones", "Boon_MaxEnergy", "Boon_BossBones", "Boon_EgyptCards", "Boon_Pirates", "Boon_TerrainSpawn" };
		//Can only have up to 2 Boons


		if (GrimoraRunState.CurrentRun.riggedDraws.Count > 0 && AvailableBoons.Contains(GrimoraRunState.CurrentRun.riggedDraws[0])) AvailableBoons.Remove(GrimoraRunState.CurrentRun.riggedDraws[0]);
		if (GrimoraRunState.CurrentRun.riggedDraws.Count > 1 && AvailableBoons.Contains(GrimoraRunState.CurrentRun.riggedDraws[1])) AvailableBoons.Remove(GrimoraRunState.CurrentRun.riggedDraws[1]);


		String BoonToAdd = AvailableBoons.GetRandomItem();

		AvailableBoons.Remove(BoonToAdd);

		String BoonToAdd2 = AvailableBoons.GetRandomItem();

		confirmStone.ResetColors();

		switch (BoonToAdd)
		{
			default:
			case "Boon_StartingDraw":
				StoneQuad.material = AssetConstants.Boon1;
				confirmStone.Enter();
				rejectStone.SetColors(GameColors.instance.darkSeafoam, GameColors.instance.darkSeafoam, GameColors.instance.brightSeafoam);

				yield return TextDisplayer.Instance.ShowUntilInput("THE BOON OF THE CUT FINGER");
				yield return TextDisplayer.Instance.ShowUntilInput("DRAW AN ADDITIONAL CARD AT THE START OF A BATTLE");
				break;
			case "Boon_TerrainBones":
				StoneQuad.material = AssetConstants.Boon2;
				confirmStone.Enter();
				rejectStone.SetColors(GameColors.instance.darkGold, GameColors.instance.darkGold, GameColors.instance.brightGold);

				yield return TextDisplayer.Instance.ShowUntilInput("THE BOON OF EBONY EYES");
				yield return TextDisplayer.Instance.ShowUntilInput("TERRAIN CARDS WILL PROVIDE 2 BONES ON DEATH");
				break;
			case "Boon_MaxEnergy":
				StoneQuad.material = AssetConstants.Boon3;
				confirmStone.Enter();
				rejectStone.SetColors(GameColors.instance.darkBlue, GameColors.instance.darkBlue, GameColors.instance.brightBlue);

				yield return TextDisplayer.Instance.ShowUntilInput("THE BOON OF REPENTANCE");
				yield return TextDisplayer.Instance.ShowUntilInput("GAIN AN ADDITIONAL SOUL AT THE START OF A BATTLE");
				break;
			case "Boon_BossBones":
				StoneQuad.material = AssetConstants.Boon4;
				confirmStone.Enter();
				rejectStone.SetColors(GameColors.instance.red, GameColors.instance.red, GameColors.instance.glowRed);
				yield return TextDisplayer.Instance.ShowUntilInput("BOON OF THE ADVERSARY");
				yield return TextDisplayer.Instance.ShowUntilInput("AT THE START OF A BOSS BATTLE, GAIN 3 EXTRA BONES");
				break;
			case "Boon_EgyptCards":
				StoneQuad.material = AssetConstants.Boon5;
				confirmStone.Enter();
				rejectStone.SetColors(GameColors.instance.yellow, GameColors.instance.yellow, GameColors.instance.brightGold);
				yield return TextDisplayer.Instance.ShowUntilInput("BOON OF THE SUN KINGDOM");
				yield return TextDisplayer.Instance.ShowUntilInput("ON THE 4TH TURN OF A BATTLE, DRAW 2 RANDOM EGYPT CARDS");
				break;
			case "Boon_Pirates":
				StoneQuad.material = AssetConstants.Boon6;
				confirmStone.Enter();
				rejectStone.SetColors(GameColors.instance.brightBlue, GameColors.instance.brightBlue, GameColors.instance.glowSeafoam);
				yield return TextDisplayer.Instance.ShowUntilInput("BOON OF THE SEASICK");
				yield return TextDisplayer.Instance.ShowUntilInput("AFTER PLACING 3 CARDS FROM YOUR HAND, A PIRATE SKELETON WILL BE PLAYED ON A RANDOM SPACE ON THE BOARD");
				break;
			case "Boon_TerrainSpawn":
				StoneQuad.material = AssetConstants.Boon7;
				confirmStone.Enter();
				rejectStone.SetColors(GameColors.instance.darkPurple, GameColors.instance.darkPurple, GameColors.instance.fuschia);
				yield return TextDisplayer.Instance.ShowUntilInput("BOON OF THE INANIMATE");
				yield return TextDisplayer.Instance.ShowUntilInput("AT THE START OF A BATTLE, A DEAD TREE WILL SPAWN IN ON A RANDOM SPACE ON THE BOARD");
				break;
		}
		confirmStone.ShowState(confirmStone.currentState, true);
		yield return new WaitForSeconds(0.3f);

		rejectStone.ResetColors();
		switch (BoonToAdd2)
		{
			default:
			case "Boon_StartingDraw":
				rejectQuad.material = AssetConstants.Boon1;
				rejectStone.Enter();
				rejectStone.SetColors(GameColors.instance.darkSeafoam, GameColors.instance.darkSeafoam, GameColors.instance.brightSeafoam);
				yield return TextDisplayer.Instance.ShowUntilInput("THE BOON OF THE CUT FINGER");
				yield return TextDisplayer.Instance.ShowUntilInput("DRAW AN ADDITIONAL CARD AT THE START OF A BATTLE");
				break;
			case "Boon_TerrainBones":
				rejectQuad.material = AssetConstants.Boon2;
				rejectStone.Enter();
				rejectStone.SetColors(GameColors.instance.darkGold, GameColors.instance.darkGold, GameColors.instance.brightGold);
				yield return TextDisplayer.Instance.ShowUntilInput("THE BOON OF EBONY EYES");
				yield return TextDisplayer.Instance.ShowUntilInput("TERRAIN CARDS WILL PROVIDE 2 BONES ON DEATH");
				break;
			case "Boon_MaxEnergy":
				rejectQuad.material = AssetConstants.Boon3;
				rejectStone.Enter();
				rejectStone.SetColors(GameColors.instance.darkBlue, GameColors.instance.darkBlue, GameColors.instance.brightBlue);
				yield return TextDisplayer.Instance.ShowUntilInput("THE BOON OF REPENTANCE");
				yield return TextDisplayer.Instance.ShowUntilInput("GAIN AN ADDITIONAL SOUL AT THE START OF A BATTLE");
				break;
			case "Boon_BossBones":
				rejectQuad.material = AssetConstants.Boon4;
				rejectStone.Enter();
				rejectStone.SetColors(GameColors.instance.red, GameColors.instance.red, GameColors.instance.glowRed);
				yield return TextDisplayer.Instance.ShowUntilInput("BOON OF THE ADVERSARY");
				yield return TextDisplayer.Instance.ShowUntilInput("AT THE START OF A BOSS BATTLE, GAIN 3 EXTRA BONES");
				break;
			case "Boon_EgyptCards":
				StoneQuad.material = AssetConstants.Boon5;
				rejectStone.Enter();
				rejectStone.SetColors(GameColors.instance.yellow, GameColors.instance.yellow, GameColors.instance.brightGold);
				yield return TextDisplayer.Instance.ShowUntilInput("BOON OF THE SUN KINGDOM");
				yield return TextDisplayer.Instance.ShowUntilInput("ON THE 4TH TURN OF A BATTLE, DRAW 2 RANDOM EGYPT CARDS");
				break;
			case "Boon_Pirates":
				StoneQuad.material = AssetConstants.Boon6;
				rejectStone.Enter();
				rejectStone.SetColors(GameColors.instance.brightBlue, GameColors.instance.brightBlue, GameColors.instance.glowSeafoam);
				yield return TextDisplayer.Instance.ShowUntilInput("BOON OF THE SEASICK");
				yield return TextDisplayer.Instance.ShowUntilInput("EVERY SECOND TURN, A PIRATE SKELETON WILL BE PLAYED ON A RANDOM SPACE ON THE BOARD");
				break;
			case "Boon_TerrainSpawn":
				StoneQuad.material = AssetConstants.Boon7;
				rejectStone.Enter();
				rejectStone.SetColors(GameColors.instance.darkPurple, GameColors.instance.darkPurple, GameColors.instance.fuschia);
				yield return TextDisplayer.Instance.ShowUntilInput("BOON OF THE INANIMATE");
				yield return TextDisplayer.Instance.ShowUntilInput("AT THE START OF A BATTLE, A DEAD TREE WILL SPAWN IN ON A RANDOM SPACE ON THE BOARD");
				break;
		}
		rejectStone.ShowState(rejectStone.currentState, true);

		yield return new WaitForSeconds(0.3f);

		StartCoroutine(ChooseBoon2(BoonToAdd2));

		yield return confirmStone.WaitUntilConfirmation();
		StopCoroutine(ChooseBoon2(BoonToAdd2));
		rejectStone.PressButton();

		yield return new WaitForSeconds(0.1f);

		yield return TextDisplayer.Instance.ShowUntilInput("A WISE CHOICE, NOW LEAVE BEFORE THE GODS GET TOO HUNGRY");


		GrimoraRunState.CurrentRun.riggedDraws.Add(BoonToAdd);

		StartCoroutine(EndSequence());
	}

	public IEnumerator EndSequence()
	{
		EventManagement.HasLearnedMechanicGoatEye = true;

		ViewManager.Instance.SwitchToView(View.Default);
		yield return new WaitForSeconds(0.25f);

		StoneQuad.material = defaultMaterialstone;
		rejectQuad.material = defaultMaterialstone;

		confirmStone.Exit();

		rejectStone.Exit();

		yield return new WaitForSeconds(0.3f);

		confirmStone.SetStoneInactive();

		rejectStone.SetStoneInactive();

		StopCoroutine(makeGoatEyefollowMouse());

		foreach (var g in goatEyes) g.SetActive(false);

		CustomCoroutine.WaitThenExecute(
	0.3f,
	delegate
	{
		ExplorableAreaManager.Instance.HangingLight.intensity = 0f;
		ExplorableAreaManager.Instance.HangingLight.gameObject.SetActive(true);
		ExplorableAreaManager.Instance.HandLight.intensity = 0f;
		ExplorableAreaManager.Instance.HandLight.gameObject.SetActive(true);
	}
);

		GameFlowManager.Instance.TransitionToGameState(GameState.Map);

	}

	public IEnumerator RejectOffer()
	{
		yield return rejectStone.WaitUntilConfirmation();

		StopCoroutine(StartSequence());
		confirmStone.PressButton();

		yield return TextDisplayer.Instance.ShowUntilInput("NO? A TERRIBLE FATE WILL AWAIT YOU");

		StartCoroutine(EndSequence());
	}

	public IEnumerator ChooseBoon2(string Boon)
	{

		yield return rejectStone.WaitUntilConfirmation();
		StopCoroutine(StartSequence());
		confirmStone.PressButton();

		yield return TextDisplayer.Instance.ShowUntilInput("A WISE CHOICE, NOW LEAVE BEFORE THE GODS GET TOO HUNGRY");

		GrimoraRunState.CurrentRun.riggedDraws.Add(Boon);

		StartCoroutine(EndSequence());
	}

	public IEnumerator makeGoatEyefollowMouse()
	{
		while (true)
		{
			foreach (var g in goatEyes) { 
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, (Input.mousePosition.y/2), 4f));
			GoatEyePatch.TurnToFacePointOnAllVectors(g, mousePos, 0.1f);
			yield return new WaitForSeconds(0.1f);
			}
		}

	}

	public static void CreateSequencerInScene()
	{
		if (SpecialNodeHandler.Instance.SafeIsUnityNull())
		{
			return;
		}

		GameObject cardStatObj = Instantiate(
		ResourceBank.Get<GameObject>("Prefabs/SpecialNodeSequences/CardRemoveSequencer"),
		SpecialNodeHandler.Instance.transform
	);

		cardStatObj.name = "GoatEyeSequencer_Grimora";

		var newSequencer = cardStatObj.AddComponent<GoatEyeSequencer>();

		//ToDo: Fix this mess
		Instance.confirmbutton = GameObject.Find("GameTable/SpecialNodeHandler_Grimora/GoatEyeSequencer_Grimora/ConfirmStoneButton");

		Instance.confirmStone = GameObject.Find("GameTable/SpecialNodeHandler_Grimora/GoatEyeSequencer_Grimora/ConfirmStoneButton/Anim/model/ConfirmButton").GetComponent<ConfirmStoneButton>();

		Instance.rejectbutton = GameObject.Instantiate(GameObject.Find("GameTable/SpecialNodeHandler_Grimora/GoatEyeSequencer_Grimora/ConfirmStoneButton"));
		Instance.rejectbutton.name = "RejectButton";
		Instance.rejectbutton.transform.SetParent(newSequencer.transform);
		Instance.rejectStone = GameObject.Find("GameTable/SpecialNodeHandler_Grimora/GoatEyeSequencer_Grimora/RejectButton/Anim/model/ConfirmButton").GetComponent<ConfirmStoneButton>();

		Instance.confirmbutton.transform.localPosition = new Vector3(-1.683f, 5f, - 2.1682f);
		Instance.rejectbutton.transform.localPosition = new Vector3(1.3436f, 5f, - 2.1682f);
		Instance.rejectbutton.transform.localRotation = new Quaternion(0, 180, 0, 1);

		int I = 10;

		List<Material> EyeMats = new List<Material>() { ResourceBank.Get<Material>($"Art/Materials/Eyeball_Goat"), ResourceBank.Get<Material>($"Art/Materials/Eyeball_Blue"), ResourceBank.Get<Material>($"Art/Materials/Eyeball_Brown"), ResourceBank.Get<Material>($"Art/Materials/Eyeball_Olive"), ResourceBank.Get<Material>($"Art/Materials/Eyeball_Wizard") };

		while (I > 0)
		{
			GameObject Eye = GameObject.Instantiate(AssetConstants.GoatEyeFigurine);
			Destroy(Eye.GetComponent<Rigidbody>());
			float scale = 1;
			Eye.transform.localScale = new Vector3(scale, scale, scale);
			Instance.goatEyes.Add(Eye);

			if (I == 10) { }
			else
			{
				Eye.FindChild("Figurine").GetComponent<MeshRenderer>().material = EyeMats.GetRandomItem();
				//Eye.transform.Find("Figurine").GetComponent<MeshRenderer>().material = EyeMats.GetRandomItem();
			}

			Eye.SetActive(false);
			I--;
		}

		Instance.goatEyes[0].transform.localPosition = new Vector3(-0.24f, 5.4077f, 0.243f);
		Instance.goatEyes[1].transform.localPosition = new Vector3(-2.7999f, 5.3723f, - 0.4406f);
		Instance.goatEyes[1].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
		Instance.goatEyes[2].transform.localPosition = new Vector3(-1.6844f, 6.6305f, 0.243f);
		Instance.goatEyes[2].transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
		Instance.goatEyes[3].transform.localPosition = new Vector3(1.7952f, 6.9824f, 1.5157f);
		Instance.goatEyes[4].transform.localPosition = new Vector3(1.4788f, 5.661f, 1.4412f);
		Instance.goatEyes[4].transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
		Instance.goatEyes[5].transform.localPosition = new Vector3(-3.5927f, 5.4606f, - 2.3861f);
		Instance.goatEyes[6].transform.localPosition = new Vector3(2.5382f, 5.7334f, 0.243f);
		Instance.goatEyes[6].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
		Instance.goatEyes[7].transform.localPosition = new Vector3(2.553f, 5.4755f, - 1.5388f);
		Instance.goatEyes[8].transform.localPosition = new Vector3(-2.7636f, 7.8377f, 0.243f);
		Instance.goatEyes[9].transform.localPosition = new Vector3(3.5945f, 6.1936f, 0.6539f);
		Instance.goatEyes[9].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

	}



}

