namespace GodSeekerPlus.Modules.BossChallenge;

[ToggleableLevel(ToggleableLevel.ChangeScene)]
internal sealed class AddSoul : Module {
	private protected override void Load() =>
		On.BossSceneController.Start += Add;

	private protected override void Unload() =>
		On.BossSceneController.Start -= Add;

	private IEnumerator Add(On.BossSceneController.orig_Start orig, BossSceneController self) {
		yield return orig(self);

		if (BossSequenceController.IsInSequence) {
			yield break;
		}

		Ref.HC.AddMPCharge(Setting.Global.SoulAmount);

		Ref.HC.StartCoroutine(UpdateHUD());

		Logger.LogDebug("Soul added");
	}

	private static IEnumerator UpdateHUD() {
		yield return new WaitUntil(() => Ref.GM.gameState == GameState.PLAYING);

		Ref.GC.soulOrbFSM.SendEvent("MP GAIN SPA");
		Ref.GC.soulVesselFSM.SendEvent("MP RESERVE UP");
	}
}
