using Modding;

namespace GodSeekerPlus.Modules {
	internal sealed class CarefreeMelodyFix : Module {
		private const string targetEntry = "destroyedNightmareLantern";

		private protected override void Load() => ModHooks.Instance.CharmUpdateHook += WatchAndFixCarefreeMelody;

		private protected override void Unload() => ModHooks.Instance.CharmUpdateHook -= WatchAndFixCarefreeMelody;

		private protected override bool ShouldLoad() => GodSeekerPlus.Instance.GlobalSetting.carefreeMelodyFix;

		private static void WatchAndFixCarefreeMelody(PlayerData data, HeroController controller) {
			if (controller.carefreeShieldEquipped && !data.GetBool(targetEntry)) {
				data.SetBool(targetEntry, true);

				Logger.LogDebug("Carefree Melody fixed");
			}
		}
	}
}
