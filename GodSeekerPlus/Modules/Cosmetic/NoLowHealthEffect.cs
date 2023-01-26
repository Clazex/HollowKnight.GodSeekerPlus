using MonoMod.Cil;

namespace GodSeekerPlus.Modules.Cosmetic;

public sealed class NoLowHealthEffect : Module {
	private static readonly GameObjectRef healthRef =
		new(GameObjectRef.DONT_DESTROY_ON_LOAD, "_GameCameras", "HudCamera", "Hud Canvas", "Health");

	private static readonly GameObjectRef damageEffectsRef =
		new(GameObjectRef.DONT_DESTROY_ON_LOAD, "Knight", "Effects", "Damage Effect");

	public NoLowHealthEffect() =>
		On.PlayMakerFSM.Start += ModifyFSM;

	private protected override void Load() {
		IL.HeroAnimationController.PlayIdle += RemoveAnimation;

		if (Ref.HC != null) {
			Fsm fsm = GameObjectUtil.FindGameObjectByRef(Ref.DDOL, healthRef).LocateMyFSM("Low Health FX").Fsm;

			if (fsm.ActiveStateName is "Low Health On Entry" or "Low Health") {
				fsm.SetState("Idle");
			}
		}
	}

	private protected override void Unload() {
		IL.HeroAnimationController.PlayIdle -= RemoveAnimation;

		if (Ref.HC != null) {
			Fsm fsm = GameObjectUtil.FindGameObjectByRef(Ref.DDOL, healthRef).LocateMyFSM("Low Health FX").Fsm;

			if (fsm.ActiveStateName == "Idle") {
				fsm.SetState("Low Health?");
			}
		}
	}

	private void ModifyFSM(On.PlayMakerFSM.orig_Start orig, PlayMakerFSM self) {
		orig(self);

		if (self.FsmName == "Low Health FX" && healthRef.MatchGameObject(self.gameObject)) {
			ModifyLowHealthFXFSM(self);
		} else if (self.FsmName == "Knight Damage" && damageEffectsRef.MatchGameObject(self.gameObject)) {
			ModifyDamageEffectFSM(self);
		}
	}

	private void ModifyLowHealthFXFSM(PlayMakerFSM fsm) {
		bool shouldActivate() => Loaded;

		fsm.Intercept(new TransitionInterceptor() {
			fromState = "Init",
			eventName = "LOW",
			toStateDefault = "Low Health On Entry",
			toStateCustom = "Idle",
			shouldIntercept = shouldActivate
		});

		fsm.Intercept(new TransitionInterceptor() {
			fromState = "HUD In HP Check",
			eventName = "LOW",
			toStateDefault = "Low Health On Entry",
			toStateCustom = "Idle",
			shouldIntercept = shouldActivate
		});

		fsm.Intercept(new TransitionInterceptor() {
			fromState = "Idle",
			eventName = "HERO DAMAGED",
			toStateDefault = "Low Health Pause",
			toStateCustom = "Idle",
			shouldIntercept = shouldActivate
		});
	}

	private void ModifyDamageEffectFSM(PlayMakerFSM fsm) => fsm.Intercept(new TransitionInterceptor() {
		fromState = "Check Focus Prompt",
		eventName = FsmEvent.Finished.Name,
		toStateDefault = "Last HP?",
		toStateCustom = "Leak",
		shouldIntercept = () => Loaded
	});

	private static void RemoveAnimation(ILContext il) {
		int index = new ILCursor(il).Goto(0).GotoNext(
			MoveType.After,
			inst => inst.MatchLdstr("Idle Hurt"),
			inst => inst.MatchCallvirt<tk2dSpriteAnimator>("Play"),
			inst => inst.MatchRet()
		).Index;

		_ = new ILCursor(il).Goto(0).RemoveRange(index);
	}
}
