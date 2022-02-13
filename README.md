# Grimora Mod

- A giant Mod made by Arackulele, and xXxStoner420BongMasterxXx that builds upon the finale with Grimora's
  chessboard.

- **MOST IMPORTANT**: **!!!YOu will probably encounter some Bugs!!!**
- **IMPORTANT**: **ENABLE ENERGY DRONE IN API CONFIG IN ORDER FOR IT TO SHOW UP**
- **BEWARE**:

  - THIS WILL UNLOCK CERTAIN STORY EVENTS IN ORDER FOR THE MOD TO WORK, WHICH ALSO MEANS UNLOCKING CERTAIN ACHIEVEMENTS!
  - THIS MOD HAS NOT BEEN TESTED WITH KAYCEE'S MOD!

- In the event you are starting from a brand new save, this mod will unlock all learned abilities, mechanics, and cards
  and some Story Events like the first tutorial battle, Bones, and a few others in order for this mod to work.
- If you are not starting from a new save, this mod will check for certain story events and determine if it needs to unlock those.
- I suggest restarting your game at least once if you haven't actually gotten to the finale.

## Known Issues

### Grimora's dialogue is unfinished and still the one from the finale and part 1 dialogue in some cases

- Ongoing process to add new dialogue.

### Current save file is already at the finale with Grimora

- Make a backup of your save, then delete it. Having your current save already at the finale will break the mod.

## Update Notes

### 2.5.8

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Corrected `Erratic` ability logic to now no longer force itself onto other cards.

- ![Bugfix](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/fixes.png) Set default ice cube creature within to `Skeleton` if the card would receive Ice Cube ability from Electric Chair.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Card changes
  - Changed `Skeleton Army` ability to now create a `Skeleton` in any owner owned open slots when played.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Electric Chair changes:
  - Removed more abilities from Electric Chair sequence:
    - Create Bells
    - Create Dams
    - Draw Copy on Death
    - Draw Vessel on Hit
    - Drop Ruby on Death
    - Latch Brittle
    - Latch Death Shield
    - Latch Explode on Death
  - Updated dialogue.
  - Reduced number of times to electrify card to a max of 2 times.
  - Changed texture of selection slot to be a plus sign with a question mark instead of 3 question marks.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Changed `Sporedigger` description to better fit the card.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Reduced `Skeleton Army` activated bone cost from 2 to 1.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Made hammer only usable up to 3 times per battle. The reasoning for this is that the hammer use should be used when necessary, and not a large crutch.

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png)

### 2.5.7

- ![Feature](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/new.png) Redid all of the Chessboard layouts...again!
  - The new nodes are now introduced gradually
  - Slightly less linearity. Beat optional enemies to gain access to more nodes.

- ![Feature](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/new.png) Difficulty rebalance!
  -Blueprints now follow a difficulty curve.
  -Kaycee now has a new Boss Mechanics, making the first map slightly easier.
  -Royal's and Grimora's areas have been buffed to provide for a more challenging experience given the new chess pieces.
  -Rebalanced the new cards to make them more interesting and less underpowered.
  -Removed the BoneLord sigil from the Electric Chair ability pool :)

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

## Special thanks to

- LavaErrorDoggo for making a lot of the Artwork

- YisusWhy for the epic Bone Lord Artwork

- JulianMods (xXxStoner420BongMasterxXx) for refactoring the code.

- Morrígan/crow_system#3539, Comrade Alpaca#0292, TheGreenDigi#8672, Plot#6972 for extensive beta testing v2.1

- Bt Y#0895 for currently working on Artwork for the mod

- Kopie for being a former developer.

- Draconis17#3692 for the new energy cells game object!
