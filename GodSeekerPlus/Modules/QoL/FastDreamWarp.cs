using Osmi.FsmActions;

namespace GodSeekerPlus.Modules.QoL;

public sealed class FastDreamWarp : Module {
	private static readonly GameObjectRef knightRef = new(GameObjectRef.DONT_DESTROY_ON_LOAD, "Knight");

	[GlobalSetting]
	[BoolOption]
	public static bool instantWarp = true;

	public override bool DefaultEnabled => true;

	public FastDreamWarp() =>
		On.PlayMakerFSM.Start += ModifyDreamNailFSM;

	private void ModifyDreamNailFSM(On.PlayMakerFSM.orig_Start orig, PlayMakerFSM self) {
		orig(self);

		if (self.FsmName == "Dream Nail" && knightRef.MatchGameObject(self.gameObject)) {
			ModifyDreamNailFSM(self);

			Logger.LogDebug("Dream Warp FSM modified");
		}
	}

	private void ModifyDreamNailFSM(PlayMakerFSM fsm) {
		fsm.Intercept(new TransitionInterceptor() {
			fromState = "Take Control",
			eventName = FsmEvent.Finished.Name,
			toStateDefault = "Start",
			toStateCustom = "Can Warp?",
			shouldIntercept = () => {
				HeroActions actions = InputHandler.Instance.inputActions;
				return Loaded
					&& instantWarp
					&& BossSceneController.IsBossScene
					&& actions.dreamNail.IsPressed
					&& actions.up.IsPressed;
			}
		});

		fsm.Intercept(new TransitionInterceptor() {
			fromState = "Warp Charge Start",
			eventName = FsmEvent.Finished.Name,
			toStateDefault = "Warp Charge",
			toStateCustom = "Can Warp?",
			shouldIntercept = () => Loaded && BossSceneController.IsBossScene
		});

		fsm.AddAction("Warp End", new InvokePredicate(() => Loaded && BossSceneController.IsBossScene) {
			trueEvent = FsmEvent.Finished
		});
	}
}
