# Grimora Mod

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

### Unable to right-click stat icons like Ant or Bellist

- Unfortunately this one is also pretty hard to track down. Will have to play around and see what the main differences are because I can't really see what's different between the Act 1 and Grimora's card render.

### Activated abilities no longer work

- The primary cause of this is whenever another ability gets added to the card. The bug lies in the `Activated Ability Fix` mod, but I'm not sure where.

### A card with Area of Effect Strike, Tri Strike, and Sniper sigils only allows 3 attacks

- Believe it or not, this is how the vanilla game code for the `Sniper` ability is handled. It doesn't base it off how many attacks you're doing, it hard codes to either 2 for `Split Strike` or 3 for `Tri Strike`.

### Current save file is already at the finale with Grimora

- Make a backup of your save, then delete your current save. Having your current save already at the finale seems to break the mod.
- Possibly fixed in 2.6.4 update.

## Update Notes

### 2.8.1

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Corrected issue with rulebook not showing all the abilities.

### 2.8.0

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed teeth not disappearing from cards that attack their owners.

  - For example, AOE Strike and now Raider. The reason being is that teeth only get cleared if `DamageDealtThisPhase` is greater than zero. There is logic for the `CombatPhaseManager.SlotAttackSequence` patch that I made to minus the damage done to this field so that it would correctly add the damage to the scale for the respective owner.
  - Now there is logic in that patch right after subtracting the damage to call `CombatPhaseManager3D.VisualizeDamageMovingToScales` and clear `damageWeights` so that it will correctly remove the teeth from the board.

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) FIXED ANNOYING BLUE LIGHT ON BOSS SKULL AFTER ROYAL'S FIGHT.

  - Apparently when the cannons are created, there is 2 spotlights created that don't get destroyed when they glitch out.

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed hammer dialogue playing each battle instead of only once.

  - Problem was that I was only setting to not play the dialogue, only if you used the hammer the 3rd time.
  - If you only used it once or twice each fight, it will play each fight.

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Giant cards will now be buffed from both side of the card if the adjacent friendly cards have `Leader`.

  - For example, if Bonelord has a Bone Prince on the left and right side of it, then the total buff will be 2 instead of 1

