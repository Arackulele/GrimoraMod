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

### A card with Area of Effect Strike, Tri Strike, and Sniper sigils only allows 3 attacks

- Believe it or not, this is how the vanilla game code for the `Sniper` ability is handled. It doesn't base it off how many attacks you're doing, it hard codes to either 2 for `Split Strike` or 3 for `Tri Strike`.

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

### 2.7.4

#### Bosses

##### Grimora

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Hopefully fixed issue with phase 2 not spawning the twin giants. Cards will no longer be weakened going into phase 2.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Modified phase 2 twin giant initial attack and health.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Removed reanimation sequence entirely. With `Double Death` especially, the difficulty ramp for the first phase due to this mechanic is just too overwhelming and can heavily swing the board in Grimora's favor.

#### Ability/Card Changes

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Reworked the logic for `Skin Crawler`, again.
  - Fixed potential softlock if `Boo Hag` is hiding under a card when you die.
  - New description: [creature] will attempt to find a host in an adjacent friendly slot, hiding under it providing a +1/+1 buff. Cards on the left take priority.

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Adjusted `Death Knell` emission art to line up.
  - Also added the bell SpecialStatIcon for the attack so that you know what it actually does now.

- ![Feature](https://i.imgur.com/uL9uYNV.png) New `Summoner` art from `Cevin2006™ (◕‿◕)#7971`.

- ![Feature](https://i.imgur.com/uL9uYNV.png) Added multiple new basic cards. All art from `Bt Y#0895`.
  - Centurion: 1/4, 6 Bones, `Armored`.
  - Dalgyal: 0/2, 2 Energy, `Sentry`. Description courtesy `TheGreenDigi#8672`.
  - Deadeye: 1/1, 5 Bones, `Hoarder`. Description courtesy `TheGreenDigi#8672`.
  - FesteringWretch: 1/1, 3 Bones, `Stinky`. Description courtesy `Blind, the Bound Demon#6475`.
  - Manananggal: 3/3, 8 Bones, `Flying`. Description courtesy `TheGreenDigi#8672`.
  - Will 'O' The Wisp. 0/1, 1 Bone, `Spirit Bearer` (Gain Battery but themed for Grimora). Description courtesy `Blind, the Bound Demon#6475`.

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
