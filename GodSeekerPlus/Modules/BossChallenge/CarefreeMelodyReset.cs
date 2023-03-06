namespace GodSeekerPlus.Modules.BossChallenge;

public sealed class CarefreeMelodyReset : Module {
	public override ToggleableLevel ToggleableLevel => ToggleableLevel.ChangeScene;

	private protected override void Load() =>
		OsmiHooks.SceneChangeHook += ResetCount;

	private protected override void Unload() =>
		OsmiHooks.SceneChangeHook -= ResetCount;

	private static void ResetCount(Scene prev, Scene next) {
		if (Ref.HC == null) {
			return;
		}

		if (!Ref.HC.carefreeShieldEquipped) {
			return;
		}

		if (BossSceneController.IsBossScene || Ref.GM.sm.mapZone != MapZone.GODS_GLORY) {
			return;
		}

		HeroControllerR.hitsSinceShielded = 7;
		LogDebug("Carefree Melody hit count reset to max");
	}
}
