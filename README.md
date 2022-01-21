# Grimora Mod

- A giant Mod made by Kopie, Arackulele, xXxStoner420BongMasterxXx that builds upon the finale with Grimora's
  chessboard.

- **IMPORTANT**: **ENABLE ENERGY DRONE IN API CONFIG IN ORDER FOR IT TO SHOW UP**
- **BEWARE**: THIS WILL UNLOCK CERTAIN STORY EVENTS IN ORDER FOR THE MOD TO WORK, WHICH ALSO MEANS UNLOCKING CERTAIN
  ACHIEVEMENTS!
- In the event you are starting from a brand new save, this mod will unlock all learned abilities, mechanics, and cards
  and some Story Events like the first tutorial battle, Bones, and a few others in order for this mod to work.
- If you are not starting from a new save, this mod will check for certain story events and determine if it needs to unlock those.
- I suggest restarting your game at least once if you haven't actually gotten to the finale.
- The most important, !!!THIS IS A BETA AND IS NOT BUG FREE!!!

- This mod is not tested with KC mod and we cannot guarantee it will work on that version of the game

## Special thanks to

- LavaErrorDoggo for making a lot of the Artwork

- YisusWhy for the epic Bone Lord Artwork

- JulianMods (xXxStoner420BongMasterxXx) for refactoring the code.

- Morrígan/crow_system#3539, Comrade Alpaca#0292, TheGreenDigi#8672, Plot#6972 for extensive beta testing v2.1

- Draconis17#3692 for the new energy cells game object!

## Update Notes

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

- ![Refactor](https://raw.githubusercontent.com/julian-perge/InscyptionReadmeMaker/main/Artwork/Git/smug.png) Changed
  CardBuilder methods to now be prefixed with `Set` instead of `With`.

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

## Known Issues

### Grimora's dialogue is unfinished and still the one from the finale and part 1 dialogue in some cases

- This is a temporary problem and will be fixed soon.
