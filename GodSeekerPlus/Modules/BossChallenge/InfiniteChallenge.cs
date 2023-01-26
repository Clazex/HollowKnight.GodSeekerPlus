namespace GodSeekerPlus.Modules.BossChallenge;

public sealed class InfiniteChallenge : Module {
	[GlobalSetting]
	[BoolOption]
	public static bool restartFightOnSuccess = false;

	private static BossSceneController.SetupEventDelegate? setupEvent;

	public override ToggleableLevel ToggleableLevel => ToggleableLevel.ChangeScene;

	private protected override void Load() {
		On.BossSceneController.Awake += RecordSetupEvent;
		// ModHooks.BeforeSceneLoadHook doesn't support editing entry gate
		On.GameManager.BeginSceneTransition += RestartFight;
		OsmiHooks.SceneChangeHook += Cleanup;
	}

	private protected override void Unload() {
		setupEvent = null;

		On.BossSceneController.Awake -= RecordSetupEvent;
		On.GameManager.BeginSceneTransition -= RestartFight;
		OsmiHooks.SceneChangeHook -= Cleanup;
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
			info.SceneName == "GG_Workshop"
			&& setupEvent != null
			&& (
				Ref.HC.heroDeathPrefab.activeSelf // Death returning
				|| ( // Success returning
					restartFightOnSuccess
					&& StaticVariableList.GetValue<bool>("finishedBossReturning")
				)
			)
		) {
			BossSceneController.SetupEvent = setupEvent;
			setupEvent = null;
			StaticVariableList.SetValue("finishedBossReturning", false);

			_ = GlobalCoroutineExecutor.Start(DelayedEnableRenderer());
			Ref.HC.EnterWithoutInput(true);
			Ref.HC.AcceptInput();
			Ref.HC.gameObject.LocateMyFSM("Dream Return").GetVariable<FsmBool>("Dream Returning").Value = false;

			info.SceneName = currentSceneName;
			info.EntryGateName = "door_dreamEnter";
		}

		orig(self, info);
	}

	private static IEnumerator DelayedEnableRenderer() {
		yield return new WaitUntil(() => Ref.GM.IsInSceneTransition);
		yield return new WaitWhile(() => Ref.GM.IsInSceneTransition);

		Ref.HC.EnableRenderer();
	}

	private static void Cleanup(Scene prev, Scene next) {
		if (next.name == "GG_Workshop" || BossSequenceController.IsInSequence) {
			setupEvent = null;
		}
	}
}
