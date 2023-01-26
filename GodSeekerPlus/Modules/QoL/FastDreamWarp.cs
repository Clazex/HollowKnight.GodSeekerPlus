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
		// Add new state. Send FAST_DREAM_WARP if both "dream nail" and "up" are pressed in a boss fight. Otherwise, send CANCEL.
		var state = fsm.AddState("Fast Dream Warp");
		state.AddCustomAction(() => {
			var inputHandler = typeof(HeroController).GetField("inputHandler", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(HeroController.instance) as InputHandler;
			if (BossSceneController.IsBossScene
				&& inputHandler != null
				&& inputHandler.inputActions.dreamNail.IsPressed
				&& inputHandler.inputActions.up.IsPressed
				&& ModuleManager.TryGetActiveModule<FastDreamWarp>(out _)) {
				fsm.SendEvent("FAST_DREAM_WARP");
			} else {
				fsm.SendEvent("CANCEL");
			}
		});

		// Connect the new state into the graph.
		fsm.ChangeTransition("Take Control", "FINISHED", "Fast Dream Warp");
		fsm.AddTransition("Fast Dream Warp", "FAST_DREAM_WARP", "Can Warp?");
		fsm.AddTransition("Fast Dream Warp", "CANCEL", "Start");
	}
}