- ![Feature](https://i.imgur.com/uL9uYNV.png) Reset button is now only available to click in the Pause Menu.

- ![Feature](https://i.imgur.com/uL9uYNV.png) Explorable crypt! Although it's mostly the building, and not a whole lot in it at the moment.

- ![Feature](https://i.imgur.com/uL9uYNV.png) Added button to get up from the chessboard!

- ![Feature](https://i.imgur.com/uL9uYNV.png) 3 more `Skeleton` cards are added to side deck.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) If no chess pieces have been interacted with at all, starting position for player piece will be defaulted. Meaning, if you haven't battled or opened a chest, your position will reset back to the start position for that board.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Stinkbug will now glitch out when dying if it hasn't been clicked yet.

#### Boneyard

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Cards having 4 abilities cannot be chosen for the Boneyard event.

  - Breaks the card rendering unfortunately.

#### Electric Chair

- ![Feature](https://i.imgur.com/uL9uYNV.png) Changed campfire sounds to be more electric.

- ![Feature](https://i.imgur.com/uL9uYNV.png) Cards that have been Electric Chaired will now have a permanent blue emission.

- ![Feature](https://i.imgur.com/uL9uYNV.png) If the Electric Chair would give `Swapper` sigil, a new sigil will be randomly chosen if the card has zero attack or fewer than 3 health.

  - This helps to avoid a card committing forever sleep on first hit.

#### Bosses

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Boss skull during boss fight will glitch out when dying.

##### Kaycee

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Corrected issue with Submerge being removed from all cards instead of the card that was frozen during Kaycee's fight. (Thanks Magnificus!)

- ![Feature](https://i.imgur.com/uL9uYNV.png) Cards with `Hook Line and Sinker`, `Possessive`, or `Waterborne` will now lose that ability when frozen, and regain it back once unfrozen. Causes the lane to die entirely and not be useable...

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Chess figurine icicle now looks more ice-like. Ice is hard to look like ice.

##### Royal

- ![Feature](https://i.imgur.com/uL9uYNV.png) Fleshed out more of Royal's first and second phases.

- ![Feature](https://i.imgur.com/uL9uYNV.png) New phase 2 music from `Akisephila (Addie Brahem)`.

##### Grimora

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed issue with Twin Giants spawning.

  - The primary issue seemed to be how the creation of the abilities were done. No more direct static calls!

- ![Feature](https://i.imgur.com/uL9uYNV.png) New Bonelord entrance!

- ![Feature](https://i.imgur.com/uL9uYNV.png) The giants now have personal names! I wouldn't make one mad though...

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Will now only reanimate every other card.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) `Giant Strike` reworked with a new description.

  - `[creature] will strike each opposing space. If only one creature is in the opposing spaces, this card will strike that creature twice.`

- ![Refactor](https://i.imgur.com/5bTRm1B.png) `Bonelord` is now 1 attack 20 health.

#### Ability/Card Changes

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed potential softlock if two `Screaming Skull` cards are on the board and the first one dies to `Leaping Trap`.

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed potential softlock if a card with `Sprinter` and `Submerge` dies from `Sentry` after moving into the new slot.

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed cards with flying no longer flying after attacking.

- ![Feature](https://i.imgur.com/uL9uYNV.png) New pirate cards, regular cards, and abilities! Art courtesy `Bt Y#0895`.

  - Captain Yellowbeard: 2/2, 7 Bones with `Sea Shanty`.
  - First Mate Snag: 2/2, 7 Bones with `Hook Line And Sinker`.
  - Privateer: 1/1, 4 Bones with `Sniper`.
  - Swashbuckler: 1/2, 0 Bones with `Raider`. Not obtainable.
  - Vellum: 0/2. Spawned from `Leaping Trap` instead of a normal pelt.

  - Abilities:
    - Raider: [creature] will strike it's adjacent slots, except other Raiders. Icon courtesy of `Blind, the Bound Demon#6475`.
    - Sea Shanty: [creature] empowers each Skeleton on the owner's side of the board, providing a +1 buff their power. Icon courtesy of `Blind, the Bound Demon#6475`.
    - Hook Line And Sinker: When [creature] perishes, the creature in the opposing slot is dragged onto the owner's side of the board. Icon courtesy of `Bt Y#0895`.

- ![Feature](https://i.imgur.com/uL9uYNV.png) New art for `Disinter` and `Screeching Call` abilities, courtesy of `Blind, the Bound Demon#6475`.

- ![Feature](https://i.imgur.com/uL9uYNV.png) New art for multiple cards.

  - Drowned Soul, courtesy of `Bt Y#0895`.
  - Ghost Ship, courtesy of `Cevin2006™ (◕‿◕)#7971`.
  - Poltergeist, courtesy of `Cevin2006™ (◕‿◕)#7971`.
  - The Walkers, courtesy of `Catboy Stinkbug#4099`.
  - Zombie, courtesy of `Bt Y#0895`.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Set all cards to `Undead` temple so they don't show up in Act 1.

### 2.7.5

- ![Feature](https://i.imgur.com/uL9uYNV.png) Added more debugging tools for adding Grimora Mod cards to either your hand or deck. There is now also a dropdown for custom added cards.

  - Enable developer mode in config file.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) `Skeleton` is now the default card that will spawn from `Ice Cube`.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Added try/catch blocks for `ProgressionData.UnlockAll` and `PlayGlitchOutAnimation` for better error logging.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Removed `Skin Crawler` from sigil pool. Until I can spend an entire day or 2 debugging this ability, it will be removed for the time being. It is the cause of way too many outlier softlocks or issues during battle and it's both frustrating for the player and myself personally.

### 2.7.4

- ![Feature](https://i.imgur.com/uL9uYNV.png) Can now access rulebook after adding a new sigil to a card in the Electric Chair sequencer.

  - This also means that you can right-click the newly added sigil so you can see wtf it is.

- ![Feature](https://i.imgur.com/uL9uYNV.png) Can now view your deck during normal or rare card choices.

- ![Feature](https://i.imgur.com/uL9uYNV.png) Added `Bellist` and `Spirit Bearer` to Electric Chair ability pool.

- ![Feature](https://i.imgur.com/uL9uYNV.png) Added new rare card appearance!

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Removed negative effects from Card Remove sequence. Community feedback overall did not like sacrificing a card and still getting fucked over (Bone Lord was a little too harsh...).

#### Bosses

##### Kaycee

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Added more dialogue to better indicate how the mechanics work.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Slight nerf to freeze mechanic from 4 to 5.

##### Sawyer

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Added more dialogue to better indicate how the mechanics work. Player now only gets 1 bone at the start of phase 2.

##### Grimora

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Hopefully fixed issue with phase 2 not spawning the twin giants. Cards will no longer be weakened going into phase 2.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Reduced twin giant attack from 2 to 1. Increased health by 1.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Removed reanimation sequence entirely. With `Double Death` especially, the difficulty ramp for the first phase due to this mechanic is just too overwhelming and can heavily swing the board in Grimora's favor in just 2-3 turns.

#### Ability/Card Changes

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Reworked the logic for `Skin Crawler`, again.

  - Fixed potential softlock if `Boo Hag` is hiding under a card when you die.
  - New description: [creature] will attempt to find a host in an adjacent friendly slot, hiding under it providing a +1/+1 buff. Cards on the left take priority.

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Adjusted `Death Knell` emission art to line up.

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Added special stat icons for `Catacomb` and `Death Knell`.

- ![Feature](https://i.imgur.com/uL9uYNV.png) Multiple new card arts

  - `Bone Lord's Horn` art from `Cevin2006™ (◕‿◕)#7971`.
  - `Bone Prince` art from `Cevin2006™ (◕‿◕)#7971`.
  - `Headless Horseman` art from `Cevin2006™ (◕‿◕)#7971`.
  - `Hydra` art from `Cevin2006™ (◕‿◕)#7971`.
  - `Skelemagus` art from `Cevin2006™ (◕‿◕)#7971`.
  - `Summoner` art from `Cevin2006™ (◕‿◕)#7971`.

- ![Feature](https://i.imgur.com/uL9uYNV.png) Added multiple new basic cards. All art from `Bt Y#0895`.

  - Centurion: 1/4, 6 Bones, `Armored`. Description courtesy of `Bt Y#0895`.
  - Compound Fracture: 1/2, 4 Bones, `Sharp Quills`. Description courtesy of `BONG MASTER`.
  - Dalgyal: 0/2, 2 Energy, `Sentry`. Description courtesy of `TheGreenDigi#8672`.
  - Deadeye: 1/1, 5 Bones, `Hoarder`. Description courtesy of `TheGreenDigi#8672`.
  - FesteringWretch: 1/1, 3 Bones, `Stinky`. Description courtesy of `Blind, the Bound Demon#6475`.
  - Manananggal: 3/3, 8 Bones, `Flying`. Description courtesy of `TheGreenDigi#8672`.
  - Will 'O' The Wisp. 0/1, 1 Bone, `Spirit Bearer` (Gain Battery but themed for Grimora). Description courtesy of `Blind, the Bound Demon#6475`.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Reduced cost of `Wyvern` to 3 bones from 4. Reduced energy cost of `Screeching Call` from 3 to 2.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) `Wendigo` has been removed from the card pool.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) `Inverted Strike` and `AOE Strike` icons are now flipped when played by the opponent.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Corrected visual issue with `Possessive` sigil where the opposing card would look like it's attacking the adjacent slots of the card bearing `Possessive`, and not the adjacent friendly slots.
  - Updated description to: [creature] cannot be attacked from the opposing slot. The opposing slot, if possible, instead attacks one of its adjacent friendly cards.

### 2.7.2

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed visual issue with energy decals showing through other cards in `Deck View`.

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed issue where you could spam click the `Deck View` button and continue overlapping the cards in your deck.

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed issue with `Electric Chair` where it was possible to add 2 more abilities if the electrocuted card had 3 abilities to start.

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed issue with hammer interaction when a card in your hand has `Corpse Eater` and `Hoarder`.

  - When the hammer is used, it disables the cursor so you can't interact with anything until the hammer sequence finishes. The problem is that with `Corpse Eater` and `Hoarder`, is that the hammer sequence doesn't finish until after you choose a card from the `Hoarder` sequence, but you can't choose a card because the hammer disabled your cursor. Hence, softlock.

- ![Feature](https://i.imgur.com/uL9uYNV.png) Added hammer dialogue option in config file. The hammer also starts to be appear more used after each use.

  - 0 = Disable hammer dialogue entirely.
  - 1 = Play only once for the entire session. (default)
  - 2 = Play dialogue each battle.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Card Removal, Boneyard, and Electric Chair initial dialogue now only gets played once, similar to how the vanilla game does not play certain dialogues once you've done it before.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Re-positioned `retrieveCardInteractable` in Electric Chair sequencer so that it's easier to take the card away from the chair.
  - Before, the slot was still positioned as if it was on the ground flat, so that you had to click between the chair and the stone.

#### Bosses

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) _Actually_ fixed the boss skull that Grimora holds glitching out exception that I thought I fixed in 2.6.6.

##### Kaycee

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed issue with cards that have been ice cubed by Kaycee not having all their abilities intact when being broken out of the ice.

  - The reason for this is that the original `IceCube` logic creates the card in the slot _by the name_ and NOT by the `CardInfo` object. Meaning, whatever the vanilla card attributes are, is what spawns, not the one in your deck.

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed icicle on Kaycee's figurine to now properly move with the head.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Tweaked Kaycee boss logic so the card freezing is less frustrating.

##### Sawyer

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed issue with Sawyer taking a bone when a player would have zero bones, causing the PlayerBones to go in the negative.
  - This would cause the player to be unable to play zero cost bone cards like Skeleton.
  - Sawyer will now only take 1 bone from the player if the player has at least 2 bones or more.

##### Royal

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Reduced cannon theme to half volume so your eardrums don't hurt.

##### Grimora

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed potential softlock in Grimora's fight if a card had `Possessive`.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Changed Grimora's theme to `Corrupted Queen` from `Akisephila (Addie Brahem)`.

#### Ability/Card Changes

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed multiple issues with `Skin Crawler` ability.

  - New description: When one of your creatures is placed in an adjacent space to [creature], [creature] will hide under it providing a +1 buff. Cards on the left take priority.

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed issue with `Handy` ability drawing a new hand for the player if the card would die and then reanimated during the first phase of Grimora's fight.

- ![Feature](https://i.imgur.com/uL9uYNV.png) New ability icon for `Skeleton Horde` courtesy of `Blind, the Bound Demon#6475`.

- ![Feature](https://i.imgur.com/uL9uYNV.png) New card, `Catacomb` from `Bt Y#0895`. 10 cost, 0/10 with `Lammergeier` (affects attack only).

- ![Feature](https://i.imgur.com/uL9uYNV.png) New card, `Death Knell` from `Bt Y#0895`. 8 cost, 0/2 with `Bell Ringer` and `Bellist`.

- ![Feature](https://i.imgur.com/uL9uYNV.png) New `Mummy` and `Sarcophagus` art courtesy of `Bt Y#0895`!

- ![Feature](https://i.imgur.com/uL9uYNV.png) Added `Exploding Pirate` card. 1 Bone, 2/2 with `Lit Fuse` sigil. Initial art from `Lich underling#7678`, with modifications from `Ara`.

  - `Lit Fuse`: [creature] loses 1 health per turn. When [creature] dies, the creature opposing it, as well as adjacent friendly creatures, are dealt 10 damage.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Changed `Tomb Robber` ability to now be `Disinter`: Pay 2 bones to create a skeleton in your hand.

  - Similar to the act 2 ability, but nerfed to cost 2 bones instead of 1.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Nerfed `Wendigo` attack from 2 to 1, and set as a normal type of card.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) `Zomb-Geck` no longer appears as a rare card. Was more or less meant as filler until more cards were added.

  - Maybe used for future event?

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Added description for `Banshee`.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Added `Bonehound` to normal card pool.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Updated `Skeleton Army` ability, `Skeleton Horde`, description to better clarify what it does.

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

- ![Feature](https://i.imgur.com/uL9uYNV.png) Added new `Flames` artwork and `FlameStrafe` ability icon courtesy of `Cevin2006™ (◕‿◕)#7971`.

## Full Credits

### Misc

- Akisephila (Addie Brahem) for the Soundtrack

- Cactus (cactus#0003) for making the official Trailer

- Arackulele for Balancing, polish, and other miscellanious things

- JulianMods (xXxStoner420BongMasterxXx) for refactoring the code.

### Code

- JulianMods (xXxStoner420BongMasterxXx) for being the Main Developer

- Arackulele for additional programming

### Artists

- LavaErrorDoggo (LavaErrorDoggo#1564) for making the Original Act 2 Cards but in full Size Artwork

- Bt Y#0895 for currently working on artwork for the mod

- Cevin_2006 (Cevin2006™ (◕‿◕)#7971) for additional Card art

- Arackulele for additional Card art

- Lich Underling (Lich underling#7678) for additional Card Art

### 3D Models

- Pink (Pink#6999) for making the Boss Skull Models , currently working on a full crypt 3D Model, etc

- Catboy Stinkbug (Catboy Stinkbug#4099) for the Board Skull 3D Models

- Draconis17#3692 for the new energy cells game object.

### Dialogue

- Primordial Clok-Roo (The Primordial Clok-Roo#2156) for a ton of future Dialogue

- Bob the Nerd (BobTheNerd10#2164) for some dialogue

- Spooky Pig (Mr. Etc.#3925) for event dialogue

- Arackulele, for the original Dialogue

- JulianMods (xXxStoner420BongMasterxXx) for additional Dialogue

### Additional Credits

- Blind, the Bound Demon (Blind, the Bound Demon#6475) for Gameplay footage

- The people of the Grimora Mod Discord Server, for Ideas , voting on features and being awesome
