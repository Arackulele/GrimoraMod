using DiskCardGame;
using HarmonyLib;
using InscryptionAPI;
using InscryptionAPI.Ascension;
using InscryptionAPI.Guid;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch]
internal static class Temp
{
	[HarmonyPatch(typeof(AscensionIconInteractable), "AssignInfo")]
	[HarmonyPostfix]
	private static void ColorsPlease(ref AscensionIconInteractable __instance, AscensionChallengeInfo info)
	{
		if (ChallengeManagement.AntiChallenges.Contains(info.challengeType))
		{
			
			Color parsed;
			ColorUtility.TryParseHtmlString("#19FFBD", out parsed);
			__instance.iconRenderer.color =parsed;
			__instance.blinkEffect.blinkOffColor = parsed;

		}

		else if (ChallengeManagement.ValidChallenges.Contains(info.challengeType))
		{

			Color parsed;
			ColorUtility.TryParseHtmlString("#FFD877", out parsed);
			__instance.iconRenderer.color = parsed;
			__instance.blinkEffect.blinkOffColor = parsed;

		}
	}

}



internal static class ChallengeToFullChallenge_Compatibility
{
	internal static ChallengeManager.FullChallenge Convert(this AscensionChallengeInfo info) => new ChallengeManager.FullChallenge(){Challenge =info, AppearsInChallengeScreen = true, UnlockLevel = 0};

	internal static List<ChallengeManager.FullChallenge> Convert(this List<AscensionChallengeInfo> infos)
	{
		var list = new List<ChallengeManager.FullChallenge>();
		foreach (var info in infos)
		{
			list.Add(info.Convert());
		}
		return list;
	}
}


[HarmonyPatch]
public class ChallengeManagement
{
	public static AscensionChallenge NoBones { get; private set; }
	public static AscensionChallenge KayceesKerfuffle { get; private set; }
	public static AscensionChallenge SawyersShowdown { get; private set; }
	public static AscensionChallenge RoyalsRevenge { get; private set; }
	public static AscensionChallenge Soulless { get; private set; }
	public static AscensionChallenge FrailHammer { get; private set; }
	public static AscensionChallenge JammedChair { get; private set; }
	public static AscensionChallenge WiltedClover { get; private set; }


	public static AscensionChallenge InfinitLives { get; private set; }

	
	public static List<AscensionChallengeInfo> PatchedChallengesReference;
	
	public static List<AscensionChallenge> ValidChallenges;
	public static List<AscensionChallenge> AntiChallenges;
	
