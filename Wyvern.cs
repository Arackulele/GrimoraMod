using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod
{
	public partial class GrimoraPlugin
	{
		private void AddAra_Wyvern()
		{
			ApiUtils.Add("ara_Wyvern", "Wyvern",
				"The Wyvern army approaches.", 0, 1, 1, 5, Resources.Wyvern, Ability.DrawCopy, CardMetaCategory.Rare);
		}
	}
}