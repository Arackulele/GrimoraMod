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
		if (info.challengeType == ChallengeManagement.InfinitLives)
		{
			
			Color parsed;
			ColorUtility.TryParseHtmlString("#19FFBD", out parsed);
			__instance.iconRenderer.color =parsed;
			__instance.blinkEffect.blinkOffColor = parsed;

		}
	}

}



[HarmonyPatch]
public class ChallengeManagement
{
	public static AscensionChallenge NoBones { get; private set; }
	public static AscensionChallenge KayceesKerfuffle { get; private set; }
	public static AscensionChallenge SawyersShowdown { get; private set; }
	public static AscensionChallenge RoyalsRevenge { get; private set; }
	public static AscensionChallenge SoullessI { get; private set; }
	public static AscensionChallenge SoullessII { get; private set; }
	public static AscensionChallenge FrailHammer { get; private set; }
	public static AscensionChallenge JammedChair { get; private set; }


	public static AscensionChallenge InfinitLives { get; private set; }

	
	public static Dictionary<AscensionChallenge, AscensionChallengeInfo> PatchedChallengesReference;
	
	public static List<AscensionChallenge> ValidChallenges;
	
	public static void UpdateGrimoraChallenges()
	{
		NoBones = GuidManager.GetEnumValue<AscensionChallenge>(GUID, "NoBones");
		KayceesKerfuffle = GuidManager.GetEnumValue<AscensionChallenge>(GUID, "KayceesKerfuffle");
		SawyersShowdown = GuidManager.GetEnumValue<AscensionChallenge>(GUID, "SawyersShowdown");
		RoyalsRevenge = GuidManager.GetEnumValue<AscensionChallenge>(GUID, "RoyalsRevenge");
		SoullessI=GuidManager.GetEnumValue<AscensionChallenge>(GUID, "SoullessI");
		SoullessII=GuidManager.GetEnumValue<AscensionChallenge>(GUID, "SoullessII");
		FrailHammer=GuidManager.GetEnumValue<AscensionChallenge>(GUID, "FrailHammer");
		JammedChair=GuidManager.GetEnumValue<AscensionChallenge>(GUID, "JammedChair");
		
		
		InfinitLives=GuidManager.GetEnumValue<AscensionChallenge>(GUID, "InfinitLives");


		PatchedChallengesReference = new Dictionary<AscensionChallenge, AscensionChallengeInfo>
		{
			{
				AscensionChallenge.NoHook, 
				new()
				{
					challengeType = NoBones,
					title = "No Bones",
					description = "You no longer gain the extra bones, from defeating bosses.",
					iconSprite = AssetUtils.GetPrefab<Sprite>("NoBonesNew"),
					activatedSprite = AssetUtils.GetPrefab<Sprite>("NoBones_Active"),
					pointValue = 5,
				}
			},
			{
				AscensionChallenge.LessConsumables, 
				new()
				{
					challengeType = KayceesKerfuffle,
					title = "Kaycee's Kerfuffle",
					description = "The fourth turn of every battle, all your cards will be Frozen Away.",
					iconSprite = AssetUtils.GetPrefab<Sprite>("KayceesKerfuffle"),
					pointValue = 15
				}
			},
			
			{
				AscensionChallenge.ExpensivePelts, 
				new()
				{
					challengeType = SawyersShowdown,
					title = "Sawyer's Showdown",
					description = "Lose 1 bone every 3rd turn.",
					iconSprite = AssetUtils.GetPrefab<Sprite>("SawyersShowdown"),
					pointValue = 10
				}
			},
			
			{
				AscensionChallenge.NoClover, 
				new()
				{
					challengeType = RoyalsRevenge,
					title = "Royal's Revenge",
					description = "Every third card you play gains the Lit Fuse sigil.",
					iconSprite = AssetUtils.GetPrefab<Sprite>("RoyalsRevenge"),
					activatedSprite = AssetUtils.GetPrefab<Sprite>("Royal_Active"),
					pointValue = 20
				}
			},
			
			{
				AscensionChallenge.BossTotems, 
				new()
				{
					challengeType = SoullessI,
					title = "Soulless I",
					description = "Skeletons cost +1 Energy.",
					iconSprite = AssetUtils.GetPrefab<Sprite>("Soulless"),
					activatedSprite =  AssetUtils.GetPrefab<Sprite>("Soulless_Active"),
					pointValue = 5
				}
			},
			{
				AscensionChallenge.StartingDamage, 
				new()
				{
					challengeType = SoullessII,
					title = "Soulless II",
					description = "Skeletons cost +1 Energy.",
					iconSprite = AssetUtils.GetPrefab<Sprite>("Soulless"),
					activatedSprite =  AssetUtils.GetPrefab<Sprite>("Soulless_Active"),
					pointValue = 5
				}
			},
			{
				AscensionChallenge.AllTotems, 
				new()
				{
					challengeType = FrailHammer,
					title = "Frail Hammer",
					description = "The Hammer charges are only refreshed after each boss instead of each fight.",
					iconSprite = AssetUtils.GetPrefab<Sprite>("FrailHammer"),
					activatedSprite =  AssetUtils.GetPrefab<Sprite>("FrailHammer_Active"),
					pointValue = 15
				}
			},
			{
				AscensionChallenge.NoBossRares, 
				new()
				{
					challengeType = JammedChair,
					title = "Jammed Chair",
					description = "The electric chair is always set to level 3.",
					iconSprite = AssetUtils.GetPrefab<Sprite>("JammedChair"),
					activatedSprite =  AssetUtils.GetPrefab<Sprite>("JammedChair_Active"),
					pointValue = 10
				}
			},
			
			
			
			{
				AscensionChallenge.LessLives, 
				new()
				{
					challengeType = InfinitLives,
					title = "Infinite Lives",
					description = "You can't die. Really.",
					iconSprite = AssetUtils.GetPrefab<Sprite>("InfLives"),
					activatedSprite =  AssetUtils.GetPrefab<Sprite>("InfLives_Active"),
					pointValue = 0
				}
			},
		};

		ValidChallenges = new List<AscensionChallenge>
		{
			AscensionChallenge.BaseDifficulty, //seems to be not overrideable normally tldr: try to place soulless instead of this
			InfinitLives,
			JammedChair,
			SoullessII,
			AscensionChallenge.WeakStarterDeck,
			AscensionChallenge.SubmergeSquirrels,
			NoBones,
			SoullessI,
			KayceesKerfuffle,
			FrailHammer,
			SawyersShowdown,
			RoyalsRevenge,
		};





		ChallengeManager.ModifyChallenges += delegate(List<AscensionChallengeInfo> challenges)
		{
			if (ScreenManagement.ScreenState == CardTemple.Undead)
			{
				for (int i = 0; i < challenges.Count; i++)
				{
					if (PatchedChallengesReference.ContainsKey(challenges[i].challengeType))
					{
						challenges[i] = PatchedChallengesReference[challenges[i].challengeType];
					}
				}

				return challenges;
			}
			challenges =  ChallengeManager.BaseGameChallenges.ToList();
			return challenges;
		};
	}
	
	
	[HarmonyPostfix, HarmonyPatch(typeof(AscensionUnlockSchedule), nameof(AscensionUnlockSchedule.ChallengeIsUnlockedForLevel))]
	[HarmonyAfter(InscryptionAPIPlugin.ModGUID)]
	public static void ValidGrimoraChallenges(ref bool __result, AscensionChallenge challenge,  int level)
	{
		if (ScreenManagement.ScreenState == CardTemple.Undead || SaveDataRelatedPatches.IsGrimoraRun) 
		{
			if (!ValidChallenges.Contains(challenge))
			{
				__result = false;
				return;
			}
			else
			{
				__result = true;
			}
		}
		else if (PatchedChallengesReference.Any(kvp => kvp.Value.challengeType == challenge))
		{
			var kvp = PatchedChallengesReference.First(kvp => kvp.Value.challengeType == challenge);
			if (kvp.Value.challengeType != kvp.Key)
			{
				__result = AscensionUnlockSchedule.ChallengeIsUnlockedForLevel(kvp.Key, level);
			}
		}
	}
}
