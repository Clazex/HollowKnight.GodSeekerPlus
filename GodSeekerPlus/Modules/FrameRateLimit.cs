using System.Threading;
using Modding;

namespace GodSeekerPlus.Modules {
	internal sealed class FrameRateLimit : Module {
		public override void Load() => ModHooks.HeroUpdateHook += ThreadSleep;

		public override void Unload() => ModHooks.HeroUpdateHook -= ThreadSleep;

		public override bool ShouldLoad() => GodSeekerPlus.Instance.GlobalSettings.frameRateLimit;

		private static void ThreadSleep() => Thread.Sleep(10 * GodSeekerPlus.Instance.GlobalSettings.frameRateLimitMultiplier);
	}
}