	public static void UpdateGrimoraChallenges()
	{
		NoBones = GuidManager.GetEnumValue<AscensionChallenge>(GUID, "NoBones");
		KayceesKerfuffle = GuidManager.GetEnumValue<AscensionChallenge>(GUID, "KayceesKerfuffle");
		SawyersShowdown = GuidManager.GetEnumValue<AscensionChallenge>(GUID, "SawyersShowdown");
		RoyalsRevenge = GuidManager.GetEnumValue<AscensionChallenge>(GUID, "RoyalsRevenge");
		Soulless=GuidManager.GetEnumValue<AscensionChallenge>(GUID, "Soulless");
		FrailHammer=GuidManager.GetEnumValue<AscensionChallenge>(GUID, "FrailHammer");
		JammedChair=GuidManager.GetEnumValue<AscensionChallenge>(GUID, "JammedChair");
		WiltedClover=GuidManager.GetEnumValue<AscensionChallenge>(GUID, "WiltedClover");

		
		InfinitLives=GuidManager.GetEnumValue<AscensionChallenge>(GUID, "InfinitLives");
		AntiChallenges = new List<AscensionChallenge>() {InfinitLives};



		PatchedChallengesReference = new List<AscensionChallengeInfo>()
		{
			new ()
				{
					challengeType = NoBones,
					title = "No Bones",
					description = "You no longer gain the extra bones, from defeating bosses.",
					iconSprite = AssetUtils.GetPrefab<Sprite>("NoBonesNew"),
					activatedSprite = AssetUtils.GetPrefab<Sprite>("NoBones_Active"),
					pointValue = 5,
				}
			,
			
				new()
				{
					challengeType = KayceesKerfuffle,
					title = "Kaycee's Kerfuffle",
					description = "The fourth turn of every battle, all your cards will be Frozen Away.",
					iconSprite = AssetUtils.GetPrefab<Sprite>("KayceesKerfuffle"),
					pointValue = 15
				}
			,
			

				new()
				{
					challengeType = SawyersShowdown,
					title = "Sawyer's Showdown",
					description = "Lose 1 bone every 3rd turn.",
					iconSprite = AssetUtils.GetPrefab<Sprite>("SawyersShowdown"),
					pointValue = 10
				}
				,

				new()
				{
					challengeType = RoyalsRevenge,
					title = "Royal's Revenge",
					description = "Every third card you play gains the Lit Fuse sigil.",
					iconSprite = AssetUtils.GetPrefab<Sprite>("RoyalsRevenge"),
					activatedSprite = AssetUtils.GetPrefab<Sprite>("Royal_Active"),
					pointValue = 20
				}
				,

				new()
				{
					challengeType = Soulless,
					title = "Soulless",
					description = "Skeletons cost +1 Energy.",
					iconSprite = AssetUtils.GetPrefab<Sprite>("Soulless"),
					activatedSprite =  AssetUtils.GetPrefab<Sprite>("Soulless_Active"),
					pointValue = 5
				},
			

				new()
				{
					challengeType = Soulless,
					title = "Soulless",
					description = "Skeletons cost +1 Energy.",
					iconSprite = AssetUtils.GetPrefab<Sprite>("Soulless"),
					activatedSprite =  AssetUtils.GetPrefab<Sprite>("Soulless_Active"),
					pointValue = 5
				}
			,
			
				new()
				{
					challengeType = FrailHammer,
					title = "Frail Hammer",
					description = "The Hammer charges are only refreshed after each boss instead of each fight.",
					iconSprite = AssetUtils.GetPrefab<Sprite>("FrailHammer"),
					activatedSprite =  AssetUtils.GetPrefab<Sprite>("FrailHammer_Active"),
					pointValue = 15
				}
			,

				new()
				{
					challengeType = JammedChair,
					title = "Jammed Chair",
					description = "The electric chair is always set to level 3.",
					iconSprite = AssetUtils.GetPrefab<Sprite>("JammedChair"),
					activatedSprite =  AssetUtils.GetPrefab<Sprite>("JammedChair_Active"),
					pointValue = 10
				}
			,
			new()
			{
				challengeType = WiltedClover,
				title = "Wilted Clover",
				description = "You choose only from 2 cards in card selection, instead of 3.",
				iconSprite = AssetUtils.GetPrefab<Sprite>("WiltedClover"),
				pointValue = 20
			}
			,
			
			
			
			
			//Anti-Challenges below for good sorting
			new()
				{
					challengeType = InfinitLives,
					title = "Infinite Lives",
					description = "You can't die. Really.",
					iconSprite = AssetUtils.GetPrefab<Sprite>("InfLives"),
					activatedSprite =  AssetUtils.GetPrefab<Sprite>("InfLives_Active"),
					pointValue = 0
				}
			,

		};

		ValidChallenges = new List<AscensionChallenge>
		{
			Soulless, 
			InfinitLives,
			JammedChair,
			WiltedClover,
			//AscensionChallenge.WeakStarterDeck,
			//AscensionChallenge.SubmergeSquirrels,
			NoBones,
			//AscensionChallenge.BossTotems,
			KayceesKerfuffle,
			FrailHammer,
			SawyersShowdown,
			RoyalsRevenge,
		};






		try
		{
			var ModifyEvent = typeof(ChallengeManager).GetEvent("ModifyChallenges");
			ModifyEvent.AddEventHandler(ModifyEvent.AddMethod, delegate(List<AscensionChallengeInfo> challenges)
			{
				if (ScreenManagement.ScreenState == CardTemple.Undead)
				{
					challenges.Clear();
					challenges.AddRange(PatchedChallengesReference);
					return challenges;
				}

				challenges = (List<AscensionChallengeInfo>) (object) ChallengeManager.BaseGameChallenges.ToList();
				return challenges;
			});
		}
		catch
		{
			GrimoraPlugin.Log.LogInfo("API 2.4.0 is being used.");
		}
		try
		{
			ChallengeManager.ModifyChallenges += delegate(List<ChallengeManager.FullChallenge> challenges)
			{
				if (ScreenManagement.ScreenState == CardTemple.Undead)
				{
					challenges.Clear();
					challenges.AddRange(PatchedChallengesReference.Convert());
					return challenges;
				}
				challenges =  ChallengeManager.BaseGameChallenges.ToList();
				return challenges;
			};
		}
		catch 
		{
			GrimoraPlugin.Log.LogWarning("You are using outdated version of api, api 2.3.0 while the mod was developed for api 2.4.0, backwards compatibility layer is turned on!");
		}


	}
	
	
	[HarmonyPostfix, HarmonyPatch(typeof(AscensionUnlockSchedule), nameof(AscensionUnlockSchedule.ChallengeIsUnlockedForLevel))]
	[HarmonyAfter(InscryptionAPIPlugin.ModGUID)]
	public static void ValidGrimoraChallenges(ref bool __result, AscensionChallenge challenge,  int level)
	{
		if (ScreenManagement.ScreenState == CardTemple.Undead || SaveDataRelatedPatches.IsGrimoraRun) 
		{
			__result = ValidChallenges.Contains(challenge);
			return;
		}

	}
}
