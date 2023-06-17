# Grimora Mod

- A giant Mod made by just a lot of people (look in the credits section or the in game credits of the readme for more info), that builds upon the finale with Grimora's chessboard, expanding it into the would-be 4th Act of Inscryption.

- **IF YOU ARE HAVNG SAVING ISSUES, OR ISSUES WITH THE BONES OR THE BELLS INSTALL THE UNLOCKALL MOD, THIS USUALLY FIXES THEM**:

   - Get that mod here : <https://inscryption.thunderstore.io/package/IngoH/Unlock_All/>

- **Please Check out this Form about the Mod! It would be very appreciated if you could fill it out ( It is a very short Form, only 5 Questions or so )** <https://forms.gle/YMfZuJ39wmqjf94u5> 

- **COMPATIBILITY WITH OTHER MODS NOT GUARANTEED, EXCEPT THE OFFICIAL EXPANSION**:

- **MOST IMPORTANT**: **!!!YOU WILL PROBABLY ENCOUNTER BUGS, SUBMIT THEM ON THE PAGE LISTED IN THE REPORTING ISSUES SECTION!!!**


  - THIS WILL UNLOCK CERTAIN STORY EVENTS IN ORDER FOR THE MOD TO WORK, WHICH ALSO MEANS UNLOCKING CERTAIN ACHIEVEMENTS!
  - In the event you are starting from a brand new save, this mod will unlock all learned abilities, mechanics, and cards
    and some Story Events like the first tutorial battle, Bones, and a few others in order for this mod to work.
  - I suggest restarting your game at least once if you haven't actually gotten to the finale.

- If you want to discuss the mod further, join our Discord server! <https://discord.gg/Xf8CBuS8a8>

## Reporting Issues

- If you would to help report issues, please raise a thread here with as much detail as you can provide: <https://github.com/Arackulele/GrimoraMod/issues>

## Icon Definitions

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Bugfix. Self-explanatory.

