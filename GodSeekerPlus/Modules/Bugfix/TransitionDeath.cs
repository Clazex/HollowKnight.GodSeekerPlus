using WaitUntil = Osmi.FsmActions.WaitUntil;

namespace GodSeekerPlus.Modules.Bugfix;

public sealed class TransitionDeath : Module {
	private static readonly GameObjectRef deathRef =
		new(GameObjectRef.DONT_DESTROY_ON_LOAD, "Knight", "Hero Death");

	public override bool DefaultEnabled => true;

	private protected override void Load() =>
		On.PlayMakerFSM.Start += ModifyHeroDeathFSM;

	private protected override void Unload() =>
		On.PlayMakerFSM.Start -= ModifyHeroDeathFSM;

	private static void ModifyHeroDeathFSM(On.PlayMakerFSM.orig_Start orig, PlayMakerFSM self) {
		if (self.FsmName == "Hero Death Anim" && deathRef.MatchGameObject(self.gameObject)) {
			ModifyHeroDeathFSM(self);

			LogDebug("Transition detection added to Hero Death FSM");
		}

		orig(self);
	}

	private static void ModifyHeroDeathFSM(PlayMakerFSM fsm) =>
		fsm.AddAction("WP Check", new WaitUntil(() => !Ref.GM.IsInSceneTransition, FsmEvent.Finished));
}
