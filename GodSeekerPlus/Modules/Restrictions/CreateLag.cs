using System.Threading;

namespace GodSeekerPlus.Modules.Restrictions;

public sealed class CreateLag : Module {
	[GlobalSetting]
	[IntOption(0, 1000, 25)]
	public static int lagTime = 50;

	private protected override void Load() =>
		ModHooks.HeroUpdateHook += ThreadSleep;

	private protected override void Unload() =>
		ModHooks.HeroUpdateHook -= ThreadSleep;

	private static void ThreadSleep() {
		if (Ref.GM.IsGamePaused()) {
			return;
		}

		Thread.Sleep(lagTime);
	}
}
