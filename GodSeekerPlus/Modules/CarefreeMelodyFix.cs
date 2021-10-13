using Modding;

namespace GodSeekerPlus.Modules {
	internal static class CarefreeMelodyFix {
		private const string targetEntry = "destroyedNightmareLantern";

		public static void Load() => ModHooks.CharmUpdateHook += WatchAndFixCarefreeMelody;

		public static void Unload() => ModHooks.CharmUpdateHook -= WatchAndFixCarefreeMelody;

		public static void WatchAndFixCarefreeMelody(PlayerData data, HeroController controller) {
			if (controller.carefreeShieldEquipped && !data.GetBool(targetEntry)) {
				data.SetBool(targetEntry, true);

				GodSeekerPlus.Instance.Log("Carefree Melody fixed");
			}
		}
	}
}
