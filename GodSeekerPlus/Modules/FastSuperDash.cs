namespace GodSeekerPlus.Modules;

[Module(toggleableLevel = ToggleableLevel.ReloadSave, defaultEnabled = true)]
internal sealed class FastSuperDash : Module {
	private protected override void Load() =>
		On.PlayMakerFSM.Start += ModifySuperDashFSM;

	private protected override void Unload() =>
		On.PlayMakerFSM.Start -= ModifySuperDashFSM;

	private void ModifySuperDashFSM(On.PlayMakerFSM.orig_Start orig, PlayMakerFSM self) {
		orig(self);

		if (self.gameObject.name == "Knight" && self.FsmName == "Superdash") {
			FsmState stateWsSpdBuff = self.CreateState("GSP Workshop Speed Buff");

			stateWsSpdBuff.AddAction(new CheckSceneName() {
				sceneName = "GG_Workshop",
				notEqualEvent = FsmEvent.Finished
			});
			stateWsSpdBuff.AddAction(new FloatMultiply() {
				floatVariable = self.FsmVariables.FindFsmFloat("Current SD Speed"),
				multiplyBy = GodSeekerPlus.Instance.GlobalSettings.fastSuperDashSpeedMultiplier
			});

			self.GetState("Left").ChangeTransition(FsmEvent.Finished.Name, stateWsSpdBuff.Name);
			self.GetState("Right").ChangeTransition(FsmEvent.Finished.Name, stateWsSpdBuff.Name);

			stateWsSpdBuff.AddTransition(FsmEvent.Finished.Name, "Dash Start");

			Logger.LogDebug("Superdash FSM modified");
		}
	}
}
