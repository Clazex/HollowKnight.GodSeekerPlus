namespace GodSeekerPlus.Modules.QoL;

public sealed class CorrectRadianceHP : Module {
	private static readonly int shift = -720;

	public override bool DefaultEnabled => true;

	public override ToggleableLevel ToggleableLevel => ToggleableLevel.ChangeScene;

	private protected override void Load() {
		On.HealthManager.Start += ModifyAbsRadStartHP;
		On.PlayMakerFSM.Start += ModifyAbsRadPhaseHP;
	}

	private protected override void Unload() {
		On.HealthManager.Start -= ModifyAbsRadStartHP;
		On.PlayMakerFSM.Start -= ModifyAbsRadPhaseHP;
	}

	private static void ModifyAbsRadStartHP(On.HealthManager.orig_Start orig, HealthManager self) {
		if (self.gameObject is {
			scene.name: "GG_Radiance",
			name: "Absolute Radiance"
		}) {
			self.hp += shift;

			Logger.LogDebug("AbsRad start health modified");
		}

		orig(self);
	}


	private static void ModifyAbsRadPhaseHP(On.PlayMakerFSM.orig_Start orig, PlayMakerFSM self) {
		orig(self);

		if (self.gameObject is {
			scene.name: "GG_Radiance",
			name: "Absolute Radiance"
		}) {
			ModifyAbsRadPhaseHP(self);
		}
	}

	private static void ModifyAbsRadPhaseHP(PlayMakerFSM fsm) {
		if (fsm.FsmName == "Control") {
			fsm.GetState("Scream").Actions
				.OfType<SetHP>().First()
				.hp.Value += shift;
			fsm.GetVariable<FsmInt>("Death HP").Value += shift;

			Logger.LogDebug("AbsRad death health modified");
		} else if (fsm.FsmName == "Phase Control") {
			fsm.GetVariable<FsmInt>("P2 Spike Waves").Value += shift;
			fsm.GetVariable<FsmInt>("P3 A1 Rage").Value += shift;
			fsm.GetVariable<FsmInt>("P4 Stun1").Value += shift;
			fsm.GetVariable<FsmInt>("P5 Acend").Value += shift; // Why TC

			Logger.LogDebug("AbsRad phase health modified");
		}
	}
}
