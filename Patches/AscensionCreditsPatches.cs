using DiskCardGame;
using GrimoraMod;
using HarmonyLib;


namespace GrimoraMod
{
	[HarmonyPatch]
	internal static class CreditsManagement
	{
		[HarmonyPatch(typeof(CreditsDisplayer), nameof(CreditsDisplayer.Start))]
		[HarmonyPrefix]
		public static void ModifyCredits(CreditsDisplayer __instance)
		{
			if (GrimoraSaveUtil.IsGrimoraModRun)
			{
				__instance.creditsData.credits.Clear();

				List<CreditEntry> AllCredits = new List<CreditEntry>
				{

				new CreditEntry("Ara(ckulele)", "Creator"),
				new CreditEntry("Julian Mods", "Programming"),

				new CreditEntry("Addie Brahem", "Soundtrack"),

				new CreditEntry("Nevernamed", "Artwork"),

				new CreditEntry("Pinks8n", "3D Modelling"),
				new CreditEntry("Catboy Stinkbug", "3D Modelling"),

				new CreditEntry("JamesGames", "Save + Items Manager"),
				new CreditEntry("Kopie", "Additional Code"),
				new CreditEntry("Ourochi", "Additional Code"),

				new CreditEntry("Cevin_2006", "Additional Art, VFX and code"),
				new CreditEntry("Anne Bean", "Additional Card Portraits"),
				new CreditEntry("Amy", "Additional Art"),
				new CreditEntry("Gold ( i eat rocks )", "Additional Art"),
				new CreditEntry("Lich Underling", "Additional Art"),
				new CreditEntry("Nonexistant", "Additional Art"),

				new CreditEntry("JestAnotherAnimator", "Attack animations"),
				new CreditEntry("Draconis17", "Energy Drone Game Object"),

				new CreditEntry("Bob the Nerd", "Adittional Dialogue"),
				new CreditEntry("Spooky Pig", "Adittional Dialogue"),

				new CreditEntry("Daniel Muliins", "Creator of Inscryption"),
				new CreditEntry("My Brothers", "Letting me annoy them about this"),
				new CreditEntry("Tatum", "Dog"),
				new CreditEntry("Anton", "Suprisingly, also Dog"),
				new CreditEntry("Cat aka Bastard Man aka Little Guy", "Cat - co-creator"),

				new CreditEntry("DivisionByZorro", "P03KCMod"),
				new CreditEntry("Silenceman", "Magnificus Mod (3 times)"),
				new CreditEntry("API Developers", "Inscryption API (Duh)"),
				new CreditEntry("Boo Hag", "Obvious reasons"),
				};

				if (EventManagement.HasBeatenSkullStorm)
				{
					AllCredits.Add(new CreditEntry("The Bone Lord", "Your overlord"));

					AllCredits.Add(new CreditEntry("Grimora", "Your dearest Scrybe"));

					AllCredits.Add(new CreditEntry("The Old Data", "[Redacted]"));

					AllCredits.Add(new CreditEntry("Thank you for playing", " "));
						
						}

				foreach(var entry in AllCredits) { 
				__instance.creditsData.credits.Add(entry);
				}
			}
		}
	}
}
