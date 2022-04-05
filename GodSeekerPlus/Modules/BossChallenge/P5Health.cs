namespace GodSeekerPlus.Modules.BossChallenge;

[ToggleableLevel(ToggleableLevel.ChangeScene)]
internal sealed class P5Health : Module {
	private protected override void Load() =>
		On.BossSceneController.Awake += OverrideLevel;

	private protected override void Unload() =>
		On.BossSceneController.Awake -= OverrideLevel;

	private void OverrideLevel(On.BossSceneController.orig_Awake orig, BossSceneController self) {
		orig(self);

		self.BossLevel = 0;
	}
}
