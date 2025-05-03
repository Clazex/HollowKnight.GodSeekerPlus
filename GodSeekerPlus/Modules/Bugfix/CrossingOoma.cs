namespace GodSeekerPlus.Modules.Bugfix;

public sealed class CrossingOoma : Module {
	public override bool DefaultEnabled => true;

	public override ToggleableLevel ToggleableLevel => ToggleableLevel.ChangeScene;

	public CrossingOoma() =>
		OsmiHooks.SceneChangeHook += ModifyPrefab;

	private protected override void Load() =>
		OsmiHooks.SceneChangeHook += DestroyJelly;

	private protected override void Unload() =>
		OsmiHooks.SceneChangeHook -= DestroyJelly;

	private static void DestroyJelly(Scene prev, Scene next) {
		if (prev.name is "GG_Uumuu" or "GG_Uumuu_V") {
			JellyMarker[] markers = JellyMarker.markers.ToArray();

			if (markers.Length == 0) {
				return;
			}

			Log($"Found {markers.Length} cross-scene ooma(s), destroying...");
			markers.ForEach(marker => UObject.Destroy(marker.gameObject));
		}
	}

	private static void ModifyPrefab(Scene prev, Scene next) {
		if (next.name is not "GG_Uumuu" and not "GG_Uumuu_V") {
			return;
		}

		EnemyDeathEffects deathEffect = next.GetRootGameObjects()
			.First(go => go.name == "Jellyfish Spawner")
			.GetComponent<PersonalObjectPool>()
			.startupPool[0].prefab
			.GetComponent<EnemyDeathEffectsBubble>();
		GameObject corpse = deathEffect.Reflect().corpsePrefab;
		GameObject jelly = corpse
			.LocateMyFSM("corpse")
			.GetAction<CreateObject>("Explode", 3)
			.gameObject.Value;

		_ = corpse.AddComponent<JellyMarker>();
		_ = jelly.AddComponent<JellyMarker>();
		JellyMarker.markers.Clear();

		OsmiHooks.SceneChangeHook -= ModifyPrefab;
	}

	private sealed class JellyMarker : MonoBehaviour {
		internal static HashSet<JellyMarker> markers = [];

		private void Awake() => markers.Add(this);

		private void OnDestroy() => markers.Remove(this);
	}
}
