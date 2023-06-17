using System.Linq;
using System.Reflection;
using DiskCardGame;
using InscryptionAPI.Card;
using Sirenix.Utilities;
using UnityEngine;

namespace GrimoraMod;

public class AbilityBuilder<T> where T: AbilityBehaviour
{
	public static AbilityBuilder<T> Builder => new(typeof(T));
	
	private readonly AbilityInfo _abilityInfo = ScriptableObject.CreateInstance<AbilityInfo>();
	
	private readonly System.Type _type;
	
	private Texture _rulebookIcon;

	private AbilityBuilder(Type type)
	{
		_type = type;
		_abilityInfo.metaCategories = new List<AbilityMetaCategory>
		{
			AbilityMetaCategory.GrimoraRulebook
		};
		
		if (_type.Name.Contains("Activated"))
		{
			_abilityInfo.activated = true;
		}
	}

	public AbilityManager.FullAbility Build()
	{
		HandleRulebookName();
		
		return SetupAbility();
	}

	private AbilityManager.FullAbility SetupAbility()
	{



		{
			
			Texture icon = _rulebookIcon
				               ? _rulebookIcon
				               : AssetUtils.GetPrefab<Texture>("ability_" + _type.Name);
		
			// instantiate
			var newAbility = AbilityManager.Add(GrimoraPlugin.GUID, _abilityInfo, _type, icon);

			// Get static field
			FieldInfo field = _type.GetField(
				"ability",
				BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance
			);
			field.SetValue(null, newAbility.Id);

				return newAbility;
		}

	}

	public AbilityBuilder<T> SetRulebookName(string rulebookName)
	{
		_abilityInfo.rulebookName = rulebookName;
		return this;
	}

	public AbilityBuilder<T> SetPixelIcon(Sprite PixelIcon)
	{
		_abilityInfo.pixelIcon= PixelIcon;
		return this;
	}

	/// <summary>
	/// Pascal split will make strings like this:
	/// "AreaOfEffectStrike" => "Area Of Effect Strike" 
	/// "Possessive" => "Possessive"
	///
	/// If the rulebookName was never set, default to the type.Name
	/// If the rulebookName contains a space, that means the name was set manually, so don't do anything further with it.
	/// </summary>
	private void HandleRulebookName()
	{
		string rulebookName = _abilityInfo.rulebookName;
		if (rulebookName.IsNullOrWhitespace())
		{
			rulebookName = _type.Name;
		}

		_abilityInfo.rulebookName = rulebookName.Contains(" ")
			                            ? rulebookName
			                            : rulebookName.SplitPascalCase();
	}

	public AbilityBuilder<T> SetRulebookDescription(string rulebookDescription)
	{
		_abilityInfo.rulebookDescription = rulebookDescription;
		return this;
	}

	public AbilityBuilder<T> FlipIconIfOnOpponentSide()
	{
		_abilityInfo.flipYIfOpponent = true;
		return this;
	}

	public AbilityBuilder<T> SetCanStack()
	{
		_abilityInfo.canStack = true;
		return this;
	}
	
	public AbilityBuilder<T> SetIcon(Texture iconToSet)
	{
		_rulebookIcon = iconToSet;
		return this;
	}

	public AbilityBuilder<T> SetPowerLevel(int powerLevel)
	{
		_abilityInfo.powerLevel = powerLevel;
		return this;
	}
}
