using System.Threading;

namespace GodSeekerPlus.Modules.Restrictions;

internal sealed class CreateLag : Module {
	[GlobalSetting]
	[IntOption(0, 1000, 25)]
	private static readonly int lagTime = 50;

	private protected override void Load() =>
		ModHooks.HeroUpdateHook += ThreadSleep;

	private protected override void Unload() =>
		ModHooks.HeroUpdateHook -= ThreadSleep;

	private void ThreadSleep() {
		if (Ref.GM.IsGamePaused()) {
			return;
		}

		Thread.Sleep(lagTime);
	}
}
