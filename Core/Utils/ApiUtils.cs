using System.Reflection;
using DiskCardGame;
using InscryptionAPI.Card;
using Sirenix.Utilities;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;


public static class ApiUtils
{
	public static void CreateSpecialAbility<T>(string nameOfAbility = null) where T : SpecialCardBehaviour
	{
		Type type = typeof(T);
		string finalName = nameOfAbility;
		if (nameOfAbility.IsNullOrWhitespace())
		{
			finalName = type.Name;
		}

		Log.LogDebug($"Starting to add special ability [{type}]");
		var specialAbility = SpecialTriggeredAbilityManager.Add(GUID, finalName, type);

		FieldInfo field = type.GetField(
			"FullSpecial",
			BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance
		);
		field.SetValue(null, specialAbility);
	}
}
