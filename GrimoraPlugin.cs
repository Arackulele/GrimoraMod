global using Color = UnityEngine.Color;
global using UnityObject = UnityEngine.Object;
global using UnityRandom = UnityEngine.Random;
using System.Collections;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI;
using InscryptionAPI.Card;
using InscryptionAPI.Helpers;
using UnityEngine;

namespace GrimoraMod;

[BepInDependency(InscryptionAPIPlugin.ModGUID)]
[BepInPlugin(GUID, Name, Version)]
public partial class GrimoraPlugin : BaseUnityPlugin
{
	public const string GUID = "arackulele.inscryption.grimoramod";
	public const string Name = "GrimoraMod";
	private const string Version = "2.8.6";

	internal static ManualLogSource Log;

	private static Harmony _harmony;

	public static List<GameObject> AllPrefabs;
	public static List<Material> AllMats;
	public static List<RuntimeAnimatorController> AllControllers;
	public static List<Sprite> AllSprites;
	public static List<AudioClip> AllSounds;
	public static List<Texture> AllAbilitiesTextures;

	// Gets populated in CardBuilder.Build()
	public static List<CardInfo> AllGrimoraModCards = new();
	public static List<CardInfo> AllPlayableGrimoraModCards = new();

	private void Awake()
	{
		Log = Logger;

		_harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), GUID);

		ConfigHelper.Instance.BindConfig();

		AllSprites = AssetUtils.LoadAssetBundle<Sprite>("grimoramod_sprites");
		AllAbilitiesTextures = AssetUtils.LoadAssetBundle<Texture>("grimoramod_abilities");
	}

	// private IEnumerator HotReloadMenuCardAdd()
	// {
	// 	if (ConfigHelper.Instance.isHotReloadEnabled && SceneManager.GetActiveScene().name.Equals("Start"))
	// 	{
	// 		if (MenuController.Instance.cardRow.Find("MenuCard_Grimora").IsNull())
	// 		{
	// 			Log.LogDebug($"Hot reload menu button creation");
	// 			MenuController.Instance.cards.Add(MenuControllerPatches.CreateButton(MenuController.Instance));
	// 		}
	// 	}
	// }

	private IEnumerator Start()
	{
		yield return LoadEverything();

		// yield return new WaitUntil(() => FindObjectOfType<StartScreenThemeSetter>());
		// StartScreenPatches.SetBackgroundToGrimoraTheme(FindObjectOfType<StartScreenThemeSetter>());
	}

	private IEnumerator LoadEverything()
	{
		yield return LoadAssetsAsync();

		LoadAbilitiesAndCards();
	}

	private void LoadAbilitiesAndCards()
	{
		Log.LogDebug($"Loading cards");

		// What this does, is that every method that exists under this partial class, Grimora Plugin,
		//	will be searched for and of the ones that start with 'Add_', will be invoked all at once after sorting by name.
		// We sort by name so Abilities come first because abilities have their method name like 'Add_Ability_' while cards have 'Add_Card_'
		var allAddMethods = AccessTools.GetDeclaredMethods(typeof(GrimoraPlugin))
		 .Where(mi => mi.Name.StartsWith("Add_"))
		 .ToList();
		allAddMethods.Sort((mi, mi2) => string.Compare(mi.Name, mi2.Name, StringComparison.Ordinal));
		allAddMethods.ForEach(mi => mi.Invoke(this, null));
		// Log.LogInfo($"Adding [{allAddMethods.Count(mi => mi.Name.Contains("_Card_"))}] cards.");

		AllGrimoraModCards.Sort((info, cardInfo) => string.Compare(info.name, cardInfo.name, StringComparison.Ordinal));
		AllPlayableGrimoraModCards = AllGrimoraModCards.Where(info => info.metaCategories.Any()).ToList();

		// change just the artwork of Starvation
		CardInfo card = CardManager.BaseGameCards.CardByName("Starvation");
		card.portraitTex = AllSprites.Single(sp => sp.name.Equals("Starvation"));
		card.portraitTex.RegisterEmissionForSprite(AllSprites.Single(sp => sp.name.Equals("Starvation_emission")));

		CardBuilder.Builder
		 .SetAbilities(Ability.BoneDigger, Ability.SteelTrap, Haunter.ability)
		 .SetBaseAttackAndHealth(0, 1)
		 .SetNames($"{GUID}_TrapTest", "Trap Test", "Trap".GetCardInfo().portraitTex)
		 .Build();
	}

	private void OnDestroy()
	{
		AllAbilitiesTextures = null;
		AllControllers = null;
		AllMats = null;
		AllPrefabs = null;
		AllSprites = null;
		AllSounds = null;
		AllGrimoraModCards = new List<CardInfo>();
		ConfigHelper.Instance.HandleHotReloadBefore();
		Resources.UnloadUnusedAssets();
		GrimoraModBattleSequencer.ActiveEnemyPiece = null;
		_harmony?.UnpatchSelf();
	}

	public static void SpawnParticlesOnCard(PlayableCard target, Texture2D tex, bool reduceY = false)
	{
		GravestoneCardAnimationController anim = target.Anim as GravestoneCardAnimationController;
		GameObject gameObject = Instantiate<ParticleSystem>(anim.deathParticles).gameObject;
		ParticleSystem particle = gameObject.GetComponent<ParticleSystem>();
		particle.startColor = Color.white;
		particle.GetComponent<ParticleSystemRenderer>().material =
			new Material(particle.GetComponent<ParticleSystemRenderer>().material) { mainTexture = tex };

		ParticleSystem.MainModule mainMod = particle.main;
		mainMod.startColor = new ParticleSystem.MinMaxGradient(Color.white);
		gameObject.gameObject.SetActive(true);
		gameObject.transform.position = anim.deathParticles.transform.position;
		gameObject.transform.localScale = anim.deathParticles.transform.localScale;
		gameObject.transform.rotation = anim.deathParticles.transform.rotation;
		if (reduceY)
		{
			particle.transform.position = new Vector3(
				particle.transform.position.x,
				particle.transform.position.y - 0.1f,
				particle.transform.position.z
			);
		}

		Destroy(gameObject, 6f);
	}

	private IEnumerator LoadAssetsAsync()
	{
		Log.LogInfo($"Loading asset bundles");

		yield return StartCoroutine(AssetUtils.LoadAssetBundleAsync<GameObject>("grimoramod_prefabs"));

		yield return StartCoroutine(AssetUtils.LoadAssetBundleAsync<AudioClip>("grimoramod_sounds"));

		yield return AssetUtils.LoadAssetBundleAsync<RuntimeAnimatorController>("grimoramod_controller");

		yield return AssetUtils.LoadAssetBundleAsync<Material>("grimoramod_mats");
	}
}
