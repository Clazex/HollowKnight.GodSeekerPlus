namespace GodSeekerPlus.Modules.Bugfix;

public sealed class GreyPrinceNailSlamCollider : Module {
	public override ToggleableLevel ToggleableLevel => ToggleableLevel.ChangeScene;

	private protected override void Load() =>
		On.PlayMakerFSM.Start += ModifyGPZFSM;

	private protected override void Unload() =>
		On.PlayMakerFSM.Start -= ModifyGPZFSM;

	private static void ModifyGPZFSM(On.PlayMakerFSM.orig_Start orig, PlayMakerFSM self) {
		orig(self);

		if (self is {
			gameObject: {
				scene.name: "GG_Grey_Prince_Zote",
				name: "Grey Prince"
			},
			FsmName: "Control"
		}) {
			ModifyGPZFSM(self);

			LogDebug("Grey Prince nail slam collider fixed");
		}
	}

	private static void ModifyGPZFSM(PlayMakerFSM fsm) =>
		fsm.AddAction("Send Event", CloneUtil.CreateMemberwiseClone(fsm.GetAction<ActivateGameObject>("Slash End", 1)));
}
