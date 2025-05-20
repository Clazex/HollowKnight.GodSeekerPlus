using UnityEngine.Audio;

namespace GodSeekerPlus.Modules.BossChallenge;

public sealed class InfiniteChallenge : Module {
	[GlobalSetting]
	[BoolOption]
	public static bool restartFightOnSuccess = false;

	[GlobalSetting]
	[BoolOption]
	public static bool restartFightAndMusic = false;

	public static readonly HashSet<Func<GameManager.SceneLoadInfo, bool>> returnScenePredicates = [
		(info) => info.SceneName is "GG_Workshop"
	];

	private static BossSceneController.SetupEventDelegate? setupEvent;

	public override ToggleableLevel ToggleableLevel => ToggleableLevel.ChangeScene;

	private protected override void Load() {
		On.BossSceneController.Awake += RecordSetupEvent;
		// ModHooks.BeforeSceneLoadHook doesn't support editing entry gate
		On.GameManager.BeginSceneTransition += RestartFight;
	}

	private protected override void Unload() {
		setupEvent = null;

		On.BossSceneController.Awake -= RecordSetupEvent;
		On.GameManager.BeginSceneTransition -= RestartFight;
	}

	private static void RecordSetupEvent(On.BossSceneController.orig_Awake orig, BossSceneController self) {
		if (!BossSequenceController.IsInSequence) {
			setupEvent = BossSceneController.SetupEvent;
		}

		orig(self);
	}

	private static void RestartFight(On.GameManager.orig_BeginSceneTransition orig, GameManager self, GameManager.SceneLoadInfo info) {
		string currentSceneName = self.sceneName;
		if (
			BossSceneController.IsBossScene
			&& setupEvent != null
			&& returnScenePredicates.Any((predicate) => predicate.Invoke(info))
			&& (
				Ref.HC.heroDeathPrefab.activeSelf // Death returning
				|| ( // Success returning
					restartFightOnSuccess
					&& StaticVariableList.GetValue<bool>("finishedBossReturning")
				)
			)
		) {
			BossSceneController.SetupEvent = setupEvent;
			StaticVariableList.SetValue("finishedBossReturning", false);

			_ = GlobalCoroutineExecutor.Start(DelayedEnableRenderer());
			Ref.HC.EnterWithoutInput(true);
			Ref.HC.AcceptInput();
			Ref.HC.gameObject.LocateMyFSM("Dream Return").GetVariable<FsmBool>("Dream Returning").Value = false;

			info.SceneName = currentSceneName;
			info.EntryGateName = "door_dreamEnter";
			
			_ = GlobalCoroutineExecutor.Start(SetAudio(restartFightAndMusic));
		}

		setupEvent = null;
		orig(self, info);
	}

	private static IEnumerator DelayedEnableRenderer() {
		yield return new WaitUntil(() => Ref.GM.IsInSceneTransition);
		yield return new WaitWhile(() => Ref.GM.IsInSceneTransition);

		Ref.HC.EnableRenderer();
	}

	private static IEnumerator SetAudio(bool reset) {
		AudioMixer mixer = Ref.GM.AudioManager.Reflect().musicSources[0].outputAudioMixerGroup.audioMixer;

		if (reset) {
			Ref.GM.AudioManager.Reflect().musicSources.ForEach(
				i => i.Stop()
			);
			Ref.GM.AudioManager.Reflect().currentMusicCue = null;
		} else {
			mixer.FindSnapshot("Silent").TransitionTo(0.5f);
		}

		yield return new WaitUntil(() => Ref.GM.IsInSceneTransition);
		yield return new WaitWhile(() => Ref.GM.IsInSceneTransition);
		mixer.FindSnapshot("Normal").TransitionTo(0f);
	}
}
