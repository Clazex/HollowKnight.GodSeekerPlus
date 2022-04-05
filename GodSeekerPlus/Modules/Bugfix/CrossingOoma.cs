namespace GodSeekerPlus.Modules.Bugfix;

[Category(nameof(Bugfix))]
[ToggleableLevel(ToggleableLevel.ChangeScene)]
[DefaultEnabled]
internal sealed class CrossingOoma : Module {
	public CrossingOoma() =>
		USceneManager.activeSceneChanged += ModifyPrefab;

	private protected override void Load() =>
		USceneManager.activeSceneChanged += DestroyJelly;

	private protected override void Unload() =>
		USceneManager.activeSceneChanged -= DestroyJelly;

	private void DestroyJelly(Scene prev, Scene _) {
		if (prev.name == "GG_Uumuu" || prev.name == "GG_Uumuu_V") {
			UObject.FindObjectsOfType<JellyMarker>()
				.ForEach(marker => UObject.Destroy(marker.gameObject));
		}
	}

	private static void ModifyPrefab(Scene _, Scene next) {
		if (next.name != "GG_Uumuu" && next.name != "GG_Uumuu_V") {
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

		corpse.AddComponent<JellyMarker>();
		jelly.AddComponent<JellyMarker>();

		USceneManager.activeSceneChanged -= ModifyPrefab;
	}

	private sealed class JellyMarker : MonoBehaviour {
	}
}
