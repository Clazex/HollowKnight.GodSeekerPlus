namespace GodSeekerPlus.Modules.QoL;

[DefaultEnabled]
internal sealed class FastDreamWarp : Module {
	[GlobalSetting]
	[BoolOption]
	private static readonly bool instantWarp = true;

	public FastDreamWarp() =>
		On.PlayMakerFSM.Start += ModifyDreamNailFSM;

	private void ModifyDreamNailFSM(On.PlayMakerFSM.orig_Start orig, PlayMakerFSM self) {
		orig(self);

		if (self is {
			gameObject: {
				name: "Knight",
				scene.name: "DontDestroyOnLoad"
			},
			FsmName: "Dream Nail"
		}) {
			ModifyDreamNailFSM(self);

			Logger.LogDebug("Dream Warp FSM modified");
		}
	}

	private static void ModifyDreamNailFSM(PlayMakerFSM fsm) {
		fsm.Intercept(new TransitionInterceptor() {
			fromState = "Start",
			eventName = FsmEvent.Finished.Name,
			toStateDefault = "Entry Cancel Check",
			toStateCustom = "Can Warp?",
			shouldIntercept = () => {
				HeroActions actions = InputHandler.Instance.inputActions;
				return instantWarp
					&& BossSceneController.IsBossScene
					&& ModuleManager.TryGetActiveModule<FastDreamWarp>(out _)
					&& actions.dreamNail.IsPressed
					&& actions.up.IsPressed;
			}
		});

		fsm.Intercept(new TransitionInterceptor() {
			fromState = "Warp Charge Start",
			eventName = FsmEvent.Finished.Name,
			toStateDefault = "Warp Charge",
			toStateCustom = "Can Warp?",
			shouldIntercept = () => BossSceneController.IsBossScene && ModuleManager.TryGetActiveModule<FastDreamWarp>(out _)
		});
	}
}
