using Osmi.FsmActions;

namespace GodSeekerPlus.Modules.QoL;

public sealed class ShortDeathAnimation : Module {
	public override bool DefaultEnabled => true;

	public ShortDeathAnimation() =>
		On.HeroController.Start += ModifyHeroDeathFSM;

	private void ModifyHeroDeathFSM(On.HeroController.orig_Start orig, HeroController self) {
		orig(self);

		ModifyHeroDeathFSM(self.heroDeathPrefab.LocateMyFSM("Hero Death Anim"));

		Logger.LogDebug("Hero Death FSM modified");
	}

	private void ModifyHeroDeathFSM(PlayMakerFSM fsm) => fsm.AddAction("Bursting",
		new InvokePredicate(() => Loaded && BossSceneController.IsBossScene) { trueEvent = FsmEvent.Finished }
	);
}
