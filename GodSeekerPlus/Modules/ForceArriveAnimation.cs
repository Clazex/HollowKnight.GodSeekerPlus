namespace GodSeekerPlus.Modules;

[Module(toggleable = true, defaultEnabled = false)]
internal sealed class ForceArriveAnimation : Module {
	private static readonly string[] scenes = {
		"GG_Vengefly",
		"GG_Vengefly_V",
		"GG_Ghost_Xero",
		"GG_Ghost_Xero_V",
		"GG_Hive_Knight",
		"GG_Crystal_Guardian_2"
	};

	private protected override void Load() =>
		On.PlayMakerFSM.OnEnable += ModifyDreamEntryFSM;

	private protected override void Unload() =>
		On.PlayMakerFSM.OnEnable -= ModifyDreamEntryFSM;

	private void ModifyDreamEntryFSM(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self) {
		orig(self);

		if (
			self.gameObject.name == "Dream Entry"
			&& self.FsmName == "Control"
			&& scenes.Contains(self.gameObject.scene.name)
		) {
			self.ChangeTransition("First Boss? 1", "STATUE", "Hide Player");

			FsmState stateFirstBoss = self.GetState("First Boss?");
			stateFirstBoss.GetAction<GGCheckIfBossSequence>().falseEvent =
				stateFirstBoss.GetAction<GGCheckIfFirstBossScene>().trueEvent;

			Logger.LogDebug("Dream Entry FSM modified");
		}
	}
}
