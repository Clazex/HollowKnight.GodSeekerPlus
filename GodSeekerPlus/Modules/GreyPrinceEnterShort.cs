namespace GodSeekerPlus.Modules;

[Category("QoL")]
[ToggleableLevel(ToggleableLevel.ChangeScene)]
[DefaultEnabled]
internal sealed class GreyPrinceEnterShort : Module {
	private protected override void Load() =>
		On.PlayMakerFSM.Start += ModifyGPFSM;

	private protected override void Unload() =>
		On.PlayMakerFSM.Start -= ModifyGPFSM;

	private void ModifyGPFSM(On.PlayMakerFSM.orig_Start orig, PlayMakerFSM self) {
		orig(self);

		if (self is {
			gameObject: { name: "Grey Prince" },
			FsmName: "Control"
		}) {
			ModifyGPFSM(self);

			Logger.LogDebug("Grey Prince FSM modified");
		}
	}

	private static void ModifyGPFSM(PlayMakerFSM fsm) =>
		fsm.ChangeTransition("Enter 1", FsmEvent.Finished.Name, "Enter Short");
}
