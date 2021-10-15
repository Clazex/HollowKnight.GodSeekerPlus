using Modding;

namespace GodSeekerPlus.Modules {
	internal sealed class CarefreeMelodyFix : Module {
		private const string targetEntry = "destroyedNightmareLantern";

		public override void Load() => ModHooks.CharmUpdateHook += WatchAndFixCarefreeMelody;

		public override void Unload() => ModHooks.CharmUpdateHook -= WatchAndFixCarefreeMelody;

		public override bool ShouldLoad() => GodSeekerPlus.Instance.GlobalSettings.carefreeMelodyFix;

		private static void WatchAndFixCarefreeMelody(PlayerData data, HeroController controller) {
			if (controller.carefreeShieldEquipped && !data.GetBool(targetEntry)) {
				data.SetBool(targetEntry, true);

				Logger.LogDebug("Carefree Melody fixed");
			}
		}
	}
}
