namespace GodSeekerPlus.Modules.BossChallenge;

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

		if (BossSequenceController.IsInSequence && BossSequenceController.BossIndex != 0) {
			yield break;
		}

		Activate();
	}

	internal void Activate() {
		if (!CharmUtil.EquippedCharm(Charm.FuryOfTheFallen)) {
			return;
		}

		if (CharmUtil.EquippedCharm(Charm.JonisBlessing)) {
			Ref.PD.joniHealthBlue = 1;
		} else {
			Ref.PD.health = 1;
		}

		Ref.HC.StartCoroutine(UpdateState());
	}

	private static IEnumerator UpdateState() {
		PlayMakerFSM fsm = Ref.GC.hudCanvas.LocateMyFSM("Slide Out");

		yield return new WaitUntil(() => Ref.GM.gameState == GameState.PLAYING);
		yield return new WaitUntil(() => Ref.HC.acceptingInput);

		// Avoid mask display issue
		yield return new WaitUntil(() => fsm.ActiveStateName == "In" || fsm.ActiveStateName == "Idle");

		// Avoid evading roar
		if (extraWaitScenes.TryGetValue(Ref.GM.sceneName, out float waitTime)) {
			yield return new WaitForSeconds(waitTime);
			yield return new WaitUntil(() => !Ref.HC.controlReqlinquished);
		}

		Ref.HC.proxyFSM.SendEvent("HeroCtrl-HeroDamaged");

		Logger.LogDebug("Fury activated");
	}
}
