namespace GodSeekerPlus.Modules.Misc;

internal sealed class InspectHelper : Module {
	public override bool Hidden => true;

	private protected override void Load() =>
		ModHooks.FinishedLoadingModsHook += CreateGameObject;

	private static void CreateGameObject() {
		if (ModHooks.GetMod("Unity Explorer", true) == null) {
			return;
		}

		_ = GameObjectUtil.CreateHolder<Inspector>($"{nameof(GodSeekerPlus)} Inspect Helper");
		Log("Creating Inspect Helper GameObject");
	}

	private sealed class Test {
#if DEBUG
#endif
	}

	private sealed class Inspector : MonoBehaviour {
		public Test test;
		public static GodSeekerPlus Instance => GodSeekerPlus.UnsafeInstance;
		public static bool Active => GodSeekerPlus.Active;

		public static Dictionary<string, Module> Modules => ModuleManager.Modules;

		public static GlobalSettings GlobalSettings => Setting.Global;
		public static LocalSettings LocalSettings => Setting.Local;

		public static Dict Dict => L11nUtil.dict;

		void Awake() => test = new();

		public static string Localize(string key) => L11nUtil.Localize(key);

		public static void KillAllSafe() => UObject.FindObjectsOfType<HealthManager>()
			.Filter(EnemyDetector.IsValidEnemy)
			.Reject(hm => hm.IsInvincible)
			.ForEach(hm => hm.Die(null, AttackTypes.RuinsWater, true));

		public static void SkipBoss(int count = 1) =>
			GlobalCoroutineExecutor.Start(SkipBossCoroutine(count));

		private static IEnumerator SkipBossCoroutine(int count) {
			if (BossSceneController.Instance is not BossSceneController controller) {
				yield break;
			}

			controller.bossesDeadWaitTime = 0f;
			controller.EndBossScene();

			if (count > 1) {
				bool flag = false;
				Ref.GM.OnFinishedEnteringScene += () => flag = true;
				yield return new WaitUntil(() => flag);

				_ = GlobalCoroutineExecutor.Start(SkipBossCoroutine(count - 1));
			}
		}
	}
}
