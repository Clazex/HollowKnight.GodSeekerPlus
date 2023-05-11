namespace GodSeekerPlus.Modules.QoL;

public sealed class FastDash : Module {
	public static readonly List<Func<Scene, bool>> predicates = new() {
		(scene) => scene.name is "GG_Workshop" or "GG_Atrium" or "GG_Atrium_Roof"
	};

	public override bool DefaultEnabled => true;

	private protected override void Load() =>
		OsmiHooks.SceneChangeHook += HookDash;

	private protected override void Unload() {
		OsmiHooks.SceneChangeHook -= HookDash;
		On.HeroController.HeroDash -= CancelCooldown;
	}

	// Hook only when needed for zero performance impact
	private static void HookDash(Scene prev, Scene next) {
		On.HeroController.HeroDash -= CancelCooldown;

		if (predicates.Any(predicate => predicate.Invoke(next))) {
			On.HeroController.HeroDash += CancelCooldown;
		}
	}

	private static void CancelCooldown(On.HeroController.orig_HeroDash orig, HeroController self) {
		orig(self);

		HeroControllerR.dashCooldownTimer = 0;
	}
}
