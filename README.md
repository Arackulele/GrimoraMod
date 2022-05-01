# Grimora Mod

- A giant Mod made by xXxStoner420BongMasterxXx and Arackulele, that builds upon the finale with Grimora's chessboard.

- **Note from BongMaster 2022-04-30**
  - Due to the toxicity/negativity of the GrimoraMod server in regards to really any discussion that might be taking place, I'm going to be taking a hiatus from working on the mod.
  - I personally have been private messaged from 6 different members in the server about ideas being not only shutdown, but aggressively attacked from other members in the server as well. I wanted to work on this mod as it was a great idea and to extend the Scrybe of the Dead even further, not deal with people who are unable to have a decent idea discussion or have no idea how to give constructive criticism.

- **MOST IMPORTANT**: **!!!YOU WILL ENCOUNTER BUGS!!!**
- **BEWARE**:

  - THIS WILL UNLOCK CERTAIN STORY EVENTS IN ORDER FOR THE MOD TO WORK, WHICH ALSO MEANS UNLOCKING CERTAIN ACHIEVEMENTS!
  - In the event you are starting from a brand new save, this mod will unlock all learned abilities, mechanics, and cards
    and some Story Events like the first tutorial battle, Bones, and a few others in order for this mod to work.
  - I suggest restarting your game at least once if you haven't actually gotten to the finale.

- If you want to discuss the mod further, join our Discord server! <https://discord.gg/Xf8CBuS8a8>

## Reporting Issues

- If you would to help report issues, please raise a thread here with as much detail as you can provide: <https://github.com/Arackulele/GrimoraMod/issues>
- Bug reports can also be submitted on the Discord Server: <https://discord.gg/Xf8CBuS8a8>

## Icon Definitions

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Bugfix. Self-explanatory.

- ![Feature](https://i.imgur.com/uL9uYNV.png) Feature. Newly added implementations or other enhancements.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Refactor. Anything that changes/modifies existing logic. Not necessarily a bugfix.

## Known Issues

### Haunter sigils can be seen through cards in hand

- Currently investigating, although I'm not quite sure where the issue exactly lies. Tried messing around with the layer ordering and that didn't seem to fix the issue.

### A card with Area of Effect Strike, Tri Strike, and Sniper sigils only allows 3 attacks

- Believe it or not, this is how the vanilla game code for the `Sniper` sigil is handled. It doesn't base it off how many attacks you're doing, it hard codes to either 2 for `Split Strike` or 3 for `Tri Strike`.

### Current save file is already at the finale with Grimora

- Make a backup of your save, then delete your current save. Having your current save already at the finale seems to break the mod.

## Update Notes

### 2.8.81

- **If you happen to encounter any further bugs, you may have to delete your `ModdedSaveFile.gwsave` file in your Inscryption directory. This is in the same directory as your main save file, SaveFile.gwsave**

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed issue with choosing a card causing a softlock. Apparently you can't use `.Any()` on the override choices.

### 2.8.8

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Potentially fixed issue with current save file already being at the Grimora finale.
  - Thanks to the work already done by `divisionbyzorro#2868` with the [P03 Mod](https://inscryption.thunderstore.io/package/Infiniscryption/P03_In_Kaycees_Mod/), I was able to borrow and tweak accordingly a lot of the code related to save file handling.
  - Still expect bugs because it is not perfect.

- ![Feature](https://i.imgur.com/uL9uYNV.png) Initial beta for Ascension Mode! EXPECT A LOT OF BUGS!
  - Challenge skull art courtesy of `Bt Y#0895`.
  - No Bones: You no longer gain the extra bones after defeating bosses.
  - Kaycee's Kerfuffle: The fourth turn of every battle, all your cards will are Frozen Away.
  - Sawyer's Showdown: Lose 1 bone every 3 turns.
  - Royal's Revenge: Every third card you play gains the `Lit Fuse` sigil.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) The last Skeleton in your side deck pile now has 2 attack instead of 1.

#### Ability Changes

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Latch related abilities can no longer choose cards with 5 abilities as targets.

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) `Chaos Strike` description updated: `[creature] will strike the opposing slots to the left, right, and center of it randomly, up to 3 times.`

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) `Skin Crawler` can no longer go under other cards with `Skin Crawler`.

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) `Marrow Sucker` can now only be activated if the card is not at max health.

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) `Marrow Sucker` now correctly shows up in Rulebook.

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) `Malnourishment` will now correctly cause the card having the sigil to die if the health reaches zero.

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) `Sea Shanty` will now only buff Skeletons on the owner's side of the board, and not all Skeletons.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) `Malnourishment` no longer appears in the chair.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) `Blood Guzzler` is no longer possible to get on zero attack cards in the Electric Chair.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Updated rulebook descriptions of `Power Dice` and `Marrow Sucker` so that it better indicates what it actually does.

#### Card Changes

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed `Spectrabbits` sigils not showing when drawn from `Fecundity`.

- ![Feature](https://i.imgur.com/uL9uYNV.png) Added `Ourobones` card. 1/1 2 Bone cost with `Brittle`.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) `Spectrabbits` sigils will no longer stack.
  - This is to fix the issue with getting `Fecundity` and `Sentry` on the rabbits to the point where you end up one-shotting everything.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Multiple card descriptions updated per `Mr. Etc.#3925`.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) `Vampire` attack nerfed from 3 to 2. Bone cost buffed from 6 to 5.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) `Giant` attack buffed from 2 to 3.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) `Ripper` bone cost buffed from 9 to 6.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) `Jikininki` attack buffed from 0 to 1. Energy cost nerfed from 4 to 6.

### 2.8.7

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed issue when assets don't load fast enough on slow hard drives, causing exceptions when entering the game.

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Added check to only replace the bomb object from `Explode On Death` when it's Grimora's finale.

- ![Feature](https://i.imgur.com/uL9uYNV.png) Cards can now have 5 abilities!
  - The sigil icons have now also been resized and repositioned slightly to better fit on the cards.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Multiple refactors to handle new API 2.2 updates.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Added KMod code logic for regular and rare card choices.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Cursor interaction disabled at beginning of Electric Chair intro so you can't break the intro sequence...

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Added custom animation controller for playing specific animations and better overall control for card animations instead of using patches.

#### Bosses

##### Grimora

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed Twin Giants and Bonelord playing attack animation after dying.

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed softlock with Twin Giants and Bonelord if their attack animations don't finish playing before another card attacks.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Now using `ModifyQueuedCard` and `ModifySpawnedCard` methods instead of patches when spawning cards.

#### Ability Changes

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed `Blood Guzzler` and `Malnourishment` causing a softlock if the card dies from `Sharp Quills` or other means before the attack sequence finishes.

- ![Feature](https://i.imgur.com/uL9uYNV.png) Skeletons Within: `Once [creature] is struck, draw a card from your Skeleton pile.`. Art courtesy of `Bt Y#0895`.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) `Alternating Strike` icon will now flip if on the opponent side.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Moved logic for enraging a Twin Giant to be inside the `Giant Strike` sigil instead of Grimora's battle sequence.

#### Card Changes

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed `Skin Crawler` cards not being destroyed correctly during Royal's fight after swaying.

- ![Feature](https://i.imgur.com/uL9uYNV.png) `Hellhand` is actually usable now!

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

- Blind, the Bound Demon (Blind, the Bound Demon#6475) for Gameplay footage

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

- The people of the Grimora Mod Discord Server, for Ideas , voting on features and being awesome
