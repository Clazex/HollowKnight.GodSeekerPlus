namespace GodSeekerPlus.Modules.QoL;

[DefaultEnabled]
internal sealed class FastDreamWarp : Module {
	public FastDreamWarp() =>
		On.PlayMakerFSM.Start += ModifyDreamNailFSM;

	private void ModifyDreamNailFSM(On.PlayMakerFSM.orig_Start orig, PlayMakerFSM self) {
		orig(self);

		if (self is {
			name: "Knight",
			FsmName: "Dream Nail"
		}) {
			ModifyDreamNailFSM(self);

			Logger.LogDebug("Dream Warp FSM modified");
		}
	}

	private static void ModifyDreamNailFSM(PlayMakerFSM fsm) =>
		fsm.InsertCustomAction("Warp Charge", (fsm) => {
			if (BossSceneController.IsBossScene && ModuleManager.TryGetActiveModule<FastDreamWarp>(out _)) {
				fsm.SendEvent("CHARGED");
			}
		}, 0);
}
