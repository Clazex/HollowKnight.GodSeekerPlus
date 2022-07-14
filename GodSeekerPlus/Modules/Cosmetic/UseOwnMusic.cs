namespace GodSeekerPlus.Modules.Cosmetic;

[ToggleableLevel(ToggleableLevel.ChangeScene)]
internal sealed class UseOwnMusic : Module {
	private protected override void Load() =>
		On.PlayMakerFSM.Start += ModifyGGMusicControlFSM;

	private protected override void Unload() =>
		On.PlayMakerFSM.Start -= ModifyGGMusicControlFSM;

	private void ModifyGGMusicControlFSM(On.PlayMakerFSM.orig_Start orig, PlayMakerFSM self) {
		if (self is {
			name: "_SceneManager",
			FsmName: "gg_music_control" or "FSM"
		}) {
			if (self.GetState("Boss Statue") != null && self.FsmEvents.Any(@event => @event.Name == "GG MUSIC")) {
				ModifyGGMusicControlFSM(self);

				Logger.LogDebug("GG Music Control modified");
			}
		} else if (self is {
			gameObject: {
				scene.name: "GG_Nosk_Hornet",
				name: "Battle Scene"
			},
			FsmName: "Battle Control"
		}) {
			ModifyWingedNoskMusicFSM(self);

			Logger.LogDebug("Winged Nosk Music Control modified");
		}

		orig(self);
	}

	private static void ModifyGGMusicControlFSM(PlayMakerFSM fsm) =>
		fsm.ChangeTransition("Init", FsmEvent.Finished.Name, "Wait");

	private static void ModifyWingedNoskMusicFSM(PlayMakerFSM fsm) =>
		fsm.ChangeTransition("Music Type", "PANTHEON", "Orig Music");
}
