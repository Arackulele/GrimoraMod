using DiskCardGame;
using HarmonyLib;
using InscryptionAPI;
using InscryptionAPI.Ascension;
using InscryptionAPI.Guid;
using System.Linq;
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
			__instance.iconRenderer.color = parsed;
			__instance.blinkEffect.blinkOffColor = parsed;

		}

		else if (ChallengeManagement.ValidChallenges.Contains(info.challengeType))
		{

			Color parsed;
			ColorUtility.TryParseHtmlString("#FFD877", out parsed);
			__instance.iconRenderer.color = parsed;
			__instance.blinkEffect.blinkOffColor = parsed;

		}


		if (ChallengeManagement.RedChallenge.Contains(info.challengeType))
		{

			Color parsed;
			ColorUtility.TryParseHtmlString("#cc1d57", out parsed);
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
	public static AscensionChallenge HardMode { get; private set; }
	public static AscensionChallenge ThreePhaseGhouls { get; private set; }

	public static AscensionChallenge InfinitLives { get; private set; }
	public static AscensionChallenge SafeChair { get; private set; }
	public static AscensionChallenge PlaceBones { get; private set; }
	public static AscensionChallenge EasyGuards { get; private set; }

	public static List<AscensionChallengeInfo> PatchedChallengesReference;

	public static List<AscensionChallenge> ValidChallenges;
	public static List<AscensionChallenge> AntiChallenges;
	public static List<AscensionChallenge> RedChallenge;

	public static void UpdateGrimoraChallenges()
	{
		NoBones = GuidManager.GetEnumValue<AscensionChallenge>(GUID, "NoBones");
		KayceesKerfuffle = GuidManager.GetEnumValue<AscensionChallenge>(GUID, "KayceesKerfuffle");
		SawyersShowdown = GuidManager.GetEnumValue<AscensionChallenge>(GUID, "SawyersShowdown");
		RoyalsRevenge = GuidManager.GetEnumValue<AscensionChallenge>(GUID, "RoyalsRevenge");
		Soulless = GuidManager.GetEnumValue<AscensionChallenge>(GUID, "Soulless");
		FrailHammer = GuidManager.GetEnumValue<AscensionChallenge>(GUID, "FrailHammer");
		JammedChair = GuidManager.GetEnumValue<AscensionChallenge>(GUID, "JammedChair");
		WiltedClover = GuidManager.GetEnumValue<AscensionChallenge>(GUID, "WiltedClover");
		HardMode = GuidManager.GetEnumValue<AscensionChallenge>(GUID, "HardMode");

		InfinitLives = GuidManager.GetEnumValue<AscensionChallenge>(GUID, "InfinitLives");
		SafeChair = GuidManager.GetEnumValue<AscensionChallenge>(GUID, "SafeChair");
		PlaceBones = GuidManager.GetEnumValue<AscensionChallenge>(GUID, "PlaceBones");
		EasyGuards = GuidManager.GetEnumValue<AscensionChallenge>(GUID, "EasyGuards");

		ThreePhaseGhouls = GuidManager.GetEnumValue<AscensionChallenge>(GUID, "ThreePhaseGhouls");
		AntiChallenges = new List<AscensionChallenge>() {InfinitLives,SafeChair, PlaceBones, EasyGuards};
		RedChallenge = new List<AscensionChallenge>() { ThreePhaseGhouls };


		PatchedChallengesReference = new List<AscensionChallengeInfo>()
		{
			new()
			{
				challengeType = NoBones,
				title = Localization.CurrentLanguage == Language.ChineseSimplified ? "没有起始骨头" : "No Bones",
				description = Localization.CurrentLanguage == Language.ChineseSimplified ? "头目不再奖励战斗开始时的额外骨头。" : "You no longer gain the extra bones, from defeating bosses.",
				iconSprite = AssetUtils.GetPrefab<Sprite>("NoBonesNew"),
				activatedSprite = AssetUtils.GetPrefab<Sprite>("NoBones_Active"),
				pointValue = 15,
			},
				new()
				{
					challengeType = KayceesKerfuffle,
					title = Localization.CurrentLanguage == Language.ChineseSimplified ? "凯茜的骚乱" : "Kaycee's Kerfuffle",
					description = Localization.CurrentLanguage == Language.ChineseSimplified ? "每场战斗的第四个回合，你的卡牌会被冰封禁锢。" : "The fourth turn of every battle, all your cards will be Frozen Away.",
					iconSprite = AssetUtils.GetPrefab<Sprite>("KayceesKerfuffle"),
					pointValue = 15
				}
			,
			new()
				{
					challengeType = SawyersShowdown,
					title = Localization.CurrentLanguage == Language.ChineseSimplified ? "索耶的骚乱" : "Sawyer's Showdown",
					description = Localization.CurrentLanguage == Language.ChineseSimplified ? "每3回合损失1根骨头，如果你的骨头少于3根，则获得1根骨头。" : "Lose 1 bone every 3rd turn, if you have less than 3, gain 1 Bone",
					iconSprite = AssetUtils.GetPrefab<Sprite>("SawyersShowdown"),
					activatedSprite = AssetUtils.GetPrefab<Sprite>("normaleyes"),
					pointValue = 5
				}
				,
				new()
				{
					challengeType = RoyalsRevenge,
					title = Localization.CurrentLanguage == Language.ChineseSimplified ? "罗亚尔的骚乱" : "Royal Rumble",
					description = Localization.CurrentLanguage == Language.ChineseSimplified ? "你每使用三张牌，第三张牌会获得Lit Fuse印记（缓慢掉血，并且死后自爆）。" : "Every third card you play gains the Lit Fuse sigil.",
					iconSprite = AssetUtils.GetPrefab<Sprite>("RoyalsRevenge"),
					activatedSprite = AssetUtils.GetPrefab<Sprite>("Royal_Active"),
					pointValue = 5
				}
				,
			new()
			{
				challengeType = Soulless,
				title = Localization.CurrentLanguage == Language.ChineseSimplified ? "贪婪的灵魂" : "Soulless",
				description = Localization.CurrentLanguage == Language.ChineseSimplified ? "骷髅额外需要1点能量成本。" : "Skeletons cost +1 Energy.",
				iconSprite = AssetUtils.GetPrefab<Sprite>("Soulless"),
				activatedSprite = AssetUtils.GetPrefab<Sprite>("Soulless_Active"),
				pointValue = 10
			},
			new()
			{
				challengeType = Soulless,
				title = Localization.CurrentLanguage == Language.ChineseSimplified ? "贪婪的灵魂" : "Soulless",
				description = Localization.CurrentLanguage == Language.ChineseSimplified ? "骷髅额外需要1点能量成本。" : "Skeletons cost +1 Energy.",
				iconSprite = AssetUtils.GetPrefab<Sprite>("Soulless"),
				activatedSprite = AssetUtils.GetPrefab<Sprite>("Soulless_Active"),
				pointValue = 10
			},
				new()
				{
					challengeType = FrailHammer,
					title = Localization.CurrentLanguage == Language.ChineseSimplified ? "难以修复的锤子" : "Frail Hammer",
					description = Localization.CurrentLanguage == Language.ChineseSimplified ? "锤子在打败头目，而不是本场战斗结束后修复。" : "The Hammer gets repaired only after every Boss.",
					iconSprite = AssetUtils.GetPrefab<Sprite>("hammerskull"),
					activatedSprite = AssetUtils.GetPrefab<Sprite>("normaleyes"),
					pointValue = 15
				}
			,
			new()
			{
				challengeType = JammedChair,
				title = Localization.CurrentLanguage == Language.ChineseSimplified ? "卡住的电椅" : "Jammed Chair",
				description = Localization.CurrentLanguage == Language.ChineseSimplified ? "电椅固定设置为最大电力。" : "The electric chair is always set to level 3.",
				iconSprite = AssetUtils.GetPrefab<Sprite>("JammedChair"),
				activatedSprite = AssetUtils.GetPrefab<Sprite>("JammedChair_Active"),
				pointValue = 10
			},
			new()
			{
				challengeType = WiltedClover,
				title = Localization.CurrentLanguage == Language.ChineseSimplified ? "小号宝箱" : "Wilted Clover",
				description = Localization.CurrentLanguage == Language.ChineseSimplified ? "卡牌宝箱中提供的可被选择的卡牌少了一张。" : "There is only 2 Cards present at every Card chest.",
				iconSprite = AssetUtils.GetPrefab<Sprite>("WiltedClover"),
				activatedSprite = AssetUtils.GetPrefab<Sprite>("normaleyes"),
				pointValue = 20
			},

			new()
			{
				challengeType = HardMode,
				title = Localization.CurrentLanguage == Language.ChineseSimplified ? "地狱模式" : "Hell Mode",
				description = Localization.CurrentLanguage == Language.ChineseSimplified ? "Dont play this one, seriously. (Makes encounters Significantly Harder)" : "Dont play this one, seriously. (Makes encounters Significantly Harder)",
				iconSprite = AssetUtils.GetPrefab<Sprite>("hellmodeskull"),
				activatedSprite = AssetUtils.GetPrefab<Sprite>("hellmodeeyes"),
				pointValue = 80
			},
			
			//Anti-Challenges below for good sorting
			new()
			{
				challengeType = InfinitLives,
				title = Localization.CurrentLanguage == Language.ChineseSimplified ? "无限生命" : "Infinite Lives",
				description = Localization.CurrentLanguage == Language.ChineseSimplified ? "你不会死。" : "You can't die. Really.",
				iconSprite = AssetUtils.GetPrefab<Sprite>("InfLives"),
				activatedSprite = AssetUtils.GetPrefab<Sprite>("InfLives_Active"),
				pointValue = -999
			},
			new()
			{
				challengeType = SafeChair,
				title = Localization.CurrentLanguage == Language.ChineseSimplified ? "高科技电椅" : "Safe Chair",
				description = Localization.CurrentLanguage == Language.ChineseSimplified ? "你的卡牌不会被电椅电坏。" : "Your cards are immune to electricity of the chair.",
				iconSprite = AssetUtils.GetPrefab<Sprite>("SafeChair"),
				activatedSprite = AssetUtils.GetPrefab<Sprite>("antichallengeeyes"),
				pointValue = -20
			},
			new()
			{
				challengeType = PlaceBones,
				title = Localization.CurrentLanguage == Language.ChineseSimplified ? "Bone Lords Mercy" : "Bone Lords Mercy",
				description = Localization.CurrentLanguage == Language.ChineseSimplified ? "Gain a Bone when you place any free Card on the Board." : "Gain a Bone when you place any free Card on the Board.",
				iconSprite = AssetUtils.GetPrefab<Sprite>("skukk"),
				activatedSprite = AssetUtils.GetPrefab<Sprite>("antichallengeeyes"),
				pointValue = -20
			},
			new()
			{
				challengeType = EasyGuards,
				title = Localization.CurrentLanguage == Language.ChineseSimplified ? "Pharaos Blessing" : "Pharaos Blessing",
				description = Localization.CurrentLanguage == Language.ChineseSimplified ? "The Ankh Guard effects always benefit you." : "The Ankh Guard effects always benefit you.",
				iconSprite = AssetUtils.GetPrefab<Sprite>("ankh"),
				activatedSprite = AssetUtils.GetPrefab<Sprite>("antichallengeeyes"),
				pointValue = -15
			},


			new()
			{
				challengeType = ThreePhaseGhouls,
				title = Localization.CurrentLanguage == Language.ChineseSimplified ? "Vengeant Ghouls" : "Vengeant Ghouls",
				description = Localization.CurrentLanguage == Language.ChineseSimplified ? "Kaycee, Sawyer and Royal have new tricks up their sleeve" : "Kaycee, Sawyer and Royal have new tricks up their sleeve",
				iconSprite = AssetUtils.GetPrefab<Sprite>("threephaseghouls"),
				activatedSprite = AssetUtils.GetPrefab<Sprite>("threephaseeyes"),
				pointValue = 40
			},
		};

		ValidChallenges = new List<AscensionChallenge>
		{
			Soulless,
			InfinitLives,
			JammedChair,
			WiltedClover,
			SafeChair,
			PlaceBones,
			EasyGuards,
			//AscensionChallenge.SubmergeSquirrels,
			NoBones,
			HardMode,
			//AscensionChallenge.BossTotems,
			KayceesKerfuffle,
			FrailHammer,
			SawyersShowdown,
			RoyalsRevenge,
			ThreePhaseGhouls
		};







		ChallengeManager.ModifyChallenges += delegate(List<ChallengeManager.FullChallenge> challenges)
		{
			if (ScreenManagement.ScreenState == CardTemple.Undead)
			{
				challenges.Clear();
				challenges.AddRange(PatchedChallengesReference.Convert());
				return challenges;
			}

			challenges = ChallengeManager.BaseGameChallenges.ToList();
			return challenges;
		};
}


	
	
	[HarmonyPostfix, HarmonyPatch(typeof(AscensionUnlockSchedule), nameof(AscensionUnlockSchedule.ChallengeIsUnlockedForLevel))]
	[HarmonyAfter(InscryptionAPIPlugin.ModGUID)]
	public static void ValidGrimoraChallenges(ref bool __result, AscensionChallenge challenge,  int level)
	{
		if (ScreenManagement.ScreenState == CardTemple.Undead || GrimoraSaveUtil.IsGrimoraModRun) 
		{
			__result = ValidChallenges.Contains(challenge);
			return;
		}

	}
}
