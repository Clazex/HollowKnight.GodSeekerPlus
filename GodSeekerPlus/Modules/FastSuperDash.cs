namespace GodSeekerPlus.Modules;

[ToggleableLevel(ToggleableLevel.ReloadSave)]
[DefaultEnabled]
internal sealed class FastSuperDash : Module {
	private const string stateName = "GSP Workshop Speed Buff";

	private protected override void Load() =>
		On.PlayMakerFSM.Start += ModifySuperDashFSM;

	private protected override void Unload() =>
		On.PlayMakerFSM.Start -= ModifySuperDashFSM;

	private void ModifySuperDashFSM(On.PlayMakerFSM.orig_Start orig, PlayMakerFSM self) {
		orig(self);

		if (self is {
			gameObject: { name: "Knight" },
			FsmName: "Superdash"
		}) {
			self.AddState(stateName);

			self.AddAction(stateName, new CheckSceneName() {
				sceneName = "GG_Workshop",
				notEqualEvent = FsmEvent.Finished
			});
			self.AddAction(stateName, new FloatMultiply() {
				floatVariable = self.GetVariable<FsmFloat>("Current SD Speed"),
				multiplyBy = GodSeekerPlus.Instance.GlobalSettings.fastSuperDashSpeedMultiplier
			});

			self.ChangeTransition("Left", FsmEvent.Finished.Name, stateName);
			self.ChangeTransition("Right", FsmEvent.Finished.Name, stateName);

			self.AddTransition(stateName, FsmEvent.Finished.Name, "Dash Start");

			Logger.LogDebug("Superdash FSM modified");
		}
	}
}
