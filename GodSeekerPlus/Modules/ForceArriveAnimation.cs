namespace GodSeekerPlus.Modules;

[Module(toggleableLevel = ToggleableLevel.AnyTime, defaultEnabled = false)]
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
		On.PlayMakerFSM.Start += ModifyDreamEntryFSM;

	private protected override void Unload() =>
		On.PlayMakerFSM.Start -= ModifyDreamEntryFSM;

	private void ModifyDreamEntryFSM(On.PlayMakerFSM.orig_Start orig, PlayMakerFSM self) {
		orig(self);

		if (
			self is {
				gameObject: { name: "Dream Entry" },
				FsmName: "Control"
			} && scenes.Contains(self.gameObject.scene.name)
		) {
			self.ChangeTransition("First Boss? 1", "STATUE", "Hide Player");

			self.GetAction<GGCheckIfBossSequence>("First Boss?", 0).falseEvent =
				self.GetAction<GGCheckIfFirstBossScene>("First Boss?", 1).trueEvent;

			Logger.LogDebug("Dream Entry FSM modified");
		}
	}
}
