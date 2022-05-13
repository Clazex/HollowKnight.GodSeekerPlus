namespace GodSeekerPlus.Modules.Bugfix;

[DefaultEnabled]
internal sealed class TransitionDeath : Module {
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

	private sealed class WaitUntil : FsmStateAction {
		private readonly Func<bool> predicate;
		private readonly FsmEvent @event;

		internal WaitUntil(Func<bool> predicate, FsmEvent @event) {
			this.predicate = predicate;
			this.@event = @event;
		}

		public override void OnUpdate() {
			if (predicate()) {
				Fsm.Event(@event);
				Finish();
			}
		}
	}
}
