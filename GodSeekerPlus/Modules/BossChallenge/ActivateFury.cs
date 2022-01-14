namespace GodSeekerPlus.Modules.BossChallenge;

[Category(nameof(BossChallenge))]
[ToggleableLevel(ToggleableLevel.ChangeScene)]
internal sealed class ActivateFury : Module {
	private static readonly Dictionary<string, float> extraWaitScenes = new() {
		{ "GG_Nosk", 0.25f },
		{ "GG_Soul_Tyrant", 0.5f }
	};

	private protected override void Load() =>
		On.BossSceneController.Start += Activate;

	private protected override void Unload() =>
		On.BossSceneController.Start -= Activate;

	private IEnumerator Activate(On.BossSceneController.orig_Start orig, BossSceneController self) {
		yield return orig(self);

		if (!BossSequenceController.IsInSequence || BossSequenceController.BossIndex == 0) {
			if (Ref.PD.equippedCharm_6) {
				if (Ref.PD.equippedCharm_27) {
					Ref.PD.joniHealthBlue = 1;
				} else {
					Ref.PD.health = 1;
				}

				Ref.HC.StartCoroutine(UpdateState());
			}
		}
	}

	private static IEnumerator UpdateState() {
		PlayMakerFSM fsm = Ref.GC.hudCanvas.LocateMyFSM("Slide Out");

		yield return new WaitUntil(() => Ref.GM.gameState == GameState.PLAYING);
		yield return new WaitUntil(() => Ref.HC.acceptingInput);

		// Avoid mask display issue
		yield return new WaitUntil(() => fsm.ActiveStateName == "In" || fsm.ActiveStateName == "Idle");

		// Avoid evading roar
		string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
		if (extraWaitScenes.ContainsKey(sceneName)) {
			yield return new WaitForSeconds(extraWaitScenes[sceneName]);
			yield return new WaitUntil(() => !Ref.HC.controlReqlinquished);
		}

		Ref.HC.proxyFSM.SendEvent("HeroCtrl-HeroDamaged");

		Logger.LogDebug("Fury activated");
	}
}
