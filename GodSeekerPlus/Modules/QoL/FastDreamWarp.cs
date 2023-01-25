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

	private static void ModifyDreamNailFSM(PlayMakerFSM fsm) {
		fsm.InsertCustomAction("Warp Charge", (fsm) => {
			if (BossSceneController.IsBossScene && ModuleManager.TryGetActiveModule<FastDreamWarp>(out _)) {
				fsm.SendEvent("CHARGED");
			}
		}, 0);

		bool? origInvincibility = null;
		fsm.InsertCustomAction("Start", (fsm) => {
			if (BossSceneController.IsBossScene && PlayerData.instance != null) {
				origInvincibility = PlayerData.instance.isInvincible;
				PlayerData.instance.isInvincible = true;
			}
		}, 0);
		fsm.InsertCustomAction("Warp End", (fsm) => {
			if (BossSceneController.IsBossScene && PlayerData.instance != null && origInvincibility != null) {
				PlayerData.instance.isInvincible = origInvincibility.Value;
			}
		}, 0);
		fsm.InsertCustomAction("Warp Cancel", (fsm) => {
			if (BossSceneController.IsBossScene && PlayerData.instance != null && origInvincibility != null) {
				PlayerData.instance.isInvincible = origInvincibility.Value;
			}
		}, 0);
		fsm.InsertCustomAction("Inactive", (fsm) => {
			if (BossSceneController.IsBossScene && PlayerData.instance != null && origInvincibility != null) {
				PlayerData.instance.isInvincible = origInvincibility.Value;
			}
		}, 0);
	}
}
