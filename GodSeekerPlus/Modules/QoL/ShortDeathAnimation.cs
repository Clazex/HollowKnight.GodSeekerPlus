namespace GodSeekerPlus.Modules.QoL;

[ToggleableLevel(ToggleableLevel.ReloadSave)]
[DefaultEnabled]
internal sealed class ShortDeathAnimation : Module {
	private protected override void Load() =>
		On.PlayMakerFSM.Start += ModifyHeroDeathFSM;

	private protected override void Unload() =>
		On.PlayMakerFSM.Start -= ModifyHeroDeathFSM;

	private void ModifyHeroDeathFSM(On.PlayMakerFSM.orig_Start orig, PlayMakerFSM self) {
		orig(self);

		if (self is {
			name: "Hero Death",
			FsmName: "Hero Death Anim"
		}) {
			ModifyHeroDeathFSM(self);

			Logger.LogDebug("Hero Death FSM modified");
		}
	}

	private static void ModifyHeroDeathFSM(PlayMakerFSM fsm) =>
		fsm.InsertAction("Bursting", new GGCheckIfBossScene() {
			bossSceneEvent = FsmEvent.Finished
		}, 5);
}
