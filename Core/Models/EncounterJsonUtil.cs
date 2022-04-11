using System.Runtime.Serialization;

namespace GrimoraMod;

public class EncounterJsonUtil
{
	[DataContract]
	public class EncountersFromJson
	{
		[DataMember]
		public List<EncounterJson> encounters { get; set; }
	}
}
