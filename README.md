# Grimora Mod

- A giant Mod made by Kopie, Arackulele, xXxStoner420BongMasterxXx that builds upon the finale with Grimora's chessboard.

- **IMPORTANT**: Disable energy drone (but not energy refresh) in the API config!!!
- **BEWARE**: THIS WILL UNLOCK CERTAIN STORY EVENTS IN ORDER FOR THE MOD TO WORK, WHICH ALSO MEANS UNLOCKING CERTAIN ACHIEVEMENTS!
- In the event you are starting from a brand new save, this mod will unlock all learned abilities, mechanics, and cards and some Story Events like the first tutorial battle, Bones, and a few others in order for this mod to work.
- If you are not starting from a new save, this mod will check for certain story events and determine if it needs to unlock those.
- I suggest restarting your game at least once if you haven't actually gotten to the finale.
- The most important, !!!THIS IS A BETA AND IS NOT BUG FREE!!!

- This mod is not tested with KC mod and we cannot guarantee it will work on that version of the game

## Special thanks to

- LavaErrorDoggo for making a lot of the Artwork

- YisusWhy for the epic Bone Lord Artwork

- JulianMods (xXxStoner420BongMasterxXx) for refactoring the code.

## Update Notes

### 2.1.0

- MASSIVE refactor for readability and overall code quality!

- Added custom config file `grimora_mod_config.cfg` to keep track of bosses defeated and active/removed pieces.

- Added 53 random chessboard layouts in JSON format for 'random' setups.

- Added Deck Review board view!

  - Still doesn't quite work right, expect bugs.

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

### Energy Cards don't work

- Try enabling energy refresh in API config, that option may be unstable with this mod but should fix the issue until we implement a more proper solution.

### Black Square Sigils

- Install [SigilArtPatch](https://inscryption.thunderstore.io/package/MADH95Mods/SigilArtPatch/)
