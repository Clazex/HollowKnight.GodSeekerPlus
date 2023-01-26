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
		// In state "Start", send FAST_DREAM_WARP if both "dream nail" and "up" are pressed in a boss fight.
		fsm.InsertCustomAction("Start", () => {
			var inputHandler = typeof(HeroController).GetField("inputHandler", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(HeroController.instance) as InputHandler;
			if (BossSceneController.IsBossScene
				&& inputHandler != null
				&& inputHandler.inputActions.dreamNail.IsPressed
				&& inputHandler.inputActions.up.IsPressed
				&& ModuleManager.TryGetActiveModule<FastDreamWarp>(out _)) {
				fsm.SendEvent("FAST_DREAM_WARP");
			}
		}, 0);

		// Route FAST_DREAM_WARP to the start of dream warp.
		fsm.AddTransition("Start", "FAST_DREAM_WARP", "Can Warp?");
	}
}
