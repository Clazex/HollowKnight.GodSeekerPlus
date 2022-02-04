using System.Threading;

namespace GodSeekerPlus.Modules.Restrictions;

[Category(nameof(Restrictions))]
internal sealed class FrameRateLimit : Module {
	private int time = default;

	private protected override void Load() {
		time = 10 * GodSeekerPlus.GlobalSettings.frameRateLimitMultiplier;
		ModHooks.HeroUpdateHook += ThreadSleep;
	}

	private protected override void Unload() =>
		ModHooks.HeroUpdateHook -= ThreadSleep;

	private void ThreadSleep() => Thread.Sleep(time);
}
