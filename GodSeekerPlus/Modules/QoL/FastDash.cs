namespace GodSeekerPlus.Modules.QoL;

[DefaultEnabled]
internal sealed class FastDash : Module {
	private protected override void Load() =>
		USceneManager.activeSceneChanged += HookDash;

	private protected override void Unload() =>
		USceneManager.activeSceneChanged -= HookDash;

	// Hook only when needed for zero performance impact
	private void HookDash(Scene _, Scene next) {
		On.HeroController.HeroDash -= CancelCooldown;

		if (next.name == "GG_Workshop") {
			On.HeroController.HeroDash += CancelCooldown;
		}
	}

	private static void CancelCooldown(On.HeroController.orig_HeroDash orig, HeroController self) {
		orig(self);

		ReflectionHelper.SetField(self, "dashCooldownTimer", 0f);
	}
}
