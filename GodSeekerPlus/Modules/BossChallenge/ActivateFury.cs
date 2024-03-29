namespace GodSeekerPlus.Modules.BossChallenge;

public sealed class ActivateFury : Module {
	private static readonly Dictionary<string, float> extraWaitScenes = new() {
		{ "GG_Nosk", 0.25f },
		{ "GG_Soul_Tyrant", 0.5f }
	};

	public override ToggleableLevel ToggleableLevel => ToggleableLevel.ChangeScene;

	private protected override void Load() =>
		On.BossSceneController.Start += Activate;

	private protected override void Unload() =>
		On.BossSceneController.Start -= Activate;

	private static IEnumerator Activate(On.BossSceneController.orig_Start orig, BossSceneController self) {
		yield return orig(self);

		if (BossSequenceController.IsInSequence && BossSequenceController.BossIndex != 0) {
			yield break;
		}

		Activate();
	}

	internal static void Activate() {
		if (!CharmUtil.EquippedCharm(Charm.FuryOfTheFallen)) {
			return;
		}

		if (CharmUtil.EquippedCharm(Charm.JonisBlessing)) {
			PlayerDataR.joniHealthBlue = 1;
		} else {
			PlayerDataR.health = 1;
		}

		_ = Ref.HC.StartCoroutine(UpdateState());
	}

	private static IEnumerator UpdateState() {
		PlayMakerFSM fsm = Ref.GC.hudCanvas.LocateMyFSM("Slide Out");

		yield return new WaitUntil(() => Ref.GM.gameState == GameState.PLAYING);
		yield return new WaitUntil(() => Ref.HC.acceptingInput);

		// Avoid mask display issue
		yield return new WaitUntil(() => fsm.ActiveStateName is "In" or "Idle");

		// Avoid evading roar
		if (extraWaitScenes.TryGetValue(Ref.GM.sceneName, out float waitTime)) {
			yield return new WaitForSeconds(waitTime);
			yield return new WaitUntil(() => !Ref.HC.controlReqlinquished);
		}

		Ref.HC.proxyFSM.SendEvent("HeroCtrl-HeroDamaged");

		LogDebug("Fury activated");
	}
}
