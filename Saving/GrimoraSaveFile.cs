using DiskCardGame;
using GBC;

namespace GrimoraMod.Saving;

public class GrimoraSaveFile
{
	public GrimoraRunState CurrentRun;
	public GrimoraAscensionSaveData AscensionSaveData;

	public void Initialize()
	{
		CurrentRun = new GrimoraRunState();
		CurrentRun.Initialize();
		
		AscensionSaveData = new GrimoraAscensionSaveData();
		AscensionSaveData.Initialize();
	}

	public void NewAscensionRun()
	{
		GrimoraRunState currentRun = (GrimoraRunState)AscensionSaveData.currentRun;
		currentRun.NewStandardGame();
	}

	public void NewStandardRun()
	{
		CurrentRun.NewStandardGame();
	}
	
	
}
