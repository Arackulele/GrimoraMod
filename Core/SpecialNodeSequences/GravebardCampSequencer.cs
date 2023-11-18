using System.Collections;
using System.Diagnostics;
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
using static UnityEngine.UIElements.UIR.BestFitAllocator;

namespace GrimoraMod;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class GravebardCampSequencer : ManagedBehaviour
{
	public static GravebardCampSequencer Instance => FindObjectOfType<GravebardCampSequencer>();

	private ConfirmStoneButton confirmStone;

	private GameObject confirmbutton;

	GameObject statue;

	MeshRenderer StoneQuad;

	LuteButton Click;

	[SerializeField] private GameObject gravebardCard;

	private CardInfo _gravebardCardReward;

	int price;

	public IEnumerator StartSequence()
	{
		Log.LogDebug("started gravebard camp sequence");
		_gravebardCardReward = NameGravebard.GetCardInfo();

		price = 3;


		yield return new WaitForSeconds(0.3f);

		ExplorableAreaManager.Instance.HangingLight.gameObject.SetActive(false);
		ExplorableAreaManager.Instance.HandLight.gameObject.SetActive(false);

		yield return new WaitForSeconds(0.6f);

		ViewManager.Instance.SwitchToView(View.Default, false, true);

		statue = GameObject.Instantiate(AssetConstants.GravebardCampStatue);

		yield return TextDisplayer.Instance.ShowUntilInput("A Gravebard has set up camp here.");

		yield return new WaitForSeconds(0.6f);

		yield return TextDisplayer.Instance.ShowUntilInput("They play songs and elegies about long forgotten warriors and rulers.");

		yield return TextDisplayer.Instance.ShowUntilInput("Though nowadays their audience consists mostly of various ghouls.");

		yield return TextDisplayer.Instance.ShowUntilInput("They are willing to play you a song and trade you an unknown gift in exchange for any grams of ash you happen to be carrying.");

		yield return TextDisplayer.Instance.ShowUntilInput("All the excess damage you have done may finally come in handy!");

		statue.transform.position = new Vector3(0f, 5.5619f, 0f);
		statue.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

		GameObject Lute = statue.transform.GetChild(1).gameObject;

		Click = Lute.AddComponent<LuteButton>();

		confirmStone.Enter();


		if (RunState.Run.consumables.Count() > 2 || RunState.Run.currency < price)
		{
			StartCoroutine(NoValidCardsSequence());


			yield break;
		}

		StartCoroutine(RejectOffer());

		yield return TextDisplayer.Instance.ShowUntilInput("Strum the Lute when you are ready to make a trade.");

		while (RunState.Run.consumables.Count() < 3 && RunState.Run.currency >= price)
		{
			yield return PriceLines();
			Click.SetButtonInteractable();

			yield return Click.WaitUntilConfirmation();

			yield return TellRandomStory();

			yield return new WaitForSeconds(0.7f);

			RunState.Run.currency -= price;

			price *= 2;

			yield return GenerateItem();

		}

		StopCoroutine(RejectOffer());
		confirmStone.PressButton();

		yield return new WaitForSeconds(0.5f);



		StartCoroutine(EndSequence(false));
	}

	public IEnumerator RejectOffer()
	{
		confirmStone.SetEnabled(true);
		yield return confirmStone.WaitUntilConfirmation();


		StopCoroutine(StartSequence());
		Click.PressButton();
		Click.StopAllCoroutines();
		Destroy(Click);

		StartCoroutine(EndSequence(true));
	}

