- A giant Mod made by xXxStoner420BongMasterxXx and Arackulele, with modeling from `Pink#9824`, that builds upon the finale with Grimora's
	chessboard.

- **MOST IMPORTANT**: **!!!You will probably encounter some Bugs!!!**
- **IMPORTANT**: **ENABLE ENERGY DRONE IN API CONFIG IN ORDER FOR IT TO SHOW UP**
- **BEWARE**:

	- THIS WILL UNLOCK CERTAIN STORY EVENTS IN ORDER FOR THE MOD TO WORK, WHICH ALSO MEANS UNLOCKING CERTAIN ACHIEVEMENTS!
	- THIS MOD HAS NOT BEEN TESTED WITH KAYCEE'S MOD!

- In the event you are starting from a brand new save, this mod will unlock all learned abilities, mechanics, and cards
	and some Story Events like the first tutorial battle, Bones, and a few others in order for this mod to work.
- If you are not starting from a new save, this mod will check for certain story events and determine if it needs to unlock those.
- I suggest restarting your game at least once if you haven't actually gotten to the finale.

- If you want to discuss the mod further, join our Discord server! <https://discord.gg/Xf8CBuS8a8>

## Reporting Issues

- If you would to help report issues, please raise a thread here with as much detail as you can provide: <https://github.com/Arackulele/GrimoraMod/issues>
- Bug reports can also be submitted on the Discord Server: <https://discord.gg/Xf8CBuS8a8>

ANY POSTS THAT JUST SAY 'A BUG HAPPENED AND IT BROKE' WILL BE IGNORED

## Known Issues

### Boss skull between fights doesn't reset colors

- Unfortunately this one is a bit hard to track down, as the table colors get reset after each fight. Not sure why the boss skull doesn't have the color reset.

### Rare card choosing breaks/throws exceptions

- The main culprit from what the team can tell, is that it's an issue with updating the asset bundles. The only recommendation we have now is to uninstall the mod completely, and redownload. Mod managers seem to be really finnicky.

### Current save file is already at the finale with Grimora

- Make a backup of your save, then delete your current save. Having your current save already at the finale seems to break the mod.
- Possibly fixed in 2.6.4 update.

### Bonelord art overlaps abilities

- Bonelord does what he wants.

## Update Notes

### 2.7.2

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed issue with hammer interaction when a card in your hand has `Corpse Eater` and `Hoarder`.
	- When the hammer is used, it disables the cursor so you can't interact with anything until the hammer sequence finishes. The problem is that with `Corpse Eater` and `Hoarder`, is that the hammer sequence doesn't finish until after you choose a card from the `Hoarder` sequence, but you can't choose a card because the hammer disabled your cursor. Hence, softlock.

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed issue where you could spam click the Deck View button and continue overlapping the cards in your deck.

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed issue with `Electric Chair` where it was possible to add 2 more abilities if the electrocuted card had 3 abilities to start.

