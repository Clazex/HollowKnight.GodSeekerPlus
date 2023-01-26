using WaitUntil = Osmi.FsmActions.WaitUntil;

namespace GodSeekerPlus.Modules.Bugfix;

public sealed class TransitionDeath : Module {
	public override bool DefaultEnabled => true;

	private protected override void Load() =>
		On.PlayMakerFSM.Start += ModifyHeroDeathFSM;

	private protected override void Unload() =>
		On.PlayMakerFSM.Start -= ModifyHeroDeathFSM;

	private void ModifyHeroDeathFSM(On.PlayMakerFSM.orig_Start orig, PlayMakerFSM self) {
		if (self is {
			name: "Hero Death",
			FsmName: "Hero Death Anim"
		}) {
			ModifyHeroDeathFSM(self);

			Logger.LogDebug("Transition detection added to Hero Death FSM");
		}

		orig(self);
	}

	private static void ModifyHeroDeathFSM(PlayMakerFSM fsm) =>
		fsm.AddAction("WP Check", new WaitUntil(() => !Ref.GM.IsInSceneTransition, FsmEvent.Finished));
}