	private IEnumerator NoValidCardsSequence()
	{
		yield return TextDisplayer.Instance.ShowUntilInput("Even if you wanted anything else, you are no longer able to afford it...");

		yield return TextDisplayer.Instance.ShowUntilInput("Sadly, you are not able to get anything from them...");

		yield return TextDisplayer.Instance.ShowUntilInput("Feeling sorry for you, the Gravebard offers to come with you.");

		gravebardCard = Instantiate(
			AssetConstants.GrimoraSelectableCard,
			new Vector3(0, 12, 5f),
			Quaternion.identity,
			transform
		);
		var revenantSelectableCard = gravebardCard.GetComponent<SelectableCard>();
		revenantSelectableCard.ClearDelegates();
		revenantSelectableCard.SetInfo(_gravebardCardReward);

		bool cardGrabbed = false;
		Log.LogDebug($"Playing lowering sequence");
		Vector3 targetPos = new Vector3(0, 5, 6f);
		Tween.Position(gravebardCard.transform, gravebardCard.transform.position - targetPos, 1f, 0f);
		revenantSelectableCard.CursorSelectEnded += delegate { cardGrabbed = true; };
		yield return new WaitUntil(() => cardGrabbed);

		RuleBookController.Instance.SetShown(false);
		TableRuleBook.Instance.SetOnBoard(false);
		yield return new WaitForEndOfFrame();

		gravebardCard.SetActive(true);
		gravebardCard.transform.parent = null;
		gravebardCard.transform.position = gravebardCard.transform.position;
		gravebardCard.transform.rotation = gravebardCard.transform.rotation;
		revenantSelectableCard.SetInfo(_gravebardCardReward);
		revenantSelectableCard.SetInteractionEnabled(false);

		string text = _gravebardCardReward.description;
		yield return LearnObjectSequence(gravebardCard.transform, 1f, new Vector3(20f, 0f, 0f), text);
		Tween.Position(
			gravebardCard.transform,
			gravebardCard.transform.position + Vector3.up * 2f + Vector3.forward * 0.5f,
			0.1f,
			0f,
			null,
			Tween.LoopType.None,
			null,
			delegate { Destroy(gravebardCard); }
		);

		yield return new WaitForSeconds(0.5f);
		GrimoraSaveData.Data.deck.AddCard(_gravebardCardReward);

		StartCoroutine(EndSequence(false));
	}


	private IEnumerator LearnObjectSequence(Transform obj, float heightOffset, Vector3 baseRotation, string text)
	{
		Tween.Position(obj, new Vector3(0f, 5.7f + heightOffset, -4.25f), 0.1f, 0f, Tween.EaseInOut);
		Tween.Rotation(obj, baseRotation, 0.1f, 0f, Tween.EaseInOut);
		Tween.Rotate(
			obj,
			new Vector3(1f, 5f, 3f),
			Space.World,
			3f,
			0.1f,
			Tween.EaseInOut,
			Tween.LoopType.PingPong
		);
		yield return TextDisplayer.Instance.ShowUntilInput(text);
	}


	public IEnumerator GenerateItem()
	{
		int type = UnityEngine.Random.Range(0, 100);

		ConsumableItemData toAdd;

		if (type < 40)
		{
			GrimoraPlugin.Log.LogDebug("getting regular items");
			toAdd = ObtainableGrimoraItems.GetRandomItem();

		}
		else if (type < 90)
		{
			List<ConsumableItemData> validItems = new List<ConsumableItemData>(GrimoraItemsSecret);
			GrimoraPlugin.Log.LogDebug("getting secret items");
			validItems.Remove(validItems.Find(g => g.name.Contains("Slivered Hoggy Bank2")));
			validItems.Remove(validItems.Find(g => g.name.Contains("Slivered Hoggy Bank3")));


			toAdd = validItems.GetRandomItem();

		}
		else
		{
			List<ConsumableItemData> validItems = ItemsUtil.AllConsumables;
			GrimoraPlugin.Log.LogDebug("getting from all items");
			//new List<ConsumableItemData>() { ItemsUtil.GetConsumableByName("BirdLegFan"), ItemsUtil.GetConsumableByName("BleachPot"), ItemsUtil.GetConsumableByName("Battery"), ItemsUtil.GetConsumableByName("Failure"), ItemsUtil.GetConsumableByName("Hourglass"), };
			validItems.Remove(ItemsUtil.GetConsumableByName("FrozenOpossumBottle"));
			GrimoraPlugin.Log.LogDebug("1");
			validItems.Remove(ItemsUtil.GetConsumableByName("GoatBottle"));
			GrimoraPlugin.Log.LogDebug("2");
			validItems.Remove(ItemsUtil.GetConsumableByName("SquirrelBottle"));
			GrimoraPlugin.Log.LogDebug("3");
			validItems.Remove(ItemsUtil.GetConsumableByName("Hammer"));
			GrimoraPlugin.Log.LogDebug("4");
			validItems.Remove(ItemsUtil.GetConsumableByName("PiggyBank"));
			GrimoraPlugin.Log.LogDebug("5");
			validItems.Remove(ItemsUtil.GetConsumableByName("SpecialDagger"));

			toAdd = validItems.GetRandomItem();

		}
		yield return new WaitForSeconds(0.4f);


		Singleton<ViewManager>.Instance.SwitchToView(View.Consumables);

		yield return new WaitForSeconds(0.6f);

		if (RunState.Run.consumables.Count() < 3)
		{

			RunState.Run.consumables.Add(toAdd.name);
			Singleton<ItemsManager>.Instance.UpdateItems();
		}

		yield return new WaitForSeconds(0.8f);

		Singleton<ViewManager>.Instance.SwitchToView(View.Default);
		
	}

