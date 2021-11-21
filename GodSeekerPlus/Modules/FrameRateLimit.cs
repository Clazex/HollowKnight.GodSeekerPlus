using System.Threading;

namespace GodSeekerPlus.Modules;

[Module(toggleable = true, defaultEnabled = false)]
internal sealed class FrameRateLimit : Module {
	private static int time = 50;

	private protected override void Load() {
		time = 10 * GodSeekerPlus.Instance.GlobalSettings.frameRateLimitMultiplier;
		ModHooks.HeroUpdateHook += ThreadSleep;
	}

	private protected override void Unload() =>
		ModHooks.HeroUpdateHook -= ThreadSleep;

	private static void ThreadSleep() => Thread.Sleep(time);
}
