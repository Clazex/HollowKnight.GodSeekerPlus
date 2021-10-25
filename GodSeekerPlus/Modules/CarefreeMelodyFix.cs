using Modding;
using Logger = GodSeekerPlus.Util.Logger;

namespace GodSeekerPlus.Modules {
	[Module(toggleable = true, defaultEnabled = true)]
	internal sealed class CarefreeMelodyFix : Module {
		private const string targetEntry = "destroyedNightmareLantern";

		private protected override void Load() => ModHooks.CharmUpdateHook += WatchAndFixCarefreeMelody;

		private protected override void Unload() => ModHooks.CharmUpdateHook -= WatchAndFixCarefreeMelody;

		private static void WatchAndFixCarefreeMelody(PlayerData data, HeroController controller) {
			if (controller.carefreeShieldEquipped && !data.GetBool(targetEntry)) {
				data.SetBool(targetEntry, true);

				Logger.LogDebug("Carefree Melody fixed");
			}
		}
	}
}
