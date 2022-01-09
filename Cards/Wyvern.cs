using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{
		private void AddAra_Wyvern()
		{
			ApiUtils.Add("ara_Wyvern", "Wyvern", 1, 1,
				"The Wyvern army approaches.", 1, Resources.Wyvern, Ability.DrawCopy
			);
		}
	}
}