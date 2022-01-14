namespace GodSeekerPlus.Modules.QoL;

[Category(nameof(QoL))]
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
			gameObject.name: "Knight",
			FsmName: "Superdash"
		}) {
			ModifySuperDashFSM(self);

			Logger.LogDebug("Superdash FSM modified");
		}
	}

	private static void ModifySuperDashFSM(PlayMakerFSM fsm) {
		fsm.AddState(stateName);

		fsm.AddAction(stateName, new CheckSceneName() {
			sceneName = "GG_Workshop",
			notEqualEvent = FsmEvent.Finished
		});
		fsm.AddAction(stateName, new FloatMultiply() {
			floatVariable = fsm.GetVariable<FsmFloat>("Current SD Speed"),
			multiplyBy = GodSeekerPlus.UnsafeInstance.GlobalSettings.fastSuperDashSpeedMultiplier
		});

		fsm.ChangeTransition("Left", FsmEvent.Finished.Name, stateName);
		fsm.ChangeTransition("Right", FsmEvent.Finished.Name, stateName);

		fsm.AddTransition(stateName, FsmEvent.Finished.Name, "Dash Start");
	}
}