- ![Feature](https://i.imgur.com/uL9uYNV.png) Feature. Newly added implementations or other enhancements.

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Refactor. Anything that changes/modifies existing logic. Not necessarily a bugfix.

## Known Issues

### Haunter sigils can be seen through cards in hand

- Non fixable for now, the renderer breaks with the layering of the sigils

### A card with Area of Effect Strike, Tri Strike, and Sniper sigils only allows 3 attacks

- Believe it or not, this is how the vanilla game code for the `Sniper` sigil is handled. It doesn't base it off how many attacks you're doing, it hard codes to either 2 for `Split Strike` or 3 for `Tri Strike`.

### The Bell doesnt appear/ bones dont appear when cards die

- This is an issue with Inscryptions Save file still thinking you're in Leshy's tutorial

- Install the unlock all mod to fix this

### Current save file is already at the finale with Grimora

- Make a backup of your save, then delete your current save. Having your current save already at the finale seems to break the mod.

- Install the unlock all mod, it helps a lot with these issues.

### Any issues relating to Boo Hag

- Boo Hag breaks sometimes, i will probably never find and fix every issues with the card

## Recent Update Notes

### Why do the Version Numbers on Thunderstore not match the actual Version?

- Because of the official Arg this mod was involved in, i set the version Number fo the ARG version of the mod to 6.6.6 (because its all demonic and spooky), but since you cannot delete a thunderstore Version or release a new version with a lower version Number, the Mod will forever be stuck after 6.6.6

### 7.2.3 (really 3.2.2)

- ![Refactor](https://i.imgur.com/5bTRm1B.png) The Electric chair Sigils now have their own metacategories so other modders can use them for their mods, and add new sigils to the list

- ![Refactor](https://i.imgur.com/5bTRm1B.png) The Dead Hand Item now gives you a Dead Hand ( Card ) if you use it while having no Cards in your main deck, or when you use it with less than 4 Cards in your main deck

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Changed the Kaycee Hell Mode encounter where it spawns a turn 1 giant

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed that one Grimora Map where people couldnt click on the boss and were softlocked

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Fixed a Softlock with the Royal Rumble challenge ( rarely the challenge still makes you unable to play Cards from your Hand, im looking for a fix for that )

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) More Pharao fight bugs squashed

### 7.2.2 (really 3.2.2)

- ![Feature](https://i.imgur.com/uL9uYNV.png) Cards spawned by the ship in a bottle now get random pirate names

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Apparition and other cards with Haunter breaking the final boss 

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Some Pharao fight crashes

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Haunter breaking when a Card placed on a Haunted Slot is immediately destroyed ( by for example Sentry )

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Terrain Cards not dropping Bones in act 1 when grimora mod is installed

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Various Dialogue issues

### 7.2.1 (really 3.2.1)

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Dead Man walking crash

### 7.2.0 (really 3.2.0)

- ![Feature](https://i.imgur.com/uL9uYNV.png) Card Changes:

  - New Card: Spectre

  - New Card: Dead Man Walking

  - Some Cards have gotten new art

  - Plague doctor: Hp buffed to 2

- ![Feature](https://i.imgur.com/uL9uYNV.png) Expansion Changes:

  - Added 8 new starter Decks, 7 of which were picked from the starter deck competition

  - The last starter deck uses a guided random generator to randomlz generate decks

  - New Card: Grave Carver

  - New Card: Grateful Dead

  - New Card: Big Bones

  - New Card: Crossbones

  - Card Change: Wyvern changed to Ectoplasm

  - Warthr: Hp buffed to 5

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Control Changes:

  - The arrow Keys controls have been Changed to IJKL, i recommend using this control scheme over the wasd one, as it does not overlap with any other controls

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) A lot of miscellanious bug fixes

### 7.1.0 (really 3.1.0)

- ![Feature](https://i.imgur.com/uL9uYNV.png) Credits, when you beat a KCmod run of Grimora Mod for the first time, the mosd full credits will play

- ![Feature](https://i.imgur.com/uL9uYNV.png) New Challenge : Vengeful Ghouls

  - All Bosses gain a third phase with this final challenge, it is the Grimora mod equivilant of Kcmods royal challenge

  - Kaycee gains a fiery new phase where she finally stops freezing

  - Sawyer finally stops being scared after you kill the Hellhound, and he uses his bones to make your life hell

  - To find out what Royal does, youll have to get there in game ( no spoilers here )

- ![Feature](https://i.imgur.com/uL9uYNV.png) New Anti-Challenge : Pharaos Blessing

  - This Final helpful modifier makes the effects of elite fights always benefit you

- ![Feature](https://i.imgur.com/uL9uYNV.png) Elite Fight difficulty has been readjustet and is now region based instead of constant

- ![Feature](https://i.imgur.com/uL9uYNV.png) New Visuals and sounds

  - Kaycees Snow Effects have been completely redone, including completely different blizzard effects in her second phase

  - Her fights second phase also features new snow storm ambience

  - Royals second phase now features a Rain Storm

  - The third phases may or may not feature some visual effects too

  - Frozen Cards ( Draugr, Glacier, Cards frozen bz Kaycee or by Cold front ) are now actually encased in ice

  - Terrain Cards are now dirty and Zombie-like

  - Every area now has unique lighting, with Kaycees region keeping the original turquoise

  - Every Node now gives Cards a unique emission Color

  - Several cards start with unique emissions

  - Most of the starter decks have new icons

  - Missing card portraits and abilities have been added to the starter deck menu

  - Hellhounds art has been redone

- ![Feature](https://i.imgur.com/uL9uYNV.png) Frankenstein ( not Frank & Stein ) now has unique Art and doesnt gain another sigil when being transformed at the electric Chair

- ![Feature](https://i.imgur.com/uL9uYNV.png) New Cards: Davy Jones and his locker

  - Davy Jones is a rare pirate card, that gains more damage for every pirate on the board

- ![Feature](https://i.imgur.com/uL9uYNV.png) Terrain Cards now no longer drop Bones

- ![Feature](https://i.imgur.com/uL9uYNV.png) Anchored Cards are now immune to Hook, Line and Sinker

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Kaycees second Phase

  - Kaycee now sends a barrage of Avalanches on the Board at the start of her second Phase

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Card spawning and custom Mods

  - Custom Card mod compatibility has been greatly increased, cards can now spawn via a custom metacategory

  - Custom Sigils can now be assigned to a specific level of the electric chair with a metacategory too

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Sniper and Sentry no longer Softlock when given mid battle

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Latchers and Sniper now use custom visuals again, instead of the ones by the api

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Latcher Bugfixes and general errors fixed

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) When having an Area of effect Strike or Raider Card on the Board, and the dealt damage by all cards on one side of the board equals 0, the resulting damage points will no longer stay on the table and will instead be deleted

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Infinite Lives now correctly disappears boss visuals when loosing

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Giant Cards not being immune to certain effects they should be immune to

### 7.0.6 (really 3.0.6)

- ![Refactor](https://i.imgur.com/uL9uYNV.png) Nerfs some of the Ankh Guard Blueprints

- ![Bugfix](https://i.imgur.com/uL9uYNV.png)  Some Bug fixes

### 7.0.4 (really 3.0.4)

- ![Feature](https://i.imgur.com/uL9uYNV.png) All Items have descriptions now

- ![Bugfix](https://i.imgur.com/uL9uYNV.png)  Two soft locks, one at the Card removal node, one at the Item Node

- ![Bugfix](https://i.imgur.com/uL9uYNV.png)  Card art fixes

### 7.0.1 - 7.0.3 (really 3.0.3)

- ![Feature](https://i.imgur.com/uL9uYNV.png) The Item Node now has custom Visuals

- ![Refactor](https://i.imgur.com/uL9uYNV.png)  New Card portraits for some Cards

- ![Bugfix](https://i.imgur.com/uL9uYNV.png)  Minor Bug fixes concerning the Items
﻿
### 7.0.0 (really 3.0.0)

- ![Feature](https://i.imgur.com/uL9uYNV.png) New Title Screen, the Mod's Logo is now displayed and the start Run Card has entirely new Art, thanks to Nevernamed

- ![Feature](https://i.imgur.com/uL9uYNV.png) The Mod has a new Icon, woo (Also thanks to Nevernamed) !

- ![Feature](https://i.imgur.com/uL9uYNV.png)  A new Event (Node) that functions like the Item Node in act 1/3: The Pharaoh's Tomb

- There is 9 unique Items you can gain here

- Revenant Bottle ( functions like the Bottle Items in act 1, just with a Revenant in it! )

- Bone Heap Bottle ( functions like the Bottle Items in act 1, just with a Bone Heap in it! You start with this in a normal run. )

- Dead Hand ( Discards your current Hand, you draw 4 new Cards, You start with this Item in both normal and KC mod runs. )

- Bone Lords Horn ( Uses up all your current Soul, you gain 2 Bones for each used Soul. )

- Spectral Urn ( Functions like the Battery Item in act 3, refills all of your Soul to its current maximum. )

- Ship in a Bottle ( Places Skeletons on every empty Slot, all existing Skeletons gain 1 Power. )

- Mallet ( Deal 1 Damage to any enemy Card, which will then become Brittle. )

- Embalming Fluid ( Select any friendly Card, which will then gain 1 Attack. )

- Trowel ( Select an empty Slot, a random Terrain Card will be played for free on that Slot, and you will gain some Bones. )

- There may be a secret 10th Item, which can only be gained in a certain way

- ![Feature](https://i.imgur.com/uL9uYNV.png)  A new type of Fight : The Ankh Guard which is much more difficult than regular fights

- These have unique Battle Blueprints and Cards, which are especially powerful

- At the start of an Ankh Guard fight, a random Rule will be generated, with both positive and negative Effects for the Player

- There is always one Ankh Guard per map, which protects the Pharaoh's Tomb Event

- ![Feature](https://i.imgur.com/uL9uYNV.png)  Terrain now spawns in fights, and is unique to each Area

- ![Feature](https://i.imgur.com/uL9uYNV.png)  2 new Challenges!

- The Bone Lords Mercy anti-challenge is intended to make the game easier and more fair, giving you increased Bone generation

- Hell Mode is a terible new challenge, which makes the Game way too difficult

- ![Feature](https://i.imgur.com/uL9uYNV.png)  Starter Deck Changes

- As most of the Starter Decks were unfun and unplayable before, all but the default one have been reworked

- 4 new Starter Decks have been added, one for each Boss in Grimora Mod

- ![Feature](https://i.imgur.com/uL9uYNV.png)  Kaycee Mode win and loss Screens

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Complete rework to the Boneyard Node

- It now has several different effects, which Grimora tells you about before you bury your Cards, so listen to what she says!

- Completely new visuals for the Node thanks to Pink

- Read more about the specifics on the Wikie here: <https://grimoramod.fandom.com/wiki/Boneyard_Event_Piece>

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Rework to the Grim Reaper / Bone Lord Node

- The Node is now less random and its probabilities are determined by the amount of Sigils the Card you sacrifice has

- The Bone Lord is a big fan of Obols

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Changes to Challenges

- Several Challenges have new Art

- Challenges now show the "Bug out" Effect that also happens during act 1 when a KC Mode challenge gets activated

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Grimoras dialogue was cleaned up and fixed

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Changes to existing Cards

- Haunted Mirror and Nosferat are now in the main Mod

- Some underused Cards like Silbon and Dead Hand have had their Bone Cost reduced

- The Descriptions of Cards have been majorly reworked, to be more mysterious and philosophical

- Several Cards have new Art

- Several new Cards have been added, mainly as Terrains

- ![Refactor](https://i.imgur.com/5bTRm1B.png) New Fair Hand! Before, you would always draw a Bone Heap and Gravedigger in your starting Hand if you had one in your Deck. The fair Hand now chooses from any cheap Cards in your Deck, making certain Strategies much more viable

- ![Refactor](https://i.imgur.com/5bTRm1B.png) The Stinkbug has been moved, and can no longer be killed

- No Animals were harmed in the making of this Mod

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Some Snow Storm Effects have been added to Kaycee, in anticipation of adding more visual Identity to the differnt Ghouls, will see how much Players like this

- Kaycees Area now also has some unique, randomized Blocker pieces

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Several Easter Eggs have been added, mostly involving placing certain Cards on certain Nodes

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Bug where after loosing in a KC Mod run, your Game would freeze and completely break

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) The Mod is now compatible with the newest API version

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Several smaller Bugs regarding Sigil interactions

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Many small Bugs, the mod should now be much more consistent to play and save


### 6.6.8 (really 2.11.1)

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Sawyer fight nerfed (Phase 2 spawns 1 less Bonehound, Sawyer gives you free cards at the start of Phase 2)

- ![Refactor](https://i.imgur.com/5bTRm1B.png) Anti Challenges now give negative Challenge Points

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) You now again gain more starting Bones after eadch Boss

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Some challenge issues fixed

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Soulless Challenge now correctly ordered, beautiful

### 6.6.7 (really 2.11.0)

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Save file issues fixed

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) Some challenge issues fixed

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) raid pirate/ screaming skull fix

- ![Bugfix](https://i.imgur.com/CYIMfjn.png) fix for new api versions


## Full Credits

### Misc

- Akisephila (Addie Brahem) for the Soundtrack

- Cactus (cactus#0003) for making the official Trailer

- Arackulele for Balancing, polish, and other miscellanious things

- JulianMods (xXxStoner420BongMasterxXx) for refactoring the code.

- Ourochi Umbra for additional work

### Code

- Arackulele (thats me) for a large Chunk of the code

- JulianMods (xXxStoner420BongMasterxXx) for a ton of the Code, and general improvements

- JamesGames for the item manager and rewrite of the saving system

- Kopie for some Bug fixes and challenges


### Artists

- Bt Y#0895 for a ton of 2D Assets for the Mods, including almost all Cards ( special thanks for the item ideas )

- Lich Underling (Lich underling#7678) for additional Card Art

- Anne Bean for some additional Card Art

- Cevin_2006 (Cevin2006™ (◕‿◕)#7971) for additional art

- Catboy Stinkbug (Catboy Stinkbug#4099) for additional art

- Amy (Amy#7082) for additional art

- Gold for additional Art

(Legacy) - LavaErrorDoggo (LavaErrorDoggo#1564) for making the Original Act 2 Cards but in full Size Artwork

### 3D Models

- Pink (Pink#6999) for making the Boss Skull Models , Bone Lord Model, Walkable Crypt Model, Event Node Models and some Item Models

- Catboy Stinkbug (Catboy Stinkbug#4099) for the Board Skull 3D Models, most of the Item Models, Node Board Models, Event Node Models etc

- Draconis17#3692 for the new energy cells game object.

- JestAnotherAnimator for the new attack animations.

### Dialogue

- Bob the Nerd (BobTheNerd10#2164) for some dialogue

- Spooky Pig (Mr. Etc.#3925) for event dialogue

- Arackulele for the much of the Dialogue

- JulianMods (xXxStoner420BongMasterxXx) for additional Dialogue

### Special Thanks

-The Grimora Mod Community
