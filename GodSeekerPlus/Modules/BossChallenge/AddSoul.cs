namespace GodSeekerPlus.Modules.BossChallenge;

[Category(nameof(BossChallenge))]
[ToggleableLevel(ToggleableLevel.ChangeScene)]
internal sealed class AddSoul : Module {
	private protected override void Load() =>
		On.BossSceneController.Start += Add;

	private protected override void Unload() =>
		On.BossSceneController.Start -= Add;

	private IEnumerator Add(On.BossSceneController.orig_Start orig, BossSceneController self) {
		yield return orig(self);

		if (!BossSequenceController.IsInSequence) {
			HeroController.instance.AddMPCharge(
				GodSeekerPlus.UnsafeInstance.GlobalSettings.soulAmount
			);

			HeroController.instance.StartCoroutine(UpdateHUD());

			Logger.LogDebug("Soul added");
		}
	}

	private static IEnumerator UpdateHUD() {
		yield return new WaitUntil(() => Ref.GM.gameState == GameState.PLAYING);

		GameCameras.instance.soulOrbFSM.SendEvent("MP GAIN SPA");
		GameCameras.instance.soulVesselFSM.SendEvent("MP RESERVE UP");
	}
}
