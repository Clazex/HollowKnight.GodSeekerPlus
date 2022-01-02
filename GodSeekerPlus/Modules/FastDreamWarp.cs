namespace GodSeekerPlus.Modules;

[Category("QoL")]
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
			gameObject: { name: "Knight" },
			FsmName: "Dream Nail"
		}) {
			self.InsertAction("Warp Charge", new GGCheckIfBossScene {
				// If in boss scene, fire CHARGED event immediately
				bossSceneEvent = self.GetAction<Wait>("Warp Charge", 0).finishEvent,
			}, 0);

			Logger.LogDebug("Dream Warp FSM modified");
		}
	}
}
