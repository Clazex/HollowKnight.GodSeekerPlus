using GodSeekerPlus.Modules.GodseekerMode;

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

	private static bool ShouldActivate() => BossSceneController.IsBossScene
		|| ColosseumOfFools.IsInGodseekerColosseum;

	private void ModifyDreamNailFSM(On.PlayMakerFSM.orig_Start orig, PlayMakerFSM self) {
		orig(self);

		if (self.FsmName == "Dream Nail" && knightRef.MatchGameObject(self.gameObject)) {
			ModifyDreamNailFSM(self);

			LogDebug("Dream Warp FSM modified");
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
					&& ShouldActivate()
					&& actions.dreamNail.IsPressed
					&& actions.up.IsPressed;
			}
		});

		fsm.Intercept(new TransitionInterceptor() {
			fromState = "Warp Charge Start",
			eventName = FsmEvent.Finished.Name,
			toStateDefault = "Warp Charge",
			toStateCustom = "Can Warp?",
			shouldIntercept = () => Loaded && ShouldActivate()
		});

		fsm.GetAction("Warp End", 8).Enabled = false;

		fsm.AddAction("Warp End", new InvokePredicate(() => Loaded && ShouldActivate()) {
			trueEvent = FsmEvent.Finished
		});
	}
}