	public IEnumerator TellRandomStory()
	{
		int type = UnityEngine.Random.Range(0, 6); // NOT INCLUSIVE MAX


		switch (type)
		{
			default:
			case 1:
			yield return TextDisplayer.Instance.ShowUntilInput("With " + price + " grams of ash, they hand you a pouch, and play a song about a brave knight from the crusades.");
			yield return TextDisplayer.Instance.ShowUntilInput("Even after a Demonic entity cursed them into a horse-like form, they were still able to liberate the lost souls trapped by the Demon!");
			yield return TextDisplayer.Instance.ShowUntilInput("Oh right, the pouch, let's see what's inside!");
			break;

			case 2:
			yield return TextDisplayer.Instance.ShowUntilInput("With " + price + " grams of ash in tow, they hand you another pouch, and play a tune about a sorceress from a mythical land.");
			yield return TextDisplayer.Instance.ShowUntilInput("Despite pressure from a prophecy to be the hero of the land, they instead pulled from a dark tome and sided with a cloudy villain to conquer the land and find peace from fame in an unexpected way!");
			yield return TextDisplayer.Instance.ShowUntilInput("I almost forgot! You must be dying to know what is in the pouch!");
			break;

			case 3:
			yield return TextDisplayer.Instance.ShowUntilInput("Feeling satisfied with  " + price + " grams of ash, they hand you a pouch, and recount the tale of 4 islands.");
			yield return TextDisplayer.Instance.ShowUntilInput("Each of the islands had a ruler fixated on a selfish goal, all except for one...");
			yield return TextDisplayer.Instance.ShowUntilInput("One island had a secluded hermit covered in moss and fungi. They surrounded themselves with beasts, and would eternally trap anyone who would wander into their forest.");
			yield return TextDisplayer.Instance.ShowUntilInput("Another island had a mysterious wizard covered head to toe in a green fur. Their true goal remained unknown, but seemed to involve the torturing of their own subjects...");
			yield return TextDisplayer.Instance.ShowUntilInput("The worst island however, was barely even an island at all! A metallic barge with a sleepless factory ruled by an arrogant, robotic tyrant...");
			yield return TextDisplayer.Instance.ShowUntilInput("Which leaves the other island, a sprawling burial ground with a Crypt at the end of it. It was ruled by a woman who only wished for the power struggle to finally end...");
			yield return TextDisplayer.Instance.ShowUntilInput("Oh! That would happen to be me!");
			yield return TextDisplayer.Instance.ShowUntilInput("I think we now know how that story ended.");
			yield return TextDisplayer.Instance.ShowUntilInput("And truthfully, the story still isn't over yet, maybe we can reach the end together!");
			yield return TextDisplayer.Instance.ShowUntilInput("That being said, you have earned this item.");
			break;

			case 4:
				yield return TextDisplayer.Instance.ShowUntilInput("Taking " + price + " grams of ash with them, they hand you a pouch, and tell you about an ancient hunter of ghouls.");
				yield return TextDisplayer.Instance.ShowUntilInput("Though he was barely a hunter, instead he spent his time searching for something he could not obtain.");
				yield return TextDisplayer.Instance.ShowUntilInput("In the end he found a glimpse of what he had seeked, but paid a horrible price.");
				yield return TextDisplayer.Instance.ShowUntilInput("You should see whats inside that pouch.");
			break;

			case 5:
				yield return TextDisplayer.Instance.ShowUntilInput("Taking " + price + " grams of your ash, they hand you a pouch, and reminisce about a medic from medieval times.");
				yield return TextDisplayer.Instance.ShowUntilInput("Instead of being content with their life, they instead chased the love of their life for years.");
				yield return TextDisplayer.Instance.ShowUntilInput("This got them into much danger, but their friends were always there to help them out!");
				yield return TextDisplayer.Instance.ShowUntilInput("Lets see what that pouch contains.");

				break;
		}

	}

