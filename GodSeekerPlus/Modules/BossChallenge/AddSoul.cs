namespace GodSeekerPlus.Modules.BossChallenge;

public sealed class AddSoul : Module {
	[GlobalSetting]
	[IntOption(0, 198, 11)]
	public static int soulAmount = 99;

	public override ToggleableLevel ToggleableLevel => ToggleableLevel.ChangeScene;

	private protected override void Load() =>
		On.BossSceneController.Start += Add;

	private protected override void Unload() =>
		On.BossSceneController.Start -= Add;

	private static IEnumerator Add(On.BossSceneController.orig_Start orig, BossSceneController self) {
		yield return orig(self);

		if (BossSequenceController.IsInSequence) {
			yield break;
		}

		Ref.HC.AddMPCharge(soulAmount);

		_ = Ref.HC.StartCoroutine(UpdateHUD());

		LogDebug("Soul added");
	}

	private static IEnumerator UpdateHUD() {
		yield return new WaitUntil(() => Ref.GM.gameState == GameState.PLAYING);

		Ref.GC.soulOrbFSM.SendEvent("MP GAIN SPA");
		Ref.GC.soulVesselFSM.SendEvent("MP RESERVE UP");
	}
}
