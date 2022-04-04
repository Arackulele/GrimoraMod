namespace GrimoraMod;

public class EncounterJsonUtil
{
	[Serializable]
	public class EncountersFromJson
	{
		public List<EncounterJson> encounters;

		public EncountersFromJson()
		{
		}

		public EncountersFromJson(List<EncounterJson> encounters)
		{
			this.encounters = encounters;
		}
	}
}