	public IEnumerator PriceLines()
	{
		switch (price)
		{
			default:
			case 3:
				yield return TextDisplayer.Instance.ShowUntilInput("Wil you offer 3 Grams worth of Ash?");
				break;
			case 6:
				yield return TextDisplayer.Instance.ShowUntilInput("The Gravebard never runs out of stories to sing about.");
				yield return TextDisplayer.Instance.ShowUntilInput("And as it so happens, they are willing to make you another offering, but they insist that you give them 6 grams of ash this time.");
				yield return TextDisplayer.Instance.ShowUntilInput("Are you willing to accept their terms?.");
			break;
			case 12:
				yield return TextDisplayer.Instance.ShowUntilInput("The Gravebard is able to give you one final gift, but now they are asking a much higher price of 12 grams of ash!");
				yield return TextDisplayer.Instance.ShowUntilInput("Astoundingly, you have enough to pay such a fee, but are you willing to part with it?");
				break;

		}
	}


	public IEnumerator EndSequence(bool exitmanually)
		{

		if (exitmanually == false)
		{
			yield return TextDisplayer.Instance.ShowUntilInput("We spent a lot of time here, I'm sure you are eager to get going again!");

			yield return TextDisplayer.Instance.ShowUntilInput("The Gravebard bids us farewell");
		}

		if (exitmanually == true)
		{

		yield return TextDisplayer.Instance.ShowUntilInput("I see you would rather hold on to the ash that you have collected.");

		yield return TextDisplayer.Instance.ShowUntilInput("It still may be of use later...");

		yield return TextDisplayer.Instance.ShowUntilInput("And don't feel bad, the Gravebard still enjoyed your company while it lasted!");

		}


		yield return new WaitForSeconds(0.1f);

		ViewManager.Instance.SwitchToView(View.Default);
		yield return new WaitForSeconds(0.25f);


		confirmStone.Exit();

		yield return new WaitForSeconds(0.3f);

		confirmStone.SetStoneInactive();

		Destroy(statue);

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


	public static void CreateSequencerInScene()
	{
		if (SpecialNodeHandler.Instance.SafeIsUnityNull())
		{
			return;
		}

		AudioController.Instance.SFX.Add(GrimoraPlugin.AllSounds.Find(g => g.name.Contains("guitar-strum-74592")));

		GameObject cardStatObj = Instantiate(
		ResourceBank.Get<GameObject>("Prefabs/SpecialNodeSequences/CardRemoveSequencer"),
		SpecialNodeHandler.Instance.transform
	);

		cardStatObj.name = "GravebardCampSequencer_Grimora";

		var newSequencer = cardStatObj.AddComponent<GravebardCampSequencer>();

		//ToDo: Fix this mess
		Instance.confirmbutton = GameObject.Find("GameTable/SpecialNodeHandler_Grimora/GravebardCampSequencer_Grimora/ConfirmStoneButton");

		Instance.confirmStone = GameObject.Find("GameTable/SpecialNodeHandler_Grimora/GravebardCampSequencer_Grimora/ConfirmStoneButton/Anim/model/ConfirmButton").GetComponent<ConfirmStoneButton>();

	}



}

