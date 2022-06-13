namespace GodSeekerPlus.Modules.Bugfix;

[ToggleableLevel(ToggleableLevel.ChangeScene)]
[DefaultEnabled]
internal sealed class CrossingOoma : Module {
	public CrossingOoma() =>
		OsmiHooks.SceneChangeHook += ModifyPrefab;

	private protected override void Load() =>
		OsmiHooks.SceneChangeHook += DestroyJelly;

	private protected override void Unload() =>
		OsmiHooks.SceneChangeHook -= DestroyJelly;

	private void DestroyJelly(Scene prev, Scene next) {
		if (prev.name is "GG_Uumuu" or "GG_Uumuu_V") {
			IEnumerable<JellyMarker> markers = UObject.FindObjectsOfType<JellyMarker>();

			if (!markers.Any()) {
				return;
			}

			Logger.Log("Found cross-scene ooma(s), destroying");
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
		GameObject corpse = ReflectionHelper
			.GetField<EnemyDeathEffects, GameObject>(deathEffect, "corpsePrefab");
		GameObject jelly = corpse
			.LocateMyFSM("corpse")
			.GetAction<CreateObject>("Explode", 3)
			.gameObject.Value;

		_ = corpse.AddComponent<JellyMarker>();
		_ = jelly.AddComponent<JellyMarker>();

		OsmiHooks.SceneChangeHook -= ModifyPrefab;
	}

	private sealed class JellyMarker : MonoBehaviour {
	}
}
