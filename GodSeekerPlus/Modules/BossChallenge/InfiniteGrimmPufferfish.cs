namespace GodSeekerPlus.Modules.BossChallenge;

public sealed class InfiniteGrimmPufferfish : Module {
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
				scene.name: "GG_Grimm" or "GG_Grimm_Nightmare",
				name: "Grimm Boss"
			},
			FsmName: "Control"
		}) {
			self.ChangeTransition("Out Pause", FsmEvent.Finished.Name, "Balloon Pos");
			Logger.LogDebug("Grimm FSM modified");
		}
	}
}
