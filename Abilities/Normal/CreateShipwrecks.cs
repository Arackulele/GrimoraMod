using DiskCardGame;

namespace GrimoraMod;

public class CreateShipwrecks : CreateCardsAdjacent
{
	public const string RulebookName = "Shipwreck Finder";

	public static Ability ability;

	public override Ability Ability => ability;

	public override string SpawnedCardId => GrimoraPlugin.NameShipwreckDams;

	public override string CannotSpawnDialogue => "Blocked on both sides. No Shipwrecks for the Forgotten Man.";

}

public partial class GrimoraPlugin
{
	public void Add_Ability_CreateShipwrecks()
	{
		const string rulebookDescriptionEnglish =
			$"When [creature] is played, a Shipwreck is created on each empty adjacent space. [define:{NameShipwreckDams}]";
		const string rulebookDescriptionChinese =
			$"使用[creature]时，相邻空位均会出现Shipwreck卡牌。 [define:{NameShipwreckDams}]";
		string rulebookDescription = Localization.CurrentLanguage == Language.ChineseSimplified ? rulebookDescriptionChinese : rulebookDescriptionEnglish;

		AbilityBuilder<CreateShipwrecks>.Builder
		 .SetIcon(AbilitiesUtil.LoadAbilityIcon(Ability.CreateDams.ToString()))
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName(CreateShipwrecks.RulebookName)
		 .Build();
	}
}
