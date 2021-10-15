using System.Threading;
using Modding;

namespace GodSeekerPlus.Modules {
	internal sealed class FrameRateLimit : Module {
		private protected override void Load() => ModHooks.HeroUpdateHook += ThreadSleep;

		private protected override void Unload() => ModHooks.HeroUpdateHook -= ThreadSleep;

		private protected override bool ShouldLoad() => GodSeekerPlus.Instance.GlobalSettings.frameRateLimit;

		private static void ThreadSleep() => Thread.Sleep(10 * GodSeekerPlus.Instance.GlobalSettings.frameRateLimitMultiplier);
	}
}
