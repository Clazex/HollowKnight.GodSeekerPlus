using Osmi.FsmActions;

namespace GodSeekerPlus.Modules.QoL;

public sealed class FastSuperDash : Module {
	[GlobalSetting]
	[BoolOption]
	private static readonly bool instantSuperDash = false;

	[GlobalSetting]
	[FloatOption(1.0f, 1.1f, 1.2f, 1.3f, 1.4f, 1.5f, 1.6f, 1.7f, 1.8f, 1.9f, 2.0f)]
	private static readonly float fastSuperDashSpeedMultiplier = 1.5f;

	public override bool DefaultEnabled => true;

	public FastSuperDash() =>
		On.PlayMakerFSM.Start += ModifySuperDashFSM;

	private void ModifySuperDashFSM(On.PlayMakerFSM.orig_Start orig, PlayMakerFSM self) {
		orig(self);

		if (self is {
			gameObject: {
				name: "Knight",
				scene.name: "DontDestroyOnLoad"
			},
			FsmName: "Superdash"
		}) {
			ModifySuperDashFSM(self);

			Logger.LogDebug("Superdash FSM modified");
		}
	}

	private void ModifySuperDashFSM(PlayMakerFSM fsm) {
		bool shouldActivate() => Loaded && Ref.GM.sceneName == "GG_Workshop";
		bool shouldRemoveWinding() => shouldActivate() && instantSuperDash;

		var waitEvent = FsmEvent.GetFsmEvent("WAIT");

		fsm.AddAction("Wall Charge", new InvokePredicate(shouldRemoveWinding) {
			trueEvent = waitEvent
		});
		fsm.AddAction("Ground Charge", new InvokePredicate(shouldRemoveWinding) {
			trueEvent = waitEvent
		});

		FsmFloat speed = fsm.GetVariable<FsmFloat>("Current SD Speed");
		fsm.Intercept(new TransitionInterceptor() {
			fromState = "Left",
			eventName = FsmEvent.Finished.Name,
			toStateDefault = "Dash Start",
			toStateCustom = "Dash Start",
			shouldIntercept = shouldActivate,
			onIntercept = (_, _) => speed.Value *= fastSuperDashSpeedMultiplier
		});
		fsm.Intercept(new TransitionInterceptor() {
			fromState = "Right",
			eventName = FsmEvent.Finished.Name,
			toStateDefault = "Dash Start",
			toStateCustom = "Dash Start",
			shouldIntercept = shouldActivate,
			onIntercept = (_, _) => speed.Value *= fastSuperDashSpeedMultiplier
		});

		fsm.AddAction("Dashing", new InvokePredicate(shouldRemoveWinding) {
			trueEvent = waitEvent
		});
		fsm.AddAction("Air Cancel", new InvokePredicate(shouldRemoveWinding) {
			trueEvent = FsmEvent.Finished
		});
		fsm.AddAction("Hit Wall", new InvokePredicate(shouldRemoveWinding) {
			trueEvent = FsmEvent.Finished
		});
	}
}
