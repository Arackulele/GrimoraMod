# Grimora Mod

- A giant Mod made by Kopie, Arackulele, xXxStoner420BongMasterxXx that builds upon the finale with Grimora's
	chessboard.

- **IMPORTANT**: **ENABLE ENERGY DRONE IN API CONFIG IN ORDER FOR IT TO SHOW UP**
- **BEWARE**: THIS WILL UNLOCK CERTAIN STORY EVENTS IN ORDER FOR THE MOD TO WORK, WHICH ALSO MEANS UNLOCKING CERTAIN
	ACHIEVEMENTS!
- In the event you are starting from a brand new save, this mod will unlock all learned abilities, mechanics, and cards and some Story Events like the first tutorial battle, Bones, and a few others in order for this mod to work.
- If you are not starting from a new save, this mod will check for certain story events and determine if it needs to unlock those.
- I suggest restarting your game at least once if you haven't actually gotten to the finale.
- The most important, !!!THIS IS A BETA AND IS NOT BUG FREE!!!

- This mod is not tested with KC mod and we cannot guarantee it will work on that version of the game

## Special thanks to

- LavaErrorDoggo for making a lot of the Artwork

- YisusWhy for the epic Bone Lord Artwork

- JulianMods (xXxStoner420BongMasterxXx) for refactoring the code.

- Morrígan/crow_system#3539 for extensive beta testing v2.1

## Update Notes

### 2.4.0

- Fixed camera view when third candle is being added in Grimora boss fight.

- Fixed softlock during Grimora's final phase.

- Changed CardBuilder methods to now be prefixed with `Set` instead of `With`.

- Fixed GBC abilities like SkeletonStrafe (Skeleton Crew rulebook name) not appearing correctly.

  - All abilities should now show up correctly, which includes the GBC abilities.

- Fixed sizing and scaling of card art to better fit inside gravestones.

	- Bonelord and Dead Hand still have scaling/portrait overlaps until fixed.

- Removed disabling act 1/2 cards from showing up as now the select card choices events now look for GrimoraMod specific
	cards.

### 2.3.3

- Added energy drone!

- Added **Reset Deck** button to now reset your deck back to the starter deck in case deck viewing throws errors.

- Corrected logic for adding bones to the start of combat once a boss has been defeated.

	- Kaycee defeated = 2 bones
	- Sawyer defeated = 5 bones
	- Royal defeated = 10 bones

- Updated project to net6.

- Removed WithPortrait method call for cards to now load portraits based on names.

- Had to undo Resource-based image loading as net6 threw a slew of errors at startup.
  - Images are now loaded through files in the directory.

### 2.1.2

- Minor patches and slightly changed Kaycee's mechanics.

### 2.1.1

- Flavor update.

- Added tons of new dialogue.

- Added **Reset Run** button (this only resets all data values in the config, not your deck).

### 2.1.0

- MASSIVE refactor for readability and overall code quality!

- Added custom config file `grimora_mod_config.cfg` to keep track of bosses defeated and active/removed pieces.

- Added 53 random chessboard layouts in JSON format for 'random' setups.

- Added Deck Review board view!

	- If you are unable to click on a chess node after viewing your deck, try viewing your deck again.

- Encounter blueprints reworked into own class.

- Made resource files of all artwork for significantly easier sprite loading.

- Fixed issue with custom card sprites being positioned too low.

- Fixed issue with loading the pieces back in their respective nodes if the player quit.

- Fixed issue with player chesspeice appearing at previous node after region change.

- Fixed issue with player chesspiece not spawning if there are no available nodes to spawn in for the row.

- Fixed issue with softlocking on final boss!

- Fixed issue with pieces overlapping and not being destroyed correctly causing newer pieces to never be created.

- Fixed issue with defeating bosses throwing an exception when changing to a new map.

### 2.0.2

- Balance overhaul

- Fixed Blueprints

- Added 30 more Blueprints

## Known Issues

### Grimora's dialogue is unfinished and still the one from the finale and part 1 dialogue in some cases

- This is a temporary problem and will be fixed soon.

### Black Square Sigils

- Install [SigilArtPatch](https://inscryption.thunderstore.io/package/MADH95Mods/SigilArtPatch/)
