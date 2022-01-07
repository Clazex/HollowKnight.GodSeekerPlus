namespace GodSeekerPlus.Modules.QoL;

[Category(nameof(QoL))]
[DefaultEnabled]
internal sealed class CarefreeMelodyFix : Module {
	private protected override void Load() =>
		ModHooks.CharmUpdateHook += WatchAndFixCarefreeMelody;

	private protected override void Unload() =>
		ModHooks.CharmUpdateHook -= WatchAndFixCarefreeMelody;

	private void WatchAndFixCarefreeMelody(PlayerData pd, HeroController hc) {
		if (hc.carefreeShieldEquipped && !pd.destroyedNightmareLantern) {
			pd.destroyedNightmareLantern = true;

			Logger.LogDebug("Carefree Melody fixed");
		}
	}
}
