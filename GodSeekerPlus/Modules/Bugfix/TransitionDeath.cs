namespace GodSeekerPlus.Modules.Bugfix;

[DefaultEnabled]
internal sealed class TransitionDeath : Module {
	private protected override void Load() {
		On.HeroController.Die += RecordDeath;
	}

	private protected override void Unload() {
		On.HeroController.Die -= RecordDeath;
	}

	private IEnumerator RecordDeath(On.HeroController.orig_Die orig, HeroController self) {
		yield return orig(self);
		HeroControllerR.cState.dead = true;
	}
}
