using System.Threading;
using Modding;

namespace GodSeekerPlus.Modules {
	internal static class FrameRateLimit {
		public static void Load() => ModHooks.HeroUpdateHook += ThreadSleep;

		public static void Unload() => ModHooks.HeroUpdateHook -= ThreadSleep;

		public static void ThreadSleep() => Thread.Sleep(10 * GodSeekerPlus.Instance.GlobalSettings.frameRateLimitMultiplier);
	}
}
