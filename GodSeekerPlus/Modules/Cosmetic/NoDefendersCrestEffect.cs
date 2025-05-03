using Modding.Utils;

namespace GodSeekerPlus.Modules.Cosmetic;

public sealed class NoDefendersCrestEffect : Module {
	[GlobalSetting]
	[EnumOption]
	[ReloadOnUpdate]
	public static EffectType dcrestEffectRemoval = EffectType.TrailOnly;

	private GameObject? cloudPrefab;

	private protected override void Load() {
		ModifyParticle(HeroController.UnsafeInstance, true);
		On.HeroController.Start += ModifyNew;
		On.ObjectPool.Spawn_GameObject_Transform_Vector3_Quaternion += ModifyCloud;
	}

	private protected override void Unload() {
		ModifyParticle(HeroController.UnsafeInstance, false);
		On.HeroController.Start -= ModifyNew;
		On.ObjectPool.Spawn_GameObject_Transform_Vector3_Quaternion -= ModifyCloud;
	}

	private void ModifyParticle(HeroController? hc, bool active) {
		cloudPrefab = null;
		CloudMarker.list.ForEach((cm) => cm.UpdateParticle(!active));

		if (hc == null) {
			return;
		}

		hc.gameObject.Child("Charm Effects", "Dung", "Particle 1")!.GetComponent<ParticleSystemRenderer>().enabled = !active;
		cloudPrefab = active
			? hc.gameObject.Child("Charm Effects", "Dung")!
				.LocateMyFSM("Control")
				.GetAction<SpawnObjectFromGlobalPoolOverTime>("Equipped", 0)
				.gameObject.Value
			: null;
	}

	private void ModifyNew(On.HeroController.orig_Start orig, HeroController self) {
		orig(self);
		ModifyParticle(self, true);
	}

	private GameObject ModifyCloud(On.ObjectPool.orig_Spawn_GameObject_Transform_Vector3_Quaternion orig, GameObject prefab, Transform parent, Vector3 position, Quaternion rotation) {
		GameObject go = orig(prefab, parent, position, rotation);

		if (cloudPrefab != null && prefab == cloudPrefab) {
			LogDebug("Modifying Dung Cloud");
			go.GetOrAddComponent<CloudMarker>().UpdateParticle(false);
		}

		return go;
	}

	public enum EffectType {
		TrailOnly = 0,
		All = 1,
	}

	private class CloudMarker : MonoBehaviour {
		public static HashSet<CloudMarker> list = [];

		public void Awake() => list.Add(this);

		public void OnDestroy() => list.Remove(this);

		public void UpdateParticle(bool enabled) {
			GameObject pt = gameObject.Child("Pt Normal")!;
			pt.GetComponent<ParticleSystemRenderer>().enabled
				= enabled || dcrestEffectRemoval != EffectType.All;
		}
	}
}
