namespace GodSeekerPlus.Modules.QoL;

[DefaultEnabled]
internal sealed class FastDash : Module {
	private static readonly string[] sceneNames = new[] {
		"GG_Workshop",
		"GG_Atrium",
		"GG_Atrium_Roof"
	};

	private protected override void Load() =>
		OsmiHooks.SceneChangeHook += HookDash;

	private protected override void Unload() {
		OsmiHooks.SceneChangeHook -= HookDash;
		On.HeroController.HeroDash -= CancelCooldown;
	}

	// Hook only when needed for zero performance impact
	private void HookDash(Scene prev, Scene next) {
		On.HeroController.HeroDash -= CancelCooldown;

		if (sceneNames.Contains(next.name)) {
			On.HeroController.HeroDash += CancelCooldown;
		}
	}

	private static void CancelCooldown(On.HeroController.orig_HeroDash orig, HeroController self) {
		orig(self);

		HeroControllerR.dashCooldownTimer = 0;
	}
}
