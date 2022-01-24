namespace GodSeekerPlus.Modules.Restrictions;

[Category(nameof(Restrictions))]
internal sealed class ForceOvercharm : Module {
	private protected override void Load() =>
		ModHooks.CharmUpdateHook += DoOvercharm;

	private protected override void Unload() =>
		ModHooks.CharmUpdateHook -= DoOvercharm;

	private void DoOvercharm(PlayerData pd, HeroController _) {
		if (pd.overcharmed) {
			return;
		}

		pd.canOvercharm = true;
		pd.overcharmed = true;

		Logger.LogDebug("Force overcharmed");
	}
}