- ![Feature](https://i.imgur.com/uL9uYNV.png) Added hammer dialogue option in config file.
	- 0 = Disable hammer dialogue entirely.
	- 1 = Play only once for the entire session. (default)
	- 2 = Play dialogue each battle.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Card Removal, Boneyard, and Electric Chair initial dialogue now only gets played once, similar to how the vanilla game does not play certain dialogues once you've done it before.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Re-positioned `retrieveCardInteractable` in Electric Chair sequencer so that it's easier to take the card away from the chair.
	- Before, the slot was still positioned as if it was on the ground flat, so that you had to click between the chair and the stone.

#### Bosses

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed potential softlock in Grimora's fight if a card had `Possessive`.

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed icicle on Kaycee's figurine to now properly move with the head.

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed issue with Sawyer taking a bone when a player would have zero bones, causing the PlayerBones to go in the negative.
	- This would cause the player to be unable to play zero cost bone cards like Skeleton.
	- Sawyer will now only take 1 bone from the player if the player has at least 2 bones or more.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Tweaked Kaycee boss logic so the card freezing is less frustrating.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Changed Grimora's theme to `Corrupted Queen` from `Akisephila (Addie Brahem)`.

#### Ability/Card Changes

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed `Skin Crawler` ability not dying after the first time the dialogue is played. Forgot to move the card dying part outside the check for if the dialogue has not been played yet...

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed issue with `Handy` ability drawing a new hand for the player if the card would die and then reanimated during the first phase of Grimora's fight.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) `Zomb-Geck` no longer appears as a rare card. Was more or less meant as filler until more cards were added.
	- Maybe used for future event?

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Added description for `Banshee`.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Added `Bonehound` to normal card pool.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Updated `Skeleton Army` ability description to better clarify what it does.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Added extra logic for when a card has `Area of Effect Strike` and `Inverted Strike` and `Alternating Strike`.
	- If `AOE Strike` and `Inverted Strike`, the slots to attack be will now be done in a counter-clockwise manner. For example,`left adj, left opposing, center, right opposing, right adj` now becomes `right adj, right opposing, center, left opposing, left adj`.
	- If `AOE Strike` and `Alternating Strike`, same as below:
	- If `AOE Strike` and `Inverted Strike` and `Alternating Strike`, the slots to attack be will now be done in a counter-clockwise alternating manner. For example,`left adj, left opposing, center, right opposing, right adj` now becomes `right adj, left adj, right opposing, left opposing, center`.
	- Unsure of how to handle having `Inverted Strike` and `Alternating Strike` as the slot targeting slot is confusing to handle...

### 2.7.1

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Made `HellHound` no longer selectable in rare chest. Hope you had fun while it lasted!

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed `Amoeba` showing up as both a rare and normal card.

- ![Feature](https://i.imgur.com/uL9uYNV.png) Added new Sawyer blocker piece courtesy of `Catboy Stinkbug#4099`.

- ![Feature](https://i.imgur.com/uL9uYNV.png) Added short and long dialogue outros for `Boneyard` sequencer courtesy of `Mr. Etc.#3925` & `Bt Y#0895`.

- ![Feature](https://i.imgur.com/uL9uYNV.png) Increased collider box for the hammer so that it's much easier to click.

- ![Feature](https://i.imgur.com/uL9uYNV.png) Added head movement for Kaycee and Sawyer boss pieces.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Moved playing boss themes to start of IntroSequence as opposed to end, except for Royal.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Cannon theme for Royal is played first, then the main theme. Flows better. Thanks to `Catboy Stinkbug` for the suggestion.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Changed ability for `HellHound` to be a `VariableStateBehaviour` SpecialBehaviour.

### 2.7.0

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Potentially fixed issue interacting with nodes at x0 y0 by just moving them to any other place on the board. Not sure why x0 y0 has issues with interaction.

- ![Feature](https://i.imgur.com/uL9uYNV.png) New boss themes courtesy of `Akisephila (Addie Brahem)`

- ![Feature](https://i.imgur.com/uL9uYNV.png) You now gain +1 `Starting Bone` for each boss you have defeated in the current run.

- ![Feature](https://i.imgur.com/uL9uYNV.png) The `Sawyer` boss now has unique mechanics, like stealing bones for you and spawning a certain hound of his.

- ![Feature](https://i.imgur.com/uL9uYNV.png) Added new Kaycee and Sawyer boss figurines courtesy of `Catboy Stinkbug#4099`.
	- Model updates for Sawyer courtesy of `Pink#9824`

- ![Feature](https://i.imgur.com/uL9uYNV.png) Added new `Flames` artwork and `FlameStrafe` ability icon courtesy of `Cevin_2006`.

### 2.6.6

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed menu card flying off the screen with one line during creation of menu card: `MenuController.Instance.cards.Add(menuCardGrimora)`.

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed softlock if boss skull is null as there was no null check.

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed issue where `Stinky` sigil would debuff both twin giants. The vanilla code handles `Giant` cards as if it was the moon and checks all player slots, and not just the opposing slots of the `Giant` card.

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed softlock after defeating Grimora and chessboard does not load.

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed Grimora queueing player cards that have died during second phase.

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed `Flying` & `Submerge` combo not showing the card being submerged.

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed issue with a card having both `Strafe Push` and `Skin Crawler` where the card would force itself out of the other card's skin, causing a softlock. It is now no longer possible to receive the `Skin Crawler` sigil if the card has `Strafe Push` and vice versa.

- ![Feature](https://i.imgur.com/uL9uYNV.png) Added new `Ember Spirit`, `Plague Doctor`, and `Vengeful Spirit` artwork courtesy of `Cevin_2006`.

- ![Feature](https://i.imgur.com/uL9uYNV.png) Added new Sawyer Boss Skull and Hammer model courtesy of `Pink#9824`.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Removed `Bonelord` from rare card pool entirely. Wasn't really meant to be playable, but hope anyone had fun while it lasted!

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Moved dialogue event `RoyalBossPreIntro` from Royal to Grimora as it fits better.

### 2.6.5 HOTFIX

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed softlock when choosing cards and one of those cards has an energy cost. The problem was that I was trying to get the `PlayableCard` component from the parent, _as opposed to literally just using the CardRenderInfo object that is passed in `GravestoneRenderStatsLayer.RenderCard`_ method.

### 2.6.4

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed card's with flying appearing no longer flying when attacked or is attacking. Animation resets loop when attacking or being attacked, but it's better than no longer flying.

- ![Feature](https://i.imgur.com/uL9uYNV.png) Added initial prefab for energy cells to show on cards. Will be tweaked accordingly.

- ![Feature](https://i.imgur.com/uL9uYNV.png) Added new `Poltergeist` artwork provided by `Cevin_2006`.

- ![Feature](https://i.imgur.com/uL9uYNV.png) Updated Kaycee's boss skull model courtesy of `Pink#9824`.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Potentially fixed issue with having your current save already at Grimora's finale breaking the mod. This should now allow you to continue from any point as long as you are not at the finale.

### 2.6.3

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Changed setting boss skull transform for Kaycee to hardcoded position, rotation, and scaling.

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed issue with NRE being thrown if `Reset Run` button was clicked. The issue was that the getter for finding Grimora's right wrist was static.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Removed `Buff Enemy (Annoying)` and `Random Consumable` ability from Electric Chair.

### 2.6.2

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Hopefully fixed weird NRE with finding Royal's skull.

### 2.6.1

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Fixed softlock in Grimora's finale phase.

- ![Feature](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/new.png) Added new `Bonelord` artwork and emission courtesy of `Ryan S. Art`!

- ![Feature](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/new.png) Added new Kaycee boss skull courtesy of `Pink#9824`!

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Separated out dialogue lines after beating Royal so it looks less jumbled.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Tweaked card's light color in Grimora fight to be a bit light so the art is more visible.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Removed ice break counter for Kaycee so that she breaks Draugrs when possible.
  - Also removed clearing the board in the final phase of Kaycee.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Cleaned up descriptions of some cards to have correct punctuation and spacing.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Corrected viewing of queue slots to be before the start of the Bonelord phase instead of after.

### 2.6.0

#### Card Changes

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Changed `Amoeba` random ability to now only choose from the list of abilities that the Electric Chair choses from. This is due to the fact that `Amoeba` can be given an ability that breaks the game in unintended ways.
  - `Amoeba` now has a modified version of RandomAbility. The ability description may not show up correctly in Deck Review, just as a heads up.

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Fixed `Screaming Skull` damage done to now correctly deal damage to the owner of the card.

- ![Feature](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/new.png) Added new `Wendigo` and `Wyvern` artwork courtesy of `Cevin2006™ (◕‿◕)#7971`.

#### Bugfixes/Features/Refactors

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Fixed issue with `Giant` cards in phase 2 fight of Grimora throwing exceptions when spawning. Issue was that I did not add the `Giant Strike` ability to the card pool...

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Fixed issue with Hoarder ability (Tutor) throwing an exception when attempting to pick a card from your deck. The issue was caused by the fact that in the finale, there is no GameObject to handle the Hoarder ability. Therefore, it throws an exception.

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Fixed `DrawNewHand (Handy)` ability to now correct the visual issue of cards still appearing in draw pile after drawing.

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Fixed issue with card removal node decreasing card costs in the negatives.

- ![Feature](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/new.png) Electric Chair sequence now uses new model provided by `Pink#9824`.
  - Moved confirm stone button closer to chair.
  - Changed camera rotation during sequence to 32.5 from 37.5.

- ![Feature](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/new.png) New hammer dialogue courtesy of `Mr. Etc.#3925`.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Ordered decklist alphabetically in viewing cards left in the deck.

### 2.5.10

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Fixed logic in Kaycee's upkeep phase to now look for the Draugr's correctly.

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Hopefully fixed issue with duplicate rare cards showing up in choice selection.

- ![Feature](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/new.png) Added toggle button for viewing cards left in the deck during battle! Auto-updates in realtime as well.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Removed `Reset Deck` button as it doesn't really get used besides for debugging purposes, and `Reset Run` will most likely be used if the player wants to restart their deck.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Card changes:
  - Changed `Revenant` to use original sprite and emission.
  - Added vanilla `Banshee` to card pool.
  - Disabled attack and health shadows as sometimes the shadows don't correlate to the number and the overlapping is ugly.

### 2.5.8

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Corrected `Double Death` rulebook name to no longer be `Handy`. Funnily enough, the game has the ability page set with the name `Handy`. Probably because it was never actually meant to be used in an other act other than 2.

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Corrected logic for determining opening hand to at least always guarantee a `Bonepile` or `Gravedigger`.

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Corrected `Erratic` ability logic to now no longer force itself onto other cards.

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Set default ice cube creature within to `Skeleton` if the card would receive Ice Cube ability from Electric Chair.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Card changes
  - Changed `Skeleton Army` ability to now create a `Skeleton` in all owner owned open slots when played.
  - Lowered `Dans Macabres` Bone cost to 5
  - Lowered `Screaming Skull` Bone Cost to 2

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Electric Chair changes:
  - Removed more abilities from Electric Chair sequence:
    - Activated Sacrifice Draw Cards (True Scholar)
    - Brittle
    - Buff Gems (Gem Animator)
    - Create Bells
    - Create Dams
    - Draw Ants
    - Draw Copy on Death
    - Draw Random Card On Death
    - Drop Ruby on Death
    - Draw Vessel on Hit
    - Latch Brittle
    - Latch Death Shield
    - Latch Explode on Death
    - Shield Gems (Gem Guardian)
  - Updated dialogue.
  - Reduced number of times to electrify card to a max of 2 times.
  - Changed texture of selection slot to be a plus sign with a question mark instead of 3 question marks.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Changed `Sporedigger` description to better fit the card.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Reduced `Skeleton Army` activated bone cost from 2 to 1.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Made hammer only usable up to 3 times per battle. The reasoning for this is that the hammer use should be used when necessary, and not a large crutch.

### 2.5.7

- ![Feature](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/new.png) Redid all of the Chessboard layouts...again!
  - The new nodes are now introduced gradually
  - Slightly less linearity. Beat optional enemies to gain access to more nodes.

- ![Feature](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/new.png) Difficulty rebalance!
  - Blueprints now follow a difficulty curve.
  - Kaycee now has a new Boss Mechanics, making the first map slightly easier.
  - Royal's and Grimora's areas have been buffed to provide for a more challenging experience given the new chess pieces.
  - Rebalanced the new cards to make them more interesting and less underpowered.
  - Removed the BoneLord sigil from the Electric Chair ability pool :)

### 2.5.6

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) HOTFIX - Removed checking for `DataFiles` and `Artwork` folders as some mod managers appear to extract the files inside them to the main directory?

### 2.5.5

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) HOTFIX - Issue with attempting to wipe deck if Grimora's deck contained previous GrimoraMod cards that were prefixed with `ara_`. This is throwing an exception because accessing the deck means calling `CardLoader.GetCardByName()`, which throws the exception.

### 2.5.4

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) HOTFIX - Royal was using original vanilla battle sequencer and not the new one, therefore unable to progress. The new GrimoraMod battle sequencers are now prefixed woith `GrimoraMod` so that I don't accidently set the wrong one again...

### 2.5.3

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) HOTFIX - Fixed issue with death touch not working. Problem was that there is a check for a certain card that the result should have been negated and it was not.

### 2.5.2

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) HOTFIX - Problem was on very first load, no cards are loaded at all. Default base cards are now used and then reset later.

### 2.5.0

#### Feature

- ![Feature](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/new.png) Added 3 new special node pieces!
  - Boneyard: Bury one of your cards to give it Brittle and halve it's bone cost rounded up. For example, 9 bones becomes 5.
  - Card Removal: Remove a card to give another card (or maybe your entire deck?!) in your deck a random special effect.
  - Electric Chair: Give one of your cards a random positive sigil. This will not give your cards blood related sigils like `Sacrificial`.
  - Goat Eye: Unsettling

- ![Feature](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/new.png) Grimora phase 2 redone to be more difficult.

- ![Feature](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/new.png) Grimora has a new set of blocker pieces!

- ![Feature](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/new.png) Added custom animations to make the skeleton arm attack animation work for both player and opponent sides.

- ![Feature](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/new.png) Added multiple cards from `Bt Y#0895`, included with artwork!

#### Cards

- **Boo Hag** - 1,1 - ![cost_bone](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_bone.png)![cost_bone_5](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_bone_5.png) Sigils: Skin Crawler.

- **Danse Macabre** - 3,3 - ![cost_bone](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_bone.png)![cost_bone_8](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_bone_8.png) Sigils: Alternating Strike, Erratic.

- **Dybbuk** - 0,1 - ![cost_bone](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_bone.png)![cost_bone_6](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_bone_6.png) Sigils: Possessive.

- **Giant** - 2,7 - ![cost_bone](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_bone.png)![cost_bone_1](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_bone_1.png)![cost_bone_5](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_bone_5.png) Sigils: Bone King, Bifurcated Strike.

- **Project** - 1,1 - ![cost_bone](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_bone.png)![cost_bone_8](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_bone_8.png) Sigils: Trifurcated Strike.

- **Ripper** - 6,6 - ![cost_bone](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_bone.png)![cost_bone_1](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_bone_1.png)![cost_bone_2](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_bone_2.png) Sigils: Brittle.

- **Screaming Skull** - 1,1 - ![cost_bone](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_bone.png)![cost_bone_6](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_bone_6.png) Sigils: Area Of Effect Strike.

- **Silbon** - 3,2 - ![cost_bone](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_bone.png)![cost_bone_7](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_bone_7.png) Sigils: Inverted Strike, Sprinter.

#### Sigils

- **Alternating Strike** - [creature] alternates between striking the opposing space to the left and right from it.
- **Area Of Effect Strike** - [creature] will strike it's adjacent slots, and each opposing space to the left, right, and center of it.
- **Erratic** - At the end of the owner's turn, [creature] will move in a random direction.
- **Inverted Strike** - [creature] will strike the opposing slot as if the board was flipped. A card in the far left slot will attack the opposing far right slot.
- **Possessive** - [creature] cannot be attacked from the opposing slot. The opposing slot instead attacks one of it's adjacent slots if possible.
- **Skin Crawler** - When [creature] resolves, if possible, will hide under an adjacent card providing a +1 buff. Otherwise, it perishes. Cards on the left take priority.

#### Bugfix

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Fixed possible infinite loop if the player would have fewer than 3 cards in their deck when starting a battle.

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Set cardbacks to gravestones in Rare Card choice. Before it was the default paper cardbacks from Act 1.

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Fixed NPE in GameEnd method for `GrimoraModBattleSequencer` to now correctly reset the run if the player has lost with no NPE.

#### Refactor

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Added PrefabConstants class for ease of grabbing prefabs and other objects.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Changed `Object.Find` calls to use `.Instance` calls where necessary. This is because `.Instance` will automatically give me the object I need to destroy/check.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Removed patch for `ChessboardMap` as the `UnrollingSequence` will never be called as it replaced by the extension class `ChessboardMapExt`.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Deactive beginning animation for original `ChessboardMap` class in the scene so the transition looks cleaner.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Updated `CardBuilder` to mimic API2.0 until kmod is officially released.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Removed power level in creation of ability as it was never used.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Replaced config entries for each boss to just `BossesDefeated`.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Added thrown exceptions in `FileUtils` to check if the `Artwork` and `DataFiles` folder exists.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Removed unused fields in `GrimoraChessboard` as the getters are the only ones used.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Cleaned up `SetupBoard` in `GrimoraChessboard` to use `PlacePieces<T>` generic method.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Added `Resources.UnloadUnusedAssets()` in `OnDestroy()` method as I think it was causing a lot of memory issues without it. Still not sure...

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Added `RandomUtils` class for having a method that will return a list of 3 random choices given a metacategory and list of cards.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Cleaned up a bunch of comments.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Moved patches to Patches directory. This is to keep patches in their own directory to easily differentiate what is custom class and what is a modifciation of an existing class.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Moved config logic and setup to own ConfigHelper class.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Updated Start patch for chests to now only set the NodeData type if it is null. This is so chests can placed with a specific node type.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Added DebugHelper class for debug related tools.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Changed ApiUtils creation of abilities to now set the rulebook name based on the type passed in, or as an optional param for setting a specific name. This just means less keystrokes when creating abilities.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Multiple classes cleaned up and reformatted for easier reading.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Renamed `CreateInsertAbilityNameHere` to just `Create`

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Added emission sprites to Sprite bundle, and logic to add emission sprites to `NewCard.emissions`.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Added extra builder methods for setting abilities.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Modified build command in project to include artwork folder.

### 2.4.1

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Fixed softlock after defeating Grimora for the first time.

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Fixed Bone Lord weakening himself instead of the player.

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Fixed issue where if a player would close the game during the rare card choice sequence after defeating a boss, the new boss does not spawn and there isn't a way to go into the next region without closing the game and removing the boss piece from the removed pieces entry in the config file.

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Fixed scaling on Plague Doctor and Summoner art.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Tweaked Grimora's second phase to no longer give a card +1 attack if it had zero.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Updated ability rulebook names to have spaces.

### 2.4.0

#### Features

- ![Feature](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/new.png) Added new
  energy object provided by Draconis17#3692!

  - Unfortunately Unity does NOT like external prefab objects. Will have to keep playing around with it to make it show up... :(

- ![Feature](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/new.png) Changed
  energy cell color from light blue to Grimora's Text Display color. When able to play an energy card, highlighted cells
  are now a dark yellow, similar to the emissions on the gravestone cards.

- ![Feature](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/new.png)
  Removed `EnemyBattleSequencerPatches` in favor of custom SpecialBattleSequencer types `GrimoraModBattleSequencer`
  and `GrimoraModBossBattleSequencer` for better control over what happens where instead of modifying a class with a
  patch.

- ![Feature](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/new.png)
  Added `GravestoneCardAnimationControllerPatches` for now being able to rotate the skeleton attacking arm correctly if
  a card has multistrike (Crazed Mantis and Hydra for example).

#### Bugfixes

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Fixed
  camera view when third candle is being added in Grimora boss fight.

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Fixed
  softlock during Grimora's final phase.

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Fixed
  Grimora queuing up too many cards.

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Fixed GBC
  abilities like SkeletonStrafe (Skeleton Crew rulebook name) not appearing correctly.

  - All abilities should now show up correctly, which includes the GBC abilities.

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Fixed
  sizing and scaling of card art to better fit inside gravestones.

  - Bonelord and Dead Hand still have scaling/portrait overlaps until fixed.

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Corrected
  outros for each boss to now play the correct dialogue if the player won or lost.

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Fixed
  dialogue not playing during Grimora's second phase.

#### Refactors

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Changed
  CardBuilder methods to now be prefixed with `Set` instead of `With`.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Updated logic for resetting run to now wipe the deck, reset the config, and start you back at the tombstones falling.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Removed
  disabling act 1/2 cards from showing up as now the select card choices events now look for GrimoraMod specific cards.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Updated
  positions of masks to be closer to Grimora's face now.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Added
  blueprints to bosses when chess pieces are created instead of at intro sequence.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png)
  Removed `BossDefeatedSequence` patches.

### 2.3.3

#### Features

- ![Feature](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/new.png) Added energy
  drone!

- ![Feature](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/new.png) Added **
  Reset Deck** button to now reset your deck back to the starter deck in case deck viewing throws errors.

#### Bugfixes

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Corrected
  logic for adding bones to the start of combat once a boss has been defeated.

  - Kaycee defeated = 2 bones
  - Sawyer defeated = 5 bones
  - Royal defeated = 10 bones

#### Refactors

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Updated
  project to net6.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Removed
  WithPortrait method call for cards to now load portraits based on names.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Had to
  undo Resource-based image loading as net6 threw a slew of errors at startup.

  - Images are now loaded through asset bundles.

### 2.1.2

#### Bugfixes

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Fixed
  Blueprints

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Minor
  patches and slightly changed Kaycee's mechanics.

### 2.1.1

#### Features

- ![Feature](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/new.png) Flavor
  update.

- ![Feature](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/new.png) Added tons
  of new dialogue.

- ![Feature](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/new.png) Added **
  Reset Run** button (this only resets all data values in the config, not your deck).

### 2.1.0

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) MASSIVE
  refactor for readability and overall code quality!

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Added
  custom config file `grimora_mod_config.cfg` to keep track of bosses defeated and active/removed pieces.

#### Features

- ![Feature](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/new.png) Added 53
  random chessboard layouts in JSON format for 'random' setups.

  - This is a now a future QOL as randomized boards look ugly as hell.

- ![Feature](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/new.png) Added Deck
  Review board view!

  - If you are unable to click on a chess node after viewing your deck, try viewing your deck again.

#### Bugfixes

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Fixed issue
  with custom card sprites being positioned too low.

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Fixed issue
  with loading the pieces back in their respective nodes if the player quit.

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Fixed issue
  with player chesspeice appearing at previous node after region change.

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Fixed issue
  with player chesspiece not spawning if there are no available nodes to spawn in for the row.

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Fixed issue
  with softlocking on final boss!

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Fixed issue
  with pieces overlapping and not being destroyed correctly causing newer pieces to never be created.

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Fixed issue
  with defeating bosses throwing an exception when changing to a new map.

#### Refactors

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Encounter
  blueprints reworked into own class.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Made
  resource files of all artwork for significantly easier sprite loading.

### 2.0.2

#### Features

- ![Feature](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/new.png) Balance
  overhaul

- ![Feature](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/new.png) Added 30
  more Blueprints

#### Bugfixes

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Fixed
  Blueprints

## Full Credits

### Misc:

- kisephila (Addie Brahem) for the Soundtrack

- Cactus (cactus#0003) for making the official Trailer

- Arackulele for Balancing, polish, and other miscellanious things

- JulianMods (xXxStoner420BongMasterxXx) for refactoring the code.

### Code:

- JulianMods (xXxStoner420BongMasterxXx) for being the Main Developer

- Arackulele for additional programming

### Artists:

- LavaErrorDoggo (LavaErrorDoggo#1564) for making the Original Act 2 Cards but in full Size Artwork

- Bt Y#0895 for currently working on artwork for the mod

- Cevin_2006 (Cevin2006™ (◕‿◕)#7971) for additional Card art

- Arackulele for additional Card art

- Lich Underling (Lich underling#7678) for additional Card Art

### 3D Models

- Pink (Pink#6999) for making the Boss Skull Models , currently working on a full crypt 3D Model, etc

- Catboy Stinkbug (Catboy Stinkbug#4099) for the Board Skull 3D Models

- Draconis17#3692 for the new energy cells game object.

### Dialogue:

- Primordial Clok-Roo (The Primordial Clok-Roo#2156) for a ton of future Dialogue

- Bob the Nerd (BobTheNerd10#2164) for some event dialogue

- Arackulele, for the original Dialogue

- JulianMods (xXxStoner420BongMasterxXx) for additional Dialogue
