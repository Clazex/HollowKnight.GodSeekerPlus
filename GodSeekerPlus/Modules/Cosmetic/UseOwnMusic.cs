using UnityEngine.Audio;

namespace GodSeekerPlus.Modules.Cosmetic;

public sealed class UseOwnMusic : Module {
	private static readonly Lazy<AudioMixerSnapshot> silent = new(() =>
		Resources.FindObjectsOfTypeAll<AudioMixerSnapshot>().First(i => i.name == "Silent")
	);

	public override ToggleableLevel ToggleableLevel => ToggleableLevel.ChangeScene;

	private static readonly SceneEdit[] handles = [
		// Pantheon 1
		new(new("GG_Vengefly", "_SceneManager"), ModifySceneManagerGGMusicControl),
		new(new("GG_Gruz_Mother", "_SceneManager"), ModifySceneManagerGGMusicControl),
		new(new("GG_False_Knight", "_SceneManager"), ModifySceneManagerGGMusicControl),
		new(new("GG_Mega_Moss_Charger", "_SceneManager"), StopMusicAndDestroySceneManagerFSM),
		new(new("GG_Mega_Moss_Charger", "Mega Moss Charger"), (go) => go
			.LocateMyFSM("Mossy Control").GetAction("Music", 0).Enabled = false
		),
		new(new("GG_Hornet_1", "_SceneManager"), ModifySceneManagerFSM),

		new(new("GG_Ghost_Gorb", "_SceneManager"), ModifySceneManagerFSM),
		new(new("GG_Dung_Defender", "_SceneManager"), StopMusicAndModifySceneManagerFSM),
		new(new("GG_Mage_Knight", "_SceneManager"), StopMusicAndModifySceneManagerFSM),
		new(new("GG_Brooding_Mawlek", "_SceneManager"), ModifySceneManagerFSM),
		// GG_Nailmasters = NoOp,


		// Pantheon 2


		// Pantheon 3


		// Pantheon 4


		// Pantheon 5
		new(new("GG_Vengefly_V", "_SceneManager"), ModifySceneManagerGGMusicControl),
		new(new("GG_Gruz_Mother_V", "_SceneManager"), ModifySceneManagerGGMusicControl),

		new(new("GG_Ghost_Gorb_V", "_SceneManager"), ModifySceneManagerFSM),
		new(new("GG_Mage_Knight_V", "_SceneManager"), StopMusicAndModifySceneManagerFSM),
		new(new("GG_Brooding_Mawlek_V", "Battle Scene"), (go) => {
			StopMusic();
			PlayMakerFSM fsm = go.LocateMyFSM("Activate Boss");
			fsm.ChangeTransition("Call Mawlek", FsmEvent.Finished.Name, "Music Up 2");
			fsm.GetAction<ApplyMusicCue>("Music", 0).musicCue.Value
				= fsm.GetAction<ApplyMusicCue>("Music Up 2", 0).musicCue.Value;
			fsm.GetAction<TransitionToAudioSnapshot>("Music", 1).snapshot.Value
				= fsm.GetAction<TransitionToAudioSnapshot>("Music Up 2", 1).snapshot.Value;
		}),

		// new(new("GG_Nosk_Hornet", "Battle Scene"), (go) => go
		// 	.LocateMyFSM("Battle Control")
		// 	.ChangeTransition("Music Type", "PANTHEON", "Orig Music")
		// ),

		// GG_Hollow_Knight = NoOp,
		// GG_Radiance = NoOp
	];

	private protected override void Load() =>
		handles.ForEach(handle => handle.Enable());

	private protected override void Unload() =>
		handles.ForEach(handle => handle.Disable());


	private static void StopMusic() =>
		silent.Value.TransitionTo(0f);

	private static void ModifySceneManagerGGMusicControl(GameObject go) => go
		.LocateMyFSM("gg_music_control")
		.ChangeTransition("Init", FsmEvent.Finished.Name, "Wait");

	private static void ModifySceneManagerFSM(GameObject go) => go
		.LocateMyFSM("FSM")
		.ChangeTransition("Init", FsmEvent.Finished.Name, "Wait");

	private static void StopMusicAndModifySceneManagerFSM(GameObject go) {
		StopMusic();
		ModifySceneManagerFSM(go);
	}

	private static void StopMusicAndDestroySceneManagerFSM(GameObject go) {
		StopMusic();
		UObject.Destroy(go.LocateMyFSM("FSM"));
	}
}
