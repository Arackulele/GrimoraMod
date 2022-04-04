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

**turns**: A list of lists. The internal list are the card names.

Using the example below for "Kaycee_Region3", the first turn has `Animator` and `Bonehound`. Second turn `Wight` and `Wight`.

For "Kaycee_Region4", the first turn has `SlingersSoul` and `IceCube`. Second turn nothing, then third turn a `Writher` and `PlagueDoctor`.

```json
{
	"encounters": [
		{
			"id": "Kaycee_Region3",
			"turns": [
				["Animator", "Bonehound"],
				["Wight", "Wight"]
			]
		},
		{
			"id": "Kaycee_Region4",
			"turns": [
				["SlingersSoul", "IceCube"],
				[],
				["Writher", "PlagueDoctor"]
			]
		}
	]
}
```
