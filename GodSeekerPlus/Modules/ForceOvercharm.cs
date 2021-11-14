using Modding;
using Logger = GodSeekerPlus.Util.Logger;

namespace GodSeekerPlus.Modules {
	[Module(toggleable = true, defaultEnabled = false)]
	internal sealed class ForceOvercharm : Module {
		private protected override void Load() =>
			ModHooks.CharmUpdateHook += DoOvercharm;

		private protected override void Unload() =>
			ModHooks.CharmUpdateHook -= DoOvercharm;

		private static void DoOvercharm(PlayerData pd, HeroController _) {
			if (!pd.overcharmed) {
				pd.canOvercharm = true;
				pd.overcharmed = true;

				Logger.LogDebug("Force overcharmed");
			}
		}
	}
}
