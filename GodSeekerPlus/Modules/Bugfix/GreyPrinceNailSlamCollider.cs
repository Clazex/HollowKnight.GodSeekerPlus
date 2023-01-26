namespace GodSeekerPlus.Modules.Bugfix;

public sealed class GreyPrinceNailSlamCollider : Module {
	public override bool DefaultEnabled => true;

	public override ToggleableLevel ToggleableLevel => ToggleableLevel.ChangeScene;

	private protected override void Load() =>
		On.PlayMakerFSM.Start += ModifyGPZFSM;

	private protected override void Unload() =>
		On.PlayMakerFSM.Start -= ModifyGPZFSM;

	private void ModifyGPZFSM(On.PlayMakerFSM.orig_Start orig, PlayMakerFSM self) {
		orig(self);

		if (self is {
			gameObject: {
				scene.name: "GG_Grey_Prince_Zote",
				name: "Grey Prince"
			},
			FsmName: "Control"
		}) {
			ModifyGPZFSM(self);

			Logger.LogDebug("Grey Prince nail slam collider fixed");
		}
	}

	private static void ModifyGPZFSM(PlayMakerFSM fsm) =>
		fsm.AddAction("Send Event", fsm.GetAction("Slash End", 1));
}
