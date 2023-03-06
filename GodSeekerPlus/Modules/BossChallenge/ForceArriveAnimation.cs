namespace GodSeekerPlus.Modules.BossChallenge;

public sealed class ForceArriveAnimation : Module {
	private static readonly string[] scenes = {
		"GG_Vengefly",
		"GG_Vengefly_V",
		"GG_Ghost_Xero",
		"GG_Ghost_Xero_V",
		"GG_Hive_Knight",
		"GG_Crystal_Guardian_2"
	};

	public override ToggleableLevel ToggleableLevel => ToggleableLevel.ChangeScene;

	private protected override void Load() =>
		On.PlayMakerFSM.Start += ModifyDreamEntryFSM;

	private protected override void Unload() =>
		On.PlayMakerFSM.Start -= ModifyDreamEntryFSM;

	private static void ModifyDreamEntryFSM(On.PlayMakerFSM.orig_Start orig, PlayMakerFSM self) {
		orig(self);

		if (
			self is {
				name: "Dream Entry",
				FsmName: "Control"
			} && scenes.Contains(self.gameObject.scene.name)
		) {
			ModifyDreamEntryFSM(self);

			LogDebug("Dream Entry FSM modified");
		}
	}

	private static void ModifyDreamEntryFSM(PlayMakerFSM fsm) {
		fsm.ChangeTransition("First Boss? 1", "STATUE", "Hide Player");

		fsm.GetAction<GGCheckIfBossSequence>("First Boss?", 0).falseEvent =
			fsm.GetAction<GGCheckIfFirstBossScene>("First Boss?", 1).trueEvent;
	}
}
