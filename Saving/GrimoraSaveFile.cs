using DiskCardGame;
using GBC;

namespace GrimoraMod.Saving;

public class GrimoraSaveFile
{
	public GrimoraRunState CurrentRun;
	public GrimoraAscensionSaveData AscensionSaveData;

	public void Initialize()
	{
		NewStandardRun();

		NewAscensionRun();
	}

	private void NewAscensionRun()
	{
		AscensionSaveData = new GrimoraAscensionSaveData();
		AscensionSaveData.Initialize();
	}

	public void NewStandardRun()
	{
		CurrentRun = new GrimoraRunState();
		CurrentRun.Initialize();

	}
}
