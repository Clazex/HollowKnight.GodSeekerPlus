namespace GodSeekerPlus.Modules.BossChallenge;

[ToggleableLevel(ToggleableLevel.ChangeScene)]
internal sealed class CarefreeMelodyReset : Module {
	private protected override void Load() =>
		USceneManager.activeSceneChanged += ResetCount;

	private protected override void Unload() =>
		USceneManager.activeSceneChanged -= ResetCount;

	private void ResetCount(Scene prev, Scene next) {
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
		Logger.LogDebug("Carefree Melody hit count reset to max");
	}
}
