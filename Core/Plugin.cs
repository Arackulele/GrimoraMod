using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading;
using APIPlugin;
using DiskCardGame;
using BepInEx;
using BepInEx.Logging;
using EasyFeedback.APIs;
using GBC;
using HarmonyLib;
using Pixelplacement;
using UnityEngine;
using Logger = UnityEngine.Logger;
using CardLoaderMod;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.XR.WSA;
using Random = System.Random;
using Object = System.Object;

namespace envapitests
{


    public static class ext
    {
        public static IEnumerator TransitionToGame2(this MenuController controller)
        {
            controller.TransitioningToScene = true;
            yield return new WaitForSecondsRealtime(0.75f);
            AudioController.Instance.FadeOutLoop(0.75f, Array.Empty<int>());
            Singleton<GBC.CameraEffects>.Instance.FadeOut();
            yield return new WaitForSecondsRealtime(0.75f);
            MenuController.LoadGameFromMenu(false);
            SaveManager.LoadFromFile();
            {
                LoadingScreenManager.LoadScene("finale_grimora");
            }
            SaveManager.savingDisabled = false;
            yield break;
        }
    }



    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class Pluginz : BaseUnityPlugin
    {


        private const string PluginGuid = "kopie.inscryption.act3tests";
        private const string PluginName = "act3tests";
        private const string PluginVersion = "1.0.0";
        public static AssetBundle bundle;
        public static UnityEngine.Object[] allAssets;
        internal static ManualLogSource Log;
        public static string staticpath;



        [HarmonyPatch(typeof(MenuController), "LoadGameFromMenu")]
        public class continueactone
        {
            public static void Postfix(bool newGameGBC)
            {
                SaveManager.LoadFromFile();
                if (newGameGBC)
                {
                    LoadingScreenManager.LoadScene("GBC_Intro");
                }
                else
                {
                    LoadingScreenManager.LoadScene("Part1_Cabin");
                }
                SaveManager.savingDisabled = false;
            }
        }

        [HarmonyPatch(typeof(GrimoraGameFlowManager), "SceneSpecificInitialization")]
        public class grimoramodinitialize
        {
            public static void Postfix(ref GrimoraGameFlowManager __instance)
            {

                SaveManager.saveFile.grimoraData.removedPieces = new List<int>();
                StoryEventsData.SetEventCompleted(StoryEvent.PlayerDeletedArchivistFile, true);
                StoryEventsData.EraseEvent(StoryEvent.FactoryConveyorBeltMoved);
                StoryEventsData.EraseEvent(StoryEvent.Part3PurchasedHoloBrush);
                StoryEventsData.EraseEvent(StoryEvent.FactoryCuckooClockAppeared);
                SaveManager.SaveToFile();
                Destroy(GameObject.FindObjectOfType<FinaleDeletionWindowManager>().gameObject);
                var epitaph = GameObject.FindObjectOfType<CryptEpitaphSlotInteractable>();
                AudioController.Instance.PlaySound3D("giant_stones_falling", MixerGroup.ExplorationSFX, epitaph.gameObject.transform.position, 1f, 0f, null, null, null, null, false);
                Tween.Position(epitaph.tombstoneParent, epitaph.tombstoneParent.position + Vector3.down * 11f, 6f, 0f, Tween.EaseIn, Tween.LoopType.None, null, null, true);
                Tween.Shake(epitaph.tombstoneAnim, epitaph.tombstoneAnim.localPosition, new Vector3(0.05f, 0.05f, 0.05f), 0.1f, 0f, Tween.LoopType.Loop, null, null, true);
                __instance.StartCoroutine((Singleton<GameFlowManager>.Instance as GrimoraGameFlowManager).RevealGrimoraSequence());
                //just to make a fucking select slot for old custom nodes
                {
                    var cardremover = (GameObject)Instantiate(Resources.Load("prefabs\\specialnodesequences\\CardRemoveSequencer"));
                    cardremover.gameObject.transform.position = Vector3.negativeInfinity;
                }

                //
                {
                    var rarechoicesgenerator = Instantiate(GameObject.FindObjectOfType<GrimoraCardChoiceGenerator>().gameObject);
                    rarechoicesgenerator.name = "RareChoices";
                    Destroy(rarechoicesgenerator.GetComponent<CardSingleChoicesSequencer>());
                    rarechoicesgenerator.AddComponent<RareCardChoicesSequencer>();
                    rarechoicesgenerator.transform.parent = GameObject.FindObjectOfType<SpecialNodeHandler>().gameObject.transform;
                    GameObject.FindObjectOfType<SpecialNodeHandler>().rareCardChoiceSequencer = rarechoicesgenerator.GetComponent<RareCardChoicesSequencer>();
                    rarechoicesgenerator.GetComponent<RareCardChoicesSequencer>().deckPile =
                        rarechoicesgenerator.GetComponentInChildren<CardPile>();
                    var rarebox = (GameObject)Instantiate(Resources.Load("prefabs\\specialnodesequences\\RareCardBox"));
                    rarebox.transform.parent = rarechoicesgenerator.transform;
                    rarebox.gameObject.transform.localPosition = new Vector3(-0.1f, rarebox.gameObject.transform.position.y, 99);
                    rarechoicesgenerator.GetComponent<RareCardChoicesSequencer>().box = rarebox.transform;
                    rarechoicesgenerator.AddComponent<Part1RareChoiceGenerator>();
                    rarechoicesgenerator.GetComponent<RareCardChoicesSequencer>().choiceGenerator =
                        rarechoicesgenerator.GetComponent<Part1RareChoiceGenerator>();
                    rarechoicesgenerator.GetComponent<RareCardChoicesSequencer>().selectableCardPrefab =
                        (GameObject)Resources.Load("prefabs\\cards\\SelectableCard_Grimora");
                    rarechoicesgenerator.GetComponent<RareCardChoicesSequencer>().gamepadGrid =
                        rarechoicesgenerator.GetComponent<GamepadGridControl>();
                }

            }
        }

        [HarmonyPatch(typeof(GrimoraCardChoiceGenerator), "GenerateChoices")]
        public class placeallact1choicestogrimora
        {
            public static bool Prefix(ref List<CardChoice> __result)
            {
                var test = new List<CardChoice>();
                foreach (var card in CardLoader.AllData.FindAll(info => info.metaCategories.Contains(CardMetaCategory.ChoiceNode) && info.temple == CardTemple.Nature))
                {
                    test.Add(new CardChoice
                    {
                        CardInfo = card
                    });
                }
                var randomizedchoices = test.ToArray().Randomize<CardChoice>().ToList();
                __result = new List<CardChoice>() { randomizedchoices[0], randomizedchoices[1], randomizedchoices[2] };
                return false;
            }
        }




        [HarmonyPatch(typeof(CardDisplayer), "SetTextColors")]
        public class render
        {
            public static bool Prefix(ref CardDisplayer __instance, CardRenderInfo renderInfo, PlayableCard playableCard)
            {
                if (SaveManager.saveFile.IsGrimora)
                {
                    if (!(playableCard == null))
                    {
                        int maxHealth = playableCard.MaxHealth;
                    }
                    else
                    {
                        int health = renderInfo.baseInfo.Health;
                    }
                    __instance.SetHealthTextColor((renderInfo.health >= renderInfo.baseInfo.Health) ? GameColors.Instance.glowSeafoam : GameColors.Instance.darkLimeGreen);
                    __instance.SetAttackTextColor(GameColors.Instance.glowSeafoam);
                    __instance.SetNameTextColor(GameColors.Instance.glowSeafoam);
                    return false;
                }

                return true;
            }
        }


        [HarmonyPatch(typeof(ChessboardEnemyManager), "MoveOpponentPieces")]
        public class zezx
        {
            public static bool Prefix()
            {
                return false;
            }
        }

        [HarmonyPatch(typeof(CardSingleChoicesSequencer), "CardSelectionSequence")]
        public class zezy
        {
            public static void Prefix(ref CardSingleChoicesSequencer __instance, out CardSingleChoicesSequencer __state)
            {
                __state = __instance;
            }

            public static IEnumerator Postfix(IEnumerator enumerator, SpecialNodeData nodeData, CardSingleChoicesSequencer __state)
            {
                if (SaveManager.saveFile.IsGrimora)
                {
                    CardSingleChoicesSequencer choicesSequencer = __state;
                    CardChoicesNodeData choicesData = nodeData as CardChoicesNodeData;
                    if (StoryEventsData.EventCompleted(StoryEvent.CloverFound) && (UnityEngine.Object)choicesSequencer.rerollInteractable != (UnityEngine.Object)null)
                    {
                        choicesSequencer.rerollInteractable.gameObject.SetActive(true);
                        choicesSequencer.rerollInteractable.SetEnabled(false);
                        CustomCoroutine.WaitThenExecute(1f, delegate
                        {
                            __state.rerollInteractable.SetEnabled(true);
                        }, false);
                    }
                    choicesSequencer.chosenReward = (SelectableCard)null;
                    int randomSeed = SaveManager.SaveFile.GetCurrentRandomSeed();
                    while ((UnityEngine.Object)choicesSequencer.chosenReward == (UnityEngine.Object)null)
                    {
                        List<CardChoice> choices;
                        if (choicesData.overrideChoices != null)
                        {
                            choices = choicesData.overrideChoices;
                        }
                        else
                        {
                            choices = choicesSequencer.choiceGenerator.GenerateChoices(choicesData, randomSeed);
                            randomSeed *= 2;
                        }
                        float x = (float)((double)(choices.Count - 1) * 0.5 * -1.5);
                        choicesSequencer.selectableCards = choicesSequencer.SpawnCards(choices.Count, choicesSequencer.transform, new Vector3(x, 5.01f, 0.0f), 1.5f);
                        for (int i = 0; i < choices.Count; ++i)
                        {
                            CardChoice choice = choices[i];
                            SelectableCard card = choicesSequencer.selectableCards[i];
                            card.gameObject.SetActive(true);
                            card.SetParticlesEnabled(true);
                            card.SetEnabled(false);
                            card.ChoiceInfo = choice;
                            if ((UnityEngine.Object)choice.CardInfo != (UnityEngine.Object)null)
                            {
                                card.Initialize(choice.CardInfo, new Action<SelectableCard>(choicesSequencer.OnRewardChosen), new Action<SelectableCard>(choicesSequencer.OnCardFlipped), true, new Action<SelectableCard>(((CardChoicesSequencer)choicesSequencer).OnCardInspected));
                            }
                            else if (choice.resourceType != ResourceType.None)
                                card.Initialize(choicesSequencer.GetCardbackTexture(choice), new Action<SelectableCard>(choicesSequencer.OnRewardChosen), new Action<SelectableCard>(choicesSequencer.OnCardFlipped), new Action<SelectableCard>(((CardChoicesSequencer)choicesSequencer).OnCardInspected));
                            else if (choice.tribe != Tribe.None)
                                card.Initialize(choicesSequencer.GetCardbackTexture(choice), new Action<SelectableCard>(choicesSequencer.OnRewardChosen), new Action<SelectableCard>(choicesSequencer.OnCardFlipped), new Action<SelectableCard>(((CardChoicesSequencer)choicesSequencer).OnCardInspected));
                            if (choice.isDeathcardChoice)
                                card.SetCardback(ResourceBank.Get<Texture>("Art/Cards/card_back_deathcard"));
                            card.SetFaceDown(true, true);
                            Vector3 position = card.transform.position;
                            card.transform.position = card.transform.position + Vector3.forward * 5f + new Vector3((float)((double)UnityEngine.Random.value * 1.0 - 0.5), 0.0f, 0.0f);
                            Tween.Position(card.transform, position, 0.3f, 0.0f, Tween.EaseInOut, Tween.LoopType.None, (Action)null, (Action)null, true);
                            Tween.Rotate(card.transform, new Vector3(0.0f, 0.0f, UnityEngine.Random.value * 1.5f), Space.Self, 0.4f, 0.0f, Tween.EaseOut, Tween.LoopType.None, (Action)null, (Action)null, true);
                            yield return (object)new WaitForSeconds(0.2f);
                            ParticleSystem componentInChildren = card.GetComponentInChildren<ParticleSystem>();
                            if ((UnityEngine.Object)componentInChildren != (UnityEngine.Object)null)
                            {
                                var emmision = componentInChildren.emission;
                                emmision.rateOverTime = 0f;
                            }

                        }
                        yield return (object)new WaitForSeconds(0.2f);
                        if (choicesData.choicesType == CardChoicesType.Cost && !ProgressionData.LearnedMechanic(MechanicsConcept.CostBasedCardChoice))
                        {
                            yield return (object)Singleton<TextDisplayer>.Instance.PlayDialogueEvent("TutorialCostBasedChoice", TextDisplayer.MessageAdvanceMode.Input, TextDisplayer.EventIntersectMode.Wait, (string[])null, (Action<DialogueEvent.Line>)null);
                            yield return (object)new WaitForSeconds(0.2f);
                        }
                        else if (choicesData.choicesType == CardChoicesType.Tribe && !ProgressionData.LearnedMechanic(MechanicsConcept.TribeBasedCardChoice))
                        {
                            yield return (object)Singleton<TextDisplayer>.Instance.PlayDialogueEvent("TutorialTribeBasedChoice", TextDisplayer.MessageAdvanceMode.Input, TextDisplayer.EventIntersectMode.Wait, (string[])null, (Action<DialogueEvent.Line>)null);
                            yield return (object)new WaitForSeconds(0.2f);
                        }
                        else if (choicesData.choicesType == CardChoicesType.Deathcard && !ProgressionData.LearnedMechanic(MechanicsConcept.DeathcardCardChoice))
                        {
                            yield return (object)Singleton<TextDisplayer>.Instance.PlayDialogueEvent("TutorialDeathcardChoice", TextDisplayer.MessageAdvanceMode.Input, TextDisplayer.EventIntersectMode.Wait, (string[])null, (Action<DialogueEvent.Line>)null);
                            yield return (object)new WaitForSeconds(0.2f);
                            ProgressionData.SetMechanicLearned(MechanicsConcept.DeathcardCardChoice);
                        }
                        choicesSequencer.SetCollidersEnabled(true);
                        choicesSequencer.choicesRerolled = false;
                        choicesSequencer.EnableViewDeck(choicesSequencer.viewControlMode, choicesSequencer.basePosition);
                        yield return (object)new WaitUntil(() => __state.chosenReward != null || __state.choicesRerolled);
                        choicesSequencer.DisableViewDeck();
                        choicesSequencer.CleanUpCards(true);
                        choices = (List<CardChoice>)null;
                    }
                    yield return (object)choicesSequencer.AddCardToDeckAndCleanUp(choicesSequencer.chosenReward);

                }
                else
                {
                    yield return enumerator;
                }
            }
        }


        [HarmonyPatch(typeof(StartScreenThemeSetter), "Start")]
        public class mainmenu1
        {
            public static void Prefix(ref StartScreenThemeSetter __instance)
            {
                var grimoratheme = __instance.themes[0];
                Color color = new Color();
                if (ColorUtility.TryParseHtmlString("0F2623", out color))
                {
                    grimoratheme.fillColor = color;
                }
                Texture2D tex3 = new Texture2D(2, 2);
                byte[] imgBytes3 = System.IO.File.ReadAllBytes(staticpath.Replace("act3tests.dll", "") + "\\Artwork\\background_grimora.png");
                tex3.LoadImage(imgBytes3);
                grimoratheme.bgSpriteWide = Sprite.Create(tex3, new Rect(0.0f, 0.0f, tex3.width, tex3.height), new Vector2(0.5f, 0.5f), 100.0f); ;
                grimoratheme.triggeringEvent = StoryEvent.PlayerDeletedArchivistFile;
                __instance.themes.Add(grimoratheme);
            }

            public static void Postfix(ref StartScreenThemeSetter __instance)
            {
                foreach (var VARIABLE in __instance.themes)
                {
                    Log.LogInfo(VARIABLE);
                }

                __instance.SetTheme(__instance.themes.Find(theme =>
                    theme.triggeringEvent == StoryEvent.PlayerDeletedArchivistFile));
                Log.LogInfo(__instance.themes.Find(theme =>
                    theme.triggeringEvent == StoryEvent.PlayerDeletedArchivistFile));
                __instance.StartCoroutine(createbutton());
            }
        }

        public static IEnumerator createbutton()
        {
            yield return new WaitUntil(() => Singleton<StartScreenController>.Instance.menu.gameObject.activeSelf);
            yield return new WaitForSeconds(1.8f);
            Log.LogWarning("test");
            var grimorabutton = Instantiate(GameObject.Find("MenuCard_Continue"));
            grimorabutton.transform.parent = GameObject.Find("CardRow").transform;
            byte[] imgBytes3 = System.IO.File.ReadAllBytes(staticpath.Replace("act3tests.dll", "") + "\\Artwork\\menucard_grimora.png");
            Texture2D tex3 = new Texture2D(2, 2);
            tex3.LoadImage(imgBytes3);
            grimorabutton.name = "MenuCard_Grimora";
            grimorabutton.gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(tex3, new Rect(0.0f, 0.0f, tex3.width, tex3.height), new Vector2(0.5f, 0.5f), 100.0f);
            var component = grimorabutton.GetComponent<MenuCard>();
            component.StartPosition = new Vector2(1.378f, 0);
            component.targetPosition = new Vector2(1.378f, 0);
            component.rotationCenter = new Vector2(1.378f, 0);
            component.menuAction = MenuAction.ReturnToStartMenu;
            component.titleText = "Start Grimora Mod";
            yield break;
        }


        [HarmonyPatch(typeof(MenuController), "OnCardReachedSlot")]
        public class mainmenu3
        {
            public static bool Prefix(ref MenuController __instance, MenuCard card, bool skipTween = false)
            {
                Log.LogInfo("it is run");
                if (card.titleText == "Start Grimora Mod")
                {
                    Log.LogInfo("it is true");
                    __instance.DoingCardTransition = false;
                    card.transform.parent = __instance.menuSlot.transform;
                    card.SetBorderColor(__instance.slottedBorderColor);
                    AudioController.Instance.PlaySound2D("crunch_short#1", MixerGroup.None, 0.6f, 0f, null, null,
                        null, null, false);
                    {
                        string option = "Yes. Erase save.";
                        string option2 = "No. Keep save.";
                        if (Localization.CurrentLanguage == Language.Spanish ||
                            Localization.CurrentLanguage == Language.French ||
                            Localization.CurrentLanguage == Language.BrazilianPortuguese ||
                            Localization.CurrentLanguage == Language.Italian)
                        {
                            option = "Yes";
                            option2 = "No";
                        }
                        __instance.Shake(0.01f, 0.25f);
                        __instance.StartCoroutine(__instance.TransitionToGame2());
                    }
                    return false;
                }

                return true;
            }
        }



        [HarmonyPatch(typeof(Part1BossOpponent), "BossDefeatedSequence")]
        public class kaycee3
        {
            public static void Prefix(out Part1BossOpponent __state, ref Part1BossOpponent __instance)
            {
                __state = __instance;
            }

            public static IEnumerator Postfix(IEnumerator enumerator, Part1BossOpponent __state)
            {
                if (SaveManager.saveFile.IsGrimora)
                {
                    GlitchOutAssetEffect.GlitchModel(GameObject.Find("Grimora_RightWrist").GetComponentsInChildren<Transform>().ToList().Find(transform1 => transform1.gameObject.name.Contains("Mask")).gameObject.transform, true, true);
                    AudioController.Instance.PlaySound2D("glitch_error", MixerGroup.TableObjectsSFX, 1f, 0f, null, null, null, null, false);
                    GrimoraAnimationController.Instance.SetHeadTrigger("hide_skull");
                    __state.DestroyScenery();
                    __state.SetSceneEffectsShown(false);
                    AudioController.Instance.StopAllLoops();
                    yield return new WaitForSeconds(0.75f);
                    __state.CleanUpBossBehaviours();
                    //CustomCoroutine.WaitThenExecute(1f, new Action(LeshyAnimationController.Instance.HideArms), false);
                    Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, true);
                    yield return new WaitForSeconds(0.8f);
                    yield return new WaitForSeconds(0.25f);
                    Singleton<TurnManager>.Instance.PostBattleSpecialNode = new ChooseRareCardNodeData();
                    yield break;
                }
                else
                {
                    yield return enumerator;
                }
            }
        }





