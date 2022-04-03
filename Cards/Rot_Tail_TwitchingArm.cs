namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameRotTail = $"{GUID}_Rot_tail";

	private void Add_Card_RotTail()
	{
		CardBuilder.Builder
			.SetBaseAttackAndHealth(0, 1)
			.SetNames(NameRotTail, "Twitching Arm")
			.Build();
	}
}
