using DiskCardGame;

namespace GrimoraMod;

public static class ViewManagerExtensions
{
	public static void SetViewLocked(this ViewManager manager)
	{
		manager.Controller.LockState = ViewLockState.Locked;
	}
	
	public static void SetViewUnlocked(this ViewManager manager)
	{
		manager.Controller.LockState = ViewLockState.Unlocked;
	}
}
