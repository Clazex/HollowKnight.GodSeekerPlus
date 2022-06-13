namespace GodSeekerPlus.Modules.QoL;

[DefaultEnabled]
internal sealed class FastSuperDash : Module {
	private const string stateName = "GSP Workshop Speed Buff";

	public FastSuperDash() =>
		On.PlayMakerFSM.Start += ModifySuperDashFSM;

	private void ModifySuperDashFSM(On.PlayMakerFSM.orig_Start orig, PlayMakerFSM self) {
		orig(self);

		if (self is {
			name: "Knight",
			FsmName: "Superdash"
		}) {
			ModifySuperDashFSM(self);

			Logger.LogDebug("Superdash FSM modified");
		}
	}

	private static void ModifySuperDashFSM(PlayMakerFSM fsm) {
		FsmState speedBuffState = fsm.AddState(stateName);

		speedBuffState.InsertAction(new CheckSceneName() {
			sceneName = "GG_Workshop",
			notEqualEvent = FsmEvent.Finished
		}, 0);
		speedBuffState.InsertAction(new FloatMultiply() {
			floatVariable = fsm.GetVariable<FsmFloat>("Current SD Speed"),
			multiplyBy = Setting.Global.FastSuperDashSpeedMultiplier
		}, 1);

		fsm.Intercept(new TransitionInterceptor() {
			fromState = "Left",
			eventName = FsmEvent.Finished.Name,
			toStateDefault = "Dash Start",
			toStateCustom = stateName,
			shouldIntercept = () => ModuleManager.TryGetActiveModule<FastSuperDash>(out _)
		});
		fsm.Intercept(new TransitionInterceptor() {
			fromState = "Right",
			eventName = FsmEvent.Finished.Name,
			toStateDefault = "Dash Start",
			toStateCustom = stateName,
			shouldIntercept = () => ModuleManager.TryGetActiveModule<FastSuperDash>(out _)
		});

		fsm.AddTransition(stateName, FsmEvent.Finished.Name, "Dash Start");
	}
}