        public class KayceeBossSequencer : Part1BossBattleSequencer
        {
            public override Opponent.Type BossType
            {
                get
                {
                    return Opponent.Type.ProspectorBoss;
                }
            }

            public override StoryEvent DefeatedStoryEvent
            {
                get
                {
                    return StoryEvent.TutorialRunCompleted;
                }
            }

            public override EncounterData BuildCustomEncounter(CardBattleNodeData nodeData)
            {
                var encouncterdata = new EncounterData();
                Opponent.Type KayceeBossz = (Opponent.Type)1001;
                encouncterdata.opponentType = KayceeBossz;
                return encouncterdata;
            }

            public override bool RespondsToUpkeep(bool playerUpkeep)
            {
                if (playerUpkeep)
                {
                    return true;
                }

                return false;
            }

            public int freezethis = 0;

            public override IEnumerator OnUpkeep(bool playerUpkeep)
            {
                freezethis++;
                if (this.freezethis == 3) 
                {
                    StartCoroutine(Singleton<TextDisplayer>.Instance.ShowUntilInput("Freeze!"));
                    foreach (var slot in Singleton<BoardManager3D>.Instance.PlayerSlotsCopy)
                    {
                        if (slot.Card != null)
                        {
                            slot.Card.Anim.StrongNegationEffect();
                            slot.Card.Anim.StrongNegationEffect();
                            slot.Card.Anim.StrongNegationEffect();
                            slot.Card.Anim.StrongNegationEffect();
                            slot.Card.Anim.StrongNegationEffect();
                            slot.Card.AddTemporaryMod(new CardModificationInfo(-1, 0));
                        }

                        freezethis = 0;
                    }
                }
                yield break;
            }
        }

        public class KayceeBoss : Part1BossOpponent
        {
            public override string DefeatedPlayerDialogue
            {
                get
                {
                    return "Youuuuuuu, painnnfulllll deaaathhh awaiiitttsss youuuuuuu!";
                }
            }

            public GameObject mask { get; set; }

