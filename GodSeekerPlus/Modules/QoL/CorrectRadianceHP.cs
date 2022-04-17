namespace GodSeekerPlus.Modules.QoL;

[ToggleableLevel(ToggleableLevel.ChangeScene)]
[DefaultEnabled]
internal sealed class CorrectRadianceHP : Module {
	private static readonly int shift = -720;

	private protected override void Load() {
		On.HealthManager.Start += ModifyAbsRadStartHP;
		On.PlayMakerFSM.Start += ModifyAbsRadPhaseHP;
	}

	private protected override void Unload() {
		On.HealthManager.Start -= ModifyAbsRadStartHP;
		On.PlayMakerFSM.Start -= ModifyAbsRadPhaseHP;
	}

	private void ModifyAbsRadStartHP(On.HealthManager.orig_Start orig, HealthManager self) {
		if (self.gameObject is {
			scene.name: "GG_Radiance",
			name: "Absolute Radiance"
		}) {
			self.hp += shift;
		}

		orig(self);
	}


	private void ModifyAbsRadPhaseHP(On.PlayMakerFSM.orig_Start orig, PlayMakerFSM self) {
		orig(self);

		if (self.gameObject is {
			scene.name: "GG_Radiance",
			name: "Absolute Radiance"
		}) {
			if (self.FsmName == "Control") {
				self.GetState("Scream").Actions
					.OfType<SetHP>().First()
					.hp.Value += shift;
				self.GetVariable<FsmInt>("Death HP").Value += shift;
			} else if (self.FsmName == "Phase Control") {
				self.GetVariable<FsmInt>("P2 Spike Waves").Value += shift;
				self.GetVariable<FsmInt>("P3 A1 Rage").Value += shift;
				self.GetVariable<FsmInt>("P4 Stun1").Value += shift;
				self.GetVariable<FsmInt>("P5 Acend").Value += shift; // Why TC
			}
		}
	}

}
