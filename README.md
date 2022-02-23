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

##### Kaycee

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed issue with cards that have been ice cubed by Kaycee not having all their abilities intact when being broken out of the ice.
  - The reason for this is that the original `IceCube` logic creates the card in the slot _by the name_ and NOT by the `CardInfo` object. Meaning, whatever the vanilla card attributes are, is what spawns, not the one in your deck.

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed icicle on Kaycee's figurine to now properly move with the head.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Tweaked Kaycee boss logic so the card freezing is less frustrating.

##### Sawyer

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed issue with Sawyer taking a bone when a player would have zero bones, causing the PlayerBones to go in the negative.
  - This would cause the player to be unable to play zero cost bone cards like Skeleton.
  - Sawyer will now only take 1 bone from the player if the player has at least 2 bones or more.

##### Grimora

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed potential softlock in Grimora's fight if a card had `Possessive`.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Changed Grimora's theme to `Corrupted Queen` from `Akisephila (Addie Brahem)`.

#### Ability/Card Changes

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed `Skin Crawler` ability not dying after the first time the dialogue is played. Forgot to move the card dying part outside the check for if the dialogue has not been played yet...

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed issue with `Handy` ability drawing a new hand for the player if the card would die and then reanimated during the first phase of Grimora's fight.

- ![Feature](https://i.imgur.com/uL9uYNV.png) New `Mummy` and `Sarcophagus` art courtesy of `Bt Y#0895`!

- ![Feature](https://i.imgur.com/uL9uYNV.png) Added `Exploding Pirate` card. 1 Bone, 2/2 with `Lit Fuse` sigil. Initial art from `Lich underling#7678`, with modifications from `Ara`.
  - `Lit Fuse`: [creature] loses 1 health per turn. When [creature] dies, the creature opposing it, as well as adjacent friendly creatures, are dealt 10 damage.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Nerfed `Wendigo` attack from 2 to 1, and set as a normal type of card.

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

### 2.5.0

#### Cards

- **Boo Hag** - 1,1 - ![cost_bone](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_bone.png)![cost_bone_5](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_bone_5.png) Sigils: Skin Crawler.

- **Danse Macabre** - 3,3 - ![cost_bone](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_bone.png)![cost_bone_8](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_bone_8.png) Sigils: Alternating Strike, Erratic.

- **Dybbuk** - 0,1 - ![cost_bone](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_bone.png)![cost_bone_3](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_bone_3.png) Sigils: Possessive.

- **Giant** - 2,7 - ![cost_bone](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_bone.png)![cost_bone_1](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_bone_1.png)![cost_bone_5](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_bone_5.png) Sigils: Bone King, Bifurcated Strike.

- **Project** - 1,3 - ![cost_bone](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_bone.png)![cost_bone_5](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_bone_5.png) Sigils: Erratic, Split Strike.

- **Ripper** - 6,6 - ![cost_bone](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_bone.png)![cost_bone_9](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_bone_9.png) Sigils: Brittle.

- **Screaming Skull** - 1,1 - ![cost_bone](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_bone.png)![cost_bone_2](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_bone_2.png) Sigils: Area Of Effect Strike.

- **Silbon** - 3,2 - ![cost_bone](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_bone.png)![cost_bone_7](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_bone_7.png) Sigils: Inverted Strike, Sprinter.

#### Sigils

- **Activated Energy Draw Wyvern** - Pay 3 Energy for [creature] to summon a Wyvern in your hand.
- **Alternating Strike** - [creature] alternates between striking the opposing space to the left and right from it.
- **Area Of Effect Strike** - [creature] will strike it's adjacent slots, and each opposing space to the left, right, and center of it.
- **Create Army Of Skeletons** - When [creature] is played, a Skeleton is created in each empty space on the owner's side.
- **Erratic** - At the end of the owner's turn, [creature] will move in a random direction.
- **FlameStrafe** - Whenever [creature] moves, it leaves a trail of embers. The warmth of the Embers shall enlighten nearby cards.
- **Inverted Strike** - [creature] will strike the opposing slot as if the board was flipped. A card in the far left slot will attack the opposing far right slot.
- **Lit Fuse** - [creature] loses 1 health per turn. When [creature] dies, the creature opposing it, as well as adjacent friendly creatures, are dealt 10 damage.
- **Possessive** - [creature] cannot be attacked from the opposing slot. The opposing slot instead attacks one of it's adjacent slots if possible.
- **Skin Crawler** - When [creature] resolves, if possible, will hide under an adjacent card providing a +1 buff. Otherwise, it perishes. Cards on the left take priority.

## Full Credits

### Misc

- kisephila (Addie Brahem) for the Soundtrack

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
