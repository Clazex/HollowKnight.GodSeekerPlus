namespace GodSeekerPlus.Modules.BossChallenge;

[Category(nameof(BossChallenge))]
[ToggleableLevel(ToggleableLevel.ChangeScene)]
internal sealed class AddLifeblood : Module {
	private protected override void Load() =>
		On.BossSceneController.Start += Add;

	private protected override void Unload() =>
		On.BossSceneController.Start -= Add;

	private IEnumerator Add(On.BossSceneController.orig_Start orig, BossSceneController self) {
		yield return orig(self);

		if (BossSequenceController.IsInSequence) {
			yield break;
		}

		for (int i = 0; i < GodSeekerPlus.UnsafeInstance.GlobalSettings.lifebloodAmount; i++) {
			EventRegister.SendEvent("ADD BLUE HEALTH");
		}

		Logger.LogDebug("Lifeblood added");
	}
}
