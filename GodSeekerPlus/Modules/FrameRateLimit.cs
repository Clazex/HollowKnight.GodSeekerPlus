using System.Threading;
using Modding;

namespace GodSeekerPlus.Modules {
	internal sealed class FrameRateLimit : Module {
		private static int time = 50;

		private protected override void Load() {
			time = 10 * GodSeekerPlus.Instance.GlobalSetting.frameRateLimitMultiplier;
			ModHooks.Instance.HeroUpdateHook += ThreadSleep;
		}

		private protected override void Unload() => ModHooks.Instance.HeroUpdateHook -= ThreadSleep;

		private protected override bool ShouldLoad() => GodSeekerPlus.Instance.GlobalSetting.frameRateLimit;

		private static void ThreadSleep() => Thread.Sleep(time);
	}
}
