namespace GodSeekerPlus.Modules.Bugfix;

public sealed class GodTamerBeastRollCollider : Module {
	public override ToggleableLevel ToggleableLevel => ToggleableLevel.ChangeScene;

	private protected override void Load() =>
		On.PlayMakerFSM.Start += ModifyLobsterFSM;

	private protected override void Unload() =>
		On.PlayMakerFSM.Start -= ModifyLobsterFSM;

	private static void ModifyLobsterFSM(On.PlayMakerFSM.orig_Start orig, PlayMakerFSM self) {
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
		fsm.AddAction("Idle", CloneUtil.CreateMemberwiseClone(fsm.GetAction<ActivateGameObject>("Spit Recover", 2)));
}

