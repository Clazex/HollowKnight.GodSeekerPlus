namespace GodSeekerPlus.Modules.QoL;

[DefaultEnabled]
internal sealed class FastSuperDash : Module {
	private const string stateName = "GSP Workshop Speed Buff";

	[GlobalSetting]
	[FloatOption(1.0f, 1.1f, 1.2f, 1.3f, 1.4f, 1.5f, 1.6f, 1.7f, 1.8f, 1.9f, 2.0f)]
	private static readonly float fastSuperDashSpeedMultiplier = 1.5f;

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
			multiplyBy = fastSuperDashSpeedMultiplier
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
