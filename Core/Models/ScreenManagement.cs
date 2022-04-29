using InscryptionAPI.Saves;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public static class ScreenManagement
{
	public static CardTemple ScreenState
	{
		get
		{
			string value = ModdedSaveManager.SaveData.GetValue(GUID, "ScreenState");
			// Log.LogDebug($"[CardTemple.ScreenState.Getter] Value is [{value}]");
			return string.IsNullOrEmpty(value) ? CardTemple.Nature : (CardTemple)Enum.Parse(typeof(CardTemple), value);
		}
		set => ModdedSaveManager.SaveData.SetValue(GUID, "ScreenState", value);
	}
}
