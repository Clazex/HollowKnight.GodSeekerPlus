using System.Threading;
using Modding;

namespace GodSeekerPlus {
	internal static class FrameRateLimit {
		public static void Hook() => ModHooks.HeroUpdateHook += OnHeroUpdate;

		public static void UnHook() => ModHooks.HeroUpdateHook -= OnHeroUpdate;

		public static void OnHeroUpdate() => Thread.Sleep(50);
	}
}
