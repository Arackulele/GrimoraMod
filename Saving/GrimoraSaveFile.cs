using DiskCardGame;
using GBC;

namespace GrimoraMod.Saving;

public class GrimoraSaveFile
{
	public GrimoraRunState CurrentRun = new GrimoraRunState();
	public GrimoraAscensionSaveData AscensionSaveData = new GrimoraAscensionSaveData();

	public void Initialize()
	{
		CurrentRun.Initialize();
		AscensionSaveData.Initialize();
	}

	public void NewAscensionRun()
	{
		GrimoraRunState currentRun = (GrimoraRunState)AscensionSaveData.currentRun;
		currentRun.Initialize();
		currentRun.NewStandardGame();
	}

	public void NewStandardRun()
	{
		CurrentRun.Initialize();
		CurrentRun.NewStandardGame();
	}
	
	
}
