using System.Reflection;
using DiskCardGame;
using InscryptionAPI.Card;
using Sirenix.Utilities;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class StatIconBuilder<T> where T : SpecialCardBehaviour
{
	public static StatIconBuilder<T> Builder => new(typeof(T));
	
	private readonly StatIconInfo _statIconInfo = ScriptableObject.CreateInstance<StatIconInfo>();

	private readonly System.Type _type;

	private StatIconBuilder(Type type)
	{
		_type = type;
	}

	public StatIconManager.FullStatIcon Build()
	{
		Log.LogDebug($"Starting to add stat icon [{_type}]");
		var statIcon = StatIconManager.Add(GUID, _statIconInfo, _type);

		FieldInfo field = _type.GetField(
			"FullStatIcon",
			BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance
		);
		field.SetValue(null, statIcon);
		
		SetupSpecialCardBehaviour();

		return statIcon;
	}

	private void SetupSpecialCardBehaviour()
	{
		string finalName = _statIconInfo.rulebookName;
		if (finalName.IsNullOrWhitespace())
		{
			finalName = _type.Name;
		}

		Log.LogDebug($"Starting to add special ability [{_type}]");
		var specialAbility = SpecialTriggeredAbilityManager.Add(GUID, finalName, _type);

		FieldInfo field = _type.GetField(
			"FullSpecial",
			BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance
		);
		field.SetValue(null, specialAbility);
	}

	public StatIconBuilder<T> SetAppliesToAttack(bool appliesToAttack = true)
	{
		_statIconInfo.appliesToAttack = appliesToAttack;
		return this;
	}
	
	public StatIconBuilder<T> SetAppliesToHealth(bool appliesToHealth = true)
	{
		_statIconInfo.appliesToHealth = appliesToHealth;
		return this;
	}
	
	public StatIconBuilder<T> SetRulebookName(string rulebookName)
	{
		_statIconInfo.rulebookName = rulebookName;
		return this;
	}

	public StatIconBuilder<T> SetRulebookDescription(string rulebookDescription)
	{
		_statIconInfo.rulebookDescription = rulebookDescription;
		return this;
	}

	public StatIconBuilder<T> SetRulebookDescriptionGBC(string rulebookDescription)
	{
		_statIconInfo.gbcDescription = rulebookDescription;
		return this;
	}

	public StatIconBuilder<T> SetIconGraphic(Texture iconGraphic)
	{
		_statIconInfo.iconGraphic = iconGraphic;
		return this;
	}
	
	public StatIconBuilder<T> SetPixelIconGraphic(Sprite pixelIconGraphic)
	{
		_statIconInfo.pixelIconGraphic = pixelIconGraphic;
		return this;
	}
}