            public override IEnumerator IntroSequence(EncounterData encounter)
            {
                StoryEventsData.SetEventCompleted(StoryEvent.FactoryConveyorBeltMoved);
                StoryEventsData.EraseEvent(StoryEvent.FactoryCuckooClockAppeared);
                StoryEventsData.EraseEvent(StoryEvent.PlayerDeletedArchivistFile);
                StoryEventsData.EraseEvent(StoryEvent.Part3PurchasedHoloBrush);

                var __state = this;

                var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
                blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>();


                var draugr = new EncounterBlueprintData.CardBlueprint();
                draugr.card = CardLoader.GetCardByName("ara_Draugr");
                var drownedsoul = new EncounterBlueprintData.CardBlueprint();
                drownedsoul.card = CardLoader.GetCardByName("ara_DrownedSoul");
                var revenant = new EncounterBlueprintData.CardBlueprint();
                revenant.card = CardLoader.GetCardByName("ara_Revenant");
                var skeleton = new EncounterBlueprintData.CardBlueprint();
                skeleton.card = CardLoader.GetCardByName("Skeleton");

                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { skeleton });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { drownedsoul });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { draugr });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { skeleton });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { revenant });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { skeleton, skeleton });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { skeleton, draugr });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { revenant });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { skeleton });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { drownedsoul });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { revenant });
                this.Blueprint = blueprint;
                List<List<CardInfo>> plan = EncounterBuilder.BuildOpponentTurnPlan(this.Blueprint, 0, false);
                this.ReplaceAndAppendTurnPlan(plan);
                yield return this.QueueNewCards(true, true);



                AudioController.Instance.FadeOutLoop(0.75f, Array.Empty<int>());
                RunState.CurrentMapRegion.FadeOutAmbientAudio();
                yield return new WaitForSeconds(0.25f);
                Singleton<ViewManager>.Instance.SwitchToView(View.BossSkull, false, true);
                yield return new WaitForSeconds(0.5f);
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(ResourceBank.Get<GameObject>("Prefabs/CardBattle/BossSkull"), __state.transform);
                __state.bossSkull = gameObject.GetComponent<BossSkull>();
                yield return new WaitForSeconds(0.166f);
                __state.SetSceneEffectsShown(true);
                AudioController.Instance.SetLoopAndPlay("boss_prospector_base", 0, true, true);
                AudioController.Instance.SetLoopAndPlay("boss_prospector_ambient", 1, true, true);
                __state.SpawnScenery("ForestTableEffects");
                yield return new WaitForSeconds(0.5f);
                AudioController.Instance.PlaySound2D("prospector_trees_enter", MixerGroup.TableObjectsSFX, 0.2f, 0f, null, null, null, null, false);
                yield return new WaitForSeconds(0.25f);
                Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
                yield return new WaitForSeconds(1.25f);
                yield return Singleton<TextDisplayer>.Instance.PlayDialogueEvent("ProspectorPreIntro", TextDisplayer.MessageAdvanceMode.Input, TextDisplayer.EventIntersectMode.Wait, null, null);
                yield return new WaitForSeconds(0.15f);
                GrimoraAnimationController.Instance.ShowBossSkull();
                GrimoraAnimationController.Instance.SetHeadTrigger("show_skull");
                Destroy(GameObject.Find("RoyalBossSkull"));
                mask = (GameObject)Instantiate(Resources.Load("Prefabs/Opponents/Leshy/Masks/MaskWoodcarver"));
                mask.transform.parent = GameObject.Find("Grimora_RightWrist").transform;
                mask.transform.localPosition = new Vector3(0, 0.19f, 0.065f);
                mask.transform.localRotation = Quaternion.Euler(0, 0, 260);

                yield return new WaitForSeconds(1.5f);
                yield return __state.FaceZoomSequence();
                yield return Singleton<TextDisplayer>.Instance.PlayDialogueEvent("ProspectorIntro", TextDisplayer.MessageAdvanceMode.Input, TextDisplayer.EventIntersectMode.Wait, null, null);
                Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
                Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                yield break; ;
            }

            public override IEnumerator StartNewPhaseSequence()
            {
                {
                    yield return this.ClearBoard();
                    if (Singleton<BoardManager3D>.Instance
                        .playerSlots.FindAll(slot => slot.Card != null).Count >= 1)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            var card = Singleton<BoardManager3D>.Instance
                                .playerSlots.FindAll(slot => slot.Card != null)[
                                    UnityEngine.Random.Range(0, Singleton<BoardManager3D>.Instance.PlayerSlotsCopy.FindAll(slot => slot.Card != null).Count)]
                                .Card;

                            card.SetIsOpponentCard(true);
                            card.transform.eulerAngles += new Vector3(0f, 0f, -180f);
                            yield return Singleton<BoardManager>.Instance.AssignCardToSlot(card, card.slot.opposingSlot, 0.25f, null, true);

                        }
                    }

                    var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
                    blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>();


                    var draugr = new EncounterBlueprintData.CardBlueprint();
                    draugr.card = CardLoader.GetCardByName("ara_Draugr");
                    var frankstein = new EncounterBlueprintData.CardBlueprint();
                    frankstein.card = CardLoader.GetCardByName("ara_Zombie");
                    var wolf = new EncounterBlueprintData.CardBlueprint();
                    wolf.card = CardLoader.GetCardByName("ara_Wolf");
                    var horseman = new EncounterBlueprintData.CardBlueprint();
                    horseman.card = CardLoader.GetCardByName("ara_HeadlessHorseman");

                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { draugr });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() {  });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { frankstein });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { frankstein });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { draugr });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { frankstein });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() {  });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { horseman });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { draugr });
                    this.Blueprint = blueprint;
                    List<List<CardInfo>> plan = EncounterBuilder.BuildOpponentTurnPlan(this.Blueprint, 0, false);
                    this.ReplaceAndAppendTurnPlan(plan);
                    yield return this.QueueNewCards(true, true);
                }
                yield break;
            }
        }


        public class DoggyBehavior : BossBehaviour
        {
            public IEnumerator OnOtherCardDie(CardSlot otherCard)
            {
                yield return Singleton<BoardManager3D>.Instance.CreateCardInSlot(CardLoader.GetCardByName("Bonehound"),
                    otherCard);
                yield break;
            }
        }


        public class DoggyBossSequencer : Part1BossBattleSequencer
        {


            public override Opponent.Type BossType
            {
                get
                {
                    return Opponent.Type.ProspectorBoss;
                }
            }

            public override StoryEvent DefeatedStoryEvent
            {
                get
                {
                    return StoryEvent.TutorialRunCompleted;
                }
            }

            public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
            {
                return true;
            }


            public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
            {
                if (Singleton<TurnManager>.Instance.Opponent.gameObject.GetComponent<DoggyBehavior>() != null)
                {
                    yield return Singleton<TurnManager>.Instance.Opponent.gameObject.GetComponent<DoggyBehavior>().OnOtherCardDie(deathSlot.opposingSlot);
                }
            }


            public override EncounterData BuildCustomEncounter(CardBattleNodeData nodeData)
            {
                var encouncterdata = new EncounterData();
                Opponent.Type DoggyBossz = (Opponent.Type)1002;
                encouncterdata.opponentType = DoggyBossz;
                return encouncterdata;
            }

            public override bool RespondsToUpkeep(bool playerUpkeep)
            {
                if (playerUpkeep)
                {
                    return true;
                }

                return false;
            }


            public override IEnumerator OnUpkeep(bool playerUpkeep)
            {
                yield break;
            }
        }

        public class DoggyBoss : Part1BossOpponent
        {
            public override string DefeatedPlayerDialogue
            {
                get
                {
                    return "My Dogs will enjoy your Bones!";
                }
            }

            public GameObject mask { get; set; }

            public override IEnumerator IntroSequence(EncounterData encounter)
            {
                var __state = this;

                StoryEventsData.SetEventCompleted(StoryEvent.FactoryCuckooClockAppeared);
                StoryEventsData.EraseEvent(StoryEvent.FactoryConveyorBeltMoved);
                StoryEventsData.EraseEvent(StoryEvent.PlayerDeletedArchivistFile);
                StoryEventsData.EraseEvent(StoryEvent.Part3PurchasedHoloBrush);


                var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
                blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>();


                var wolf = new EncounterBlueprintData.CardBlueprint();
                wolf.card = CardLoader.GetCardByName("ara_Wolf");
                var serpent = new EncounterBlueprintData.CardBlueprint();
                serpent.card = CardLoader.GetCardByName("ara_Serpent");
                var sarcophagus = new EncounterBlueprintData.CardBlueprint();
                sarcophagus.card = CardLoader.GetCardByName("ara_sarcophagus");
                var skeleton = new EncounterBlueprintData.CardBlueprint();
                skeleton.card = CardLoader.GetCardByName("Skeleton");

                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { serpent });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { skeleton, serpent });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { sarcophagus, serpent });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { skeleton, serpent });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { serpent });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { wolf });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { serpent, serpent });
                this.Blueprint = blueprint;
                List<List<CardInfo>> plan = EncounterBuilder.BuildOpponentTurnPlan(this.Blueprint, 0, false);
                this.ReplaceAndAppendTurnPlan(plan);
                yield return this.QueueNewCards(true, true);



                AudioController.Instance.FadeOutLoop(0.75f, Array.Empty<int>());
                RunState.CurrentMapRegion.FadeOutAmbientAudio();
                yield return new WaitForSeconds(0.25f);
                Singleton<ViewManager>.Instance.SwitchToView(View.BossSkull, false, true);
                yield return new WaitForSeconds(0.5f);
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(ResourceBank.Get<GameObject>("Prefabs/CardBattle/BossSkull"), __state.transform);
                __state.bossSkull = gameObject.GetComponent<BossSkull>();
                yield return new WaitForSeconds(0.166f);
                __state.SetSceneEffectsShown(true);
                AudioController.Instance.SetLoopAndPlay("boss_prospector_base", 0, true, true);
                AudioController.Instance.SetLoopAndPlay("boss_prospector_ambient", 1, true, true);
                __state.SpawnScenery("ForestTableEffects");
                yield return new WaitForSeconds(0.5f);
                AudioController.Instance.PlaySound2D("prospector_trees_enter", MixerGroup.TableObjectsSFX, 0.2f, 0f, null, null, null, null, false);
                yield return new WaitForSeconds(0.25f);
                Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
                yield return new WaitForSeconds(1.25f);
                yield return Singleton<TextDisplayer>.Instance.PlayDialogueEvent("ProspectorPreIntro", TextDisplayer.MessageAdvanceMode.Input, TextDisplayer.EventIntersectMode.Wait, null, null);
                yield return new WaitForSeconds(0.15f);
                GrimoraAnimationController.Instance.ShowBossSkull();
                GrimoraAnimationController.Instance.SetHeadTrigger("show_skull");
                Destroy(GameObject.Find("RoyalBossSkull"));
                mask = (GameObject)Instantiate(Resources.Load("Prefabs/Opponents/Leshy/Masks/MaskWoodcarver"));
                mask.transform.parent = GameObject.Find("Grimora_RightWrist").transform;
                mask.transform.localPosition = new Vector3(0, 0.19f, 0.065f);
                mask.transform.localRotation = Quaternion.Euler(0, 0, 260);

                yield return new WaitForSeconds(1.5f);
                yield return __state.FaceZoomSequence();
                yield return Singleton<TextDisplayer>.Instance.PlayDialogueEvent("ProspectorIntro", TextDisplayer.MessageAdvanceMode.Input, TextDisplayer.EventIntersectMode.Wait, null, null);
                Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
                Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                yield break; ;
            }

            public override IEnumerator StartNewPhaseSequence()
            {
                {
                    base.InstantiateBossBehaviour<DoggyBehavior>();


                    var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
                    blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>();


                    var wolf = new EncounterBlueprintData.CardBlueprint();
                    wolf.card = CardLoader.GetCardByName("ara_Wolf");
                    var serpent = new EncounterBlueprintData.CardBlueprint();
                    serpent.card = CardLoader.GetCardByName("ara_Serpent");
                    var sarcophagus = new EncounterBlueprintData.CardBlueprint();
                    sarcophagus.card = CardLoader.GetCardByName("ara_sarcophagus");
                    var skeleton = new EncounterBlueprintData.CardBlueprint();
                    skeleton.card = CardLoader.GetCardByName("Skeleton");

                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { skeleton });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { wolf });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { serpent });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { serpent });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { serpent });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { wolf });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { serpent, serpent });
                    this.Blueprint = blueprint;
                    List<List<CardInfo>> plan = EncounterBuilder.BuildOpponentTurnPlan(this.Blueprint, 0, false);
                    this.ReplaceAndAppendTurnPlan(plan);
                    yield return this.QueueNewCards(true, true);
                }
                yield break;
            }
        }




        public class RoyalBossSequencer : Part1BossBattleSequencer
        {

            public override Opponent.Type BossType
            {
                get
                {
                    return Opponent.Type.ProspectorBoss;
                }
            }

            public override StoryEvent DefeatedStoryEvent
            {
                get
                {
                    return StoryEvent.TutorialRunCompleted;
                }
            }


            public override EncounterData BuildCustomEncounter(CardBattleNodeData nodeData)
            {
                var encouncterdata = new EncounterData();
                Opponent.Type RoyalBossz = (Opponent.Type)1003;
                encouncterdata.opponentType = RoyalBossz;
                return encouncterdata;
            }

            public override bool RespondsToUpkeep(bool playerUpkeep)
            {
                if (playerUpkeep)
                {
                    return true;
                }

                return false;
            }


            public override IEnumerator OnUpkeep(bool playerUpkeep)
            {
                if (Singleton<BoardManager3D>.Instance.playerSlots.FindAll(slot => slot.Card != null).Count >= 1)
                {
                    var card = Singleton<BoardManager3D>.Instance.playerSlots.FindAll(slot => slot.Card != null)[
                            UnityEngine.Random.Range(0,
                                Singleton<BoardManager3D>.Instance.playerSlots.FindAll(slot => slot.Card != null)
                                    .Count)]
                        .Card;
                    card.AddTemporaryMod(new CardModificationInfo(Ability.Submerge));
                }
                yield break;
            }
        }

        public class RoyalBoss : Part1BossOpponent
        {
            public override string DefeatedPlayerDialogue
            {
                get
                {
                    return "Arrg!Walk off a Plank yee dirty Scallywag!!";
                }
            }

            public GameObject mask { get; set; }

            public override IEnumerator IntroSequence(EncounterData encounter)
            {


                StoryEventsData.SetEventCompleted(StoryEvent.FactoryConveyorBeltMoved);
                StoryEventsData.EraseEvent(StoryEvent.FactoryCuckooClockAppeared);
                StoryEventsData.EraseEvent(StoryEvent.PlayerDeletedArchivistFile);
                StoryEventsData.EraseEvent(StoryEvent.Part3PurchasedHoloBrush);

                var __state = this;
                var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
                blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>();


                var pirateship = new EncounterBlueprintData.CardBlueprint();
                pirateship.card = CardLoader.GetCardByName("ara_Pirateship");
                var boneprince = new EncounterBlueprintData.CardBlueprint();
                boneprince.card = CardLoader.GetCardByName("ara_BonePrince");
                var skeleton = new EncounterBlueprintData.CardBlueprint();
                skeleton.card = CardLoader.GetCardByName("Skeleton");
                var revenant = new EncounterBlueprintData.CardBlueprint();
                revenant.card = CardLoader.GetCardByName("ara_Revenant");

                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { skeleton });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() {  });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { boneprince });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { skeleton });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() {  });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { pirateship });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { revenant });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { boneprince });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { revenant });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { pirateship });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { boneprince });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { boneprince });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { revenant });
                this.Blueprint = blueprint;
                List<List<CardInfo>> plan = EncounterBuilder.BuildOpponentTurnPlan(this.Blueprint, 0, false);
                this.ReplaceAndAppendTurnPlan(plan);
                yield return this.QueueNewCards(true, true);





                StoryEventsData.SetEventCompleted(StoryEvent.Part3PurchasedHoloBrush);
                StoryEventsData.EraseEvent(StoryEvent.FactoryConveyorBeltMoved);
                StoryEventsData.EraseEvent(StoryEvent.PlayerDeletedArchivistFile);
                StoryEventsData.EraseEvent(StoryEvent.FactoryCuckooClockAppeared);






                AudioController.Instance.FadeOutLoop(0.75f, Array.Empty<int>());
                RunState.CurrentMapRegion.FadeOutAmbientAudio();
                yield return new WaitForSeconds(0.25f);
                Singleton<ViewManager>.Instance.SwitchToView(View.BossSkull, false, true);
                yield return new WaitForSeconds(0.5f);
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(ResourceBank.Get<GameObject>("Prefabs/CardBattle/BossSkull"), __state.transform);
                __state.bossSkull = gameObject.GetComponent<BossSkull>();
                yield return new WaitForSeconds(0.166f);
                __state.SetSceneEffectsShown(true);
                AudioController.Instance.SetLoopAndPlay("boss_prospector_base", 0, true, true);
                AudioController.Instance.SetLoopAndPlay("boss_prospector_ambient", 1, true, true);
                __state.SpawnScenery("ForestTableEffects");
                yield return new WaitForSeconds(0.5f);
                AudioController.Instance.PlaySound2D("prospector_trees_enter", MixerGroup.TableObjectsSFX, 0.2f, 0f, null, null, null, null, false);
                yield return new WaitForSeconds(0.25f);
                Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
                yield return new WaitForSeconds(1.25f);
                yield return Singleton<TextDisplayer>.Instance.PlayDialogueEvent("ProspectorPreIntro", TextDisplayer.MessageAdvanceMode.Input, TextDisplayer.EventIntersectMode.Wait, null, null);
                yield return new WaitForSeconds(0.15f);
                GrimoraAnimationController.Instance.ShowBossSkull();
                GrimoraAnimationController.Instance.SetHeadTrigger("show_skull");
                Destroy(GameObject.Find("RoyalBossSkull"));
                mask = (GameObject)Instantiate(Resources.Load("Prefabs/Opponents/Leshy/Masks/MaskWoodcarver"));
                mask.transform.parent = GameObject.Find("Grimora_RightWrist").transform;
                mask.transform.localPosition = new Vector3(0, 0.19f, 0.065f);
                mask.transform.localRotation = Quaternion.Euler(0, 0, 260);

                yield return new WaitForSeconds(1.5f);
                yield return __state.FaceZoomSequence();
                yield return Singleton<TextDisplayer>.Instance.PlayDialogueEvent("ProspectorIntro", TextDisplayer.MessageAdvanceMode.Input, TextDisplayer.EventIntersectMode.Wait, null, null);
                Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
                Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                yield break; ;
            }

            public override IEnumerator StartNewPhaseSequence()
            {
                if (Singleton<BoardManager3D>.Instance.playerSlots.FindAll(slot => slot.Card != null).Count >= 1)
                {
                    foreach (var slot in Singleton<BoardManager3D>.Instance.playerSlots.FindAll(slot => slot.Card != null))
                    {
                        slot.Card.Anim.PlayDeathAnimation();
                    }

                    var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
                    blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>();


                    var pirateship = new EncounterBlueprintData.CardBlueprint();
                    pirateship.card = CardLoader.GetCardByName("ara_Pirateship");
                    var boneprince = new EncounterBlueprintData.CardBlueprint();
                    boneprince.card = CardLoader.GetCardByName("ara_BonePrince");
                    var skeleton = new EncounterBlueprintData.CardBlueprint();
                    skeleton.card = CardLoader.GetCardByName("Skeleton");
                    var revenant = new EncounterBlueprintData.CardBlueprint();
                    revenant.card = CardLoader.GetCardByName("ara_Revenant");

                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { boneprince });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { boneprince });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { pirateship });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { skeleton });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { revenant });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { pirateship });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { boneprince, skeleton });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { boneprince, boneprince });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { boneprince });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { revenant });
                    this.Blueprint = blueprint;
                    List<List<CardInfo>> plan = EncounterBuilder.BuildOpponentTurnPlan(this.Blueprint, 0, false);
                    this.ReplaceAndAppendTurnPlan(plan);
                    yield return this.QueueNewCards(true, true);
                }
                yield break;

            }
        }






        public class GrimoraBossSequencer : Part1BossBattleSequencer
        {
            public override Opponent.Type BossType
            {
                get
                {
                    return Opponent.Type.ProspectorBoss;
                }
            }

            public override StoryEvent DefeatedStoryEvent
            {
                get
                {
                    return StoryEvent.TutorialRunCompleted;
                }
            }


            public override EncounterData BuildCustomEncounter(CardBattleNodeData nodeData)
            {
                var encouncterdata = new EncounterData();
                Opponent.Type GrimoraBossz = (Opponent.Type)1004;
                encouncterdata.opponentType = GrimoraBossz;
                return encouncterdata;
            }

            public override bool RespondsToUpkeep(bool playerUpkeep)
            {
                if (playerUpkeep)
                {
                    return true;
                }

                return false;
            }

            public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
            {
                if (!card.OpponentCard)
                {
                    return true;
                }

                return false;
            }


            public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
            {
                yield return Singleton<TurnManager>.Instance.Opponent.QueueCard(card.Info, Singleton<BoardManager3D>.Instance.opponentSlots[UnityEngine.Random.Range(0, 3)]);
            }

            public override IEnumerator OnUpkeep(bool playerUpkeep)
            {
                if (Singleton<BoardManager3D>.Instance.playerSlots.FindAll(slot => slot.Card != null).Count >= 1)
                {
                    var card = Singleton<BoardManager3D>.Instance.playerSlots.FindAll(slot => slot.Card != null)[
                            UnityEngine.Random.Range(0,
                                Singleton<BoardManager3D>.Instance.playerSlots.FindAll(slot => slot.Card != null)
                                    .Count)]
                        .Card;
                }
                yield break;
            }
        }

        public class GrimoraBoss : Part1BossOpponent
        {
            public override string DefeatedPlayerDialogue
            {
                get
                {
                    return "Thank you!";
                }
            }

            public override int StartingLives
            {
                get
                {
                    return 3;
                }
            }




            public GameObject mask { get; set; }

            public override IEnumerator IntroSequence(EncounterData encounter)
            {
                var __state = this;
                StoryEventsData.SetEventCompleted(StoryEvent.PlayerDeletedArchivistFile);
                StoryEventsData.EraseEvent(StoryEvent.FactoryConveyorBeltMoved);
                StoryEventsData.EraseEvent(StoryEvent.FactoryCuckooClockAppeared);
                StoryEventsData.EraseEvent(StoryEvent.Part3PurchasedHoloBrush);


                var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
                this.Blueprint = blueprint;
                List<List<CardInfo>> plan = EncounterBuilder.BuildOpponentTurnPlan(this.Blueprint, 0, false);
                this.ReplaceAndAppendTurnPlan(plan);
                yield return this.QueueNewCards(true, true);



                AudioController.Instance.FadeOutLoop(0.75f, Array.Empty<int>());
                RunState.CurrentMapRegion.FadeOutAmbientAudio();
                yield return new WaitForSeconds(0.25f);
                Singleton<ViewManager>.Instance.SwitchToView(View.BossSkull, false, true);
                yield return new WaitForSeconds(0.5f);
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(ResourceBank.Get<GameObject>("Prefabs/CardBattle/BossSkull"), __state.transform);
                __state.bossSkull = gameObject.GetComponent<BossSkull>();
                yield return new WaitForSeconds(0.166f);
                __state.SetSceneEffectsShown(true);
                AudioController.Instance.SetLoopAndPlay("boss_prospector_base", 0, true, true);
                AudioController.Instance.SetLoopAndPlay("boss_prospector_ambient", 1, true, true);
                __state.SpawnScenery("ForestTableEffects");
                yield return new WaitForSeconds(0.5f);
                AudioController.Instance.PlaySound2D("prospector_trees_enter", MixerGroup.TableObjectsSFX, 0.2f, 0f, null, null, null, null, false);
                yield return new WaitForSeconds(0.25f);
                Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
                yield return new WaitForSeconds(1.25f);
                yield return Singleton<TextDisplayer>.Instance.PlayDialogueEvent("ProspectorPreIntro", TextDisplayer.MessageAdvanceMode.Input, TextDisplayer.EventIntersectMode.Wait, null, null);
                yield return new WaitForSeconds(0.15f);
                GrimoraAnimationController.Instance.ShowBossSkull();
                GrimoraAnimationController.Instance.SetHeadTrigger("show_skull");
                Destroy(GameObject.Find("RoyalBossSkull"));
                mask = (GameObject)Instantiate(Resources.Load("Prefabs/Opponents/Leshy/Masks/MaskWoodcarver"));
                mask.transform.parent = GameObject.Find("Grimora_RightWrist").transform;
                mask.transform.localPosition = new Vector3(0, 0.19f, 0.065f);
                mask.transform.localRotation = Quaternion.Euler(0, 0, 260);

                yield return new WaitForSeconds(1.5f);
                yield return __state.FaceZoomSequence();
                yield return Singleton<TextDisplayer>.Instance.PlayDialogueEvent("ProspectorIntro", TextDisplayer.MessageAdvanceMode.Input, TextDisplayer.EventIntersectMode.Wait, null, null);
                Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
                Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                yield break; ;
            }

            public override IEnumerator StartNewPhaseSequence()
            {
                if (this.NumLives == 2)
                {
                    yield return Singleton<BoardManager3D>.Instance.CreateCardInSlot(CardLoader.GetCardByName("ara_Bonelord"), Singleton<BoardManager3D>.Instance.opponentSlots[0]);
                    using (List<CardSlot>.Enumerator enumerator = Singleton<BoardManager>.Instance.OpponentSlotsCopy.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            CardSlot cardSlot = enumerator.Current;
                            cardSlot.Card = Singleton<BoardManager3D>.Instance.opponentSlots[0].Card;
                        }
                    }
                }
                if (this.NumLives == 1)
                {
                    if (Singleton<BoardManager3D>.Instance.playerSlots.FindAll(slot => slot.Card != null).Count >= 1)
                    {
                        foreach (var slot in Singleton<BoardManager3D>.Instance.playerSlots.FindAll(slot => slot.Card != null))
                        {
                            slot.Card.AddTemporaryMod(new CardModificationInfo(-slot.Card.Attack + 1, -slot.Card.Health + 1));
                        }
                    }
                }
                yield break;
            }
        }








        [HarmonyPatch(typeof(Opponent), "SpawnOpponent")]
        public class fixbosseszxu
        {
            public static bool Prefix(EncounterData encounterData, ref Opponent __result)
            {
                Opponent.Type KayceeBossz = (Opponent.Type)1001;
                Opponent.Type DoggyBossz = (Opponent.Type)1002;
                Opponent.Type RoyalBossz = (Opponent.Type)1003;
                Opponent.Type GrimoraBossz = (Opponent.Type)1004;
                if (encounterData.opponentType == GrimoraBossz)
                {
                    GameObject gameObject = new GameObject();
                    gameObject.name = "Opponent";
                    Opponent.Type opponentType = (!ProgressionData.LearnedMechanic(MechanicsConcept.OpponentQueue)) ? Opponent.Type.NoPlayQueue : encounterData.opponentType;
                    Opponent opponent;
                    opponent = gameObject.AddComponent<GrimoraBoss>();
                    string text = encounterData.aiId;
                    if (string.IsNullOrEmpty(text))
                    {
                        text = "AI";
                    }
                    opponent.AI = (Activator.CreateInstance(CustomType.GetType("DiskCardGame", text)) as AI);
                    opponent.NumLives = opponent.StartingLives;
                    opponent.OpponentType = opponentType;
                    opponent.TurnPlan = opponent.ModifyTurnPlan(encounterData.opponentTurnPlan);
                    opponent.Blueprint = encounterData.Blueprint;
                    opponent.Difficulty = encounterData.Difficulty;
                    opponent.ExtraTurnsToSurrender = SeededRandom.Range(3, 4, SaveManager.SaveFile.GetCurrentRandomSeed());
                    __result = opponent;
                    return false;
                }
                if (encounterData.opponentType == RoyalBossz)
                {
                    GameObject gameObject = new GameObject();
                    gameObject.name = "Opponent";
                    Opponent.Type opponentType = (!ProgressionData.LearnedMechanic(MechanicsConcept.OpponentQueue)) ? Opponent.Type.NoPlayQueue : encounterData.opponentType;
                    Opponent opponent;
                    opponent = gameObject.AddComponent<RoyalBoss>();
                    string text = encounterData.aiId;
                    if (string.IsNullOrEmpty(text))
                    {
                        text = "AI";
                    }
                    opponent.AI = (Activator.CreateInstance(CustomType.GetType("DiskCardGame", text)) as AI);
                    opponent.NumLives = opponent.StartingLives;
                    opponent.OpponentType = opponentType;
                    opponent.TurnPlan = opponent.ModifyTurnPlan(encounterData.opponentTurnPlan);
                    opponent.Blueprint = encounterData.Blueprint;
                    opponent.Difficulty = encounterData.Difficulty;
                    opponent.ExtraTurnsToSurrender = SeededRandom.Range(3, 4, SaveManager.SaveFile.GetCurrentRandomSeed());
                    __result = opponent;
                    return false;
                }
                if (encounterData.opponentType == KayceeBossz)
                {
                    GameObject gameObject = new GameObject();
                    gameObject.name = "Opponent";
                    Opponent.Type opponentType = (!ProgressionData.LearnedMechanic(MechanicsConcept.OpponentQueue)) ? Opponent.Type.NoPlayQueue : encounterData.opponentType;
                    Opponent opponent;
                    opponent = gameObject.AddComponent<KayceeBoss>();
                    string text = encounterData.aiId;
                    if (string.IsNullOrEmpty(text))
                    {
                        text = "AI";
                    }
                    opponent.AI = (Activator.CreateInstance(CustomType.GetType("DiskCardGame", text)) as AI);
                    opponent.NumLives = opponent.StartingLives;
                    opponent.OpponentType = opponentType;
                    opponent.TurnPlan = opponent.ModifyTurnPlan(encounterData.opponentTurnPlan);
                    opponent.Blueprint = encounterData.Blueprint;
                    opponent.Difficulty = encounterData.Difficulty;
                    opponent.ExtraTurnsToSurrender = SeededRandom.Range(3, 4, SaveManager.SaveFile.GetCurrentRandomSeed());
                    __result = opponent;
                    return false;
                }
                if (encounterData.opponentType == DoggyBossz)
                {
                    GameObject gameObject = new GameObject();
                    gameObject.name = "Opponent";
                    Opponent.Type opponentType = (!ProgressionData.LearnedMechanic(MechanicsConcept.OpponentQueue)) ? Opponent.Type.NoPlayQueue : encounterData.opponentType;
                    Opponent opponent;
                    opponent = gameObject.AddComponent<DoggyBoss>();
                    string text = encounterData.aiId;
                    if (string.IsNullOrEmpty(text))
                    {
                        text = "AI";
                    }
                    opponent.AI = (Activator.CreateInstance(CustomType.GetType("DiskCardGame", text)) as AI);
                    opponent.NumLives = opponent.StartingLives;
                    opponent.OpponentType = opponentType;
                    opponent.TurnPlan = opponent.ModifyTurnPlan(encounterData.opponentTurnPlan);
                    opponent.Blueprint = encounterData.Blueprint;
                    opponent.Difficulty = encounterData.Difficulty;
                    opponent.ExtraTurnsToSurrender = SeededRandom.Range(3, 4, SaveManager.SaveFile.GetCurrentRandomSeed());
                    __result = opponent;
                    return false;
                }

                return true;
            }
        }


        [HarmonyPatch(typeof(TurnManager), "UpdateSpecialSequencer")]
        public class fixbosseszx
        {
            public static bool Prefix(ref TurnManager __instance, string specialBattleId)
            {
                if (SaveManager.saveFile.IsGrimora)
                {
                    UnityEngine.Object.Destroy(__instance.SpecialSequencer);
                    __instance.SpecialSequencer = null;
                    if (!string.IsNullOrEmpty(specialBattleId))
                    {
                        Type type = specialBattleId == "KayceeBoss" ? typeof(KayceeBossSequencer) : specialBattleId == "DoggyBoss" ? typeof(DoggyBossSequencer) : specialBattleId == "RoyalBoss" ? typeof(RoyalBossSequencer) : specialBattleId == "GrimoraBoss" ? typeof(GrimoraBossSequencer) : typeof(ChessboardEnemyBattleSequencer);
                        //if (specialBattleId == "KayceeBoss")
                        {
                        }
                        __instance.SpecialSequencer = (__instance.gameObject.AddComponent(type) as SpecialBattleSequencer);
                    }

                    return false;
                }

                return true;
            }
        }


        [HarmonyPatch(typeof(ChessboardMap), "UnrollingSequence")]
        public class zez
        {
            public static void Prefix(ref ChessboardMap __instance, float unrollSpeed)
            {
                var defaulttombstone = (GameObject)Resources.Load("prefabs\\map\\chessboardmap\\Chessboard_Tombstone_3");
                var enemypiece = (GameObject)Resources.Load("prefabs\\map\\chessboardmap\\ChessboardEnemyPiece");
                var bosspiece = (GameObject)Resources.Load("prefabs\\map\\chessboardmap\\BossFigurine");
                var chestpiece = (GameObject)Resources.Load("prefabs\\map\\chessboardmap\\ChessboardChestPiece");
                foreach (var VARIABLE in __instance.pieces.FindAll(piece => !piece.gameObject.GetComponent<ChessboardEnemyPiece>()))
                {
                    Destroy(VARIABLE.gameObject);
                    __instance.pieces.Remove(VARIABLE);
                }
                //Singleton<ChessboardEnemyManager>.Instance.enemyPieces=new List<ChessboardEnemyPiece>();

                if (StoryEventsData.EventCompleted(StoryEvent.PlayerDeletedArchivistFile))
                {
                    {
                        //__instance.pieces.FindAll(piece => piece.gameObject.GetComponent<ChessboardEnemyPiece>())[0].gameObject.GetComponent<MeshFilter>().mesh=allAssets[0] as Mesh;

                        __instance.pieces.FindAll(piece => piece.gameObject.GetComponent<ChessboardEnemyPiece>())[0].gameObject.GetComponent<ChessboardEnemyPiece>().gridXPos = 2;
                        __instance.pieces.FindAll(piece => piece.gameObject.GetComponent<ChessboardEnemyPiece>())[0].gameObject.GetComponent<ChessboardEnemyPiece>().gridYPos = 7;
                        __instance.pieces.FindAll(piece => piece.gameObject.GetComponent<ChessboardEnemyPiece>())[0].gameObject.GetComponent<ChessboardEnemyPiece>().GoalPosX = 2;
                        __instance.pieces.FindAll(piece => piece.gameObject.GetComponent<ChessboardEnemyPiece>())[0].gameObject.GetComponent<ChessboardEnemyPiece>().GoalPosY = 7;
                        __instance.pieces.FindAll(piece => piece.gameObject.GetComponent<ChessboardEnemyPiece>())[1].gameObject.GetComponent<ChessboardEnemyPiece>().gridXPos = 3;
                        __instance.pieces.FindAll(piece => piece.gameObject.GetComponent<ChessboardEnemyPiece>())[1].gameObject.GetComponent<ChessboardEnemyPiece>().gridYPos = 4;
                        __instance.pieces.FindAll(piece => piece.gameObject.GetComponent<ChessboardEnemyPiece>())[1].gameObject.GetComponent<ChessboardEnemyPiece>().GoalPosX = 3;
                        __instance.pieces.FindAll(piece => piece.gameObject.GetComponent<ChessboardEnemyPiece>())[1].gameObject.GetComponent<ChessboardEnemyPiece>().GoalPosY = 4;
                        {
                            __instance.pieces.FindAll(piece => piece.gameObject.GetComponent<ChessboardEnemyPiece>())[2].gameObject.GetComponent<ChessboardEnemyPiece>().specialEncounterId = "KayceeBoss";

                            __instance.pieces.FindAll(piece => piece.gameObject.GetComponent<ChessboardEnemyPiece>())[2]
                                    .gameObject.GetComponent<ChessboardEnemyPiece>().NodeData =
                                __instance.pieces.FindAll(
                                        piece => piece.gameObject.GetComponent<ChessboardEnemyPiece>())[2]
                                    .gameObject.GetComponent<ChessboardEnemyPiece>().CreateCardBattleData();

                            __instance.pieces.FindAll(piece => piece.gameObject.GetComponent<ChessboardEnemyPiece>())[2]
                                .gridYPos = 0;
                            __instance.pieces.FindAll(piece => piece.gameObject.GetComponent<ChessboardEnemyPiece>())[2]
                                .gridXPos = 0;
                        }

                    }
                    {
                        {
                            var chest = Instantiate(chestpiece);
                            chest.name = "SpecialPiece";
                            var piece = chest.GetComponent<ChessboardChestPiece>();
                            piece.gridYPos = 7;
                            piece.gridXPos = 0;
                            var nodeData = piece.NodeData;
                            var randomvalue = UnityEngine.Random.Range(0, 4);
                            if (randomvalue != 2)
                            {
                                piece.NodeData = new CardChoicesNodeData();
                            }
                            else
                            {
                                piece.NodeData = new ChooseRareCardNodeData();
                            }
                            //nodeData.gridX = piece.gridXPos;
                            //nodeData.gridY = piece.gridYPos;
                            //nodeData.id = MapGenerator.GetNewID();
                            //piece.MapNode.Data = nodeData;

                            piece.saveId = piece.gridXPos * 10 + piece.gridYPos * 1000;
                            Log.LogInfo(piece.saveId);
                            __instance.pieces.Add(piece);
                        }
                        {
                            var chest = Instantiate(chestpiece);
                            chest.name = "SpecialPiece";
                            var piece = chest.GetComponent<ChessboardChestPiece>();
                            piece.gridYPos = 6;
                            piece.gridXPos = 6;
                            var nodeData = piece.NodeData;
                            var randomvalue = UnityEngine.Random.Range(0, 4);
                            if (randomvalue != 2)
                            {
                                piece.NodeData = new CardChoicesNodeData();
                            }
                            else
                            {
                                piece.NodeData = new ChooseRareCardNodeData();
                            }
                            //nodeData.gridX = piece.gridXPos;
                            //nodeData.gridY = piece.gridYPos;
                            //nodeData.id = MapGenerator.GetNewID();
                            ////piece.MapNode.Data = nodeData;
                            piece.saveId = piece.gridXPos * 10 + piece.gridYPos * 1000;
                            __instance.pieces.Add(piece);
                        }
                        {
                            var chest = Instantiate(chestpiece);
                            chest.name = "SpecialPiece";
                            var piece = chest.GetComponent<ChessboardChestPiece>();
                            piece.gridYPos = 3;
                            piece.gridXPos = 5;
                            var nodeData = piece.NodeData;
                            var randomvalue = UnityEngine.Random.Range(0, 4);
                            if (randomvalue != 2)
                            {
                                piece.NodeData = new CardChoicesNodeData();
                            }
                            else
                            {
                                piece.NodeData = new ChooseRareCardNodeData();
                            }
                            //nodeData.gridX = piece.gridXPos;
                            //nodeData.gridY = piece.gridYPos;
                            //nodeData.id = MapGenerator.GetNewID();
                            //piece.MapNode.Data = nodeData;
                            piece.saveId = piece.gridXPos * 10 + piece.gridYPos * 1000;
                            __instance.pieces.Add(piece);
                        }
                        {
                            var chest = Instantiate(chestpiece);
                            chest.name = "SpecialPiece";
                            var piece = chest.GetComponent<ChessboardChestPiece>();
                            piece.gridYPos = 3;
                            piece.gridXPos = 7;
                            var nodeData = piece.NodeData;
                            var randomvalue = UnityEngine.Random.Range(0, 4);
                            if (randomvalue != 2)
                            {
                                piece.NodeData = new CardChoicesNodeData();
                            }
                            else
                            {
                                piece.NodeData = new ChooseRareCardNodeData();
                            }
                            //nodeData.gridX = piece.gridXPos;
                            //nodeData.gridY = piece.gridYPos;
                            //nodeData.id = MapGenerator.GetNewID();
                            //piece.MapNode.Data = nodeData;
                            piece.saveId = piece.gridXPos * 10 + piece.gridYPos * 1000;
                            __instance.pieces.Add(piece);
                        }







                        {
                            {
                                if (ChessboardNavGrid.instance.zones[0, 2].GetComponent<ChessboardMapNode>()
                                    .OccupyingPiece == null)
                                {
                                    var enemy = Instantiate(enemypiece);
                                    var piece = enemy.GetComponent<ChessboardEnemyPiece>();
                                    piece.GoalPosX = 0;
                                    piece.GoalPosY = 2;
                                    piece.gridXPos = 0;
                                    piece.gridYPos = 2;
                                    piece.saveId = piece.gridXPos * 10 + piece.gridYPos * 1000;
                                    Log.LogInfo(piece.saveId);
                                    __instance.pieces.Add(piece);
                                }

                            }
                            {
                                if (ChessboardNavGrid.instance.zones[2, 1].GetComponent<ChessboardMapNode>()
                                    .OccupyingPiece == null)
                                {
                                    var enemy = Instantiate(enemypiece);
                                    var piece = enemy.GetComponent<ChessboardEnemyPiece>();
                                    piece.GoalPosX = 2;
                                    piece.GoalPosY = 1;
                                    piece.gridXPos = 2;
                                    piece.gridYPos = 1;
                                    piece.saveId = piece.gridXPos * 10 + piece.gridYPos * 1000;
                                    __instance.pieces.Add(piece);
                                }



                            }
                            {
                                if (ChessboardNavGrid.instance.zones[5, 1].GetComponent<ChessboardMapNode>()
                                    .OccupyingPiece == null)
                                {
                                    var enemy = Instantiate(enemypiece);
                                    var piece = enemy.GetComponent<ChessboardEnemyPiece>();
                                    piece.GoalPosX = 5;
                                    piece.GoalPosY = 1;
                                    piece.gridXPos = 5;
                                    piece.gridYPos = 1;

                                    var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
                                    blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>();


                                    var pirateship = new EncounterBlueprintData.CardBlueprint();
                                    pirateship.card = CardLoader.GetCardByName("ara_Draugr");
                                    var boneprince = new EncounterBlueprintData.CardBlueprint();
                                    boneprince.card = CardLoader.GetCardByName("ara_Draugr");
                                    var zombie = new EncounterBlueprintData.CardBlueprint();
                                    zombie.card = CardLoader.GetCardByName("ara_Draugr");

                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { pirateship });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { boneprince, zombie });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { zombie, zombie });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { pirateship });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { zombie, zombie });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { boneprince });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { pirateship });
                                    piece.blueprint = blueprint;
                                    piece.saveId = piece.gridXPos * 10 + piece.gridYPos * 1000;
                                    __instance.pieces.Add(piece);
                                }
                            }
                        }
                        {
                            var blocker = Instantiate(defaulttombstone);
                            var component = blocker.GetComponent<ChessboardPiece>();
                            __instance.pieces.Add(component);
                            foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                            {
                                if (meshfilter.gameObject.name != "Base")
                                {
                                    Destroy(meshfilter.gameObject);
                                }
                                else
                                {
                                    foreach (var asset in allAssets)
                                    {
                                        Log.LogInfo(asset);
                                    }
                                    meshfilter.mesh = allAssets[2] as Mesh;
                                    meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                    meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                    meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                    meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                                }
                            }
                            component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                            component.gridXPos = 2;
                            component.gridYPos = 0;
                        }
                        {
                            var blocker = Instantiate(defaulttombstone);
                            var component = blocker.GetComponent<ChessboardPiece>();
                            __instance.pieces.Add(component);
                            foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                            {
                                if (meshfilter.gameObject.name != "Base")
                                {
                                    Destroy(meshfilter.gameObject);
                                }
                                else
                                {
                                    foreach (var asset in allAssets)
                                    {
                                        Log.LogInfo(asset);
                                    }
                                    meshfilter.mesh = allAssets[2] as Mesh;
                                    meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                    meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                    meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                    meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                                }
                            }
                            component.saveId = component.gridXPos * 10 + component.gridYPos * 1000; ;
                            component.gridXPos = 6;
                            component.gridYPos = 0;
                        }
                        {
                            var blocker = Instantiate(defaulttombstone);
                            var component = blocker.GetComponent<ChessboardPiece>();
                            __instance.pieces.Add(component);
                            foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                            {
                                if (meshfilter.gameObject.name != "Base")
                                {
                                    Destroy(meshfilter.gameObject);
                                }
                                else
                                {
                                    foreach (var asset in allAssets)
                                    {
                                        Log.LogInfo(asset);
                                    }
                                    meshfilter.mesh = allAssets[2] as Mesh;
                                    meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                    meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                    meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                    meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                                }
                            }
                            component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                            component.gridXPos = 1;
                            component.gridYPos = 2;
                        }
                        {
                            var blocker = Instantiate(defaulttombstone);
                            var component = blocker.GetComponent<ChessboardPiece>();
                            __instance.pieces.Add(component);
                            foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                            {
                                if (meshfilter.gameObject.name != "Base")
                                {
                                    Destroy(meshfilter.gameObject);
                                }
                                else
                                {
                                    foreach (var asset in allAssets)
                                    {
                                        Log.LogInfo(asset);
                                    }
                                    meshfilter.mesh = allAssets[2] as Mesh;
                                    meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                    meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                    meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                    meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                                }
                            }
                            component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                            component.gridXPos = 2;
                            component.gridYPos = 2;
                        }
                        {
                            var blocker = Instantiate(defaulttombstone);
                            var component = blocker.GetComponent<ChessboardPiece>();
                            __instance.pieces.Add(component);
                            foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                            {
                                if (meshfilter.gameObject.name != "Base")
                                {
                                    Destroy(meshfilter.gameObject);
                                }
                                else
                                {
                                    foreach (var asset in allAssets)
                                    {
                                        Log.LogInfo(asset);
                                    }
                                    meshfilter.mesh = allAssets[2] as Mesh;
                                    meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                    meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                    meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                    meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                                }
                            }
                            component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                            component.gridXPos = 4;
                            component.gridYPos = 2;
                        }
                        {
                            var blocker = Instantiate(defaulttombstone);
                            var component = blocker.GetComponent<ChessboardPiece>();
                            __instance.pieces.Add(component);
                            foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                            {
                                if (meshfilter.gameObject.name != "Base")
                                {
                                    Destroy(meshfilter.gameObject);
                                }
                                else
                                {
                                    foreach (var asset in allAssets)
                                    {
                                        Log.LogInfo(asset);
                                    }
                                    meshfilter.mesh = allAssets[2] as Mesh;
                                    meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                    meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                    meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                    meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                                }
                            }
                            component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                            component.gridXPos = 4;
                            component.gridYPos = 3;
                        }
                        {
                            var blocker = Instantiate(defaulttombstone);
                            var component = blocker.GetComponent<ChessboardPiece>();
                            __instance.pieces.Add(component);
                            foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                            {
                                if (meshfilter.gameObject.name != "Base")
                                {
                                    Destroy(meshfilter.gameObject);
                                }
                                else
                                {
                                    foreach (var asset in allAssets)
                                    {
                                        Log.LogInfo(asset);
                                    }
                                    meshfilter.mesh = allAssets[2] as Mesh;
                                    meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                    meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                    meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                    meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                                }
                            }
                            component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                            component.gridXPos = 5;
                            component.gridYPos = 4;
                        }
                        {
                            var blocker = Instantiate(defaulttombstone);
                            var component = blocker.GetComponent<ChessboardPiece>();
                            __instance.pieces.Add(component);
                            foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                            {
                                if (meshfilter.gameObject.name != "Base")
                                {
                                    Destroy(meshfilter.gameObject);
                                }
                                else
                                {
                                    foreach (var asset in allAssets)
                                    {
                                        Log.LogInfo(asset);
                                    }
                                    meshfilter.mesh = allAssets[2] as Mesh;
                                    meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                    meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                    meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                    meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                                }
                            }
                            component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                            component.gridXPos = 6;
                            component.gridYPos = 4;
                        }
                        {
                            var blocker = Instantiate(defaulttombstone);
                            var component = blocker.GetComponent<ChessboardPiece>();
                            __instance.pieces.Add(component);
                            foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                            {
                                if (meshfilter.gameObject.name != "Base")
                                {
                                    Destroy(meshfilter.gameObject);
                                }
                                else
                                {
                                    foreach (var asset in allAssets)
                                    {
                                        Log.LogInfo(asset);
                                    }
                                    meshfilter.mesh = allAssets[2] as Mesh;
                                    meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                    meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                    meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                    meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                                }
                            }
                            component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                            component.gridXPos = 7;
                            component.gridYPos = 4;
                        }
                        {
                            var blocker = Instantiate(defaulttombstone);
                            var component = blocker.GetComponent<ChessboardPiece>();
                            __instance.pieces.Add(component);
                            foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                            {
                                if (meshfilter.gameObject.name != "Base")
                                {
                                    Destroy(meshfilter.gameObject);
                                }
                                else
                                {
                                    foreach (var asset in allAssets)
                                    {
                                        Log.LogInfo(asset);
                                    }
                                    meshfilter.mesh = allAssets[2] as Mesh;
                                    meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                    meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                    meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                    meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                                }
                            }
                            component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                            component.gridXPos = 0;
                            component.gridYPos = 5;
                        }
                        {
                            var blocker = Instantiate(defaulttombstone);
                            var component = blocker.GetComponent<ChessboardPiece>();
                            __instance.pieces.Add(component);
                            foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                            {
                                if (meshfilter.gameObject.name != "Base")
                                {
                                    Destroy(meshfilter.gameObject);
                                }
                                else
                                {
                                    foreach (var asset in allAssets)
                                    {
                                        Log.LogInfo(asset);
                                    }
                                    meshfilter.mesh = allAssets[2] as Mesh;
                                    meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                    meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                    meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                    meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                                }
                            }
                            component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                            component.gridXPos = 1;
                            component.gridYPos = 5;
                        }
                        {
                            var blocker = Instantiate(defaulttombstone);
                            var component = blocker.GetComponent<ChessboardPiece>();
                            __instance.pieces.Add(component);
                            foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                            {
                                if (meshfilter.gameObject.name != "Base")
                                {
                                    Destroy(meshfilter.gameObject);
                                }
                                else
                                {
                                    foreach (var asset in allAssets)
                                    {
                                        Log.LogInfo(asset);
                                    }
                                    meshfilter.mesh = allAssets[2] as Mesh;
                                    meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                    meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                    meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                    meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                                }
                            }
                            component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                            component.gridXPos = 2;
                            component.gridYPos = 5;
                        }
                        {
                            var blocker = Instantiate(defaulttombstone);
                            var component = blocker.GetComponent<ChessboardPiece>();
                            __instance.pieces.Add(component);
                            foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                            {
                                if (meshfilter.gameObject.name != "Base")
                                {
                                    Destroy(meshfilter.gameObject);
                                }
                                else
                                {
                                    foreach (var asset in allAssets)
                                    {
                                        Log.LogInfo(asset);
                                    }
                                    meshfilter.mesh = allAssets[2] as Mesh;
                                    meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                    meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                    meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                    meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                                }
                            }
                            component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                            component.gridXPos = 2;
                            component.gridYPos = 6;
                        }
                    }
                    StoryEventsData.EraseEvent(StoryEvent.FactoryConveyorBeltMoved);
                    StoryEventsData.EraseEvent(StoryEvent.FactoryCuckooClockAppeared);
                    StoryEventsData.EraseEvent(StoryEvent.Part3PurchasedHoloBrush);
                }
                else if (StoryEventsData.EventCompleted(StoryEvent.FactoryConveyorBeltMoved))
                {
                    //tptohereforzone2
                    foreach (var VARIABLE in __instance.pieces.FindAll((piece => true)))
                    {

                        VARIABLE.MapNode.OccupyingPiece = null;
                        ChessboardNavGrid.instance.zones[VARIABLE.gridXPos, VARIABLE.gridYPos].GetComponent<ChessboardMapNode>().OccupyingPiece = null;
                        __instance.pieces.Remove(VARIABLE);
                        SaveManager.saveFile.grimoraData.removedPieces.Remove(VARIABLE.saveId);

                        Destroy(VARIABLE.gameObject);
                    }

                    foreach (var VARIABLE in ChessboardNavGrid.instance.zones)
                    {
                        VARIABLE.gameObject.GetComponent<ChessboardMapNode>().gameObject.SetActive(true);
                    }


                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 5;
                        component.gridYPos = 0;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 2;
                        component.gridYPos = 1;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 4;
                        component.gridYPos = 1;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 6;
                        component.gridYPos = 1;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 1;
                        component.gridYPos = 2;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 6;
                        component.gridYPos = 3;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 1;
                        component.gridYPos = 4;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 2;
                        component.gridYPos = 4;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 6;
                        component.gridYPos = 4;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 7;
                        component.gridYPos = 4;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 1;
                        component.gridYPos = 5;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 6;
                        component.gridYPos = 5;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 1;
                        component.gridYPos = 6;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 2;
                        component.gridYPos = 6;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 6;
                        component.gridYPos = 6;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 5;
                        component.gridYPos = 6;
                    }


                    {
                        var chest = Instantiate(chestpiece);
                        chest.name = "SpecialPiece";
                        var piece = chest.GetComponent<ChessboardChestPiece>();
                        piece.gridYPos = 0;
                        piece.gridXPos = 6;
                        var nodeData = piece.NodeData;
                        var randomvalue = UnityEngine.Random.Range(0, 4);
                        if (randomvalue != 2)
                        {
                            piece.NodeData = new CardChoicesNodeData();
                        }
                        else
                        {
                            piece.NodeData = new ChooseRareCardNodeData();
                        }
                        //nodeData.gridX = piece.gridXPos;
                        //nodeData.gridY = piece.gridYPos;
                        //nodeData.id = MapGenerator.GetNewID();
                        //piece.MapNode.Data = nodeData;
                        piece.saveId = piece.gridXPos * 10 + piece.gridYPos * 1000;
                        __instance.pieces.Add(piece);
                    }
                    {
                        var chest = Instantiate(chestpiece);
                        chest.name = "SpecialPiece";
                        var piece = chest.GetComponent<ChessboardChestPiece>();
                        piece.gridYPos = 3;
                        piece.gridXPos = 7;
                        var nodeData = piece.NodeData;
                        var randomvalue = UnityEngine.Random.Range(0, 4);
                        if (randomvalue != 2)
                        {
                            piece.NodeData = new CardChoicesNodeData();
                        }
                        else
                        {
                            piece.NodeData = new ChooseRareCardNodeData();
                        }
                        //nodeData.gridX = piece.gridXPos;
                        //nodeData.gridY = piece.gridYPos;
                        //nodeData.id = MapGenerator.GetNewID();
                        //piece.MapNode.Data = nodeData;
                        piece.saveId = piece.gridXPos * 10 + piece.gridYPos * 1000;
                        __instance.pieces.Add(piece);
                    }
                    {
                        var chest = Instantiate(chestpiece);
                        chest.name = "SpecialPiece";
                        var piece = chest.GetComponent<ChessboardChestPiece>();
                        piece.gridYPos = 5;
                        piece.gridXPos = 2;
                        var nodeData = piece.NodeData;
                        var randomvalue = UnityEngine.Random.Range(0, 4);
                        if (randomvalue != 2)
                        {
                            piece.NodeData = new CardChoicesNodeData();
                        }
                        else
                        {
                            piece.NodeData = new ChooseRareCardNodeData();
                        }
                        //nodeData.gridX = piece.gridXPos;
                        //nodeData.gridY = piece.gridYPos;
                        //nodeData.id = MapGenerator.GetNewID();
                        //piece.MapNode.Data = nodeData;
                        piece.saveId = piece.gridXPos * 10 + piece.gridYPos * 1000;
                        __instance.pieces.Add(piece);
                    }

                    {
                        if (ChessboardNavGrid.instance.zones[3, 5].GetComponent<ChessboardMapNode>()
                            .OccupyingPiece == null)
                        {
                            var enemy = Instantiate(enemypiece);
                            var piece = enemy.GetComponent<ChessboardEnemyPiece>();
                            piece.GoalPosX = 3;
                            piece.GoalPosY = 5;
                            piece.gridXPos = 3;
                            piece.gridYPos = 5;
                            piece.saveId = piece.gridXPos * 10 + piece.gridYPos * 1000;
                            __instance.pieces.Add(piece);
                        }
                    }
                    {
                        if (ChessboardNavGrid.instance.zones[1, 0].GetComponent<ChessboardMapNode>()
                            .OccupyingPiece == null)
                        {
                            var enemy = Instantiate(enemypiece);
                            var piece = enemy.GetComponent<ChessboardEnemyPiece>();
                            piece.GoalPosX = 1;
                            piece.GoalPosY = 0;
                            piece.gridXPos = 1;
                            piece.gridYPos = 0;
                            piece.saveId = piece.gridXPos * 10 + piece.gridYPos * 1000;
                            __instance.pieces.Add(piece);
                        }
                    }
                    {
                        if (ChessboardNavGrid.instance.zones[4, 2].GetComponent<ChessboardMapNode>()
                            .OccupyingPiece == null)
                        {
                            var enemy = Instantiate(enemypiece);
                            var piece = enemy.GetComponent<ChessboardEnemyPiece>();
                            piece.GoalPosX = 5;
                            piece.GoalPosY = 2;
                            piece.gridXPos = 4;
                            piece.gridYPos = 2;
                            piece.saveId = piece.gridXPos * 10 + piece.gridYPos * 1000;
                            __instance.pieces.Add(piece);
                        }
                    }
                    {
                        if (ChessboardNavGrid.instance.zones[7, 2].GetComponent<ChessboardMapNode>()
                            .OccupyingPiece == null)
                        {
                            var enemy = Instantiate(enemypiece);
                            var piece = enemy.GetComponent<ChessboardEnemyPiece>();
                            piece.GoalPosX = 7;
                            piece.GoalPosY = 2;
                            piece.gridXPos = 7;
                            piece.gridYPos = 2;
                            piece.saveId = piece.gridXPos * 10 + piece.gridYPos * 1000;
                            __instance.pieces.Add(piece);
                        }
                    }
                    {
                        if (ChessboardNavGrid.instance.zones[5, 1].GetComponent<ChessboardMapNode>()
                            .OccupyingPiece == null)
                        {
                            var enemy = Instantiate(bosspiece);
                            var piece = enemy.GetComponent<ChessboardEnemyPiece>();
                            piece.specialEncounterId = "DoggyBoss";
                            piece.GoalPosX = 5;
                            piece.GoalPosY = 1;
                            piece.gridXPos = 5;
                            piece.gridYPos = 1;
                            piece.saveId = piece.gridXPos * 10 + piece.gridYPos * 1000;
                            __instance.pieces.Add(piece);
                        }
                    }
                    StoryEventsData.EraseEvent(StoryEvent.PlayerDeletedArchivistFile);
                    StoryEventsData.EraseEvent(StoryEvent.FactoryCuckooClockAppeared);
                    StoryEventsData.EraseEvent(StoryEvent.Part3PurchasedHoloBrush);
                }
                else if (StoryEventsData.EventCompleted(StoryEvent.FactoryCuckooClockAppeared))
                {
                    foreach (var VARIABLE in __instance.pieces.FindAll((piece => true)))
                    {

                        VARIABLE.MapNode.OccupyingPiece = null;
                        ChessboardNavGrid.instance.zones[VARIABLE.gridXPos, VARIABLE.gridYPos].GetComponent<ChessboardMapNode>().OccupyingPiece = null;
                        __instance.pieces.Remove(VARIABLE);
                        SaveManager.saveFile.grimoraData.removedPieces.Remove(VARIABLE.saveId);

                        Destroy(VARIABLE.gameObject);
                    }

                    foreach (var VARIABLE in ChessboardNavGrid.instance.zones)
                    {
                        VARIABLE.gameObject.GetComponent<ChessboardMapNode>().gameObject.SetActive(true);
                    }
                    if (ChessboardNavGrid.instance.zones[6, 2].GetComponent<ChessboardMapNode>()
                        .OccupyingPiece == null)
                    {
                        var enemy = Instantiate(bosspiece);
                        var piece = enemy.GetComponent<ChessboardEnemyPiece>();
                        piece.specialEncounterId = "RoyalBoss";
                        piece.GoalPosX = 6;
                        piece.GoalPosY = 2;
                        piece.gridXPos = 6;
                        piece.gridYPos = 2;
                        piece.saveId = piece.gridXPos * 10 + piece.gridYPos * 1000;
                        __instance.pieces.Add(piece);
                    }

                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 4;
                        component.gridYPos = 0;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 5;
                        component.gridYPos = 0;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 6;
                        component.gridYPos = 0;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 7;
                        component.gridYPos = 0;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 0;
                        component.gridYPos = 2;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 1;
                        component.gridYPos = 2;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 2;
                        component.gridYPos = 2;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 4;
                        component.gridYPos = 2;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 1;
                        component.gridYPos = 4;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 5;
                        component.gridYPos = 4;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 6;
                        component.gridYPos = 4;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 7;
                        component.gridYPos = 4;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 0;
                        component.gridYPos = 6;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 1;
                        component.gridYPos = 6;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 2;
                        component.gridYPos = 6;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 5;
                        component.gridYPos = 6;
                    }


                    {
                        var chest = Instantiate(chestpiece);
                        chest.name = "SpecialPiece";
                        var piece = chest.GetComponent<ChessboardChestPiece>();
                        piece.gridYPos = 0;
                        piece.gridXPos = 6;
                        var nodeData = piece.NodeData;
                        var randomvalue = UnityEngine.Random.Range(0, 4);
                        if (randomvalue != 2)
                        {
                            piece.NodeData = new CardChoicesNodeData();
                        }
                        else
                        {
                            piece.NodeData = new ChooseRareCardNodeData();
                        }
                        //nodeData.gridX = piece.gridXPos;
                        //nodeData.gridY = piece.gridYPos;
                        //nodeData.id = MapGenerator.GetNewID();
                        //piece.MapNode.Data = nodeData;
                        piece.saveId = piece.gridXPos * 10 + piece.gridYPos * 1000;
                        __instance.pieces.Add(piece);
                    }
                    {
                        var chest = Instantiate(chestpiece);
                        chest.name = "SpecialPiece";
                        var piece = chest.GetComponent<ChessboardChestPiece>();
                        piece.gridYPos = 7;
                        piece.gridXPos = 7;
                        var nodeData = piece.NodeData;
                        var randomvalue = UnityEngine.Random.Range(0, 4);
                        if (randomvalue != 2)
                        {
                            piece.NodeData = new CardChoicesNodeData();
                        }
                        else
                        {
                            piece.NodeData = new ChooseRareCardNodeData();
                        }
                        //nodeData.gridX = piece.gridXPos;
                        //nodeData.gridY = piece.gridYPos;
                        //nodeData.id = MapGenerator.GetNewID();
                        //piece.MapNode.Data = nodeData;
                        piece.saveId = piece.gridXPos * 10 + piece.gridYPos * 1000;
                        __instance.pieces.Add(piece);
                    }
                    {
                        var chest = Instantiate(chestpiece);
                        chest.name = "SpecialPiece";
                        var piece = chest.GetComponent<ChessboardChestPiece>();
                        piece.gridYPos = 5;
                        piece.gridXPos = 3;
                        var nodeData = piece.NodeData;
                        var randomvalue = UnityEngine.Random.Range(0, 4);
                        if (randomvalue != 2)
                        {
                            piece.NodeData = new CardChoicesNodeData();
                        }
                        else
                        {
                            piece.NodeData = new ChooseRareCardNodeData();
                        }
                        //nodeData.gridX = piece.gridXPos;
                        //nodeData.gridY = piece.gridYPos;
                        //nodeData.id = MapGenerator.GetNewID();
                        //piece.MapNode.Data = nodeData;
                        piece.saveId = piece.gridXPos * 10 + piece.gridYPos * 1000;
                        __instance.pieces.Add(piece);
                    }

                    {
                        if (ChessboardNavGrid.instance.zones[3, 5].GetComponent<ChessboardMapNode>()
                            .OccupyingPiece == null)
                        {
                            var enemy = Instantiate(enemypiece);
                            var piece = enemy.GetComponent<ChessboardEnemyPiece>();
                            piece.GoalPosX = 3;
                            piece.GoalPosY = 1;
                            piece.gridXPos = 3;
                            piece.gridYPos = 1;
                            piece.saveId = piece.gridXPos * 10 + piece.gridYPos * 1000;
                            __instance.pieces.Add(piece);
                        }
                    }
                    {
                        if (ChessboardNavGrid.instance.zones[1, 0].GetComponent<ChessboardMapNode>()
                            .OccupyingPiece == null)
                        {
                            var enemy = Instantiate(enemypiece);
                            var piece = enemy.GetComponent<ChessboardEnemyPiece>();
                            piece.GoalPosX = 0;
                            piece.GoalPosY = 5;
                            piece.gridXPos = 0;
                            piece.gridYPos = 5;
                            piece.saveId = piece.gridXPos * 10 + piece.gridYPos * 1000;
                            __instance.pieces.Add(piece);
                        }
                    }
                    {
                        if (ChessboardNavGrid.instance.zones[4, 2].GetComponent<ChessboardMapNode>()
                            .OccupyingPiece == null)
                        {
                            var enemy = Instantiate(enemypiece);
                            var piece = enemy.GetComponent<ChessboardEnemyPiece>();
                            piece.GoalPosX = 5;
                            piece.GoalPosY = 3;
                            piece.gridXPos = 5;
                            piece.gridYPos = 2;
                            piece.saveId = piece.gridXPos * 10 + piece.gridYPos * 1000;
                            __instance.pieces.Add(piece);
                        }
                    }
                    {
                        if (ChessboardNavGrid.instance.zones[7, 2].GetComponent<ChessboardMapNode>()
                            .OccupyingPiece == null)
                        {
                            var enemy = Instantiate(enemypiece);
                            var piece = enemy.GetComponent<ChessboardEnemyPiece>();
                            piece.GoalPosX = 0;
                            piece.GoalPosY = 7;
                            piece.gridXPos = 5;
                            piece.gridYPos = 7;
                            piece.saveId = piece.gridXPos * 10 + piece.gridYPos * 1000;
                            __instance.pieces.Add(piece);
                        }
                    }

                    StoryEventsData.EraseEvent(StoryEvent.PlayerDeletedArchivistFile);
                    StoryEventsData.EraseEvent(StoryEvent.FactoryConveyorBeltMoved);
                    StoryEventsData.EraseEvent(StoryEvent.Part3PurchasedHoloBrush);
                }
                else if (StoryEventsData.EventCompleted(StoryEvent.Part3PurchasedHoloBrush))
                {
                    //tptohereforzone4
                    foreach (var VARIABLE in __instance.pieces.FindAll((piece => true)))
                    {

                        VARIABLE.MapNode.OccupyingPiece = null;
                        ChessboardNavGrid.instance.zones[VARIABLE.gridXPos, VARIABLE.gridYPos].GetComponent<ChessboardMapNode>().OccupyingPiece = null;
                        __instance.pieces.Remove(VARIABLE);
                        SaveManager.saveFile.grimoraData.removedPieces.Remove(VARIABLE.saveId);

                        Destroy(VARIABLE.gameObject);
                    }

                    foreach (var VARIABLE in ChessboardNavGrid.instance.zones)
                    {
                        VARIABLE.gameObject.GetComponent<ChessboardMapNode>().gameObject.SetActive(true);
                    }

                    if (ChessboardNavGrid.instance.zones[3, 1].GetComponent<ChessboardMapNode>()
                        .OccupyingPiece == null)
                    {
                        var enemy = Instantiate(bosspiece);
                        var piece = enemy.GetComponent<ChessboardEnemyPiece>();
                        piece.specialEncounterId = "GrimoraBoss";
                        piece.GoalPosX = 3;
                        piece.GoalPosY = 1;
                        piece.gridXPos = 3;
                        piece.gridYPos = 1;
                        piece.saveId = piece.gridXPos * 10 + piece.gridYPos * 1000;
                        __instance.pieces.Add(piece);
                    }

                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 1;
                        component.gridYPos = 0;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 3;
                        component.gridYPos = 0;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 4;
                        component.gridYPos = 0;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 6;
                        component.gridYPos = 0;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 6;
                        component.gridYPos = 1;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 1;
                        component.gridYPos = 2;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 1;
                        component.gridYPos = 3;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 6;
                        component.gridYPos = 3;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 1;
                        component.gridYPos = 4;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 6;
                        component.gridYPos = 4;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 1;
                        component.gridYPos = 6;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 6;
                        component.gridYPos = 6;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 1;
                        component.gridYPos = 7;
                    }
                    {
                        var blocker = Instantiate(defaulttombstone);
                        var component = blocker.GetComponent<ChessboardPiece>();
                        __instance.pieces.Add(component);
                        foreach (var meshfilter in blocker.GetComponentsInChildren<MeshFilter>())
                        {
                            if (meshfilter.gameObject.name != "Base")
                            {
                                Destroy(meshfilter.gameObject);
                            }
                            else
                            {
                                foreach (var asset in allAssets)
                                {
                                    Log.LogInfo(asset);
                                }
                                meshfilter.mesh = allAssets[2] as Mesh;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().material.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
                                meshfilter.gameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                                meshfilter.gameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
                            }
                        }
                        component.saveId = component.gridXPos * 10 + component.gridYPos * 1000;
                        component.gridXPos = 6;
                        component.gridYPos = 7;
                    }



                    {
                        var chest = Instantiate(chestpiece);
                        chest.name = "SpecialPiece";
                        var piece = chest.GetComponent<ChessboardChestPiece>();
                        piece.gridYPos = 5;
                        piece.gridXPos = 1;
                        var nodeData = piece.NodeData;
                        var randomvalue = UnityEngine.Random.Range(0, 4);
                        if (randomvalue != 2)
                        {
                            piece.NodeData = new CardChoicesNodeData();
                        }
                        else
                        {
                            piece.NodeData = new ChooseRareCardNodeData();
                        }
                        //nodeData.gridX = piece.gridXPos;
                        //nodeData.gridY = piece.gridYPos;
                        //nodeData.id = MapGenerator.GetNewID();
                        //piece.MapNode.Data = nodeData;
                        piece.saveId = piece.gridXPos * 10 + piece.gridYPos * 1000;
                        __instance.pieces.Add(piece);
                    }
                    {
                        var chest = Instantiate(chestpiece);
                        chest.name = "SpecialPiece";
                        var piece = chest.GetComponent<ChessboardChestPiece>();
                        piece.gridYPos = 5;
                        piece.gridXPos = 6;
                        var nodeData = piece.NodeData;
                        var randomvalue = UnityEngine.Random.Range(0, 4);
                        if (randomvalue != 2)
                        {
                            piece.NodeData = new CardChoicesNodeData();
                        }
                        else
                        {
                            piece.NodeData = new ChooseRareCardNodeData();
                        }
                        //nodeData.gridX = piece.gridXPos;
                        //nodeData.gridY = piece.gridYPos;
                        //nodeData.id = MapGenerator.GetNewID();
                        //piece.MapNode.Data = nodeData;
                        piece.saveId = piece.gridXPos * 10 + piece.gridYPos * 1000;
                        __instance.pieces.Add(piece);
                    }

                    {
                        if (ChessboardNavGrid.instance.zones[3, 5].GetComponent<ChessboardMapNode>()
                            .OccupyingPiece == null)
                        {
                            var enemy = Instantiate(enemypiece);
                            var piece = enemy.GetComponent<ChessboardEnemyPiece>();
                            piece.GoalPosX = 2;
                            piece.GoalPosY = 3;
                            piece.gridXPos = 2;
                            piece.gridYPos = 3;
                            piece.saveId = piece.gridXPos * 10 + piece.gridYPos * 1000;
                            __instance.pieces.Add(piece);
                        }
                    }
                    {
                        if (ChessboardNavGrid.instance.zones[1, 0].GetComponent<ChessboardMapNode>()
                            .OccupyingPiece == null)
                        {
                            var enemy = Instantiate(enemypiece);
                            var piece = enemy.GetComponent<ChessboardEnemyPiece>();
                            piece.GoalPosX = 5;
                            piece.GoalPosY = 3;
                            piece.gridXPos = 5;
                            piece.gridYPos = 3;
                            piece.saveId = piece.gridXPos * 10 + piece.gridYPos * 1000;
                            __instance.pieces.Add(piece);
                        }
                    }
                }
                //__instance.pieces.RemoveAll(piece => piece.gameObject.name.Contains("Tombstone"));
            }

            public static void Postfix(ref ChessboardMap __instance, float unrollSpeed)
            {
                Log.LogInfo("POSTFIX HAS RUN DONT IGNORE THIS THIS IS NOT AN ERROR IT IS A GOOD THING IF YOU ARE READING THIS HAVE A NICE DAY");

                foreach (var objectz in GameObject.FindObjectsOfType(typeof(ChessboardPiece)))
                {
                    var piece = (ChessboardPiece)objectz;
                    piece.gameObject.transform.parent = __instance.dynamicElementsParent;
                    __instance.pieces.ForEach(delegate (ChessboardPiece x)
                    {
                        x.UpdateSaveState();
                        x.Hide(true);
                    });

                    if (StoryEventsData.EventCompleted(StoryEvent.FactoryConveyorBeltMoved))
                    {
                        Log.LogInfo("Area2 blueprints loaded");
                        if ((objectz as ChessboardPiece).gameObject.GetComponent<ChessboardEnemyPiece>())
                        {
                            if (!(objectz as ChessboardPiece).gameObject.name.Contains("Boss"))
                            {
                                var allblueprints = new List<EncounterBlueprintData>();
                                {
                                    var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
                                    blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>();


                                    var skeleton = new EncounterBlueprintData.CardBlueprint();
                                    skeleton.card = CardLoader.GetCardByName("Skeleton");
                                    var bonelord = new EncounterBlueprintData.CardBlueprint();
                                    bonelord.card = CardLoader.GetCardByName("ara_Bonelord");
                                    var draugr = new EncounterBlueprintData.CardBlueprint();
                                    draugr.card = CardLoader.GetCardByName("ara_Draugr");

                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { skeleton });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { draugr });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { skeleton, skeleton, draugr });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { draugr, draugr });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { skeleton });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { bonelord });
                                    allblueprints.Add(blueprint);
                                }
                                {
                                    var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
                                    blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>();


                                    var skeleton = new EncounterBlueprintData.CardBlueprint();
                                    skeleton.card = CardLoader.GetCardByName("Skeleton");
                                    var hound = new EncounterBlueprintData.CardBlueprint();
                                    hound.card = CardLoader.GetCardByName("Bonehound");
                                    var zombie = new EncounterBlueprintData.CardBlueprint();
                                    zombie.card = CardLoader.GetCardByName("ara_Zombie");

                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { hound });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { skeleton, zombie, skeleton });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { zombie, hound });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { zombie, zombie });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { skeleton, skeleton });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { hound });
                                    allblueprints.Add(blueprint);
                                }
                                {
                                    var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
                                    blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>();


                                    var bonedigger = new EncounterBlueprintData.CardBlueprint();
                                    bonedigger.card = CardLoader.GetCardByName("ara_BoneDigger");
                                    var sporedigger = new EncounterBlueprintData.CardBlueprint();
                                    sporedigger.card = CardLoader.GetCardByName("ara_SporeDigger");
                                    var horseman = new EncounterBlueprintData.CardBlueprint();
                                    horseman.card = CardLoader.GetCardByName("ara_HeadlessHorseman");

                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { bonedigger });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { bonedigger, bonedigger });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { sporedigger });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { horseman });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { sporedigger, bonedigger });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { horseman });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { horseman, bonedigger });
                                    allblueprints.Add(blueprint);
                                }
                                {
                                    var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
                                    blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>();


                                    var sarcophagus = new EncounterBlueprintData.CardBlueprint();
                                    sarcophagus.card = CardLoader.GetCardByName("ara_sarcophagus");
                                    var hound = new EncounterBlueprintData.CardBlueprint();
                                    hound.card = CardLoader.GetCardByName("Bonehound");
                                    var skelemage = new EncounterBlueprintData.CardBlueprint();
                                    skelemage.card = CardLoader.GetCardByName("ara_SkeletonMage");

                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { sarcophagus });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { skelemage, skelemage });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { skelemage, hound });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { sarcophagus });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { skelemage, skelemage });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { hound });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { skelemage, skelemage });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { sarcophagus });
                                    allblueprints.Add(blueprint);
                                }
                                {
                                    var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
                                    blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>();


                                    var wolf = new EncounterBlueprintData.CardBlueprint();
                                    wolf.card = CardLoader.GetCardByName("ara_Wolf");
                                    var obol = new EncounterBlueprintData.CardBlueprint();
                                    obol.card = CardLoader.GetCardByName("ara_obol");
                                    var zombie = new EncounterBlueprintData.CardBlueprint();
                                    zombie.card = CardLoader.GetCardByName("ara_Zombie");
                                    var skeleton = new EncounterBlueprintData.CardBlueprint();
                                    skeleton.card = CardLoader.GetCardByName("Skeleton");

                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { obol });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { wolf });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { zombie, zombie });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { wolf });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { obol });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { zombie });
                                    allblueprints.Add(blueprint);
                                }
                                var randomvalue = UnityEngine.Random.Range(0, allblueprints.Count);
                                (objectz as ChessboardPiece).gameObject.GetComponent<ChessboardEnemyPiece>().blueprint = allblueprints[randomvalue];
                            }
                        }
                    }

                    else if (StoryEventsData.EventCompleted(StoryEvent.FactoryCuckooClockAppeared))
                    {
                        Log.LogInfo("Area3 blueprints loaded");
                        if ((objectz as ChessboardPiece).gameObject.GetComponent<ChessboardEnemyPiece>())
                        {
                            if (!(objectz as ChessboardPiece).gameObject.name.Contains("Boss"))
                            {
                                var allblueprints = new List<EncounterBlueprintData>();
                                {
                                    var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
                                    blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>();


                                    var pirateship = new EncounterBlueprintData.CardBlueprint();
                                    pirateship.card = CardLoader.GetCardByName("ara_Pirateship");
                                    var boneprince = new EncounterBlueprintData.CardBlueprint();
                                    boneprince.card = CardLoader.GetCardByName("ara_BonePrince");
                                    var zombie = new EncounterBlueprintData.CardBlueprint();
                                    zombie.card = CardLoader.GetCardByName("ara_Zombie");

                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { pirateship });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() {  zombie });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { zombie });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { zombie, zombie });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { pirateship });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { boneprince });
                                    allblueprints.Add(blueprint);
                                }
                                {
                                    var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
                                    blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>();


                                    var pirateship = new EncounterBlueprintData.CardBlueprint();
                                    pirateship.card = CardLoader.GetCardByName("ara_Pirateship");
                                    var spirit = new EncounterBlueprintData.CardBlueprint();
                                    spirit.card = CardLoader.GetCardByName("ara_Ember_spirit");
                                    var zombie = new EncounterBlueprintData.CardBlueprint();
                                    zombie.card = CardLoader.GetCardByName("ara_Zombie");

                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { spirit });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { zombie, zombie });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { spirit });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { zombie, zombie });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { pirateship });
                                    allblueprints.Add(blueprint);
                                }
                                {
                                    var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
                                    blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>();


                                    var bonedigger = new EncounterBlueprintData.CardBlueprint();
                                    bonedigger.card = CardLoader.GetCardByName("ara_BoneDigger");
                                    var sporedigger = new EncounterBlueprintData.CardBlueprint();
                                    sporedigger.card = CardLoader.GetCardByName("ara_SporeDigger");
                                    var horseman = new EncounterBlueprintData.CardBlueprint();
                                    horseman.card = CardLoader.GetCardByName("ara_HeadlessHorseman");

                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { bonedigger });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { bonedigger, bonedigger });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { sporedigger });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { horseman });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { sporedigger, bonedigger });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { horseman });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { horseman, bonedigger });
                                    allblueprints.Add(blueprint);
                                }
                                {
                                    var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
                                    blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>();


                                    var soul = new EncounterBlueprintData.CardBlueprint();
                                    soul.card = CardLoader.GetCardByName("ara_DrownedSoul");
                                    var snapper = new EncounterBlueprintData.CardBlueprint();
                                    snapper.card = CardLoader.GetCardByName("ara_Snapper");
                                    var zombie = new EncounterBlueprintData.CardBlueprint();
                                    zombie.card = CardLoader.GetCardByName("ara_Zombie");

                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() {  });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { zombie });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { snapper });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { zombie });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { snapper });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { zombie, zombie });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { soul });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { snapper });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { snapper });
                                    allblueprints.Add(blueprint);
                                }

                                var randomvalue = UnityEngine.Random.Range(0, allblueprints.Count);
                                (objectz as ChessboardPiece).gameObject.GetComponent<ChessboardEnemyPiece>().blueprint = allblueprints[randomvalue];
                            }
                        }




                    }


                    else if (StoryEventsData.EventCompleted(StoryEvent.Part3PurchasedHoloBrush))
                    {
                        Log.LogInfo("Area4 blueprints loaded");
                        if ((objectz as ChessboardPiece).gameObject.GetComponent<ChessboardEnemyPiece>())
                        {
                            if (!(objectz as ChessboardPiece).gameObject.name.Contains("Boss"))
                            {
                                var allblueprints = new List<EncounterBlueprintData>();
                                {
                                    var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
                                    blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>();


                                    var bonepile = new EncounterBlueprintData.CardBlueprint();
                                    bonepile.card = CardLoader.GetCardByName("GraveRobber");
                                    var frankstein = new EncounterBlueprintData.CardBlueprint();
                                    frankstein.card = CardLoader.GetCardByName("ara_Franknstein");
                                    var skeleton = new EncounterBlueprintData.CardBlueprint();
                                    skeleton.card = CardLoader.GetCardByName("Skeleton");
                                    var revenant = new EncounterBlueprintData.CardBlueprint();
                                    revenant.card = CardLoader.GetCardByName("ara_Revenant");

                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { bonepile });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { bonepile, frankstein });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { frankstein, frankstein, frankstein });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { revenant });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { skeleton });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { skeleton, skeleton, skeleton, skeleton });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { frankstein });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { frankstein, frankstein });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { frankstein });
                                    allblueprints.Add(blueprint);
                                }
                                {
                                    var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
                                    blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>();


                                    var mummylord = new EncounterBlueprintData.CardBlueprint();
                                    mummylord.card = CardLoader.GetCardByName("ara_mummylord");
                                    var wolf = new EncounterBlueprintData.CardBlueprint();
                                    wolf.card = CardLoader.GetCardByName("ara_Wolf");
                                    var robber = new EncounterBlueprintData.CardBlueprint();
                                    robber.card = CardLoader.GetCardByName("ara_GraveRobber");

                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { mummylord, robber });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { mummylord });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { robber });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { robber });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { wolf });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { robber });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { mummylord });
                                    allblueprints.Add(blueprint);
                                }


                                var randomvalue = UnityEngine.Random.Range(0, allblueprints.Count);
                                (objectz as ChessboardPiece).gameObject.GetComponent<ChessboardEnemyPiece>().blueprint = allblueprints[randomvalue];
                            }
                        }



                    }

                    else
                    {
                        Log.LogInfo("Area1 blueprints loaded");

                        if ((objectz as ChessboardPiece).gameObject.GetComponent<ChessboardEnemyPiece>())
                        {
                            if (!(objectz as ChessboardPiece).gameObject.name.Contains("Boss"))
                            {

                                var allblueprints = new List<EncounterBlueprintData>();
                                {
                                    var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
                                    blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>();


                                    var zombie = new EncounterBlueprintData.CardBlueprint();
                                    zombie.card = CardLoader.GetCardByName("ara_Zombie");
                                    var franknstein = new EncounterBlueprintData.CardBlueprint();
                                    franknstein.card = CardLoader.GetCardByName("ara_Franknstein");

                                    zombie.difficultyReplace = false;
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { zombie });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { zombie });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { zombie });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { franknstein });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { zombie });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { zombie, franknstein });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { zombie, zombie });
                                    allblueprints.Add(blueprint);
                                }
                                {
                                    var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
                                    blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>();

                                    var skeleton = new EncounterBlueprintData.CardBlueprint();
                                    skeleton.card = CardLoader.GetCardByName("Skeleton");
                                    var revenant = new EncounterBlueprintData.CardBlueprint();
                                    revenant.card = CardLoader.GetCardByName("ara_Revenant");
                                    var draugr = new EncounterBlueprintData.CardBlueprint();
                                    draugr.card = CardLoader.GetCardByName("ara_Draugr");

                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { skeleton, skeleton });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { draugr });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { draugr });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { revenant });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { revenant, skeleton });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { skeleton, skeleton });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { draugr });
                                    allblueprints.Add(blueprint);
                                }
                                {
                                    var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
                                    blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>();

                                    var graverobber = new EncounterBlueprintData.CardBlueprint();
                                    graverobber.card = CardLoader.GetCardByName("ara_GraveRobber");
                                    var snapper = new EncounterBlueprintData.CardBlueprint();
                                    snapper.card = CardLoader.GetCardByName("ara_Snapper");
                                    var draugr = new EncounterBlueprintData.CardBlueprint();
                                    draugr.card = CardLoader.GetCardByName("ara_Draugr");
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { graverobber });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { draugr, snapper });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { draugr });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { graverobber });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { snapper });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { draugr, draugr });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { graverobber, graverobber, snapper });
                                    allblueprints.Add(blueprint);
                                }
                                {
                                    var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
                                    blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>();

                                    var bonedigger = new EncounterBlueprintData.CardBlueprint();
                                    bonedigger.card = CardLoader.GetCardByName("ara_BoneDigger");
                                    var revenant = new EncounterBlueprintData.CardBlueprint();
                                    revenant.card = CardLoader.GetCardByName("ara_Revenant");
                                    var serpent = new EncounterBlueprintData.CardBlueprint();
                                    serpent.card = CardLoader.GetCardByName("ara_Serpent");
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { bonedigger });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { revenant });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { bonedigger });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { serpent });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { serpent, bonedigger });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { revenant });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { serpent });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { bonedigger });
                                    allblueprints.Add(blueprint);
                                }
                                {
                                    var blueprint = ScriptableObject.CreateInstance<EncounterBlueprintData>();
                                    blueprint.turns = new List<List<EncounterBlueprintData.CardBlueprint>>();

                                    var skelemage = new EncounterBlueprintData.CardBlueprint();
                                    skelemage.card = CardLoader.GetCardByName("ara_SkeletonMage");
                                    var skelemancer = new EncounterBlueprintData.CardBlueprint();
                                    skelemancer.card = CardLoader.GetCardByName("ara_Skelemancer");
                                    var draugr = new EncounterBlueprintData.CardBlueprint();
                                    draugr.card = CardLoader.GetCardByName("ara_Draugr");
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { skelemancer });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { skelemancer });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { skelemage });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { skelemancer });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { skelemage, draugr });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { draugr });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { skelemage });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { draugr, skelemancer, skelemancer });
                                    blueprint.turns.Add(new List<EncounterBlueprintData.CardBlueprint>() { skelemage });
                                    allblueprints.Add(blueprint);
                                }
                                var randomvalue = UnityEngine.Random.Range(0, allblueprints.Count);
                                (objectz as ChessboardPiece).gameObject.GetComponent<ChessboardEnemyPiece>().blueprint = allblueprints[randomvalue];
                            }
                        }

                    }
                }
            }



        [HarmonyPatch(typeof(ChessboardChestPiece), "Start")]
            public class specialpiecepatch
            {
                public static bool Prefix(ref ChessboardChestPiece __instance)
                {
                    //if (__instance.gameObject.name.Contains("SpecialPiece"))
                    {
                        __instance.gameObject.transform.position = ChessboardNavGrid.instance.zones[__instance.gridXPos, __instance.gridYPos].transform.position;
                        ChessboardNavGrid.instance.zones[__instance.gridXPos, __instance.gridYPos].GetComponent<ChessboardMapNode>().OccupyingPiece = __instance;

                        return false;
                    }

                    return true;
                }
            }

            [HarmonyPatch(typeof(ChessboardChestPiece), "OpenSequence")]
            public class removeme
            {
                public static void Prefix(ref ChessboardChestPiece __instance)
                {
                    //if (__instance.gameObject.name.Contains("SpecialPiece"))
                    {
                        Log.LogInfo(__instance.MapNode.OccupyingPiece);
                        SaveManager.saveFile.grimoraData.removedPieces.Add(__instance.saveId);
                        ChessboardNavGrid.instance.zones[__instance.gridXPos, __instance.gridYPos].GetComponent<ChessboardMapNode>().OccupyingPiece = null;
                        Singleton<MapNodeManager>.Instance.SetAllNodesInteractable(false);
                        __instance.MapNode.OccupyingPiece = null;
                        Log.LogInfo(__instance.MapNode.OccupyingPiece);
                        __instance.MapNode.nodeId = __instance.saveId;
                    }
                }
            }


            [HarmonyPatch(typeof(MapNodeManager), "DoMoveToNewNode")]
            public class sks
            {
                public static void Prefix(ref MapNodeManager __instance, out MapNodeManager __state)
                {
                    __state = __instance;
                }

                public static IEnumerator Postfix(IEnumerator enumerator, MapNodeManager __state, MapNode newNode)
                {
                    Log.LogInfo(__state);
                    Log.LogInfo(newNode.Data);
                    __state.MovingNodes = true;
                    __state.SetAllNodesInteractable(false);
                    __state.transitioningGridY = newNode.Data.gridY;
                    Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Locked;
                    yield return PlayerMarker.Instance.MoveToPoint(newNode.transform.position, true);
                    if (newNode.Data is null)
                    {
                        Log.LogInfo("this is indeed null");
                        newNode.Data = new NodeData();
                        newNode.Data.prefabPath = "Prefabs/Map/MapNodesPart1/MapNode_Empty";
                    }
                    if (newNode.Data != null)
                    {
                        yield return newNode.OnArriveAtNode();
                    }

                    Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                    __state.MovingNodes = false;
                    CustomCoroutine.WaitOnConditionThenExecute(() => !Singleton<GameFlowManager>.Instance.Transitioning, delegate
                    {
                        RunState.Run.currentNodeId = newNode.nodeId;
                    });
                    yield break;
                }
            }



            [HarmonyPatch(typeof(ChessboardEnemyBattleSequencer), "PreCleanUp")]
            public class death
            {
                public static void Prefix(ref ChessboardEnemyBattleSequencer __instance, out ChessboardEnemyBattleSequencer __state)
                {
                    __state = __instance;
                }

                public static IEnumerator Postfix(IEnumerator enumeratorz, ChessboardEnemyBattleSequencer __state)
                {
                    if (!Singleton<TurnManager>.Instance.PlayerWon)
                    {
                        AudioController.Instance.FadeOutLoop(3f, Array.Empty<int>());
                        Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, true);
                        Singleton<PlayerHand>.Instance.PlayingLocked = true;
                        Singleton<InteractionCursor>.Instance.InteractionDisabled = true;
                        __state.StartCoroutine(Singleton<CardDrawPiles>.Instance.CleanUp());
                        __state.StartCoroutine(Singleton<TurnManager>.Instance.Opponent.CleanUp());
                        yield return (Singleton<BoardManager>.Instance as BoardManager3D).HideSlots();
                        foreach (PlayableCard c in Singleton<BoardManager>.Instance.CardsOnBoard)
                        {
                            __state.GlitchOutCard(c);
                            yield return new WaitForSeconds(0.1f);
                        }
                        List<PlayableCard>.Enumerator enumerator = default(List<PlayableCard>.Enumerator);
                        foreach (PlayableCard c2 in Singleton<PlayerHand>.Instance.CardsInHand)
                        {
                            __state.GlitchOutCard(c2);
                            yield return new WaitForSeconds(0.1f);
                        }
                        enumerator = default(List<PlayableCard>.Enumerator);
                        Singleton<PlayerHand>.Instance.SetShown(false, false);
                        yield return new WaitForSeconds(0.75f);
                        Singleton<TableRuleBook>.Instance.enabled = false;
                        GlitchOutAssetEffect.GlitchModel(Singleton<TableRuleBook>.Instance.transform, false, true);
                        yield return new WaitForSeconds(0.75f);
                        yield return Singleton<TextDisplayer>.Instance.ShowUntilInput("Let the circle reset");
                        yield return new WaitForSeconds(0.5f);
                        Singleton<InteractionCursor>.Instance.InteractionDisabled = false;
                        ///SaveManager.saveFile.ResetRun();
                        SaveManager.saveFile.grimoraData.Initialize();
                        SaveManager.SaveToFile();
                        StoryEventsData.EraseEvent(StoryEvent.PlayerDeletedArchivistFile);
                        StoryEventsData.EraseEvent(StoryEvent.FactoryConveyorBeltMoved);
                        StoryEventsData.EraseEvent(StoryEvent.Part3PurchasedHoloBrush);
                        StoryEventsData.EraseEvent(StoryEvent.FactoryCuckooClockAppeared);
                        SceneLoader.Load("Start");
                    }
                    else if (!DialogueEventsData.EventIsPlayed("FinaleGrimoraBattleWon"))
                    {
                        Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Locked;
                        yield return new WaitForSeconds(0.5f);
                        yield return Singleton<TextDisplayer>.Instance.PlayDialogueEvent("FinaleGrimoraBattleWon", TextDisplayer.MessageAdvanceMode.Input, TextDisplayer.EventIntersectMode.Wait, null, null);
                    }
                    yield break;

                }


            }





        }







    private void Awake()
            {
                staticpath = Info.Location.Replace("Grimoramod_Core.dll", "");
                bundle = AssetBundle.LoadFromFile(staticpath + "Artwork/grimora");
                allAssets = bundle.LoadAllAssets();
                Pluginz.Log = base.Logger;
                Harmony harmony = new Harmony(PluginGuid);
                harmony.PatchAll();
            }



    }
}
