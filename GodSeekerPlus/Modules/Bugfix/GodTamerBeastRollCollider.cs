namespace GodSeekerPlus.Modules.Bugfix;

[ToggleableLevel(ToggleableLevel.ChangeScene)]
internal sealed class GodTamerBeastRollCollider : Module {
	private protected override void Load() =>
		On.PlayMakerFSM.Start += ModifyLobsterFSM;

	private protected override void Unload() =>
		On.PlayMakerFSM.Start -= ModifyLobsterFSM;

	private void ModifyLobsterFSM(On.PlayMakerFSM.orig_Start orig, PlayMakerFSM self) {
		orig(self);

		if (self is {
			gameObject: {
				scene.name: "GG_God_Tamer",
				name: "Lobster"
			},
			FsmName: "Control"
		}) {
			ModifyLobsterFSM(self);

			Logger.LogDebug("God Tamer Beast roll collider fixed");
		}
	}

	private static void ModifyLobsterFSM(PlayMakerFSM fsm) =>
		fsm.AddAction("Idle", fsm.GetAction("Spit Recover", 2));
}

