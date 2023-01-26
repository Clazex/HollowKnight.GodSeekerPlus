namespace GodSeekerPlus.Modules.QoL;

public sealed class ShortDeathAnimation : Module {
	private static readonly GameObjectRef deathRef =
		new(GameObjectRef.DONT_DESTROY_ON_LOAD, "Knight", "Hero Death");

	public override bool DefaultEnabled => true;

	public override ToggleableLevel ToggleableLevel => ToggleableLevel.ReloadSave;

	private protected override void Load() =>
		On.PlayMakerFSM.Start += ModifyHeroDeathFSM;

	private protected override void Unload() =>
		On.PlayMakerFSM.Start -= ModifyHeroDeathFSM;

	private static void ModifyHeroDeathFSM(On.PlayMakerFSM.orig_Start orig, PlayMakerFSM self) {
		orig(self);

		if (self.FsmName == "Hero Death Anim" && deathRef.MatchGameObject(self.gameObject)) {
			ModifyHeroDeathFSM(self);

			Logger.LogDebug("Hero Death FSM modified");
		}
	}

	private static void ModifyHeroDeathFSM(PlayMakerFSM fsm) =>
		fsm.InsertAction("Bursting", new GGCheckIfBossScene() {
			bossSceneEvent = FsmEvent.Finished
		}, 5);
}
