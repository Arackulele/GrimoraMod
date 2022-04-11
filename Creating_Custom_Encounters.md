# Creating custom encounters

<details>
<summary>Internal card names</summary>

Animator
Apparition
BalBal
Banshee
BloodySack
BoneCollective
Bonehound
BonelordsHorn
Bonepile
BooHag
CalaveraCatrina
Catacomb
Centurion
CompoundFracture
Dalgyal
DanseMacabre
DeadHand
DeadPets
Deadeye
DeathKnell
DisturbedGrave
Doll
Draugr
DrownedSoul
Dybbuk
Ember_Spirit
ExplodingPirate
Family
FesteringWretch
Flameskull
Franknstein
Fylgja
GhostShip
Giant
Glacier
Gravebard
Gravedigger
Haltia
HauntedMirror
HeadlessHorseman
Hydra
IceCube
Jikininki
LaLlorona
Manananggal
MassGrave
Moroi
Mummy
Necromancer
Nixie
Nosferat
Obol
PirateCaptainYellowbeard
PirateFirstMateSnag
PiratePolly
PiratePrivateer
PlagueDoctor
Poltergeist
PossessedArmour
Project
Revenant
Ripper
Rot
Sarcophagus
ScreamingSkull
Silbon
Skelemagus
SkeletonArmy
SlingersSoul
Sluagh
Sporedigger
Summoner
TombRobber
VengefulSpirit
Warthr
Wechuge
Wight
WillOTheWisp
Writher
Wyvern
Zombie

</details>

## How to Enable

In this `grimoramod_config.cfg` file, set the option `Encounter Blueprints` to either 2 or 3.

If you wish to immediately go into that specific encounter, then enable developer mode and there is a `Debug Encounters` toggle with a bunch of the encounters to choose from.

## File Creation

Place a `.json` file prefixed with the name `GrimoraMod_Encounter` in the `DataFiles` directory, the same directory where `GrimoraChessboards.json` file is.

If you do not have a DataFiles directory, the same directory as `GrimoraMod.dll` will suffice.

Example:

```
Arackulele-GrimoraMod
- DataFiles
-- GrimoraMod_Encounter_custom1.json
-- GrimoraChessboards.json
-- grimoramod_sprites
- GrimoraMod.dll
- icon.png
- README.md
- manifest.json
```

**id**: The name of the encounter. Required.

- See `WHAT TO NAME THE ID FOR THE ENCOUNTER` section.

**turns**: A list of lists. The internal list are the card names.

Using the example below for "Custom_Kaycee_Region3", the first turn has `Animator` and `Bonehound`. Second turn `Wight` and `Wight`.

For "Custom_Kaycee_Region4", the first turn has `SlingersSoul` and `IceCube`. Second turn nothing, then third turn a `Writher` and `PlagueDoctor`.

```json
{
	"encounters": [
		{
			"id": "Kaycee_Region_3",
			"turns": [
				["Animator", "Bonehound"],
				["Wight", "Wight"]
			]
		},
		{
			"id": "Kaycee_Region_4",
			"turns": [
				["SlingersSoul", "IceCube"],
				[],
				["Writher", "PlagueDoctor"]
			]
		},
		{
			"id": "Sawyer_Fight",
			"turns": [
				["SlingersSoul", "IceCube"],
				[],
				["Writher", "PlagueDoctor"]
			]
		}
	]
}
```

### WHAT TO NAME THE ID FOR THE ENCOUNTER

The scheme goes like this between the underscores. Meaning, if you split the id on the underscore, using `Kaycee_Region_3` as an example, each word is it's own section.

1. `Kaycee` - The name of the boss for the region. Allowable values are: Kaycee, Sawyer, Royal, Grimora
2. `Region` - Whether or not the bluprint is for the region or the boss fight.
   1. If you want to set this blueprint for the boss fight's first phase, change `Region` to `Fight`.
3. `3` - The number is optional. If no number is provided, one will be append at the end.
   1. IF YOU DO DECIDE TO PUT A NUMBER IN THE ID, PLEASE MAKE SURE IT HAS AN UNDERSCORE BEFORE THE NUMBER.
