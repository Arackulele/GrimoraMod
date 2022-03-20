namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameWardingPresence = $"{GUID}_WardingPresence";

	private void Add_WardingPresence()
	{
		CardBuilder.Builder
			.SetBaseAttackAndHealth(0, 1)
			.SetNames(NameWardingPresence, "Warding Presence")
			.Build();
	}
}
