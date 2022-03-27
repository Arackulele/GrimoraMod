namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameRotTail = $"{GUID}_Rot_tail";

	private void Add_RotTail()
	{
		CardBuilder.Builder
			.SetBaseAttackAndHealth(0, 1)
			// .SetDescription("A SENSE OF DREAD CONSUMES YOU AS YOU REALIZE YOU ARE NOT ALONE IN THESE WOODS.")
			.SetNames(NameRotTail, "Twitching Arm")
			.Build();
	}
}
