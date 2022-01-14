namespace GodSeekerPlus.Modules.QoL;

[Category(nameof(QoL))]
[ToggleableLevel(ToggleableLevel.ReloadSave)]
[DefaultEnabled]
internal sealed class FastDreamWarp : Module {
	private protected override void Load() =>
		On.PlayMakerFSM.Start += ModifyDreamNailFSM;

	private protected override void Unload() =>
		On.PlayMakerFSM.Start -= ModifyDreamNailFSM;

	private void ModifyDreamNailFSM(On.PlayMakerFSM.orig_Start orig, PlayMakerFSM self) {
		orig(self);

		if (self is {
			gameObject.name: "Knight",
			FsmName: "Dream Nail"
		}) {
			ModifyDreamNailFSM(self);

			Logger.LogDebug("Dream Warp FSM modified");
		}
	}

	private static void ModifyDreamNailFSM(PlayMakerFSM fsm) =>
		fsm.InsertAction("Warp Charge", new GGCheckIfBossScene {
			// If in boss scene, fire CHARGED event immediately
			bossSceneEvent = fsm.GetAction<Wait>("Warp Charge", 0).finishEvent,
		}, 0);
}
