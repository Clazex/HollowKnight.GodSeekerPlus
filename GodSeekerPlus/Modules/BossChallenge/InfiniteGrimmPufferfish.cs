namespace GodSeekerPlus.Modules.BossChallenge;

public sealed class InfiniteGrimmPufferfish : Module {
	private static readonly SceneEdit grimmHandle = new(
		new("GG_Grimm", "Grimm Scene", "Grimm Boss"),
		ModifyGrimmFSM
	);

	private static readonly SceneEdit nkgHandle = new(
		new("GG_Grimm_Nightmare", "Grimm Control", "Nightmare Grimm Boss"),
		ModifyGrimmFSM
	);

	public override ToggleableLevel ToggleableLevel => ToggleableLevel.ChangeScene;

	private protected override void Load() {
		grimmHandle.Enable();
		nkgHandle.Enable();
	}

	private protected override void Unload() {
		grimmHandle.Disable();
		nkgHandle.Disable();
	}

	private static void ModifyGrimmFSM(GameObject go) {
		if (BossSequenceController.IsInSequence) {
			return;
		}

		go.LocateMyFSM("Control")
			.ChangeTransition("Out Pause", FsmEvent.Finished.Name, "Balloon Pos");

		LogDebug("Grimm FSM modified");
	}
}
