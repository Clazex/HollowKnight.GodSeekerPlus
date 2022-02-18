namespace GodSeekerPlus.Modules.BossChallenge;

[Category(nameof(BossChallenge))]
[ToggleableLevel(ToggleableLevel.ChangeScene)]
internal sealed class InfiniteGrimmPufferfish : Module {
	private protected override void Load() =>
		On.PlayMakerFSM.Start += ModifyFSM;

	private protected override void Unload() =>
		On.PlayMakerFSM.Start -= ModifyFSM;

	private void ModifyFSM(On.PlayMakerFSM.orig_Start orig, PlayMakerFSM self) {
		orig(self);

		if (BossSequenceController.IsInSequence) {
			return;
		}

		if (self is {
			gameObject: {
				scene.name: "GG_Grimm",
				name: "Grimm Boss"
			},
			FsmName: "Control"
		}) {
			ModifyGrimmFSM(self);

			Logger.LogDebug("Grimm FSM modified");
		} else if (self is {
			gameObject: {
				scene.name: "GG_Grimm_Nightmare",
				name: "Nightmare Grimm Boss"
			},
			FsmName: "Control"
		}) {
			ModifyNKGFSM(self);

			Logger.LogDebug("NKG FSM modified");
		}
	}

	private static void ModifyGrimmFSM(PlayMakerFSM fsm) =>
		fsm.ChangeTransition("Out Pause", FsmEvent.Finished.Name, "Balloon Pos");

	private static void ModifyNKGFSM(PlayMakerFSM fsm) =>
		fsm.ChangeTransition("Out Pause", FsmEvent.Finished.Name, "Balloon Pos");
}
