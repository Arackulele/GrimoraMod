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
		GrimoraPlugin.Log.LogInfo("[GrimoraSaveFile] NewAscensionRun");
		AscensionSaveData.Initialize();
		AscensionSaveData.NewRun(new List<CardInfo>());
	}

	public void NewStandardRun()
	{
		CurrentRun.Initialize();
		CurrentRun.NewStandardGame();
	}
}
